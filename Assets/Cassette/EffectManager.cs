using System.Collections;
using UnityEngine;

// エフェクト発動を管理するクラス
public class EffectManager : MonoBehaviour
{
    private Coroutine spawnCoroutine;

    public void SpawnEffect(
       GameObject prefab,
       Transform spawnPoint,
       float delay,
       System.Action<GameObject> onSpawn)
    {
        // 前回の生成待機が残っていたら止める
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
        }

        spawnCoroutine =
        StartCoroutine(
            SpawnRoutine(
                prefab,
                spawnPoint,
                delay,
                onSpawn));
    }

    private IEnumerator SpawnRoutine(
        GameObject prefab,
        Transform spawnPoint,
        float delay,
        System.Action<GameObject> onSpawn)
    {
        yield return new WaitForSeconds(delay);

        if (prefab == null)
            yield break;

        Vector3 pos =
            spawnPoint != null ?
            spawnPoint.position :
            transform.position;

        Quaternion rot =
            spawnPoint != null ?
            spawnPoint.rotation :
            Quaternion.identity;

        GameObject obj =
            Instantiate(prefab, pos, rot);

        onSpawn?.Invoke(obj);

        spawnCoroutine = null;
    }

    public void CancelSpawn()
    {
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }
    }
}