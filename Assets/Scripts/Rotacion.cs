using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotacion : MonoBehaviour
{
    // Start is called before the first frame update
   

    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(0, 2, 0);
        //this.transform.Translate(0, 0.5f, 0);
        //StartCoroutine(BloquearSeleccion(2f));
        //this.transform.Translate(0, 0, 0);

    }
    IEnumerator BloquearSeleccion(float tiempo)
    {
       
        yield return new WaitForSeconds(tiempo);
    
    }
}
