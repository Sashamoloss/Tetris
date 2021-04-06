using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playfield : MonoBehaviour
{
    private static int w = 10;
    private static int h = 20;
    private static int x = 0;
    private Transform[,] grid = new Transform[w, h];
    // Start is called before the first frame update
    void Start()
    {

    }
    private static Vector2 roundVec2(Vector2 v)
    {
        return new Vector2(Mathf.Round(v.x),
                           Mathf.Round(v.y));
    }

    private static DeleteRow()
        {
            for (x=0,x<w)
            {
                Destroy.GameObject
            }
        }

    // Update is called once per frame
    void Update()
    {
        
    }
}
