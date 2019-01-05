using UnityEngine;

[CreateAssetMenu(menuName = "Creep Wave")]
public class CreepWave : ScriptableObject
{
    [Tooltip("In seconds.")] public float timeBetweenSpawns;
    public int countCreeps;
    public GameObject creepPrefab;
}