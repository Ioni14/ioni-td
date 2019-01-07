using UnityEngine;

public class FireDotDebuff : Buff
{
    private AttackStats sourceStats;
    private float damagePerTick;
    private float timeBetweenTicks;
    private float maxDuration;

    private float currentCooldown;
    private float duration;

    public FireDotDebuff(AttackStats sourceStats, float damagePerTick, float timeBetweenTicks, float maxDuration)
    {
        harmful = true;
        this.sourceStats = sourceStats;
        this.damagePerTick = damagePerTick;
        this.timeBetweenTicks = timeBetweenTicks;
        this.maxDuration = maxDuration;
        duration = 0;
    }

    public bool IsFinished()
    {
        return duration >= maxDuration;
    }

    public override void Update(UnitStats stats)
    {
        duration += Time.deltaTime;
        if (IsFinished()) {
            return;
        }

        if (currentCooldown > 0) currentCooldown = timeBetweenTicks * Time.deltaTime;

        if (currentCooldown <= 0) {
            stats.ReceiveDamage(CalculateDamage());
        }
    }

    private Damage CalculateDamage()
    {
        var fireDmg = sourceStats.GetRealDamage(damagePerTick, Damage.Type.Fire);
        var rand = random.NextDouble(); // [0.0, 1.0[
        if (sourceStats.percentCritic > 0 && rand <= sourceStats.percentCritic) {
            fireDmg += sourceStats.CalculateDamageCritic(fireDmg);
        }

        return new Damage(fireDmg, Damage.Type.Fire);
    }
}
