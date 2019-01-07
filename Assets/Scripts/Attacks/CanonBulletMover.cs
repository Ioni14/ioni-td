using UnityEngine;

public class CanonBulletMover : MonoBehaviour
{
    [SerializeField] private float speed = 20f;
    [SerializeField] private float targetHitRadius = 0.25f;
    [SerializeField] [Range(0f, 10f)] private float arcHeight = 1f;
    [SerializeField] [Range(-1f, -0.01f)] private float coefDir = -0.25f;
    
    private Attack attackComponent;
    private Vector2 target;
    private Vector2 startPosition;

    private void Awake()
    {
        attackComponent = GetComponent<Attack>();
    }

    private void Start()
    {
        startPosition = transform.position;
    }

    private void FixedUpdate()
    {
        var step = speed * Time.fixedDeltaTime;

//        transform.right = (target - transform.position).normalized;
//        transform.Translate(Vector2.right * step);

        // close to target ?
        if ((target - (Vector2) transform.position).sqrMagnitude < targetHitRadius * targetHitRadius) {
            // TODO : get all creeps within an area, then attack them
//            attackComponent.DoAttack(target);
            Destroy(gameObject);
        }
        
        // Compute the next position, with arc added in
        float x0 = startPosition.x;
        float x1 = target.x;
        float dist = x1 - x0;
        float nextX = Mathf.MoveTowards(transform.position.x, x1, step);
        float baseY = Mathf.Lerp(startPosition.y, target.y, (nextX - x0) / dist);
        float arc = arcHeight * (nextX - x0) * (nextX - x1) / (coefDir * dist * dist);
        var nextPos = new Vector3(nextX, baseY + arc, transform.position.z);

        // Rotate to face the next position, and then move there
//        transform.rotation = LookAt2D(nextPos - transform.position);
        transform.right = (nextPos - transform.position).normalized;
        transform.position = nextPos;
		
//        // Do something when we reach the target
//        if (nextPos == targetPos) Arrived();
    }

    public void SetTarget(UnitStats target)
    {
        this.target = target.transform.position;
    }
    
    private void OnDrawGizmos()
    {
        var step = speed * Time.fixedDeltaTime; 

        var currentPosition = new Vector3(startPosition.x, startPosition.y, 0);
        while ((target - (Vector2) currentPosition).sqrMagnitude >= targetHitRadius * targetHitRadius) {
            Gizmos.DrawWireSphere(currentPosition, 0.025f);
            
            // Compute the next position, with arc added in
            float x0 = startPosition.x;
            float x1 = target.x;
            float dist = x1 - x0;
            float nextX = Mathf.MoveTowards(currentPosition.x, x1, step);
            float baseY = Mathf.Lerp(startPosition.y, target.y, (nextX - x0) / dist);
            float arc = arcHeight * (nextX - x0) * (nextX - x1) / (coefDir * dist * dist);
            var nextPos = new Vector3(nextX, baseY + arc, currentPosition.z);

            // Rotate to face the next position, and then move there
//        transform.rotation = LookAt2D(nextPos - transform.position);
            currentPosition = nextPos;
        }

    }
}
