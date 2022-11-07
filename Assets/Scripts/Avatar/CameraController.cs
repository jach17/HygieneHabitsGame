using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Transform player;
    private Vector3 offset = new Vector3(-.2f, 3.5f, -2.5f);
    private void LateUpdate()
    {
        gameObject.transform.position = offset + player.position;
    }
}
