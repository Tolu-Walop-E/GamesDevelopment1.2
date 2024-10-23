using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreTextEditor : MonoBehaviour
{
    private TextMeshProUGUI ScoreCount;
    public static int score = 0;
    // Start is called before the first frame update
    void Start()
    {
        ScoreCount = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        ScoreCount.text = "Score: " + score;
    }
}
