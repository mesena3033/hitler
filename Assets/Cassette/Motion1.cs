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

    [Header("Animator 参照")]
    public Animator animator; // Animator をアサイン

    // 保留リクエスト（UseSkill からの同期リクエストを保持）
    private class PendingRequest
    {
        public int skillID1;
        public string stateName;
        public float normalizedTime;
        public int requestedAtStateHash = -1;
    }

    private List<PendingRequest> pendingRequests = new List<PendingRequest>();

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

    // 外部から直接プレハブを渡して生成したい場合に使える公開メソッド
    public void SpawnPrefab(GameObject prefab)
    {
        Spawn(prefab);
    }

    // スキルIDからエフェクトを再生する（即時）。往来互換用。
    public void SpawnEffectForSkill(int skillID1)
    {
        if (skillID1 >= 0 && skillID1 < effectPrefabs.Count && effectPrefabs[skillID1] != null)
        {
            Spawn(effectPrefabs[skillID1]);
        }
    }

    // UseSkill から呼ばれて、アニメーションの指定フレーム（normalizedTime）で再生したいときに使う。
    // stateName を指定するとそのステートに入ったタイミングで監視し、normalizedTime を超えたら一度だけ再生します。
    // stateName が空文字の場合は即時再生します.
    public void RequestSpawnForSkill(int skillID1, string stateName, float normalizedTime)
    {
        if (string.IsNullOrEmpty(stateName))
        {
            // state 指定無しなら即時再生
            SpawnEffectForSkill(skillID1);
            return;
        }

        // 既に同じリクエストが存在していれば追加しない
        for (int i = 0; i < pendingRequests.Count; i++)
        {
            var r = pendingRequests[i];
            if (r.skillID1 == skillID1 && r.stateName == stateName && Mathf.Approximately(r.normalizedTime, normalizedTime))
            {
                return;
            }
        }

        var req = new PendingRequest { skillID1 = skillID1, stateName = stateName, normalizedTime = Mathf.Clamp01(normalizedTime) };

        if (animator != null)
        {
            var state = animator.GetCurrentAnimatorStateInfo(0);
            if (state.IsName(stateName) && state.normalizedTime >= req.normalizedTime)
            {
                req.requestedAtStateHash = state.shortNameHash;
            }
            else
            {
                req.requestedAtStateHash = -1;
            }
        }

        pendingRequests.Add(req);
    }

    // Animator を外部から一度だけ設定するための API
    public void SetAnimator(Animator a)
    {
        animator = a;
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
        // pendingRequests を監視して Animator が一致したらエフェクトを再生
        if (animator != null && pendingRequests.Count > 0)
        {
            var state = animator.GetCurrentAnimatorStateInfo(0);
            // 複数ある場合は後ろから走査して削除しやすくする
            for (int i = pendingRequests.Count - 1; i >= 0; i--)
            {
                var r = pendingRequests[i];
                if (string.IsNullOrEmpty(r.stateName))
                {
                    // 即時再生
                    SpawnEffectForSkill(r.skillID1);
                    pendingRequests.RemoveAt(i);
                    continue;
                }

                // リクエスト作成時に既に同ステートで閾値を超えていた場合は再入場を要求
                if (r.requestedAtStateHash != -1)
                {
                    if (state.shortNameHash != r.requestedAtStateHash && state.IsName(r.stateName) && state.normalizedTime >= r.normalizedTime)
                    {
                        SpawnEffectForSkill(r.skillID1);
                        pendingRequests.RemoveAt(i);
                    }
                }
                else
                {
                    if (state.IsName(r.stateName) && state.normalizedTime >= r.normalizedTime)
                    {
                        SpawnEffectForSkill(r.skillID1);
                        pendingRequests.RemoveAt(i);
                    }
                }
            }
        }
    }
}
