using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth;

    [SerializeField] private int currentHealth;

    private List<HealthViewer> viewers = new List<HealthViewer>();

    private void Start()
    {
        currentHealth = maxHealth;
        FireHealthChanged();
    }

    public int getMaxHealth()
    {
        return maxHealth;
    }

    public int getCurrentHealth()
    {
        return currentHealth;
    }

    public float getPercentHealth()
    {
        return currentHealth / (float) maxHealth;
    }

    public void SubscribeViewer(HealthViewer viewer)
    {
        viewers.Add(viewer);
    }

    private void FireHealthChanged()
    {
        foreach (var viewer in viewers) {
            viewer.OnHealthChanged(this);
        }
    }

    public void ReceiveDamage(float damage)
    {
        // TODO : delegate to an other component for stats computations ?
        
        // TODO : apply damage reductions (en fonction des types de dégats ? composite/fire/ice...)
        
        currentHealth = Mathf.Max(0, currentHealth - (int) damage);
        FireHealthChanged();

        if (currentHealth <= 0) {
            // TODO : do something before Destroy ? VFX, sound, death animation
            
            // TODO : change SubscribeViewer with an Interface HealthObserver (implemented by HealthViewer and CreepMover)
            // TODO : CreepMover manages the death of the Creep
            Destroy(gameObject);
        }
    }
}
