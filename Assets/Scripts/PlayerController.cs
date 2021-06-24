using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Player move")]
    [SerializeField] private Vector2 speed;
    [SerializeField] private int multiplierSpeedX;

    [Header("Player Stats")]
    [SerializeField] [Range(0, 6)] private int health;
    [SerializeField] public bool isLive { get; private set; }
    [SerializeField] public int coin { get; private set; }
    [SerializeField] public int distance { get; private set; }

    [Header("UI")]
    [SerializeField] private Text coinText;
    [SerializeField] private Text distanceText;
    [SerializeField] private List<GameObject> hearth;
    [SerializeField] private GameObject shieldTimer;
    [SerializeField] private GameObject deadUI;

    [Header("Effects")]
    private bool invincible;

    [Header("Audio")]
    [SerializeField] private AudioSource coinSfx;
    [SerializeField] private AudioSource damageSfx;
    [SerializeField] private AudioSource healingSFX;

    [Header("Other")]
    [SerializeField] private Generator worldGenerator;
    [SerializeField] private GameObject[] wing;
    private Transform playerSprite;
    private PlayerStats playerStats;

    private void Start()
    {
        playerSprite = transform.GetChild(0);
        playerStats = GameObject.Find("Player Stats").GetComponent<PlayerStats>();
        
        StartGame();
    }

    private void FixedUpdate()
    {
        if (isLive)
        {
            speed.x = Input.GetAxisRaw("Horizontal") * multiplierSpeedX;
            if (speed.y >= -10f)
                speed.y -= 0.0005f;
            if (speed.x < 0) playerSprite.rotation = new Quaternion(0, 180, 0, 0);
            else if (speed.x > 0) playerSprite.rotation = new Quaternion(0, 0, 0, 0);
        }

        transform.Translate(speed * Time.deltaTime);
    }

    private void Update()
    {
        if (isLive)
        {
            distance = -(int)transform.position.y;
            distanceText.text = $"{distance} m";
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        switch (collision.transform.tag)
        {
            case "Hearth":
                healingSFX.Play();
                ChangeHealth(1);
                Destroy(collision.gameObject);
                break;
            case "Coin":
                if (collision.GetComponent<Coin>().coinType == CoinType.Red)
                    ChangeMoney(10);
                else if (collision.GetComponent<Coin>().coinType == CoinType.Gold)
                    ChangeMoney(5);
                else
                    ChangeMoney(1);
                coinSfx.Play();
                Destroy(collision.gameObject);
                break;
            case "Ghost":
                if (collision.gameObject.GetComponent<Ghost>().statusAngry)
                    ChangeHealth(-1);
                StartCoroutine(collision.GetComponent<Ghost>().Dead());
                break;
            case "BlueFire":
                ChangeHealth(-1);
                break;
            case "Saw":
                ChangeHealth(-1);
                break;
            case "DeathFloor":
                speed = new Vector2(0, 0);
                break;
            default:
                break;
        }
    }

    private void StartGame()
    {
        health = playerStats.maxHealth;
        for (int i = 0; i < hearth.Count; i++)
        {
            hearth[i].gameObject.SetActive(i < playerStats.maxHealth);
            hearth[i].transform.GetChild(0).gameObject.SetActive(i < health);
        }
        isLive = true;
    }

    private IEnumerator Death()
    {
        this.GetComponent<Collider2D>().enabled = false;
        damageSfx.Play();
        wing[0].GetComponent<Animator>().SetTrigger("Dead");
        wing[1].GetComponent<Animator>().SetTrigger("Dead");
        transform.GetChild(0).GetComponent<Animator>().SetTrigger("Dead");
        isLive = false;
        speed = new Vector2(0, -4);
        playerStats.Dead(coin, distance);
        playerStats.SaveData();
        yield return new WaitForSeconds(1f);
        transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
        wing[0].SetActive(false);
        wing[1].SetActive(false);
        yield return null;
    }

    public IEnumerator DeathOpenUI()
    {
        speed = new Vector2(0, 0);
        yield return new WaitForSeconds(1f);
        deadUI.SetActive(true);
        yield return null;
    }

    private IEnumerator giveInvincible(float second, Color color)
    {
        invincible = true;
        StartCoroutine(invincibleTimer(playerStats.invincibleTime));
        Shield(color, true);
        yield return new WaitForSeconds(playerStats.invincibleTime);
        Shield(color, false);
        invincible = false;
    }

    private IEnumerator invincibleTimer(float seconds)
    {
        shieldTimer.SetActive(true);
        Image timer = shieldTimer.transform.GetChild(0).GetComponent<Image>();
        Text timerText = shieldTimer.transform.GetChild(1).GetComponent<Text>();
        timer.fillAmount = 1;
        for (float i = 1; i > 0; i-= Time.deltaTime / seconds)
        {
            timerText.text = $"Invincible - {Math.Round(i * seconds,1)} s";
            timer.fillAmount = i;
            yield return null;
        }
        shieldTimer.SetActive(false);
    }

    private void Shield(Color color, bool status)
    {
        transform.GetChild(0).GetChild(0).gameObject.SetActive(status);
        if (status) transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().color = color;
    }

    public Vector2 getSpeed() { return speed; }

    public void ChangeMoney(int _coin)
    {
        coin += _coin;
        coinText.text = coin.ToString();
    }

    public void ChangeHealth(int hp)
    {
        if (isLive)
        {
            if (hp > 0 || (hp < 0 && !invincible))
            {
                health += hp;
                if (health > playerStats.maxHealth)
                    health = playerStats.maxHealth;

                if (health == 0)
                    StartCoroutine(Death());
                else if (hp < 0)
                {
                    damageSfx.Play();
                    transform.GetChild(0).GetComponent<Animator>().SetTrigger("Damage");
                    StartCoroutine(giveInvincible(3, new Color(255, 255, 255, 0.3f)));
                }
                for (int i = 0; i < hearth.Count; i++)
                    hearth[i].transform.GetChild(0).gameObject.SetActive(i < health);
            }
        }
    }
}
