using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class SkillHolder : MonoBehaviour
{
    public SkillDataNo2[] skilldata;
    private Directionary<int, SkillDataNo2> dict;

    void Start()
    {
        var factory = new SkillFactory();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
