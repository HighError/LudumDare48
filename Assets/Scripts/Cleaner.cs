using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cleaner : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Chunk") || collision.transform.CompareTag("BlueFire"))
        {
            Destroy(collision.gameObject);
        }
    }
}
