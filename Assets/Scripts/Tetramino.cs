using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Tetramino : MonoBehaviour
{
    [SerializeField] PlayfieldSO playfield;

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
}
