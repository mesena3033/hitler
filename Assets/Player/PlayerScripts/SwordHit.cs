using UnityEngine;

[RequireComponent(typeof(Collider))]
public class SwordHit : MonoBehaviour
{
    // 剣オブジェクトにアタッチ
    private PlayerAttack owner;

    public void Init(PlayerAttack ownerAttack)
    {
        owner = ownerAttack;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (owner == null) return;

        if (other.CompareTag("Object"))
        {
            owner.DisableSword();
        }

        owner.OnSwordHit(other);
    }

}
