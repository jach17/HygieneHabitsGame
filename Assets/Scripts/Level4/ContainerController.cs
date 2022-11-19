using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerController : MonoBehaviour
{
    [SerializeField]
    private Level4min1Manager level4;
    [SerializeField]
    private string gameObjectTag;
    // Start is called before the first frame update
    void Start()
    {
        level4 = FindObjectOfType<Level4min1Manager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != gameObjectTag)
        {
            return;
        }
        level4.AddPoint();
        Destroy(collision.gameObject);

    }
}
