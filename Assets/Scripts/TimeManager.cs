using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    private int totalRows;
    public FloatVariable timeBeforeAutoDrop;
    public AnimationCurve test;

    // Start is called before the first frame update
    void Start()
    {
        CheckNumberRows();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTetrisCompleted()
    {
        for (int i = 0; i < 4; i++)
        {
            OnRowCompleted();
        }
    }

    public void OnRowCompleted()
    {
        totalRows++;
        CheckNumberRows();
    }

    private void CheckNumberRows()
    {
        Debug.Log(totalRows % 10);
        if (totalRows % 10 == 0) //toutes les 10 lignes (à chaque fois que totalRows/10 est entier)
        {
            timeBeforeAutoDrop.value = test.Evaluate(totalRows / 10); //on réduit la valeur de la vitesse de chute
            Debug.Log("Check Number Rows");
        }
    }    
}
