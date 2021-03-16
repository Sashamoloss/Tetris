using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class piece_comportement : MonoBehaviour
{
    private Controles Inputs;
    private Collider2D myCollider; // variable pour appeler le collider de la pièce
    // Start is called before the first frame update
    void OnEnable()
    {
        Inputs = new Controles();
        Inputs.Enable(); // pour les nouvelles inputs de Unity
        Inputs.AM.Rotation.performed += Rotation;
    }

    private void Start()
    {
        myCollider = GetComponent<Collider2D>(); // On va chercher le collider de la pièce pour savoir s'il overlap celui d'une autre
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Rotation (InputAction.CallbackContext CBC)
    {
        //Debug.Log("Rotation: " + transform.rotation);
        transform.RotateAround(transform.GetChild(0).position, Vector3.back, 90 * CBC.ReadValue<float>()); // rotation à 90° dans un sens ou dans l'autre (selon l'input)
        StartCoroutine(VerifOverlap(CBC.ReadValue<float>()));
        //Debug.Log("Rotation: " + transform.rotation);

    }

    IEnumerator VerifOverlap(float SensRotation)
    {
        var results = new Collider2D[1]; // on crée une variable - tableau pour mettre les résultats du check d'overlap
        var filter = new ContactFilter2D(); // on crée une variable pour mettre un filtre nécessaire au check
        yield return null;
        var checkOverlap = Physics2D.OverlapCollider(myCollider, filter.NoFilter(), results);
        while (checkOverlap > 0) { //on check une première fois si ça overlap
            transform.position += Vector3.right;
            yield return null;
            checkOverlap = Physics2D.OverlapCollider(myCollider, filter.NoFilter(), results);
            if (checkOverlap > 0)
            {
                transform.position += Vector3.left * 2;
                yield return null;
                checkOverlap = Physics2D.OverlapCollider(myCollider, filter.NoFilter(), results);
                if (checkOverlap > 0)
                {
                    transform.position += Vector3.right * 2;
                    transform.RotateAround(transform.GetChild(0).position, Vector3.back, 90 * SensRotation * -1 );
                }
            }
            Debug.Log("Colliders: " + checkOverlap);
        }
    }

}
