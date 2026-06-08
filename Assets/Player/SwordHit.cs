using UnityEngine;

[RequireComponent(typeof(Collider))]
public class SwordHit : MonoBehaviour
{
    private PlayerAttack owner;

    public void Init(PlayerAttack ownerAttack)
    {
        owner = ownerAttack;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (owner == null) return;

        owner.OnSwordHit(other);
    }
}
