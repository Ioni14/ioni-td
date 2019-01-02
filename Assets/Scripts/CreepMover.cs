using UnityEngine;

public class CreepMover : MonoBehaviour
{
    [SerializeField] [Tooltip("In wu/seconds")] private float baseSpeed;

    private CreepManager creepManager;
    private WaypointManager waypointManager;

    private int nextWaypointIndex;
    private float currentSpeed;

    private void Awake()
    {
        creepManager = FindObjectOfType<CreepManager>();
        waypointManager = FindObjectOfType<WaypointManager>();
    }

    private void Start()
    {
        currentSpeed = baseSpeed;

        transform.position = waypointManager.GetStart().position;
    }

    private void FixedUpdate()
    {
        var nextWaypoint = waypointManager.GetWaypoint(nextWaypointIndex);
        var step = currentSpeed * Time.fixedDeltaTime;
        
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
        Debug.Log("Finish!");
        creepManager.RemoveCreep(gameObject);
    }
}
