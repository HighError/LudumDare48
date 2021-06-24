using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteObjectModule : MonoBehaviour
{
    public void Delete()
    {
        Destroy(this.gameObject);
    }
}
