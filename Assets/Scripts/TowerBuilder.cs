using UnityEngine;
using UnityEngine.Tilemaps;

public class TowerBuilder : MonoBehaviour
{
    [SerializeField] private Tilemap tilemapBuild;
    private Grid gridBuild;

    [SerializeField] private GameObject towerPrefab;
    [SerializeField] private Color ghostValidColor;
    [SerializeField] private Color ghostErrorColor;

    private TowerManager towerManager;
    private Camera mainCamera;

    private GameObject ghostGameObject;
    private BasicTower ghostTower;
    private SpriteRenderer ghostSpriteRenderer;

    private void Awake()
    {
        towerManager = FindObjectOfType<TowerManager>();
        Destroy(tilemapBuild.GetComponent<TilemapRenderer>());
        gridBuild = tilemapBuild.GetComponentInParent<Grid>();
    }

    private void Start()
    {
        mainCamera = Camera.main;
    }

    public void SelectTower()
    {
        CancelSelection();

        var worldMousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        ghostGameObject = Instantiate(towerPrefab, worldMousePos, Quaternion.identity);
        ghostTower = ghostGameObject.GetComponent<BasicTower>();
        ghostSpriteRenderer = ghostGameObject.GetComponent<SpriteRenderer>();
    }

    private void CancelSelection()
    {
        if (ghostGameObject) {
            Destroy(ghostGameObject);
            ghostGameObject = null;
        }

        ghostTower = null;
        ghostSpriteRenderer = null;
    }

    private void Update()
    {
        if (ghostTower) {
            var worldMousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            var towerCellPos = ToGridPosition(worldMousePos);

//            ghostTower.transform.position = ComputeTowerWorldPosition(ghostSpriteRenderer, ghostTower, towerCellPos);
            ghostTower.transform.position = ToWorldPosition(towerCellPos);
            if (!CanPlaceTowerHere(ghostTower, towerCellPos)) {
                ghostSpriteRenderer.color = ghostErrorColor;
                // TODO : change color of tiles under tower ?
            }
            else {
                ghostSpriteRenderer.color = ghostValidColor;
                // TODO : change color of tiles under tower ?
            }
        }

        if (Input.GetButtonDown("Fire1")) {
            AttemptToPlaceTower();
        }

        if (Input.GetButtonDown("Cancel")) {
            CancelSelection();
        }
    }

    // TODO : ComputeTowerCellPosition
//    private Vector2 ComputeTowerWorldPosition(SpriteRenderer towerSpriteRenderer, BasicTower tower, Vector2Int towerCellPos)
//    {
//        var bounds = towerSpriteRenderer.sprite.bounds;
//        var worldPos = ToWorldPosition(towerCellPos);
//        var buildSize = tower.GetBuildSize();
//
//        var computedCenter = new Vector2(
//            bounds.size.x * Mathf.Floor(buildSize.x / 2f) / buildSize.x - bounds.extents.x,
//            bounds.size.y * Mathf.Floor(buildSize.y / 2f) / buildSize.y - bounds.extents.y
//        );
//        Debug.Log("---------------");
//        Debug.Log("bounds : "+bounds);
//        Debug.Log("computedCenter : "+computedCenter);
//        Debug.Log("worldPos : "+worldPos);
//        Debug.Log("(towerSpriteRenderer.sprite.pivot - computedCenter) : "+((Vector2) bounds.center - computedCenter));
//        Debug.Log("worldPos + (towerSpriteRenderer.sprite.pivot - computedCenter) : "+(worldPos - ((Vector2) bounds.center - computedCenter)));
//
////        return worldPos;
//        return worldPos - ((Vector2) bounds.center - computedCenter);
//    }

    private Vector2Int ToGridPosition(Vector2 worldPos)
    {
        return (Vector2Int) gridBuild.WorldToCell(worldPos);
    }

    private Vector2 ToWorldPosition(Vector2Int cellPos)
    {
        return gridBuild.CellToWorld((Vector3Int) cellPos);
    }

    private void AttemptToPlaceTower()
    {
        if (!ghostTower) {
            return;
        }

        if (!CanPlaceTowerHere(ghostTower, ToGridPosition(ghostTower.transform.position))) {
            return;
        }

        towerManager.AddTower(ghostTower);
        ghostTower.Enable();

        // TODO : continue to select or unselect ?
        ghostGameObject = null;
        ghostTower = null;
        ghostSpriteRenderer = null;
        // create a new selection of same prefab
        SelectTower();
    }

    private bool CanPlaceTowerHere(BasicTower tower, Vector2Int cellPosition)
    {
        var minPos = CalculateMinCellPosition(cellPosition, tower.GetBuildSize());
        var maxPos = CalculateMaxCellPosition(cellPosition, tower.GetBuildSize());

        // good terrain ?
        for (var x = minPos.x; x <= maxPos.x; ++x) {
            for (var y = minPos.y; y <= maxPos.y; ++y) {
                var tile = (TowerBuildTile) tilemapBuild.GetTile(new Vector3Int(x, y, 0));
                if (!tile || !tile.canBuild) {
                    return false;
                }
            }
        }

        // no other towers ?
        foreach (var managedTower in towerManager.GetTowers()) {
            var towerManagedCellPos = (Vector2Int) gridBuild.WorldToCell(managedTower.transform.position);

            // if a tower take the cellPosition, we can't place here
            var managedMinPos = CalculateMinCellPosition(towerManagedCellPos, managedTower.GetBuildSize());
            var managedMaxPos = CalculateMaxCellPosition(towerManagedCellPos, managedTower.GetBuildSize());

            // Test AABB vs AABB
            if (minPos.x <= managedMaxPos.x
                && maxPos.x >= managedMinPos.x
                && minPos.y <= managedMaxPos.y
                && maxPos.y >= managedMinPos.y) {
                return false;
            }
        }

        return true;
    }

    private static Vector2Int CalculateMinCellPosition(Vector2Int originCellPosition, Vector2Int buildSize)
    {
        return originCellPosition;
//        return new Vector2Int(
//            Mathf.CeilToInt(originCellPosition.x - buildSize.x / 2f),
//            Mathf.CeilToInt(originCellPosition.y - buildSize.y / 2f));
    }

    private static Vector2Int CalculateMaxCellPosition(Vector2Int originCellPosition, Vector2Int buildSize)
    {
        return new Vector2Int(
            originCellPosition.x + buildSize.x - 1,
            originCellPosition.y + buildSize.y - 1);

        return new Vector2Int(
            Mathf.CeilToInt(originCellPosition.x + buildSize.x / 2f) - 1,
            Mathf.CeilToInt(originCellPosition.y + buildSize.y / 2f) - 1);
    }
}