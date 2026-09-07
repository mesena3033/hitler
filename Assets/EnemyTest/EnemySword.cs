using UnityEngine;

public class EnemySword : MonoBehaviour
{
    [SerializeField] Collider col;

    void Start()
    {
        col.enabled = false;
    }

    public void EnableEnemySword()
    {
        col.enabled = true;
    }

    public void DisableEnemySword()
    {
        col.enabled = false;
    }
}
