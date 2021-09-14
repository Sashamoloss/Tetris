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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTetrisCompleted()
    {
        totalRows += 4;
    }

    public void OnRowCompleted()
    {
        totalRows++;
    }

    private void CheckNumberRows()
    {
        if (totalRows % 10 == 0) //toutes les 10 lignes (à chaque fois que totalRows/10 est entier)
            timeBeforeAutoDrop.value -= 0.2f; //on réduit la valeur de la vitesse de chute
    }
    
}
