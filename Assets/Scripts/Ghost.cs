using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour 
{
    public bool statusAngry { get; private set; }

    [Range(0, 100)] [SerializeField] private int chanseAngry;
    [SerializeField] private Vector3 speed;
    private void Start()
    {
        this.transform.rotation = new Quaternion(0,Random.Range(0,2)*180,0,0);
        this.transform.position = new Vector3(Random.Range(-2.0f,2.1f), transform.position.y, transform.position.z);
        statusAngry = (Random.Range(0, 100) <= chanseAngry);
        if (statusAngry)
            gameObject.GetComponent<Animator>().SetTrigger("Angry");
    }

    private void FixedUpdate()
    {
        transform.Translate(speed * Time.deltaTime);
    }

    private Quaternion Flip(Quaternion rotate)
    {
        if (rotate.y > 0) return new Quaternion(rotate.x, 0, rotate.z, rotate.w);
        else return new Quaternion(rotate.x, 180.0f, rotate.z, rotate.w);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Wall"))
        {
            transform.rotation = Flip(transform.rotation);
        }
    }

    public IEnumerator Dead()
    {
        gameObject.GetComponent<Animator>().SetTrigger("Dead");
        gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
        speed = new Vector3(0,0,0);
        Destroy(this.gameObject, 40f/60f);
        yield return null;
    }
}
