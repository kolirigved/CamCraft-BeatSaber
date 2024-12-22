using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSystem : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip audioClip;
    public Score score;
    public void PlaySound()
    {
        score.score += 1;
        audioSource.PlayOneShot(audioClip);
    }
}
