using UnityEngine;

public class LoopScroll : MonoBehaviour
{
    [SerializeField]
    private Transform content;

    [SerializeField]
    private SkillItem template;

    private NEWSkillMane skillManager;

    private void Awake()
    {
        skillManager =
            FindFirstObjectByType<NEWSkillMane>();
    }

    private void Start()
    {
        CreateItems();
    }

    private void CreateItems()
    {
        // ”O‚Ì‚½‚ßContent‚ð‹ó‚É‚·‚é
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < skillManager.SkillCount; i++)
        {
            SkillItem item =
                Instantiate(template, content);

            item.gameObject.SetActive(true);

            item.Setup(
                i,
                skillManager.GetSkill(i));
        }
    }
}