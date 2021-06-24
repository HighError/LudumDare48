using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject playerStats;
    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }
    public void DeleteData()
    {
        var playerStats = GameObject.Find("Player Stats").GetComponent<PlayerStats>();
        PlayerPrefs.DeleteAll();
        playerStats.LoadData();
    }
    private void OnApplicationQuit()
    {
        var playerStats = GameObject.Find("Player Stats").GetComponent<PlayerStats>();
        playerStats.SaveData();
    }
}
