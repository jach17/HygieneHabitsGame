using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    [SerializeField]
    Transform cam;
    [SerializeField]
    private Joystick joystickMove;
    private Rigidbody rb;
    private float rotateV;
    private float rotateH;
    [SerializeField]
    private float speed = 10f;
    [SerializeField]
    private GameObject meshGameobject;
    private Animator animator;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        animator = meshGameobject.GetComponent<Animator>();
        cam = Camera.main.transform;
    }

    void Move()
    {
        rb.velocity = new Vector3(joystickMove.Horizontal * speed + Input.GetAxis("Horizontal"),rb.velocity.y,joystickMove.Vertical * speed + Input.GetAxis("Vertical"));
        
        Vector3 movement = new Vector3(joystickMove.Horizontal, 0.0f, joystickMove.Vertical);
        if (movement.magnitude != 0)
        {
            transform.rotation = Quaternion.LookRotation(movement);
            
        }

        if (animator != null)
        {
            if (movement.magnitude == 0)
            {
                animator.SetFloat("Velocity", 0);
            }
            else
            {
                animator.SetFloat("Velocity", 1);
            }
        }
    }

    private void Update()
    {
        Move();
    }
}
