using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawneur : MonoBehaviour
{
    [SerializeField] private GameObject[] tetraminos;
#if UNITY_EDITOR
    [SerializeField] private GameObject[] tetraminosDebug; //le tableau des tetraminos à utiliser en mode debug pour tester + facilement
    [SerializeField] private BoolVariable isInDebug;
#endif
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
#if UNITY_EDITOR
        if (isInDebug.value)
            SpawnNextDebug();
        else
            SpawnNextStandard();
#else
        SpawnNextStandard();
#endif
    }

    public void SpawnNextStandard()
    {
        int i = Random.Range(0, tetraminos.Length);
        Instantiate(tetraminos[i], transform.position, Quaternion.identity, transform.parent);
    }

    public void SpawnNextDebug()
    {
        int i = Random.Range(0, tetraminosDebug.Length);
        Instantiate(tetraminosDebug[i], transform.position, Quaternion.identity, transform.parent);
    }

}
