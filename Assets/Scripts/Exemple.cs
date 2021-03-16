using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exemple : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var myclass = new MyClass(4);
        var mysecondclass = new MyClass(3);
        //myclass.MyInt = 5;
        Debug.Log("myclass :" + myclass.MyInt);
        Debug.Log("mysecondclass :" + mysecondclass.MyInt);
        var myarray = new MyClass[50];
        for (var i = 0; i < myarray.Length; i++)
        {
            myarray[i] = new MyClass(i);
        }
        /*foreach (var classinstance in myarray)
        {
            classinstance = new MyClass(5);
        }*/
        Debug.Log("index 13: " + myarray[12].MyInt);
        var mysecondarray = new MyClass[20, 10];
        var mythirdarray = new MyClass[20][];
        for (var x = 0; x < mysecondarray.GetLength(0); x++)
        { 
        for (var y = 0; y < mysecondarray.GetLength(1); y++)
            {
                mysecondarray[x, y] = new MyClass(4);
            }
        }
        Debug.Log("Second Array: " + mysecondarray[15, 9]);
        Debug.Log("MyInt de SecondArray: " + mysecondarray[15, 9].MyInt);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
