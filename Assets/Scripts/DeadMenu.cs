using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeadMenu : MonoBehaviour
{
    [SerializeField]private Text stats;
    [SerializeField] private GameObject panel;
    private void Start()
    {
        PlayerStats playerStats = GameObject.Find("Player Stats").GetComponent<PlayerStats>();
        PlayerController playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        stats.text = $"Coins: +{playerController.coin} ({playerStats.coin})\n\n" +
                        $"Distance: {playerController.distance}\n";
        if (playerStats.recordDistance == playerController.distance)
            stats.text += "New record!!!";
        else
            stats.text += $"Record distance: {playerStats.recordDistance}";
        StartCoroutine(Show());
    }

    private IEnumerator Show()
    {
        for (float i = 1; i > 0; i-= Time.deltaTime/3)
        {
            panel.GetComponent<RectTransform>().anchoredPosition = new Vector3(2000 * i, 0, 0);
            this.GetComponent<Image>().color = new Color(0, 0, 0, (1 - i) * 0.8f);
            yield return null;
        }
        panel.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
    }

    public void Menu()
    {
        SceneManager.LoadScene("Menu");
    }
    public void Restart()
    {
        SceneManager.LoadScene("Game");
    }
}
