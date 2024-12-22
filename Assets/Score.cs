using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    public int score = 0;
    public TMPro.TextMeshProUGUI scoreText;

    void Update()
    {
        scoreText.text = "Score: " + score;
    }
}
