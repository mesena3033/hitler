using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemy;

    // スポーン地点
    [SerializeField] private Transform[] stage1spawnPoints;
    [SerializeField] private Transform[] stage2spawnPoints;
    [SerializeField] private Transform[] stage3spawnPoints;

    [SerializeField] private StageManager stageManager;

    public void EnemySpawn(int wave)
    {
        Transform[] currentSpawnPoints = null;
        switch(stageManager.CurrentStage)
        {
            case 0:
                currentSpawnPoints = stage1spawnPoints;
                break;
            case 1:
                currentSpawnPoints = stage2spawnPoints;
                break;
            case 2:
                currentSpawnPoints = stage3spawnPoints;
                break;
            default:
                Debug.LogError("Invalid stage index");
                return;
        }

        // スポーン
        foreach (Transform spawnPoint in currentSpawnPoints)
        {
            // SpawnPoint確認
            if (spawnPoint == null)
            {
                Debug.LogError($"Wave {wave + 1} スポーン地点がない");
                continue;
            }


            if (wave < 0 || wave >= currentSpawnPoints.Length)
            {
                return;
            }


            // SpawnPoint確認
            if (spawnPoint == null)
            {
                Debug.LogError($"Wave {wave + 1} スポーン地点がない");
                return;
            }

            Debug.Log($"Wave {wave + 1} / " + $"SpawnPoint[{wave}] / "
                + $"位置 = {spawnPoint.position}");

            NavMeshHit hit;

            if (NavMesh.SamplePosition
                (spawnPoint.position, out hit, 2.0f, NavMesh.AllAreas))
            {
                GameObject obj =
                    Instantiate(enemy, hit.position, spawnPoint.rotation);


                Debug.Log($"Wave {wave + 1} Spawn位置: {hit.position}");

                NavMeshAgent agent = obj.GetComponent<NavMeshAgent>();

                if (agent != null)
                {
                    {
                        Debug.Log($"isOnNavMesh = {agent.isOnNavMesh}");
                    }
                }

            }

            else
            {
                Debug.LogError($"Spawnpoint[{wave}]の位置にNavMeshがない");
            }
        }
    }

}