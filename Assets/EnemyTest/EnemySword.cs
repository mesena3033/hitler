using UnityEngine;

public class EnemySword : MonoBehaviour
{
    Collider col;

    void Start()
    {
        col = GetComponent<Collider>();
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
