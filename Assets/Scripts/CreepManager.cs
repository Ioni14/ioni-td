using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreepManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> creeps = new List<GameObject>();

    private void Start()
    {
        // Only if there are creeps already spawned
        foreach (var creep in FindObjectsOfType<CreepMover>())
        {
            creeps.Add(creep.gameObject);
        }
    }

    public IEnumerable<GameObject> GetCreeps()
    {
        return creeps;
    }

    public void RemoveCreep(GameObject creep)
    {
        creeps.Remove(creep);
        Destroy(creep);
    }
}
