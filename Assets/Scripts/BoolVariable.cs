using UnityEngine;

[CreateAssetMenu(fileName ="Bool Variable", menuName = "GameSO/Bool Variable")]

public class BoolVariable : Variable<bool>
{
    public void Switch()
    {
        value = !value; //on change la valeur de value
    }
}
