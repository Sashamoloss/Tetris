using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Rotations : MonoBehaviour
{
    [SerializeField] PlayfieldSO playfield;
    [SerializeField] FloatVariable tempsAvantChuteAuto;
    [SerializeField] TetraminoSO config;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    /// <summary>
    /// Pour tourner le tetramino à droite ou à gauche (le retourne dans l'autre sens si la position est invalide)
    /// </summary>
    /// <param name="CBC"></param>
    private void Rotation(InputAction.CallbackContext CBC)
    {
        var Rotation = CBC.ReadValue<float>();
        transform.Rotate(0, 0, Rotation * 90);
        if (playfield.IsValidGridPos(transform))
            playfield.UpdateGrid(transform);
        else
            transform.Rotate(0, 0, -Rotation * 90);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
