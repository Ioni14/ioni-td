using UnityEngine;

public class SplashMover : MonoBehaviour
{
    [SerializeField] private float speed = 20f;
    [SerializeField] private float targetHitRadius = 0.5f;
    [SerializeField] private float areaEffectRadius = 2f;

    private CreepManager creepManager;
    
    private Attack attackComponent;
    private Vector2 target;

    private void Awake()
    {
        creepManager = FindObjectOfType<CreepManager>();
        attackComponent = GetComponent<Attack>();
    }

    private void FixedUpdate()
    {
        var step = speed * Time.fixedDeltaTime;

        transform.right = (target - (Vector2) transform.position).normalized;
        transform.Translate(Vector2.right * step);

        // close to target ?
        if ((target - (Vector2) transform.position).sqrMagnitude < targetHitRadius * targetHitRadius) {
//            attackComponent.DoAttack(target);

            foreach (var creep in creepManager.GetCreeps()) {
                if (Vector2.Distance(creep.transform.position, target) <= areaEffectRadius) {
                    attackComponent.DoAttack(creep.GetComponent<UnitStats>());
                }
            }

            Destroy(gameObject);
        }
    }

    public void SetTarget(UnitStats target)
    {
        this.target = target.transform.position;
    }
}
