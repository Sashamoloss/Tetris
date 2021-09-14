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
    /// Descend le transform du Tetramino d'un cran en bas puis v�rifie si la position est valide
    /// Si elle ne l'est pas, remonte le Tetramino d'un cran, efface les lignes pleines, spawn la prochaine pi�ce et d�truit ce script
    /// </summary>
    private void FonctionChute()
    {
        transform.position += Vector3.down;//On descend le transform d'un cran
        if (playfield.IsValidGridPos(transform))//Si la position est valide, on update la grille
            playfield.UpdateGrid(transform);
        //Sinon on remonte le transform, on efface les lignes pleines car �a veut dire qu'on est arriv�s tout en bas
        //Puis on fait spawner le prochain tetramino avant de d�truire ce script pour ne pas d�placer le tetramino tomb� tout en bas
        else
        {
            transform.position += Vector3.up;
            playfield.DeleteFullRows();
            FindObjectOfType<Spawneur>().SpawnNext();
            GetComponent<PlayerInput>().enabled = false;
            Destroy(this);//On d�truit ce script pour �viter de contr�ler plusieurs pi�ces � la fois
            //� faire: d�truire les autres scripts qui contr�lent le tetramino (?)
        }
    }

    /// <summary>
    /// Fait tomber le tetramino en boucle (gr�ce � la fonction chute) en attendant un temps donn� entre chaque chute (la fonction chute s'arr�te automatiquement en bas)
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
    /// Fait tomber le tetramino en boucle (gr�ce � la fonction chute) jusqu'� atteindre le bas, o� la fonction chute s'arr�te automatiquement
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
    /// Lance la coroutine de chute instantan�e
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
    /// Lance la coroutine de chute en boucle tant qu'on appuie sur le bouton, l'arr�te si on n'appuie plus
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
        if (Time.time - lastFall >= timeBeforeAutoDrop.value) //Si la diff�rence entre le temps �coul� depuis le d�but du jeu et celui depuis la derni�re chute est sup�rieur � la variable de chute automatique
        {
            FonctionChute();//On descend automatiquement
            lastFall = Time.time;//on reset la variable contenant le temps depuis la derni�re chute
        }
    }
    private void OnDestroy()
    {
        StopAllCoroutines();//On arr�te toutes les coroutines � la destruction du script
    }
}
