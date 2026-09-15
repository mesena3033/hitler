using UnityEngine;

public class HitEffect : MonoBehaviour
{
    // hitエフェクト
    [SerializeField] private GameObject hitEffectPrefab;
    

    public void PlayHitEffect(Vector3 position)
    {
        if (hitEffectPrefab != null)
        {
            position.y += 3.0f; // エフェクトの高さを調整
            GameObject hitEffect = Instantiate(hitEffectPrefab, position, Quaternion.identity);
            Destroy(hitEffect, 1f); // エフェクトを1秒後に破棄
        }
    }

}
