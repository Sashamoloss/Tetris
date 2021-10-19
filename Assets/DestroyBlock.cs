using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBlock : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Destroy()
    {
        GetComponentInParent<Tetramino>().playfield.EmptyBlock(transform.position);//On vide notre position
        Destroy(gameObject);//On détruit le block
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
