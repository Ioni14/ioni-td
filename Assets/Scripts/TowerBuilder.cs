using UnityEngine;

public class TowerBuilder : MonoBehaviour
{
    [SerializeField] private Grid gridBuild;

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
    }

    private void Start()
    {
        mainCamera = Camera.main;
    }

    public void SelectTower()
    {
        var worldMousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        ghostGameObject = Instantiate(towerPrefab, worldMousePos, Quaternion.identity);
        ghostTower = ghostGameObject.GetComponent<BasicTower>();
        ghostSpriteRenderer = ghostGameObject.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (ghostTower) {
            var worldMousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            var towerCellPos = ToGridPosition(worldMousePos);
            ghostTower.transform.position = ToWorldPosition(towerCellPos);
            if (!CanPlaceTowerHere(ghostTower, towerCellPos)) {
                ghostSpriteRenderer.color = ghostErrorColor;
            }
            else {
                ghostSpriteRenderer.color = ghostValidColor;
            }
        }

        if (Input.GetButtonDown("Fire1")) {
            AttemptToPlaceTower();
        }
    }

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

        Debug.Log("place tower to : " + ghostTower.transform.position.ToString());

        towerManager.AddTower(ghostTower);
        ghostTower.Enable();
        ghostGameObject = null;
        ghostTower = null;
        ghostSpriteRenderer = null;
    }

    private bool CanPlaceTowerHere(BasicTower tower, Vector2Int cellPosition)
    {
        var minPos = CalculateMinCellPosition(cellPosition, tower.GetBuildSize());
        var maxPos = CalculateMaxCellPosition(cellPosition, tower.GetBuildSize());

        foreach (var managedTower in towerManager.GetTowers()) {
            var towerManagedCellPos = (Vector2Int) gridBuild.WorldToCell(managedTower.transform.position);

            // if a tower take the cellPosition, we can't place here
            var managedMinPos = CalculateMinCellPosition(towerManagedCellPos, managedTower.GetBuildSize());
            var managedMaxPos = CalculateMaxCellPosition(towerManagedCellPos, managedTower.GetBuildSize());

            Debug.Log(""+minPos+" // "+maxPos+" // "+managedMinPos+" // "+managedMaxPos);

            // Test AABB vs AABB
            if (minPos.x <= managedMaxPos.x
                && maxPos.x >= managedMinPos.x
                && minPos.y <= managedMaxPos.y
                && maxPos.y >= managedMinPos.y) {
                Debug.Log("collision!");
                return false;
            }
        }

        return true;
    }

    private static Vector2Int CalculateMinCellPosition(Vector2Int centerCellPosition, Vector2Int buildSize)
    {
        return new Vector2Int(
            Mathf.FloorToInt(centerCellPosition.x - buildSize.x / 2f) + 1,
            Mathf.FloorToInt(centerCellPosition.y - buildSize.y / 2f) + 1);
    }

    private static Vector2Int CalculateMaxCellPosition(Vector2Int centerCellPosition, Vector2Int buildSize)
    {
        return new Vector2Int(
            Mathf.FloorToInt(centerCellPosition.x + buildSize.x / 2f),
            Mathf.FloorToInt(centerCellPosition.y + buildSize.y / 2f));
    }
}
