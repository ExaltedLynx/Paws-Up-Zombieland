using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillBar : MonoBehaviour
{
    [SerializeField] private Slider HPBar;
    [SerializeField] private Image fillArea;
    public PlayableUnit unit { get; set; }

    void Update()
    {
        float barFillAmt = (float)unit.GetSkillPoints() / unit.GetAbilityCost();
        HPBar.value = barFillAmt;
        transform.position = new Vector3(unit.transform.position.x, unit.transform.position.y - 0.46f, 0);
    }
}
