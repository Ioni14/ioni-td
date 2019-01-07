using UnityEngine;

public class ProjectileMover : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float targetHitRadius = 0.5f;

    private Attack attackComponent;
    private UnitStats target;

    private void Awake()
    {
        attackComponent = GetComponent<Attack>();
    }

    private void FixedUpdate()
    {
        if (!target) {
            // became useless
            Destroy(gameObject);
            return;
        }

        var step = speed * Time.fixedDeltaTime;

        transform.right = (target.transform.position - transform.position).normalized;
        transform.Translate(Vector2.right * step);

        // close to target ?
        if ((target.transform.position - transform.position).sqrMagnitude < targetHitRadius * targetHitRadius) {
            attackComponent.DoAttack(target);
            Destroy(gameObject);
        }
    }

    public void SetTarget(UnitStats target)
    {
        this.target = target;
    }
}