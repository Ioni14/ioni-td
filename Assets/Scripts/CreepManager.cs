using System.Collections.Generic;
using UnityEngine;

public class CreepManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> creeps = new List<GameObject>();

    private List<GameObject> creepsToDestroy = new List<GameObject>();
    
    private void Start()
    {
        // Only if there are creeps already spawned
        foreach (var creep in FindObjectsOfType<CreepMover>()) {
            creeps.Add(creep.gameObject);
        }
    }

    public IEnumerable<GameObject> GetCreeps()
    {
        return creeps;
    }

    public void AddCreep(GameObject creep)
    {
        creeps.Add(creep);
    }

    private void LateUpdate()
    {
        foreach (var creep in creepsToDestroy) {
            creeps.Remove(creep);
            Destroy(creep); // TODO : delay this
        }
    }

    public void RemoveCreep(GameObject creep)
    {
        creepsToDestroy.Add(creep);
    }
}
