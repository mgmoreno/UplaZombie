using UnityEngine;

public class ZombieSpawnPoint : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject[] tombstones;

    public Transform SpawnPoint => spawnPoint;

    [ContextMenu("SelectTombstone")]
    private void SelectTombstone()
    {
        foreach (var tomb in tombstones)
        {
            tomb.SetActive(false);
        }

        tombstones[Random.Range(0, tombstones.Length)].SetActive(true);
        transform.Rotate(0, Random.Range(0,360), 0);
    }
}
