using NUnit.Framework;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class PlayerStatus : MonoBehaviour
{
    // HP
    [SerializeField] private int maxHP = 100;
    [SerializeField] private int currentHP;

    private float hitDisableDuration = 0.5f;
    private PlayerMove move;
    public int CurrentHP => currentHP;

    private bool isPlayerDead = false;
    public bool IsPlayerDead => isPlayerDead;

    // 無敵（被弾無効）
    private bool isInvincible = false;
    // 被弾判定
    private bool isDamaged = false;
    public bool IsDamaged => isDamaged;
    public bool IsInvincible => isInvincible;
    [SerializeField] private float invincibilityDuration = 1f;
    private float invincibilityTimer = 0f;

    // スキルID取得して格納
    // UI

    // 攻撃力
    [SerializeField] private float baseAttackPower = 10f;
    [SerializeField] private float buffMultiplier = 1f; // バフ倍率（デフォルト1）
    public float BaseAttackPower => baseAttackPower;
    public float BuffMultiplier => buffMultiplier;
    public float AttackPower => baseAttackPower * Mathf.Max(0.0001f, buffMultiplier);
    
    // 防御力
    [SerializeField] private float defensePower = 5f;
    public float DefensePower => defensePower;

    void Start()
    {
        currentHP = maxHP;
        move = GetComponent<PlayerMove>();
        Cursor.visible = false;
    }

    void Update()
    {
        Debug.Log(currentHP);
        // 無敵タイマーの処理
        if (isInvincible)
        {
            invincibilityTimer -= Time.deltaTime;
            if (invincibilityTimer <= 0f)
            {
                isInvincible = false;
                invincibilityTimer = 0f;
            }
        }

        // プレイヤーが死んだら
        if (currentHP <= 0)
        {
            // 死亡処理を呼び出す
            var respawn = GetComponent<PlayerRespawn>();
            if (respawn != null)
            {
                respawn.PlayerDied();
            }
        }

        CheatingHeal();
    }

    // ダメージ処理
    public void ApplyDamage(int damage)
    {
        // 無敵中はダメージを受けない
        if (isInvincible) return;

        // 回避中はダメージを受けない
        if (move != null && move.IsDodging) return;

        currentHP -= damage;
        if (currentHP <= 0)
        {
            currentHP = 0;
            isPlayerDead = true;
            // 被弾時に移動無効
            var deadMover = GetComponent<PlayerMove>();
            if (deadMover != null)
            {
                deadMover.SetBeingHit(0.5f);
            }
            return;
        }

        // 被弾時に移動無効
        var mover = GetComponent<PlayerMove>();
        if (mover != null)
        {
            mover.SetBeingHit(1.2f);
        }

        // 被弾後に一定時間無敵にする
        isInvincible = true;
        invincibilityTimer = invincibilityDuration;
    }

    // 敵の攻撃力でダメージを受ける（攻撃力 - 防御力）
    public void ReceiveAttack(float enemyAttackPower)
    {
        if (!isInvincible)
        {
            float raw = enemyAttackPower;
            int dmg = Mathf.Max(0, Mathf.FloorToInt(raw - defensePower));
            isDamaged = true;
            ApplyDamage(dmg);
        }
    }

    private void CheatingHeal()
    {
        if (Keyboard.current.hKey.wasPressedThisFrame)
        {
            currentHP += 100;
        }
    }

}
