using UnityEngine;

public class BasicTower : MonoBehaviour
{
    [SerializeField] protected bool bypassEditor;
    [SerializeField] protected Vector2Int buildSize = new Vector2Int(1, 1);
    
    protected CreepManager creepManager;
    
    protected bool isActive; // for example when in ghost mode
    
    protected virtual void Awake()
    {
        if (bypassEditor)
            Enable();
        else
            Disable();

        creepManager = FindObjectOfType<CreepManager>();
    }
    
    public void Enable()
    {
        isActive = true;
        // reset color
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    public void Disable()
    {
        isActive = false;
    }

    public Vector2Int GetBuildSize()
    {
        return buildSize;
    }
}
