using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Rotations : MonoBehaviour
{
    [SerializeField] PlayfieldSO playfield;

    /// <summary>
    /// Pour tourner le tetramino à droite ou à gauche (le retourne dans l'autre sens si la position est invalide)
    /// </summary>
    /// <param name="CBC"></param>
    public void Rotation(InputAction.CallbackContext CBC)
    {
        if (CBC.phase == InputActionPhase.Performed)
        {
            var Rotation = CBC.ReadValue<float>();
            transform.Rotate(0, 0, Rotation * 90);
            if (playfield.IsValidGridPos(transform))
                playfield.UpdateGrid(transform);
            else
                transform.Rotate(0, 0, -Rotation * 90);
        }
    }
}
