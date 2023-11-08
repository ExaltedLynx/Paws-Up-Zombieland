using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UnitButtonText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tmp;
    [SerializeField] private PlayableUnit unit;

    void Start()
    {
        tmp.SetText(unit.GetUnitCost().ToString());
    }
}
