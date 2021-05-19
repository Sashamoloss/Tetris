using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Tetramino : MonoBehaviour
{
    private Controles Inputs;
    float lastFall = 0;
    private Coroutine coroutineChute;
    private void OnEnable()//Mise en place des contr�les
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
    /// Lance la coroutine de chute instantan�e
    /// </summary>
    /// <param name="CBC"></param>
    private void ChuteInstant(InputAction.CallbackContext CBC)
    {
        StartCoroutine(CoroutineChuteInstant());
    }
    /// <summary>
    /// D�place le tetramino � droite ou � gauche (le red�place dans l'autre sens si la position est invalide)
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
    /// Pour tourner le tetramino � droite ou � gauche (le retourne dans l'autre sens si la position est invalide)
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
   /// Fait tomber le tetramino en boucle (gr�ce � la fonction chute) en attendant un temps donn� entre chaque chute (la fonction chute s'arr�te automatiquement en bas)
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
    /// Fait tomber le tetramino en boucle (gr�ce � la fonction chute) jusqu'� atteindre le bas, o� la fonction chute s'arr�te automatiquement
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
    /// Annule la coroutine de chute quand on arr�te d'appuyer sur la touche de chute
    /// </summary>
    /// <param name="CBC"></param>
    private void ChuteManuelleCanceled(InputAction.CallbackContext CBC)
    {
        StopCoroutine(coroutineChute);
    }
    /// <summary>
    /// Descend le transform du Tetramino d'un cran en bas puis v�rifie si la position est valide
    /// Si elle ne l'est pas, remonte le Tetramino d'un cran, efface les lignes pleines, spawn la prochaine pi�ce et d�truit ce script
    /// </summary>
    private void Chute()
    {
        transform.position += Vector3.down;//On descend le transform d'un cran
        if (IsValidGridPos())//Si la position est valide, on update la grille
            UpdateGrid();
        //Sinon on remonte le transform, on efface les lignes pleines car �a veut dire qu'on est arriv�s tout en bas
        //Puis on fait spawner le prochain tetramino avant de d�truire ce script pour ne pas d�placer le tetramino tomb� tout en bas
        else
        {
            transform.position += Vector3.up;
            Playfield.instance.DeleteFullRows();
            FindObjectOfType<Spawneur>().SpawnNext();
            Destroy(this);//On d�truit ce script pour �viter de contr�ler plusieurs pi�ces � la fois
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //Si la position du Tetramino qui spawn n'est pas valide, on d�truit celui-ci et c'est le Game Over (et y a pas de spawnnext)
        //C'est dans le start pour �viter de le d�placer alors qu'il ne pourrait pas
        if (!IsValidGridPos())
        {
            Debug.Log("GAME OVER");
            Destroy(gameObject);
        }
    }
    /// <summary>
    /// V�rifie que la position des blocs est entre les bordures et ne collisionne pas avec un autre T�tramino
    /// </summary>
    /// <returns></returns>
    bool IsValidGridPos()
    {
        foreach (Transform child in transform)//Pour chaque Transform dans le transform = pour chaque enfant
        {
            Vector2 roundedPos = Playfield.instance.RoundVec2(child.position);//On arrondit la position de l�enfant pour la stocker dans roundedPos
            if (!Playfield.instance.InsideBorder(roundedPos))
                return false;//Si la position de l�enfant n�est pas entre les bordures, on renvoie faux
            if (Playfield.instance.grid[(int)roundedPos.x, (int)roundedPos.y] != null && //S�il y a qqch � x,y dans le tableau
                Playfield.instance.grid[(int)roundedPos.x, (int)roundedPos.y].parent != transform) //Et si le parent de ce bloc n�est pas le m�me que le n�tre (pour qu�il puisse collisionner avec lui m�me))
                return false;
        }
        return true; //Sinon, la position est valide
    }
   /// <summary>
   /// V�rifie que chaque bloc est � la bonne place et est bien li� � son tetramino
   /// </summary>
    void UpdateGrid()
    {
        for (int i = 0; i < Playfield.instance.h; i++)//On v�rifie de haut en bas
        {
            for (int j = 0; j < Playfield.instance.w; j++)//Et de gauche � droite
            {
                if (Playfield.instance.grid[j, i] != null)//S�il y a qqch � i,j
                    if (Playfield.instance.grid[j, i].parent == transform)//Et que le parent de ce qqch est le n�tre
                        Playfield.instance.grid[j, i] = null;//On efface ce qu�il y a � cet endroit
            }
        }
        foreach (Transform child in transform)//Pour chaque Transform dans le transform = pour chaque enfant
        {
            var roundedPos = Playfield.instance.RoundVec2(child.position);//On arrondit la position de l�enfant pour la stocker dans roundedPos
            Playfield.instance.grid[(int)roundedPos.x, (int)roundedPos.y] = child;//On met le transform � cette position
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Time.time - lastFall >= Playfield.instance.tempsAvantChuteAuto) //Si la diff�rence entre le temps �coul� depuis le d�but du jeu et celui depuis la derni�re chute est sup�rieur � la variable de chute automatique
        {
            Chute();//On descend automatiquement
            lastFall = Time.time;//on reset la variable contenant le temps depuis la derni�re chute
        }
    }
    /// <summary>
    /// D�sactive les inputs � la d�sactivation du script (pour �viter des bugs ?)
    /// </summary>
    private void OnDisable()
    {
        Inputs.Disable();
    }
}
