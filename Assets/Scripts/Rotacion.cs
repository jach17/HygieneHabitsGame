using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotacion : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private float speedRotation = 1.5f;
    [SerializeField]
    private float speedPosition = 1.2f;
    [SerializeField]
    private float distance = 0.2f;
    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(0, speedRotation, 0);
        transform.position = startPosition + new Vector3(0.0f, Mathf.Sin(Time.time * speedPosition) * distance, 0.0f);
        //this.transform.Translate(0, 0.5f, 0);
        //StartCoroutine(BloquearSeleccion(2f));
        //this.transform.Translate(0, 0, 0);

    }
    IEnumerator BloquearSeleccion(float tiempo)
    {
       
        yield return new WaitForSeconds(tiempo);
    
    }
}
