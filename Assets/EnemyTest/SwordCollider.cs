using UnityEngine;

public class SwordCollider : MonoBehaviour
{
    [SerializeField] Collider col;

    void Start()
    {
        col.enabled = false;
    }

    public void EnableSword()
    {
        col.enabled = true;
    }

    public void DisableSword()
    {
        col.enabled = false;
    }
}
