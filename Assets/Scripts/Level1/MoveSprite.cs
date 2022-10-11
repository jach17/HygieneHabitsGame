using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSprite : MonoBehaviour
{
    [SerializeField]
    float speed = 5;

    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        //Physics2D.gravity = new Vector2(9.8f,0.0f);
        //GetComponent<Rigidbody>().velocity = transform.up * speed;
        rb = GetComponent<Rigidbody2D>();
        speed = Random.Range(2, 5);
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = transform.position * Time.deltaTime * speed;
    }

    private void FixedUpdate()
    {
        rb.velocity = transform.right * speed;
       
    }
}
