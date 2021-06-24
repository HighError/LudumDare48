using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fly : MonoBehaviour
{
    [SerializeField] public Vector2 speed;

    private void FixedUpdate()
    {
        transform.Translate(speed * Time.deltaTime);
    }
}
