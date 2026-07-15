using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.PostProcessing;

public class PlayerStatus : MonoBehaviour
{
    // HP
    private int maxHP = 100;
    public int MaxHP => maxHP;
    private int currentHP;

    //private float hitDisableDuration = 0.5f;
    private PlayerMove move;

    public int CurrentHP => currentHP;

    private bool isPlayerDead = false;
    public bool IsPlayerDead => isPlayerDead;
    
    // 無敵（被弾無効）
    private bool isInvincible = false;
    // 被弾判定
    private bool isDamaged = false;
    public bool IsInvincible => isInvincible;
    [SerializeField] private float invincibilityDuration = 3.5f;
    private float invincibilityTimer = 0f;

    private float hitStunTime = 1.2f;

    // スキルID取得して格納
    // UI


    // 攻撃力
    private float baseAttackPower = 10f;

    private float skillPower = 0f;
    private float buffMultiplier = 1f; // バフ倍率（デフォルト1）

    public float BuffMuktiPlier
    {
        get { return buffMultiplier; }
        set { buffMultiplier = value; }
    }

    public float BaseAttackPower => baseAttackPower;
    public float AttackPower => baseAttackPower * Mathf.Max(0.0001f, buffMultiplier);
    
    // 防御力
    private float defensePower = 5f;
    public float DefensePower => defensePower;

    //  マウス制御関数
    //  メニュー監視
    EventManager _event = null;

    NEWSkillMane skillMane;
    private PlayerAnimation playerAnimation;
    private void Start()
    {
        currentHP = maxHP;
        move = GetComponent<PlayerMove>();
        playerAnimation = GetComponent<PlayerAnimation>();

        //  マウス非表示 + 中央固定
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        //  EventManagerから引っ張れる
        _event = FindAnyObjectByType<EventManager>();
        skillMane = GetComponent<NEWSkillMane>();
    }

    private void Update()
    {
        //Debug.Log(currentHP);
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

        //カメラ制御用
        var brain = Camera.main.GetComponent<CinemachineBrain>();

        //  メニューがない時だけAltキーでマウス呼出し
        if (_event.GetPanelActive() == false)
        {
            //  Altキーでマウス呼び出し
            if (Keyboard.current.altKey.isPressed)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;

                //  呼出し中カメラ動かさない
                brain.enabled = false; // 停止

                return;
            }

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            brain.enabled = true;  // 再開
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;

            brain.enabled = false; // 停止
        }

            CheatingHeal();
    }

    // ダメージ処理
    public async Task ApplyDamage(int damage)
    {
        //Debug.Log("Damage");
        // 無敵中はダメージを受けない
        if (isInvincible || Cursor.visible) return;

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
                deadMover.SetBeingHit(1.2f);
            }

            //  ここに作ってねよろ--------------------------------
            await Task.Delay(3000); // 3秒待機
            _event.PlayerDespawn();
            return;
        }

        // 被弾時に移動無効
        var mover = GetComponent<PlayerMove>();
        if (mover != null)
        {
            mover.SetBeingHit(hitStunTime);
        }

        // 被弾後に一定時間無敵にする
        isInvincible = true;
        invincibilityTimer = invincibilityDuration;
    }

    // 敵の攻撃力でダメージを受ける（攻撃力 - 防御力）
    public void ReceiveAttack(float enemyAttackPower)
    {
        if (!isInvincible || move.CanNotMoving())
        {
            skillMane.HitChangeAnimation();
            float raw = enemyAttackPower;
            int dmg = Mathf.Max(0, Mathf.FloorToInt(raw - defensePower));
            isDamaged = true;
            skillMane.HitChangeAnimation();

            ////////////////
            Task task = ApplyDamage(dmg);
        }
    }
    

    /// <summary>
    /// ////////////////////////実装前に消す/////////////////////////////////////
    /// </summary>
    private void CheatingHeal()
    {
        if (Keyboard.current.hKey.wasPressedThisFrame)
        {
            currentHP += 100;
        }
    }

}
