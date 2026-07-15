using UnityEngine;
using UnityEngine.InputSystem;

// スキル発動のためのキーボード入力を監視するスクリプト
public class NewMonoBehaviourScript : MonoBehaviour
{
    [SerializeField]
    private NEWSkillMane skillManager;

    [SerializeField]
    private SkillSlotMane SlotManager;

    private PlayerMove playerMove;
    private PlayerStatus playerStatus;

    private void Awake()
    {
        if (skillManager == null)
            skillManager = GetComponent<NEWSkillMane>();

        playerMove = GetComponent<PlayerMove>();
        playerStatus = GetComponent<PlayerStatus>();
    }

    private void Update()
    {
        var kb = Keyboard.current;
        if (kb == null) return;
        
        if (playerMove.IsBeingHit) return;
        if (playerStatus.IsPlayerDead) return;
        if (skillManager.isUsingSkill) return;

        if (kb.digit1Key.wasPressedThisFrame)
        {
            //Debug.Log($"Slot0 = {SlotManager.GetSkillID(0)}");
            skillManager.UseSkill(SlotManager.GetSkillID(0));
        }
        else if (kb.digit2Key.wasPressedThisFrame)
        {
            //Debug.Log($"Slot1 = {SlotManager.GetSkillID(1)}");
            skillManager.UseSkill(SlotManager.GetSkillID(1));
        }
        else if (kb.digit3Key.wasPressedThisFrame)
        {
            //Debug.Log($"Slot2 = {SlotManager.GetSkillID(2)}");
            skillManager.UseSkill(SlotManager.GetSkillID(2));
        }
    }
}
