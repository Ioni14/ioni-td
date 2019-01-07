using UnityEngine;

public class ProjectileTower : BasicTower
{
    [SerializeField] private float baseRange = 3f;
    [SerializeField] private float baseAttackCooldown = 1f;
    [SerializeField] private float damagePerAttack = 10f;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;

    private UnitStats unitStatsComponent;

    private UnitStats target;
    private float attackCooldown;

    protected override void Awake()
    {
        base.Awake();
        unitStatsComponent = GetComponent<UnitStats>();
    }

    private void Update()
    {
        if (!isActive) {
            return;
        }

        if (attackCooldown > 0) attackCooldown -= Time.deltaTime;

        if (target && CanAttackCreep(target.gameObject)) {
            AttackTarget();
            return;
        }

        // switch target because we can't attack anymore
        target = PickCreepAtRange();
        if (target) {
            AttackTarget();
        }
    }

    private UnitStats PickCreepAtRange()
    {
        // Get first creep at range
        foreach (var creep in creepManager.GetCreeps()) {
            if (CanAttackCreep(creep)) {
                return creep.GetComponent<UnitStats>();
            }
        }

        return null;
    }

    private bool CanAttackCreep(GameObject creep)
    {
        var distanceToCreep = Vector2.Distance(firePoint.position, creep.transform.position);

        return distanceToCreep <= baseRange;
    }

    private void AttackTarget()
    {
        if (attackCooldown > 0) return;

        attackCooldown = baseAttackCooldown;

        var projectile = Instantiate(
            projectilePrefab,
            firePoint.position,
            Quaternion.FromToRotation(Vector3.right, target.transform.position - firePoint.position) // towards target
        );
        
        // TODO : refactor this shit
        if (projectile.GetComponent<ProjectileMover>()) {
            projectile.GetComponent<ProjectileMover>().SetTarget(target);
        }
        if (projectile.GetComponent<SplashMover>()) {
            projectile.GetComponent<SplashMover>().SetTarget(target);
        }
        var projectileAttack = projectile.GetComponent<Attack>();
        projectileAttack.SetSourceStats(unitStatsComponent.CreateSnapshotAttackStats());
        projectileAttack.SetBaseDamage(damagePerAttack);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(firePoint.position, baseRange);
        if (target) {
            Gizmos.DrawLine(firePoint.position, target.transform.position);
        }
    }
}
