using System.Collections.Generic;
using UnityEngine;

public class UnitStats : MonoBehaviour
{
    private List<Buff> buffs = new List<Buff>();

    [SerializeField] private int maxHealth;
    private int currentHealth;

    [SerializeField] private float maxSpeed;
    private float currentSpeed;

    [SerializeField] private float strengh;
    [SerializeField] private float intelligence;

    [SerializeField] private float percentCritic; // [0, 1]
    [SerializeField] private float damageCritic; // percentage (e.g. 10 dmg with 2.5 dmgCritic => 10 * (1 + 250%) = 35)

    [SerializeField] private float bonusDmgPhysic;
    [SerializeField] private float bonusDmgFire;
    [SerializeField] private float bonusDmgIce;
    [SerializeField] private float bonusDmgShadow;
    [SerializeField] private float bonusDmgLight;
    [SerializeField] private float bonusDmgNature;

    [SerializeField] private float resistPhysic;
    [SerializeField] private float resistFire;
    [SerializeField] private float resistIce;
    [SerializeField] private float resistShadow;
    [SerializeField] private float resistLight;
    [SerializeField] private float resistNature;

    private Dictionary<Damage.Type, float> bonusDamages = new Dictionary<Damage.Type, float>(); // percentages
    private Dictionary<Damage.Type, float> resistances = new Dictionary<Damage.Type, float>(); // percentages

    private List<IStatsObserver> viewers = new List<IStatsObserver>();

    private void Start()
    {
        currentHealth = maxHealth;
        currentSpeed = maxSpeed;

        bonusDamages[Damage.Type.Physical] = bonusDmgPhysic;
        bonusDamages[Damage.Type.Fire] = bonusDmgFire;
        bonusDamages[Damage.Type.Ice] = bonusDmgIce;
        bonusDamages[Damage.Type.Shadow] = bonusDmgShadow;
        bonusDamages[Damage.Type.Light] = bonusDmgLight;
        bonusDamages[Damage.Type.Nature] = bonusDmgNature;

        resistances[Damage.Type.Physical] = resistPhysic;
        resistances[Damage.Type.Fire] = resistFire;
        resistances[Damage.Type.Ice] = resistIce;
        resistances[Damage.Type.Shadow] = resistShadow;
        resistances[Damage.Type.Light] = resistLight;
        resistances[Damage.Type.Nature] = resistNature;

        FireStatsChanged();
    }

    private void FireStatsChanged()
    {
        foreach (var viewer in viewers) {
            viewer.OnStatsChanged(this);
        }
    }

    public void SubscribeViewer(IStatsObserver observer)
    {
        viewers.Add(observer);
    }

    public IEnumerable<Buff> GetBuffs()
    {
        return buffs;
    }

    public void ReceiveDamage(Damage damage)
    {
        var effectiveDamage = ComputeWithReduceDamage(damage);
        currentHealth -= Mathf.Max(0, Mathf.RoundToInt(effectiveDamage));
        currentHealth = Mathf.Max(0, currentHealth);
        FireStatsChanged();
    }

    private float ComputeWithReduceDamage(Damage damage)
    {
        return damage.amount / (1 + resistances[damage.type]);
    }

    public AttackStats CreateSnapshotAttackStats()
    {
        return new AttackStats(strengh, intelligence, percentCritic, damageCritic, bonusDamages);
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public float GetCurrentSpeed()
    {
        return currentSpeed;
    }

    public bool IsDead()
    {
        return currentHealth <= 0;
    }
}

public struct AttackStats
{
    public float strengh;
    public float intelligence;

    public float percentCritic; // [0, 1]
    public float damageCritic; // percentage (e.g. 10 dmg with 2.5 dmgCritic => 10 * (1 + 250%) = 35)

    public Dictionary<Damage.Type, float> bonusDamages; // percentages

    public float GetRealDamage(float baseDamage, Damage.Type type)
    {
        if (type == Damage.Type.Physical) {
            return baseDamage * (1 + bonusDamages[type]) + strengh / 5f;
        }

        return baseDamage * (1 + bonusDamages[type]) + intelligence / 5f;
    }

    public float CalculateDamageCritic(float damage)
    {
        return damage * damageCritic;
    }

    public AttackStats(
        float strengh,
        float intelligence,
        float percentCritic,
        float damageCritic,
        Dictionary<Damage.Type, float> bonusDamages)
    {
        this.strengh = strengh;
        this.intelligence = intelligence;
        this.percentCritic = percentCritic;
        this.damageCritic = damageCritic;

        this.bonusDamages = new Dictionary<Damage.Type, float>();
        this.bonusDamages[Damage.Type.Physical] = 0;
        this.bonusDamages[Damage.Type.Fire] = 0;
        this.bonusDamages[Damage.Type.Ice] = 0;
        this.bonusDamages[Damage.Type.Shadow] = 0;
        this.bonusDamages[Damage.Type.Light] = 0;
        this.bonusDamages[Damage.Type.Nature] = 0;
        foreach (var bonusDamage in bonusDamages) {
            this.bonusDamages[bonusDamage.Key] = bonusDamage.Value;
        }
    }
}

public struct Damage
{
    public enum Type
    {
        Physical,
        Fire,
        Ice,
        Shadow,
        Light,
        Nature,
    }

    public float amount;
    public Type type;

    public Damage(float amount, Type type = Type.Physical)
    {
        this.amount = amount;
        this.type = type;
    }
}
