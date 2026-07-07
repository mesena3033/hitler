using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoopScroll : MonoBehaviour
{
    [SerializeField] private Transform content;
    [SerializeField] private SkillItem itemPrefab;
    [SerializeField] private NEWSkillMane skillManager;

    private void Start()
    {
        CreateList();
    }

    private void CreateList()
    {
        for (int i = 0; i < skillManager.SkillCount; i++)
        {
            SkillItem item = Instantiate(itemPrefab, content);

            item.Setup(skillManager.GetSkill(i));
        }
    }
}