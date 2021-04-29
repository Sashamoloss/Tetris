using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawneur : MonoBehaviour
{
    [SerializeField] private GameObject[] tetraminos;
    // Start is called before the first frame update
    void Start()
    {
        SpawnNext();
    }

    // Update is called once per frame
    public void SpawnNext()
    {
        int i = Random.Range(0, tetraminos.Length);
        Instantiate(tetraminos[i], transform.position, Quaternion.identity);
    }
}
