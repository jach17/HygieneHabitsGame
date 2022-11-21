using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSprite : MonoBehaviour
{
    [SerializeField]
    private float speed = 5;
    
    private float time =2.2f;
    
    private float increaseSpeed = 1.5f;
    private Rigidbody2D rb;

    private bool ban;
    
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
        if (ban)
        {
            return;
        }
        //transform.position = transform.position * Time.deltaTime * speed;
        time -= Time.deltaTime;
        if (time <= 0)
        {
            speed += increaseSpeed;
            ban = true;
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = transform.right * speed;
       
    }
}
