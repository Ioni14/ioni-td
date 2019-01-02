using UnityEngine;

public class ArcherTower : BasicTower
{
    [SerializeField] private float baseRange = 3f;
    [SerializeField] private float baseAttackCooldown = 1f;
    [SerializeField] private ProjectileMover projectilePrefab;

    private GameObject target;
    private float attackCooldown;

    private void Update()
    {
        if (!isActive) {
            return;
        }

        if (attackCooldown > 0)
            attackCooldown -= Time.deltaTime;

        if (target) {
            if (CanAttackCreep(target)) {
                AttackTarget();
                return;
            }

//            Debug.Log(name + " lost target");
        }

        target = PickCreepAtRange();
        if (target) {
//            Debug.Log(name + " changes target : " + target.name);
            AttackTarget();
        }
    }

    private GameObject PickCreepAtRange()
    {
        // Get first creep at range
        foreach (var creep in creepManager.GetCreeps()) {
            if (CanAttackCreep(creep)) {
                return creep;
            }
        }

        return null;
    }

    private bool CanAttackCreep(GameObject creep)
    {
        var distanceToCreep = Vector2.Distance(transform.position, creep.transform.position);

        return distanceToCreep <= baseRange;
    }

    private void AttackTarget()
    {
        if (attackCooldown > 0) {
            return;
        }

        attackCooldown = baseAttackCooldown;
//        Debug.Log(name + " attacks creep : " + target.name);

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
