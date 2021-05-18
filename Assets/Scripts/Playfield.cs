using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playfield : MonoBehaviour
{
    public static Playfield instance;
    public int w = 10; //Largeur du Playfield
    public int h = 20; //Hauteur du Playfield
    public Transform[,] grid;
    public float tempsAvantChuteAuto;
    public float tempsAvantChuteManuelle;
    
    private void Awake()//Pour qu’il n’y ait qu’un seul Playfield par scène
    {
        if (instance != null)
        { 
            Destroy(this);
        }
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        grid = new Transform[w, h];
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
    }

    /// <summary>
    /// Pour baisser d’une case tous les blocs d’une ligne y
    /// </summary>
    /// <param name="y">Position de la ligne qu’on va baisser</param>
    public void DecreaseRow(int y)
    {
        for (int i = 0; i < w; i++)
        {
            if (grid[i,y] !=null)
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
    public bool InsideBorder (Vector2 pos)
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
    public bool IsRowFull (int y)
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
    public void DeleteFullRows ()
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
    //debug
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        if (grid == null)
        {
            return;
        }
        for (int i = 0; i < w; i++)
        {
            for (int j = 0; j < h; j++)
            {
                if (grid[i,j]!=null)
                {
                    Gizmos.DrawCube(grid[i, j].position, Vector3.one);
                }
                
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
