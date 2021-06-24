using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    private PlayerStats playerStats;
    [SerializeField] private Text money;
    [Header("Button")]
    [SerializeField] private Button healthUpgradeButton;
    [SerializeField] private Button invincibleTimeButton;
    [Header("Text")]
    [SerializeField] private Text healthUpgradeText;
    [SerializeField] private Text invincibleTimeText;

    private void OnEnable()
    {
        playerStats = GameObject.Find("Player Stats").GetComponent<PlayerStats>();
        updateShop();
    }

    public void updateShop()
    {
        money.text = playerStats.coin.ToString();
        if (playerStats.maxHealth < 6)
        {
            int cost = 25 * (int)Math.Pow(3, playerStats.maxHealth);
            healthUpgradeText.text = $"Lvl: {playerStats.maxHealth}/6\n" +
                                    $"Cost: {cost}¢";
        }
        else
        {
            healthUpgradeText.text = $"Lvl: MAX!\n" +
                                    $"Cost: -";
            healthUpgradeButton.interactable = false;
        }

        if (playerStats.invincibleTime < 5)
        {
            int cost = 50 * (int)Math.Pow(2, playerStats.invincibleTime);
            invincibleTimeText.text = $"Lvl: {playerStats.invincibleTime-1}/4\n" +
                                    $"Cost: {cost}¢";
        }
        else
        {
            invincibleTimeText.text = $"Lvl: MAX!\n" +
                                    $"Cost: -";
            invincibleTimeButton.interactable = false;
        }
    }

    public void UpgradeHealth()
    {
        if (playerStats.maxHealth == 6) return;

        int cost = 25 * (int)Math.Pow(3, playerStats.maxHealth);
        if (playerStats.coin >= cost)
        {
            playerStats.maxHealth++;
            playerStats.coin -= cost;
            updateShop();
        }
        playerStats.SaveData();
    }

    public void UpgradeInvincible()
    {
        if (playerStats.invincibleTime == 5) return;

        int cost = 50 * (int)Math.Pow(2, playerStats.invincibleTime);
        if (playerStats.coin >= cost)
        {
            playerStats.invincibleTime++;
            playerStats.coin -= cost;
            updateShop();
        }
        playerStats.SaveData();
    }
}

