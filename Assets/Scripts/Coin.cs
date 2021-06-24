using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CoinType
{
    Silver,
    Gold,
    Red
}

public class Coin : MonoBehaviour
{
    [Header("Chances")]
    [SerializeField] private float chanceGoldCoin;
    [SerializeField] private float chanceRedCoin;

    public CoinType coinType;

    private void Start()
    {
        float chance = Random.Range(1f, 100f);
        if (chance <= chanceRedCoin)
        {
            coinType = CoinType.Red;
            GetComponent<Animator>().SetTrigger("Coin10");
        }
        else if (chance <= (chanceGoldCoin + chanceRedCoin))
        {
            coinType = CoinType.Gold;
            GetComponent<Animator>().SetTrigger("Coin5");
        }
        else
        {
            coinType = CoinType.Silver;
        }
    }

}
