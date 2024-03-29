using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName="Playfield", menuName = "GameSO/Playfield")]
public class PlayfieldSO : ScriptableObject
{
    [SerializeField] private int w = 10; //Largeur du Playfield
    [SerializeField] private int h = 30; //Hauteur du Playfield
    [SerializeField] private UnityEvent rowCompleted;
    [SerializeField] private UnityEvent suddenDrop;
    [SerializeField] GameEventSO tetrisEvent;
    private Vector2 playfieldPosition;
    private Transform[,] grid;
    
    public void Init(Transform playfieldOrigin)
    {
        playfieldPosition = RoundVec2(playfieldOrigin.position);
    }

    private void OnEnable()
    {
        grid = new Transform[w, h];
    }

    /// <summary>
    /// V�rifie que la position des blocs est entre les bordures et ne collisionne pas avec un autre T�tramino
    /// </summary>
    /// <returns></returns>
    public bool IsValidGridPos(Transform transform)//on met le transform du tetramino en param�tre
    {
        foreach (Transform child in transform)//Pour chaque Transform dans le transform = pour chaque enfant
        {
            Vector2 roundedPos = RoundOffsetVec2(child.position);//On arrondit la position de l�enfant pour la stocker dans roundedPos
            if (!InsideBorder(roundedPos))
                return false;//Si la position de l�enfant n�est pas entre les bordures, on renvoie faux
            if (grid[(int)roundedPos.x, (int)roundedPos.y] != null && //S�il y a qqch � x,y dans le tableau
                grid[(int)roundedPos.x, (int)roundedPos.y].parent != transform) //Et si le parent de ce bloc n�est pas le m�me que le n�tre (pour qu�il puisse collisionner avec lui m�me))
                return false;
        }
        return true; //Sinon, la position est valide
    }
    /// <summary>
    /// V�rifie que chaque bloc est � la bonne place et est bien li� � son tetramino
    /// </summary>
    public void UpdateGrid(Transform transform)
    {
        for (int i = 0; i < h; i++)//On v�rifie de haut en bas
        {
            for (int j = 0; j < w; j++)//Et de gauche � droite
            {
                if (grid[j, i] != null)//S�il y a qqch � i,j
                    if (grid[j, i].parent == transform)//Et que le parent de ce qqch est le n�tre
                        grid[j, i] = null;//On efface ce qu�il y a � cet endroit
            }
        }
        foreach (Transform child in transform)//Pour chaque Transform dans le transform = pour chaque enfant
        {
            var roundedPos = RoundOffsetVec2(child.position);//On arrondit la position de l�enfant pour la stocker dans roundedPos
            grid[(int)roundedPos.x, (int)roundedPos.y] = child;//On met le transform � cette position
        }
    }

    /// <summary>
    /// Pour arrondir un vecteur (n�cessaire avec les rotations)
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public Vector2 RoundVec2(Vector2 v)
    {
        return new Vector2(Mathf.Round(v.x),
                           Mathf.Round(v.y));
    }

    public Vector2 RoundOffsetVec2(Vector2 v)
    {
        return RoundVec2(v) - playfieldPosition;
    }

    /// <summary>
    /// Supprime chaque bloc de la ligne y
    /// </summary>
    /// <param name="y"></param>
    public void DeleteRow(int y)
    {

        for (int i = 0; i < w; i++)
        {
            var animators = grid[i, y].gameObject.GetComponentsInChildren<Animator>();//On getcomponent tous les animators de chaque block
            foreach (var animator in animators)//Pour chaque animator
            {
                animator.enabled = true;//On l'active
            }
        }
    }

    public void EmptyBlock(Vector2 position)
    {
        var roundedPos = RoundOffsetVec2(position);//On arrondit la position du block pour avoir un int
        grid[(int)roundedPos.x, (int)roundedPos.y] = null;//On vide ce qu'il y a � cette position
        if (IsRowEmpty((int)roundedPos.y))
            DecreaseRowsAbove((int)roundedPos.y);//On baisse toutes les lignes au dessus de celle qu�on vient d�effacer
    }

    /// <summary>
    /// Pour baisser d�une case tous les blocs d�une ligne y
    /// </summary>
    /// <param name="y">Position de la ligne qu�on va baisser</param>
    public void DecreaseRow(int y)
    {
        for (int i = 0; i < w; i++)
        {
            if (grid[i, y] != null)
            {
                grid[i, y - 1] = grid[i, y]; //la case d�en dessous devient ce qu�il y avait au dessus
                grid[i, y] = null; //on vide le dessus
                grid[i, y - 1].position += Vector3.down;
            }
        }
    }

    /// <summary>
    /// Pour baisser toutes les lignes de y jusqu�en haut du playfield
    /// </summary>
    /// <param name="y"></param>
    public void DecreaseRowsAbove(int y)
    {
        for (int i = y; i < h; i++)
        {
            DecreaseRow(i);
        }
    }

    /// <summary>
    /// V�rifie si le vecteur 2 pos (pour position) ne d�passe pas du playfield
    /// </summary>
    /// <param name="pos"></param>
    /// <returns>Le r�sultat du test logique demandant si pos est bien entre les bordures</returns>
    public bool InsideBorder(Vector2 pos)
    {
        return ((int)pos.x >= 0 && // on cast le float pos.x en int car on ne peut pas avoir d�index � virgule
                (int)pos.x < w &&
                (int)pos.y >= 0);
    }

    /// <summary>
    /// V�rifie si une ligne est pleine (si c�est le cas il faut la supprimer)
    /// </summary>
    /// <param name="y"></param>
    /// <returns>Vrai si la ligne est pleine, faux si au moins une case est vide</returns>
    public bool IsRowFull(int y)
    {
        for (int i = 0; i < w; i++)
        {
            if (grid[i, y] == null) //d�s qu�un bloc est vide, la ligne n�est pas pleine et on peut arr�ter de v�rifier
            {
                return false;
            }
        }
        return true;
    }

    public bool IsRowEmpty(int y)
    {
        for (int i = 0; i < w; i++)
        {
            if (grid[i, y] != null) //d�s qu�un bloc est plein, la ligne n�est pas vide et on peut arr�ter de v�rifier
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// Efface toutes les lignes pleines dans tout le Playfield
    /// </summary>
    public void DeleteFullRows()
    {
        var rowsCounter = 0;
        for (int i = 0; i < h; i++)//De tout en bas jusqu�en haut
        {
            if (IsRowFull(i))//Si la ligne est pleine
            {
                rowsCounter++;//On incr�mente le compteur de lignes pour savoir si on fait un Tetris
                DeleteRow(i);//On l�efface
            }
        }
        if (rowsCounter == 4)
            tetrisEvent.Raise();
        else
        { 
            for (int i = 0; i < rowsCounter; i++)
            {
                rowCompleted.Invoke();
            }
        }
    }
}
