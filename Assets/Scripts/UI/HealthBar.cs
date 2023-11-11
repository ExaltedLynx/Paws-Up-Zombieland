using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider HPBar;
    [SerializeField] private Image FillArea;
    public IEntity Entity { get; set; }

    private void Start()
    {
        if (Entity is EnemyBehavior)
        {
            FillArea.color = new Color32(255, 87, 40, 255);
            gameObject.SetActive(false);
        }
        else 
        {
            FillArea.color = new Color32(20, 170, 255, 255);
        }
    }

    private void Update()
    {
        transform.position = new Vector3(Entity.position.x, Entity.position.y - 0.4f, 0);
    }

    public void UpdateHealthBar(int maxHealth, int currentHealth)
    {
        HPBar.value = (float)currentHealth / maxHealth;
        if (HPBar.value < 1)
            gameObject.SetActive(true);
    }
}