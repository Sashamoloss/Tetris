using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Chute : MonoBehaviour
{
    float lastFall = 0;
    private Coroutine coroutineChute;
    [SerializeField] PlayfieldSO playfield;
    [SerializeField] FloatVariable timeBeforeAutoDrop;
    [SerializeField] TetraminoSO config;
    [SerializeField] GameEventSO startSoftDrop;
    [SerializeField] GameEventSO stopSoftDrop;
    [SerializeField] GameEventSO suddenDrop;
    /// <summary>
    /// Descend le transform du Tetramino d'un cran en bas puis vérifie si la position est valide
    /// Si elle ne l'est pas, remonte le Tetramino d'un cran, efface les lignes pleines, spawn la prochaine pièce et détruit ce script
    /// </summary>
    private void FonctionChute()
    {
        transform.position += Vector3.down;//On descend le transform d'un cran
        if (playfield.IsValidGridPos(transform))//Si la position est valide, on update la grille
            playfield.UpdateGrid(transform);
        //Sinon on remonte le transform, on efface les lignes pleines car ça veut dire qu'on est arrivés tout en bas
        //Puis on fait spawner le prochain tetramino avant de détruire ce script pour ne pas déplacer le tetramino tombé tout en bas
        else
        {
            transform.position += Vector3.up;
            playfield.DeleteFullRows();
            FindObjectOfType<Spawneur>().SpawnNext();
            GetComponent<PlayerInput>().enabled = false;
            Destroy(this);//On détruit ce script pour éviter de contrôler plusieurs pièces à la fois
            //à faire: détruire les autres scripts qui contrôlent le tetramino (?)
        }
    }

    /// <summary>
    /// Fait tomber le tetramino en boucle (grâce à la fonction chute) en attendant un temps donné entre chaque chute (la fonction chute s'arrête automatiquement en bas)
    /// </summary>
    /// <returns></returns>
    IEnumerator CoroutineChute()
    {
        while (true)
        {
            yield return new WaitForSeconds(config.timeBeforeSoftDrop);
            FonctionChute();
        }

    }
    /// <summary>
    /// Fait tomber le tetramino en boucle (grâce à la fonction chute) jusqu'à atteindre le bas, où la fonction chute s'arrête automatiquement
    /// </summary>
    /// <returns></returns>
    IEnumerator CoroutineSuddenDrop()
    {
        while (true)
        {
            yield return null;
            FonctionChute();
        }
    }
    /// <summary>
    /// Lance la coroutine de chute instantanée
    /// </summary>
    /// <param name="CBC"></param>
    public void SuddenDrop(InputAction.CallbackContext CBC)
    {
        if (CBC.phase == InputActionPhase.Performed)
        {
            StartCoroutine(CoroutineSuddenDrop());
            suddenDrop.Raise();
        }
    }
    /// <summary>
    /// Lance la coroutine de chute en boucle tant qu'on appuie sur le bouton, l'arrête si on n'appuie plus
    /// </summary>
    /// <param name="CBC"></param>
    public void SoftDrop(InputAction.CallbackContext CBC)
    {
        if (CBC.phase == InputActionPhase.Performed)
        {
            coroutineChute = StartCoroutine(CoroutineChute());
            FonctionChute();
            startSoftDrop.Raise();
        }
        else if (CBC.phase == InputActionPhase.Canceled)
        {
            StopCoroutine(coroutineChute);
            stopSoftDrop.Raise();
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (Time.time - lastFall >= timeBeforeAutoDrop.value) //Si la différence entre le temps écoulé depuis le début du jeu et celui depuis la dernière chute est supérieur à la variable de chute automatique
        {
            FonctionChute();//On descend automatiquement
            lastFall = Time.time;//on reset la variable contenant le temps depuis la dernière chute
        }
    }
    private void OnDestroy()
    {
        StopAllCoroutines();//On arrête toutes les coroutines à la destruction du script
    }
}
