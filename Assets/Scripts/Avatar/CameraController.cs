using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Transform player;
    private Vector3 offset = new Vector3(-0.19f, 2.64f, -1.47f);
    private void LateUpdate()
    {
        gameObject.transform.position = offset + player.position;
    }
}
