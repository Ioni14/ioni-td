using UnityEngine;

public class CreepMover : MonoBehaviour, IStatsObserver
{
    private CreepManager creepManager;
    private WaypointManager waypointManager;
    
    private UnitStats unitStatsComponent;

    private int nextWaypointIndex;
    
    private void Awake()
    {
        creepManager = FindObjectOfType<CreepManager>();
        waypointManager = FindObjectOfType<WaypointManager>();
        
        unitStatsComponent = GetComponent<UnitStats>();
        if (unitStatsComponent) {
            unitStatsComponent.SubscribeViewer(this);
        }
    }

    private void Start()
    {
        transform.position = waypointManager.GetStart().position;
    }

    private void FixedUpdate()
    {
        var nextWaypoint = waypointManager.GetWaypoint(nextWaypointIndex);
        var step = unitStatsComponent.GetCurrentSpeed() * Time.fixedDeltaTime;
        
        var distance = Vector2.Distance(transform.position, nextWaypoint.position);
        var rest = step;
        Vector2 direction;
        if (distance < step) {
            if (waypointManager.IsEnd(nextWaypointIndex)) {
                FinishPath();
            }
            rest = step - distance;
            nextWaypointIndex++;
            var nextNextWaypoint = waypointManager.GetWaypoint(nextWaypointIndex);
            direction = (nextNextWaypoint.position - nextWaypoint.position).normalized;
            transform.position = nextWaypoint.position; // we correctly replace the creep before next translation
        }
        else {
            direction = (nextWaypoint.position - transform.position).normalized;
        }

        transform.Translate(direction * rest);
    }

    private void FinishPath()
    {
        creepManager.RemoveCreep(gameObject);
    }

    public void OnStatsChanged(UnitStats stats)
    {
        if (stats.IsDead()) {
            Kill(); // TODO : move this shit
        }
    }
    
    private void Kill()
    {
        // TODO : do something before Destroy ? VFX, sound, death animation
        creepManager.RemoveCreep(gameObject);
    }
}
