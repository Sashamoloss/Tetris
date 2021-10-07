using UnityEngine;
[CreateAssetMenu(fileName = "TotalRowsCounter", menuName = "GameSO/TotalRowsCounter")]

public class TotalRowsCounter : IntVariable
{
    private void OnEnable()
    {
        value = 0;
    }
    public void IncrementCounter()
    {
        value++;
        Debug.Log("Increment");
    }

    public void OnTetris()
    {
        for (int i = 0; i < 4; i++)
        {
            IncrementCounter();
            Debug.Log("Tetris");
        }
    }
}
