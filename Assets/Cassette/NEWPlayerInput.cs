using UnityEngine;
using UnityEngine.InputSystem;

// スキル発動のためのキーボード入力を監視するスクリプト
public class NewMonoBehaviourScript : MonoBehaviour
{
    [SerializeField]
    private NEWSkillMane skillManager;

    private void Awake()
    {
        if (skillManager == null)
            skillManager = GetComponent<NEWSkillMane>();
    }

    private void Update()
    {
        var kb = Keyboard.current;
        if (kb == null) return;

        if (kb.digit1Key.wasPressedThisFrame)
        {
            skillManager.UseSkill(0);
        }
        else if (kb.digit2Key.wasPressedThisFrame)
        {
            skillManager.UseSkill(1);
        }
        else if (kb.digit3Key.wasPressedThisFrame)
        {
            skillManager.UseSkill(2);
        }
    }
}
