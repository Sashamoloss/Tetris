using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playfield : MonoBehaviour
{
    public static Playfield instance;
    private int w = 10;
    private int h = 20;
    private Transform[,] grid;
    // Start is called before the first frame update
    private void Awake()
    {
        if (instance != null)
        { 
            Destroy(this);
        }
        instance = this;
    }
    void Start()
    {
        grid = new Transform[w, h];
    }
    private  Vector2 RoundVec2(Vector2 v)
    {
        return new Vector2(Mathf.Round(v.x),
                           Mathf.Round(v.y));
    }

    private void DeleteRow(int y)
    {

        for (int i = 0; i < w; i++)
        {
            Destroy(grid[i, y].gameObject);
            grid[i, y] = null;
        }
    }

    /// <summary>
    /// N’importe quoi en texte
    /// </summary>
    /// <param name="y"></param>
    private void DecreaseRow(int y)
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

    private void DecreaseRowsAbove(int y)
    {
        for (int i = y; i < h; i++)
        {
            DecreaseRow(i);
        }
    }

    /// <summary>
    /// renvoie le résultat du test logique demandant si pos est bien entre les bordures
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    private bool InsideBorder (Vector2 pos)
    {
        return ((int)pos.x >= 0 &&
                (int)pos.x >= w &&
                (int)pos.y >= 0);
    }

    private bool IsRowFull (int y)
    {
        for (int i = 0; i < w; i++)
        {
            if (grid[i, y] == null)
            { 
                return false;
            }
        }            
        return true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
