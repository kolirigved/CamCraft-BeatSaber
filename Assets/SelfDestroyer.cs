using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SelfDestroyer : MonoBehaviour
{
    public SoundSystem soundSystem;
    public float destroyTime = 6.0f;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, destroyTime);
        //find the sound system
        soundSystem = GameObject.Find("Main Camera").GetComponent<SoundSystem>();
    }
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            soundSystem.PlaySound();
            //wait for 2 seconds before destroying the object
            Destroy(gameObject, 2.0f);
        }
    }
}
