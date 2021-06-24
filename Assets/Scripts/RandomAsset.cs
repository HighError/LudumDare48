using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAsset : MonoBehaviour
{
    [SerializeField] private Sprite[] asset;

    private void Start()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = asset[Random.Range(0, asset.Length)];
    }
}
