using UnityEngine;

public class HealthViewer : MonoBehaviour, IStatsObserver
{
    [SerializeField] private HealthBar bar;

//    private Health healthComponent;
    private UnitStats unitStatsComponent;

    private void Awake()
    {
        var trans = transform;
        while (trans.parent) {
            var unitStatsCompo = trans.parent.GetComponent<UnitStats>();
            if (unitStatsCompo) {
                unitStatsComponent = unitStatsCompo;
                break;
            }

            trans = trans.parent;
        }

        if (!unitStatsComponent) {
            Destroy(this);
        }
        unitStatsComponent.SubscribeViewer(this);
    }

    public void OnStatsChanged(UnitStats stats)
    {
        bar.UpdateSize(stats.GetCurrentHealth() / (float) stats.GetMaxHealth());
    }
}
