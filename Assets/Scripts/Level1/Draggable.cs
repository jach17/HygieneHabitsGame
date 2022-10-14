using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable : MonoBehaviour
{
    Vector3 mousePossitionOffset;

    private bool canDrag = true;

    public bool CanDrag
    {
        get { return canDrag; }
        set { canDrag = value; }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private Vector3 GetMouseWorldPosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnMouseDown()
    {

        mousePossitionOffset = gameObject.transform.position - GetMouseWorldPosition();
    }
    private void OnMouseDrag()
    {
        if (canDrag)
        {
            transform.position = GetMouseWorldPosition() + mousePossitionOffset;
        }

    }


}
