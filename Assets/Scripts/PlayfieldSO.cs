using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName="Playfield", menuName = "GameSO/Playfield")]
public class PlayfieldSO : ScriptableObject
{
    [SerializeField] private int w = 10; //Largeur du Playfield
    [SerializeField] private int h = 30; //Hauteur du Playfield
    [SerializeField] private UnityEvent rowCompleted;
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
    /// Vérifie que la position des blocs est entre les bordures et ne collisionne pas avec un autre Tétramino
    /// </summary>
    /// <returns></returns>
    public bool IsValidGridPos(Transform transform)//on met le transform du tetramino en paramètre
    {
        foreach (Transform child in transform)//Pour chaque Transform dans le transform = pour chaque enfant
        {
            Vector2 roundedPos = RoundOffsetVec2(child.position);//On arrondit la position de l’enfant pour la stocker dans roundedPos
            if (!InsideBorder(roundedPos))
                return false;//Si la position de l’enfant n’est pas entre les bordures, on renvoie faux
            if (grid[(int)roundedPos.x, (int)roundedPos.y] != null && //S’il y a qqch à x,y dans le tableau
                grid[(int)roundedPos.x, (int)roundedPos.y].parent != transform) //Et si le parent de ce bloc n’est pas le même que le nôtre (pour qu’il puisse collisionner avec lui même))
                return false;
        }
        return true; //Sinon, la position est valide
    }
    /// <summary>
    /// Vérifie que chaque bloc est à la bonne place et est bien lié à son tetramino
    /// </summary>
    public void UpdateGrid(Transform transform)
    {
        for (int i = 0; i < h; i++)//On vérifie de haut en bas
        {
            for (int j = 0; j < w; j++)//Et de gauche à droite
            {
                if (grid[j, i] != null)//S’il y a qqch à i,j
                    if (grid[j, i].parent == transform)//Et que le parent de ce qqch est le nôtre
                        grid[j, i] = null;//On efface ce qu’il y a à cet endroit
            }
        }
        foreach (Transform child in transform)//Pour chaque Transform dans le transform = pour chaque enfant
        {
            var roundedPos = RoundOffsetVec2(child.position);//On arrondit la position de l’enfant pour la stocker dans roundedPos
            grid[(int)roundedPos.x, (int)roundedPos.y] = child;//On met le transform à cette position
        }
    }

    /// <summary>
    /// Pour arrondir un vecteur (nécessaire avec les rotations)
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
            Destroy(grid[i, y].gameObject);
            grid[i, y] = null;
        }
        rowCompleted.Invoke(); //On invoque l'événement de ligne complétée pour le score
    }

    /// <summary>
    /// Pour baisser d’une case tous les blocs d’une ligne y
    /// </summary>
    /// <param name="y">Position de la ligne qu’on va baisser</param>
    public void DecreaseRow(int y)
    {
        for (int i = 0; i < w; i++)
        {
            if (grid[i, y] != null)
            {
                grid[i, y - 1] = grid[i, y]; //la case d’en dessous devient ce qu’il y avait au dessus
                grid[i, y] = null; //on vide le dessus
                grid[i, y - 1].position += Vector3.down;
            }
        }
    }

    /// <summary>
    /// Pour baisser toutes les lignes de y jusqu’en haut du playfield
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
    /// Vérifie si le vecteur 2 pos (pour position) ne dépasse pas du playfield
    /// </summary>
    /// <param name="pos"></param>
    /// <returns>Le résultat du test logique demandant si pos est bien entre les bordures</returns>
    public bool InsideBorder(Vector2 pos)
    {
        return ((int)pos.x >= 0 && // on cast le float pos.x en int car on ne peut pas avoir d’index à virgule
                (int)pos.x < w &&
                (int)pos.y >= 0);
    }

    /// <summary>
    /// Vérifie si une ligne est pleine (si c’est le cas il faut la supprimer)
    /// </summary>
    /// <param name="y"></param>
    /// <returns>Vrai si la ligne est pleine, faux si au moins une case est vide</returns>
    public bool IsRowFull(int y)
    {
        for (int i = 0; i < w; i++)
        {
            if (grid[i, y] == null) //dès qu’un bloc est vide, la ligne n’est pas pleine et on peut arrêter de vérifier
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
        for (int i = 0; i < h; i++)//De tout en bas jusqu’en haut
        {
            if (IsRowFull(i))//Si la ligne est pleine
            {
                DeleteRow(i);//On l’efface
                DecreaseRowsAbove(i + 1);//On baisse toutes les lignes au dessus de celle qu’on vient d’effacer
                --i;//On baisse i de un pour revérifier si la ligne est pleine
            }
        }
    }
}
