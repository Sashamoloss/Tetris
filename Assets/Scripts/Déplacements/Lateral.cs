using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Lateral : MonoBehaviour
{
    [SerializeField] PlayfieldSO playfield;
    [SerializeField] FloatVariable tempsAvantChuteAuto;
    [SerializeField] TetraminoSO config;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    /// <summary>
    /// D�place le tetramino � droite ou � gauche (le red�place dans l'autre sens si la position est invalide)
    /// </summary>
    /// <param name="CBC"></param>
    private void DeplacementLateral(InputAction.CallbackContext CBC)
    {
        var Direction = CBC.ReadValue<float>();
        transform.position += new Vector3(Direction, 0, 0);
        if (playfield.IsValidGridPos(transform))
            playfield.UpdateGrid(transform);
        else
            transform.position += new Vector3(-Direction, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
