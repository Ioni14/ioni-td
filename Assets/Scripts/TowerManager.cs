using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    [SerializeField] private List<BasicTower> towers = new List<BasicTower>();

    private void Start()
    {
        // Only if there are creeps already spawned
        foreach (var tower in FindObjectsOfType<BasicTower>())
        {
            towers.Add(tower);
        }
    }
    
    public void AddTower(BasicTower tower)
    {
        towers.Add(tower);
    }

    public void RemoveTower(BasicTower tower)
    {
        towers.Remove(tower);
        Destroy(tower);
    }

    public IEnumerable<BasicTower> GetTowers()
    {
        return towers;
    }
}
