using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] public int coin;
    [SerializeField] public int maxHealth;
    [SerializeField] public int invincibleTime;
    [SerializeField] public int recordDistance;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        LoadData();
    }

    public void Dead(int _coin, int _distance)
    {
        if (_distance > recordDistance) recordDistance = _distance;
        coin += _coin;
        SaveData();
    }

    public void LoadData()
    {
        coin = PlayerPrefs.HasKey("Coin") ? PlayerPrefs.GetInt("Coin") : 0;
        maxHealth = PlayerPrefs.HasKey("maxHealth") ? PlayerPrefs.GetInt("maxHealth") : 1;
        recordDistance = PlayerPrefs.HasKey("recordDistance") ? PlayerPrefs.GetInt("recordDistance") : 0;
        invincibleTime = PlayerPrefs.HasKey("invincibleTime") ? PlayerPrefs.GetInt("invincibleTime") : 2;
    }

    public void SaveData()
    {
        PlayerPrefs.SetInt("Coin", coin);
        PlayerPrefs.SetInt("maxHealth", maxHealth);
        PlayerPrefs.SetInt("recordDistance", recordDistance);
        PlayerPrefs.SetInt("invincibleTime", invincibleTime);
    }
}
