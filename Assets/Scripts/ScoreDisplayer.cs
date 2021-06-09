using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreDisplayer : MonoBehaviour
{
    private int totalScore;
    private TextMeshProUGUI txtMeshPro;
    // Start is called before the first frame update
    void Start()
    {
        txtMeshPro = GetComponent<TextMeshProUGUI>();
    }
    public void UpdateScore()
    {
        totalScore += 5;
        txtMeshPro.text = totalScore.ToString();
    }

}
