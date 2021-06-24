using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    [Header("Chunks")]
    [SerializeField] private GameObject[] chunk;
    [SerializeField] private GameObject lastChunk;
    private int nowChunksCreate = 3;
    private PlayerController player;
    private bool lastChunkSet;

    [Header("BlueFire")]
    [SerializeField] private float spawnBlueFireY;
    [SerializeField] private RectTransform warningBlueFire;
    [SerializeField] private GameObject blueFire;

    [Header("Hearth")]
    [SerializeField] private int hearthSpawnCordY;
    [SerializeField] private GameObject hearthPrefab;

    private void Start()
    {
        spawnBlueFireY = Random.Range(-100, -150);
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        hearthSpawnCordY = -(UnityEngine.Random.Range(250, 400));
        lastChunkSet = false;
    }
    public void GenerateNextChunks()
    {
        Instantiate(chunk[Random.Range(0, chunk.Length - 1)], new Vector3(0, -5 * nowChunksCreate, 0), Quaternion.identity);
        nowChunksCreate++;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Chunk"))
        {
            if (player.isLive) GenerateNextChunks();
            else if (!player.isLive && !lastChunkSet)
            {
                Instantiate(lastChunk, new Vector3(0, -5 * nowChunksCreate, 0), Quaternion.identity);
                lastChunkSet = true;
            }
        }
    }

    private void FixedUpdate()
    {
        if (player.isLive)
        {
            if (player.transform.position.y <= spawnBlueFireY)
            {
                spawnBlueFireY = Random.Range(player.transform.position.y - 50, player.transform.position.y - 150);
                StartCoroutine(SpawnBlueFire());
            }

            if (player.distance % hearthSpawnCordY == 0 && player.distance != 0)
            {
                Instantiate(hearthPrefab, 
                    new Vector3(UnityEngine.Random.Range(-2, 3), hearthSpawnCordY - 15, 0), 
                    new Quaternion(0, 0, 0, 0));

                hearthSpawnCordY -= UnityEngine.Random.Range(250, 350);
            }
        }
        
    }

    private IEnumerator SpawnBlueFire()
    {
        float x = 1.5f * Random.Range(-1, 2);
        warningBlueFire.gameObject.SetActive(true);
        warningBlueFire.transform.position =
            new Vector3(x, warningBlueFire.transform.position.y, warningBlueFire.transform.position.z);

        yield return new WaitForSeconds(2f);
        warningBlueFire.gameObject.SetActive(false);
        Fly enemy = Instantiate(blueFire, 
            new Vector3(x, player.transform.position.y + 5, 0), 
            new Quaternion(0, 0, 0, 0)).GetComponent<Fly>();
        enemy.speed.y = player.getSpeed().y - 4f;
        yield return null;
    }
}
