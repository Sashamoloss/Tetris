using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawneur : MonoBehaviour
{
    [SerializeField] private GameObject[] tetraminos;
    [SerializeField] private PlayfieldSO playfield;
    // Start is called before the first frame update
    void Start()
    {
        playfield.Init(transform.parent);
        SpawnNext();
    }

    // Update is called once per frame
    /// <summary>
    /// Instancie un nouveau Tetramino de façon totalement aléatoire
    /// </summary>
    public void SpawnNext()
    {
        int i = Random.Range(0, tetraminos.Length);
        Instantiate(tetraminos[i], transform.position, Quaternion.identity,transform.parent);
    }
}
