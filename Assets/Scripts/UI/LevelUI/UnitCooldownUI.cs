using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitCooldownUI : MonoBehaviour
{
    [SerializeField] private Image cooldownTimerImage;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Button button;
    private PlayableUnit unit;
    private float cooldownTime;
    private float cooldownTimer;
    private bool shouldUpdate = false;

    void Update()
    {
        if(shouldUpdate)
        {
            cooldownTimer -= Time.deltaTime;
            cooldownTimerImage.gameObject.SetActive(true);
            cooldownTimerImage.fillAmount = cooldownTimer / cooldownTime;
            text.SetText(cooldownTimer.ToString(".0"));

            if (cooldownTimer <= 0)
            {
                cooldownTimer = cooldownTime;
                cooldownTimerImage.fillAmount = 1;
                text.SetText(cooldownTimer.ToString());
                cooldownTimerImage.gameObject.SetActive(false);
                unit.onUnitDeath.RemoveListener(StartCooldown);
                button.interactable = true;
                shouldUpdate = false;
            }
        }
    }

    private void StartCooldown()
    {
        Debug.Log("test");
        shouldUpdate = true;
        button.interactable = false;
    }

    public void InitCooldownUI(PlayableUnit unit)
    {
        this.unit = unit;
        cooldownTime = unit.GetPlacementCooldown();
        cooldownTimer = cooldownTime;
        text.SetText(cooldownTimer.ToString());
        unit.onUnitDeath.AddListener(StartCooldown);
    }

    public void DisableButton()
    {
        button.interactable = false;
    }
}
