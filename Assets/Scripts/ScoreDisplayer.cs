using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreDisplayer : MonoBehaviour
{
    public IntVariable pointsPerRow;
    private int totalScore;
    private TextMeshProUGUI txtMeshPro;
    // Start is called before the first frame update
    void Start()
    {
        txtMeshPro = GetComponent<TextMeshProUGUI>();
    }
    public void UpdateScore()
    {
        totalScore += pointsPerRow.value;
        txtMeshPro.text = totalScore.ToString();
    }

}
