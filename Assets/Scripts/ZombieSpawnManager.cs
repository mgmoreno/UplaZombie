using UnityEngine;

public class ZombieSpawnManager : MonoBehaviour
{
    [SerializeField] private Zombie zombiePrefab;
    [SerializeField] private ZombieSpawnPoint[] spawnPoints;
    [SerializeField] private float spawnTime = 4;

    private float _spawnTimer;

    private void Awake()
    {
        _spawnTimer = spawnTime;
    }

    private void Update()
    {
        _spawnTimer -= Time.deltaTime;

        if(_spawnTimer <= 0)
        {
            SpawnZombie();
            _spawnTimer = spawnTime;
        }
    }

    private void SpawnZombie()
    {
        ZombieSpawnPoint tomb = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Zombie zombie = GameObject.Instantiate(zombiePrefab, tomb.SpawnPoint.position, tomb.transform.rotation);
    }

}
