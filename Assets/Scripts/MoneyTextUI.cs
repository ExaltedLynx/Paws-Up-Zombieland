using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyTextUI : MonoBehaviour
{
    public MoneyManager moneyManager;

    public Text moneyText;

    public void Update()
    {
        moneyText.text = "$ " + moneyManager.GetCurrentMoney();
    }

}
