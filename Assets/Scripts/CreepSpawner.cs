using UnityEngine;

public class CreepSpawner : MonoBehaviour
{
    [SerializeField] private WaypointManager waypointManager;
    [SerializeField] private CreepManager creepManager;
    [SerializeField] private CreepWave[] waves;
    [SerializeField] private float timeBetweenWaves = 5f;

    private int currentWaveIndex;
    private float currentCooldownCreep;
    private int countWaveSpawnedCreeps;

    private float currentCooldownWave;

    private bool launched = false;

    private void Awake()
    {
        waypointManager = FindObjectOfType<WaypointManager>();
        creepManager = FindObjectOfType<CreepManager>();
    }

    private void Start()
    {
        Launch();
    }

    public void Launch()
    {
        currentWaveIndex = 0;
        countWaveSpawnedCreeps = 0;
        launched = true;
    }

    private void Update()
    {
        if (!launched) {
            return;
        }

        if (currentWaveIndex >= waves.Length) {
            // TODO : WIN ?
            return;
        }
        
        var wave = waves[currentWaveIndex];
        if (countWaveSpawnedCreeps < wave.countCreeps) {
            if (currentCooldownCreep > 0) currentCooldownCreep -= Time.deltaTime;
            if (currentCooldownCreep <= 0) {
                currentCooldownCreep = wave.timeBetweenSpawns;
                var creep = Instantiate(wave.creepPrefab, waypointManager.GetStart().position, Quaternion.identity);
                creepManager.AddCreep(creep);

                countWaveSpawnedCreeps++;
                if (countWaveSpawnedCreeps >= wave.countCreeps) {
                    currentCooldownWave = timeBetweenWaves;
                }
            }
        }
        else {
            if (currentCooldownWave > 0) currentCooldownWave -= Time.deltaTime;
            if (currentCooldownWave <= 0) {
                currentWaveIndex++;
                countWaveSpawnedCreeps = 0;
                currentCooldownCreep = 0;
            }
        }
    }
}
