using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    [SerializeField]
    private SkillManager skillManager;

    private void Awake()
    {
        if (skillManager == null)
            skillManager = GetComponent<SkillManager>();
    }

    private void Update()
    {
        var kb = Keyboard.current;
        if (kb == null) return;

        if (kb.digit1Key.wasPressedThisFrame)
        {
            skillManager.UseSkill(1);
        }
        else if (kb.digit2Key.wasPressedThisFrame)
        {
            skillManager.UseSkill(2);
        }
        else if (kb.digit3Key.wasPressedThisFrame)
        {
            skillManager.UseSkill(3);
        }
    }
}