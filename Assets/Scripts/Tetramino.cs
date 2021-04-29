using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Tetramino : MonoBehaviour
{
    private Controles Inputs;
    float lastFall = 0;
    private void OnEnable()
    {
        Inputs = new Controles();
        Inputs.Enable();
        Inputs.AM.Deplacement.performed += DeplacementLateral;
        Inputs.AM.Rotation.performed += Rotation;
        Inputs.AM.Chute.performed += Chute;
    }



    private void DeplacementLateral(InputAction.CallbackContext CBC)
    {
        var Direction = CBC.ReadValue<float>();
        transform.position += new Vector3(Direction, 0, 0);
        if (IsValidGridPos())
            UpdateGrid();
        else
            transform.position += new Vector3(-Direction, 0, 0);
    }
    private void Rotation(InputAction.CallbackContext CBC)
    {
        var Rotation = CBC.ReadValue<float>();
        transform.Rotate (0, 0, Rotation*90);
        if (IsValidGridPos())
            UpdateGrid();
        else
            transform.Rotate(0, 0, -Rotation * 90);
    }
    private void Chute(InputAction.CallbackContext CBC)
    {
        transform.position += Vector3.down;
        if (IsValidGridPos())
            UpdateGrid();
        else
        {
            transform.position += Vector3.up;
            Playfield.instance.DeleteFullRows();
            FindObjectOfType<Spawneur>().SpawnNext();
            Destroy(this);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    /// <summary>
    /// Vérifie que la position des blocs est entre les bordures et ne collisionne pas avec un autre Tétramino
    /// </summary>
    /// <returns></returns>
    bool IsValidGridPos()
    {
        foreach (Transform child in transform)//Pour chaque Transform dans le transform = pour chaque enfant
        {
            Vector2 roundedPos = Playfield.instance.RoundVec2(child.position);//On arrondit la position de l’enfant pour la stocker dans roundedPos
            if (!Playfield.instance.InsideBorder(roundedPos))
                return false;//Si la position de l’enfant n’est pas entre les bordures, on renvoie faux
            if (Playfield.instance.grid[(int)roundedPos.x, (int)roundedPos.y] != null && //S’il y a qqch à x,y dans le tableau
                Playfield.instance.grid[(int)roundedPos.x, (int)roundedPos.y].parent != transform) //Et si le parent de ce bloc n’est pas le même que le nôtre (pour qu’il puisse collisionner avec lui même))
                return false;
        }
        return true; //Sinon, la position est valide
    }
    void UpdateGrid()
    {
        for (int i = 0; i < Playfield.instance.h; i++)//On vérifie de haut en bas
        {
            for (int j = 0; j < Playfield.instance.w; j++)//Et de gauche à droite
            {
                if (Playfield.instance.grid[i, j] != null)//S’il y a qqch à i,j
                    if (Playfield.instance.grid[i, j].parent == transform)//Et que le parent de ce qqch est le nôtre
                        Playfield.instance.grid[i, j] = null;//On efface ce qu’il y a à cet endroit
            }
        }
        foreach (Transform child in transform)//Pour chaque Transform dans le transform = pour chaque enfant
        {
            var roundedPos = Playfield.instance.RoundVec2(child.position);//On arrondit la position de l’enfant pour la stocker dans roundedPos
            Playfield.instance.grid[(int)roundedPos.x, (int)roundedPos.y] = child;//On met le transform à cette position
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDisable()
    {
        Inputs.Disable();
    }
}
