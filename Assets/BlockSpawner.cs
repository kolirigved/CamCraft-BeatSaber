using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    public GameObject BlockPrefab;
    public float zPos = 12.0f;
    public float speed = 5.0f;
    public float spawnRate = 1.0f;
    public float difficulty = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        
        InvokeRepeating("SpawnBlock", 0.0f, spawnRate);
    }
    void SpawnBlock(){
        GameObject block = Instantiate(BlockPrefab, new Vector3(Random.Range(-0.75f, 0.75f),Random.Range(0.1f, 1.0f), zPos), Quaternion.identity);
        block.GetComponent<Rigidbody>().velocity = new Vector3(0.0f, 0.0f, -speed);
    }
    void Update(){
        spawnRate = 1.0f /(Random.Range(1.5f,2.5f))*(difficulty * Time.timeSinceLevelLoad);
        speed = Random.Range(2.0f,5.0f) + 0.01f*(difficulty * Time.timeSinceLevelLoad);
    }
}
