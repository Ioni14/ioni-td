using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMover : MonoBehaviour
{
    [SerializeField] private float baseSpeed;
    [SerializeField] private float baseDamage;
    
    private GameObject target;
    private Health targetHealth;
    
    private float currentSpeed;
    
    // Start is called before the first frame update
    void Start()
    {
        currentSpeed = baseSpeed;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (!target) {
            // became useless
            Destroy(gameObject);
            return;
        }
        var step = currentSpeed * Time.fixedDeltaTime;

        transform.right = (target.transform.position - transform.position).normalized;
        transform.Translate(Vector2.right * step);
        
        // close to target ?
        if (Vector2.Distance(target.transform.position, transform.position) < 0.5) {
            targetHealth.ReceiveDamage(ComputeOutDamage());
            Destroy(gameObject);
        }
    }

    public void SetTarget(GameObject target)
    {
        this.target = target;
        targetHealth = target.GetComponent<Health>();
    }

    private float ComputeOutDamage()
    {
        // TODO : apply bonus damages
        return baseDamage;
    }
    
    // TODO
    public void SetBonusDamage()
    {
        
    }
}
