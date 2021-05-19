using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Tetramino : MonoBehaviour
{
    private Controles Inputs;
    float lastFall = 0;
    private Coroutine coroutineChute;
    private void OnEnable()//Mise en place des contrôles
    {
        Inputs = new Controles();
        Inputs.Enable();
        Inputs.AM.Deplacement.performed += DeplacementLateral;
        Inputs.AM.Rotation.performed += Rotation;
        Inputs.AM.Chute.performed += ChuteManuellePerformed;
        Inputs.AM.Chute.canceled += ChuteManuelleCanceled;
        Inputs.AM.ChuteInstant.performed += ChuteInstant;
    }
    /// <summary>
    /// Lance la coroutine de chute instantanée
    /// </summary>
    /// <param name="CBC"></param>
    private void ChuteInstant(InputAction.CallbackContext CBC)
    {
        StartCoroutine(CoroutineChuteInstant());
    }
    /// <summary>
    /// Déplace le tetramino à droite ou à gauche (le redéplace dans l'autre sens si la position est invalide)
    /// </summary>
    /// <param name="CBC"></param>
    private void DeplacementLateral(InputAction.CallbackContext CBC)
    {
        var Direction = CBC.ReadValue<float>();
        transform.position += new Vector3(Direction, 0, 0);
        if (IsValidGridPos())
            UpdateGrid();
        else
            transform.position += new Vector3(-Direction, 0, 0);
    }
    /// <summary>
    /// Pour tourner le tetramino à droite ou à gauche (le retourne dans l'autre sens si la position est invalide)
    /// </summary>
    /// <param name="CBC"></param>
    private void Rotation(InputAction.CallbackContext CBC)
    {
        var Rotation = CBC.ReadValue<float>();
        transform.Rotate (0, 0, Rotation*90);
        if (IsValidGridPos())
            UpdateGrid();
        else
            transform.Rotate(0, 0, -Rotation * 90);
    }
   /// <summary>
   /// Fait tomber le tetramino en boucle (grâce à la fonction chute) en attendant un temps donné entre chaque chute (la fonction chute s'arrête automatiquement en bas)
   /// </summary>
   /// <returns></returns>
    IEnumerator CoroutineChute()
    {
        while (true)
        {
            yield return new WaitForSeconds(Playfield.instance.tempsAvantChuteManuelle);
            Chute();
        }

    }
    /// <summary>
    /// Fait tomber le tetramino en boucle (grâce à la fonction chute) jusqu'à atteindre le bas, où la fonction chute s'arrête automatiquement
    /// </summary>
    /// <returns></returns>
    IEnumerator CoroutineChuteInstant()
    {
        while (true)
        {
            yield return null;
                Chute();
        }
    }
    /// <summary>
    /// Lance la coroutine de chute en boucle tant qu'on appuie sur le bouton
    /// </summary>
    /// <param name="CBC"></param>
    private void ChuteManuellePerformed(InputAction.CallbackContext CBC)
    {
        coroutineChute = StartCoroutine(CoroutineChute());
        Chute();
    }    
    /// <summary>
    /// Annule la coroutine de chute quand on arrête d'appuyer sur la touche de chute
    /// </summary>
    /// <param name="CBC"></param>
    private void ChuteManuelleCanceled(InputAction.CallbackContext CBC)
    {
        StopCoroutine(coroutineChute);
    }
    /// <summary>
    /// Descend le transform du Tetramino d'un cran en bas puis vérifie si la position est valide
    /// Si elle ne l'est pas, remonte le Tetramino d'un cran, efface les lignes pleines, spawn la prochaine pièce et détruit ce script
    /// </summary>
    private void Chute()
    {
        transform.position += Vector3.down;//On descend le transform d'un cran
        if (IsValidGridPos())//Si la position est valide, on update la grille
            UpdateGrid();
        //Sinon on remonte le transform, on efface les lignes pleines car ça veut dire qu'on est arrivés tout en bas
        //Puis on fait spawner le prochain tetramino avant de détruire ce script pour ne pas déplacer le tetramino tombé tout en bas
        else
        {
            transform.position += Vector3.up;
            Playfield.instance.DeleteFullRows();
            FindObjectOfType<Spawneur>().SpawnNext();
            Destroy(this);//On détruit ce script pour éviter de contrôler plusieurs pièces à la fois
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //Si la position du Tetramino qui spawn n'est pas valide, on détruit celui-ci et c'est le Game Over (et y a pas de spawnnext)
        //C'est dans le start pour éviter de le déplacer alors qu'il ne pourrait pas
        if (!IsValidGridPos())
        {
            Debug.Log("GAME OVER");
            Destroy(gameObject);
        }
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
   /// <summary>
   /// Vérifie que chaque bloc est à la bonne place et est bien lié à son tetramino
   /// </summary>
    void UpdateGrid()
    {
        for (int i = 0; i < Playfield.instance.h; i++)//On vérifie de haut en bas
        {
            for (int j = 0; j < Playfield.instance.w; j++)//Et de gauche à droite
            {
                if (Playfield.instance.grid[j, i] != null)//S’il y a qqch à i,j
                    if (Playfield.instance.grid[j, i].parent == transform)//Et que le parent de ce qqch est le nôtre
                        Playfield.instance.grid[j, i] = null;//On efface ce qu’il y a à cet endroit
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
        if (Time.time - lastFall >= Playfield.instance.tempsAvantChuteAuto) //Si la différence entre le temps écoulé depuis le début du jeu et celui depuis la dernière chute est supérieur à la variable de chute automatique
        {
            Chute();//On descend automatiquement
            lastFall = Time.time;//on reset la variable contenant le temps depuis la dernière chute
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
