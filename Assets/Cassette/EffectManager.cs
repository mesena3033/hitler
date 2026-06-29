using System.Collections;
using UnityEngine;

// エフェクト発動を管理するクラス
public class EffectManager : MonoBehaviour
{
    [SerializeField]
    private float destroyTime = 5f;

    public void SpawnEffect(
        GameObject prefab,
        Transform spawnPoint,
        float delay
    )
    {
        StartCoroutine(SpawnRoutine(prefab, spawnPoint, delay));
    }

    private IEnumerator SpawnRoutine(
        GameObject prefab,
        Transform spawnPoint,
        float delay
    )
    {
        yield return new WaitForSeconds(delay);

        if (prefab == null) yield break;

        Vector3 pos =
            spawnPoint != null
                ? spawnPoint.position
                : transform.position;

        Quaternion rot =
            spawnPoint != null
                ? spawnPoint.rotation
                : Quaternion.identity;

        GameObject obj =
            Instantiate(prefab, pos, rot);

        Destroy(obj, destroyTime);
    }
}