using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBarManager : MonoBehaviour
{
    [SerializeField] GameObject skillBarPrefab;
    public static SkillBarManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public SkillBar InitSkillBar(PlayableUnit unit)
    {
        GameObject skillBarObj = Instantiate(skillBarPrefab, transform);
        SkillBar skillBar = skillBarObj.GetComponent<SkillBar>();
        skillBar.unit = unit;
        return skillBar;
    }
}
