using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemy;
    [SerializeField] private Transform spawnPoint;

    NavMeshHit hit;
    public void EnemySpawn()
    {
        if (NavMesh.SamplePosition(spawnPoint.position, out hit, 2f, NavMesh.AllAreas))
        {
            GameObject obj = Instantiate(enemy, hit.position, spawnPoint.rotation);

            Debug.Log($"SpawnˆÊ’u: {hit.position}");
            Debug.Log($"isOnNavMesh = {obj.GetComponent<NavMeshAgent>().isOnNavMesh}");
        }
    }
}
