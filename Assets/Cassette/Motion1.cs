using System.Collections.Generic;
using UnityEngine;

public class Motion1 : MonoBehaviour
{
    [Header("エフェクト一覧")]
    public List<GameObject> effectPrefabs = new List<GameObject>();

    [Header("出現位置")]
    public Transform effectSpawnPoint; // 未設定なら自身の位置を使う

    [Header("自動破棄")]
    public float destroyAfter = 5; // 生成したエフェクトを何秒後に破棄するか

    [Header("Animator監視（任意）")]
    public Animator animator;                 // Animator をアサイン（未設定でもOK）
    public string watchStateName = "";        // 監視するステート名（例: "Attack"）
    [Range(0f, 1f)]
    public float playAtNormalizedTime = 0.1f; // 何%時点で再生するか
    private int lastStateHash = 0;
    private bool effectPlayedForState = false;

    // --------------------
    // Animation Event から呼べるメソッド（整数パラメータ）
    // Animation Event に登録する関数名は "SpawnEffectByIndex" にする
    public void SpawnEffectByIndex(int index)
    {
        if (index < 0 || index >= effectPrefabs.Count) return;
        Spawn(effectPrefabs[index]);
    }

    // Animation Event から呼べるメソッド（文字列パラメータ）
    // クリップごとに名前で管理したい場合に使える
    public void SpawnEffectByName(string prefabName)
    {
        if (string.IsNullOrEmpty(prefabName)) return;
        for (int i = 0; i < effectPrefabs.Count; i++)
        {
            if (effectPrefabs[i] != null && effectPrefabs[i].name == prefabName)
            {
                Spawn(effectPrefabs[i]);
                return;
            }
        }
    }

    // 任意に呼べるユーティリティ（ランダム再生）
    public void SpawnRandom()
    {
        if (effectPrefabs.Count == 0) return;
        int idx = Random.Range(0, effectPrefabs.Count);
        Spawn(effectPrefabs[idx]);
    }

    // 実際の生成処理（内部）
    private void Spawn(GameObject prefab)
    {
        if (prefab == null) return;
        // 出現位置と回転を決定
        Vector3 pos = effectSpawnPoint != null ? effectSpawnPoint.position : transform.position;
        // 回転はエフェクト側で調整することが多いと思うので
        Quaternion rot = effectSpawnPoint != null ? effectSpawnPoint.rotation : Quaternion.identity;
        // エフェクト生成
        GameObject go = Instantiate(prefab, pos, rot);
        // destroyAfter > 0 のときだけ破棄予約する
        if (destroyAfter > 0f) Destroy(go, destroyAfter);
    }

    void Update()
    {
        
        // Animator監視でステート到達時に一回だけエフェクトを出す（必要なら）
        if (animator == null || string.IsNullOrEmpty(watchStateName)) return;

        var state = animator.GetCurrentAnimatorStateInfo(0);
        if (state.shortNameHash != lastStateHash)
        {
            effectPlayedForState = false;
        }
        lastStateHash = state.shortNameHash;

        if (!effectPlayedForState && state.IsName(watchStateName) && state.normalizedTime >= playAtNormalizedTime)
        {
            SpawnRandom(); //特定インデックスにしたければ SpawnEffectByIndex を直接呼ぶ
            effectPlayedForState = true;
        }
    }
}
