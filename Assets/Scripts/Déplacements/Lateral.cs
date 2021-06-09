using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Lateral : MonoBehaviour
{
    [SerializeField] PlayfieldSO playfield;
    // Start is called before the first frame update
    /// <summary>
    /// D�place le tetramino � droite ou � gauche (le red�place dans l'autre sens si la position est invalide)
    /// </summary>
    /// <param name="CBC"></param>
    public void DeplacementLateral(InputAction.CallbackContext CBC)
    {
        if (CBC.phase == InputActionPhase.Performed)
        {
            var Direction = CBC.ReadValue<float>();
            transform.position += new Vector3(Direction, 0, 0);
            if (playfield.IsValidGridPos(transform))
                playfield.UpdateGrid(transform);
            else
                transform.position += new Vector3(-Direction, 0, 0);
        }
    }

}
