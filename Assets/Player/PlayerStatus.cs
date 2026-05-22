using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    // HP
    [SerializeField] private int maxHP = 100;
    [SerializeField] private int currentHP;

    // 攻撃力
    [SerializeField] private float attackPower = 10f;
    
    // 防御力
    [SerializeField] private float defensePower = 5f;

    void Start()
    {
        currentHP = maxHP;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
