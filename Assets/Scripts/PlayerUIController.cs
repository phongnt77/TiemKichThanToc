using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUIController : MonoBehaviour
{
    public Slider healthBar;
    public TMP_Text healthText;

    void Start()
    {
        if (healthBar != null)
            healthBar.gameObject.SetActive(false);
        if (healthText != null)
            healthText.gameObject.SetActive(false);
    }

    public void SetMaxHealth(float maxHealth)
    {
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = maxHealth;
            healthBar.gameObject.SetActive(true);
        }
        if (healthText != null)
        {
            healthText.text = $"HP: {maxHealth}/{maxHealth}";
            healthText.gameObject.SetActive(true);
        }
    }

    public void SetHealth(float health)
    {
        if (healthBar != null)
            healthBar.value = health;
        if (healthText != null)
            healthText.text = $"HP: {health}/{healthBar.maxValue}";
    }
}