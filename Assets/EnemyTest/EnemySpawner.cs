using NUnit.Framework.Interfaces;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemy;

    // スポーン地点
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private StageManager stageManager;

    public void EnemySpawn(int wave)
    {
        if(spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError($"Wave {wave} スポーン地点が設定されていません");
            return;
        }

        // スポーン
        foreach (Transform spawnPoint in spawnPoints)
        {
            // SpawnPoint確認
            if (spawnPoint == null)
            {
                //Debug.LogError($"Wave {wave} スポーン地点がない");
                continue;
            }


            NavMeshHit hit;

            if (NavMesh.SamplePosition
                (spawnPoint.position, out hit, 2.0f, NavMesh.AllAreas))
            {
                GameObject obj =
                    Instantiate(enemy, hit.position, spawnPoint.rotation);


                //Debug.Log($"Wave {wave + 1} Spawn位置: {hit.position}");

                NavMeshAgent agent = obj.GetComponent<NavMeshAgent>();

                if (agent != null)
                {
                    {
                        //Debug.Log($"isOnNavMesh = {agent.isOnNavMesh}");
                    }
                }

            }

            else
            {
                {
                    Debug.LogError(
                        $"SpawnPointの位置にNavMeshがありません: {spawnPoint.position}"
                    );
                }
            }

            
        }
    }
}