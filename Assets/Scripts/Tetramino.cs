using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Tetramino : MonoBehaviour
{
    private Controles Inputs;
    [SerializeField] PlayfieldSO playfield;
    private void OnEnable()//Mise en place des contrôles
    {
        Inputs = new Controles();
        Inputs.Enable();
        //Inputs.AM.Deplacement.performed += DeplacementLateral;
        //Inputs.AM.Rotation.performed += Rotation;
    }





    // Start is called before the first frame update
    void Start()
    {
        //Si la position du Tetramino qui spawn n'est pas valide, on détruit celui-ci et c'est le Game Over (et y a pas de spawnnext)
        //C'est dans le start pour éviter de le déplacer alors qu'il ne pourrait pas
        if (!playfield.IsValidGridPos(transform))
        {
            Debug.Log("GAME OVER");
            Destroy(gameObject);
        }
    }


    /// <summary>
    /// Désactive les inputs à la désactivation du script (pour éviter des bugs ?)
    /// </summary>
    private void OnDisable()
    {
        Inputs.Disable();
    }
}
