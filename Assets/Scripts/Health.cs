using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth;

    [SerializeField] private int currentHealth;

    private List<IHealthObserver> viewers = new List<IHealthObserver>();

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

    public void SubscribeViewer(IHealthObserver viewer)
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
    }

    public bool IsDead()
    {
        return currentHealth <= 0;
    }
}
