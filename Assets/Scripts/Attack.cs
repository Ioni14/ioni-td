using UnityEngine;
using Random = System.Random;

public class Attack : MonoBehaviour
{
    private Random random = new Random();

    private float baseDamage;
    private AttackStats sourceStats;

    public void SetBaseDamage(float baseDamage)
    {
        this.baseDamage = baseDamage;
    }

    public void SetSourceStats(AttackStats stats)
    {
        sourceStats = stats;
    }

    public void DoAttack(UnitStats stats)
    {
        stats.ReceiveDamage(CalculateDamage());
    }

    private Damage CalculateDamage()
    {
        var physicalDmg = sourceStats.GetRealDamage(baseDamage, Damage.Type.Physical);
        var rand = random.NextDouble(); // [0.0, 1.0[
        if (sourceStats.percentCritic > 0 && rand <= sourceStats.percentCritic) {
            physicalDmg += sourceStats.CalculateDamageCritic(physicalDmg);
        }

        return new Damage(physicalDmg, Damage.Type.Physical);
    }
}
