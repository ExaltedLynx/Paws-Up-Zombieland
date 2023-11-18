using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UnitButtonText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tmp;
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI[] unitInfoText = new TextMeshProUGUI[6];
    [SerializeField] private PlayableUnit unit;

    void Start()
    {
        tmp.SetText(unit.GetUnitCost().ToString());

        unitInfoText[0].SetText("Health: " + unit.GetMaxHealth().ToString());
        if (unit is FighterUnit fighter)
            unitInfoText[1].SetText("Attack " + fighter.GetAttack().ToString());
        else if(unit is HealerUnit healer)
            unitInfoText[1].SetText("Heal " + healer.GetHealPower().ToString());

        unitInfoText[2].SetText("Defense: " + unit.GetDefense().ToString());
        unitInfoText[3].SetText("Max Block:" + unit.GetMaxHealth().ToString());
        unitInfoText[4].SetText("Attack Interval: " + unit.GetAttackInterval().ToString() + "s");

        if(unit.GetValidTile() == Tile.TileType.Ground)
            unitInfoText[5].SetText("Tile: Ground");
        else
            unitInfoText[5].SetText("Tile: Elevated");

    }
}
