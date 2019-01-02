using UnityEngine;

public class HealthViewer : MonoBehaviour, IHealthObserver
{
    [SerializeField] private HealthBar bar;

    private Health healthComponent;

    private void Awake()
    {
        var trans = transform;
        while (trans.parent) {
            var healthCompo = trans.parent.GetComponent<Health>();
            if (healthCompo) {
                healthComponent = healthCompo;
                break;
            }

            trans = trans.parent;
        }

        if (!healthComponent) {
            Destroy(this);
        }

        healthComponent.SubscribeViewer(this);
    }

    public void OnHealthChanged(Health health)
    {
        bar.UpdateSize(health.getPercentHealth());
    }
}
