using UnityEngine;

public class ArcherTower : MonoBehaviour
{
    [SerializeField] private float baseRange = 3f;
    [SerializeField] private float baseAttackCooldown = 1f;
    [SerializeField] private ProjectileMover projectilePrefab;

    private CreepManager creepManager;

    private GameObject target;
    private float attackCooldown;

    private void Awake()
    {
        creepManager = FindObjectOfType<CreepManager>();
    }

    private void Update()
    {
        if (attackCooldown > 0)
            attackCooldown -= Time.deltaTime;

        if (target) {
            var distanceToCreep = Vector2.Distance(transform.position, target.transform.position);
            if (distanceToCreep <= baseRange) {
                AttackTarget();
                return;
            }

            Debug.Log(name + " lost target");
        }

        target = PickCreepAtRange();
        if (target) {
            Debug.Log(name + " changes target : " + target.name);
            AttackTarget();
        }
    }

    private GameObject PickCreepAtRange()
    {
        // Get first creep at range
        foreach (var creep in creepManager.GetCreeps()) {
            var distanceToCreep = Vector2.Distance(transform.position, creep.transform.position);
            if (distanceToCreep <= baseRange) {
                return creep;
            }
        }

        return null;
    }

    private void AttackTarget()
    {
        if (attackCooldown > 0) {
            return;
        }

        attackCooldown = baseAttackCooldown;
        Debug.Log(name + " attacks creep : " + target.name);

        var projectile = Instantiate(
            projectilePrefab,
            transform.position,
            Quaternion.FromToRotation(Vector3.right, target.transform.position - transform.position)
        );
        projectile.SetTarget(target);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, baseRange);
        if (target) {
            Gizmos.DrawLine(transform.position, target.transform.position);
        }
    }
}
