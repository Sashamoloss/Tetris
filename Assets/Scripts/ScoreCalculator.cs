using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCalculator : MonoBehaviour
{
    [Header("Variables")] public IntVariable totalScore;
    public FloatVariable timeBeforeSoftDrop;
    public IntVariable pointsPerRow;
    public IntVariable pointsPerTetris;
    public IntVariable pointsPerSuddenDrop;
    public IntVariable pointsPerSecondSoftDrop;
    [Header("Event")] public GameEventSO onScoreChanged;
    private Coroutine coroutinePointsPerSoftDrop;

    // Start is called before the first frame update
    void Start()
    {
        totalScore.value = 0;
    }

    private void AddToScore(int ScoreToAdd)
    {
        totalScore.value += ScoreToAdd;
        onScoreChanged.Raise();
    }

    public void OnRowCompleted()
    {
        AddToScore(pointsPerRow.value);
    }

    public void OnTetrisCompleted()
    {
        AddToScore(pointsPerTetris.value);
    }

    public void OnSuddenDropCompleted()
    {
        AddToScore(pointsPerSuddenDrop.value);
        //Debug.Log("Sudden Drop");
    }
    public void OnSoftDropStarted()
    {
        coroutinePointsPerSoftDrop = StartCoroutine(CoroutinePointsPerSoftDrop());
    }
    IEnumerator CoroutinePointsPerSoftDrop()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBeforeSoftDrop.value);
            AddToScore(pointsPerSecondSoftDrop.value);
        }
    }
    public void OnSoftDropCanceled()
    {
        StopCoroutine(coroutinePointsPerSoftDrop);
    }
}
