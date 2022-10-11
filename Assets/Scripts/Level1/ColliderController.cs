using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderController : MonoBehaviour
{
    [SerializeField]
    private Level1Minigame level1;
    [SerializeField]
    private string gameObjectTag;
    // Start is called before the first frame update
    void Start()
    {
        level1 = FindObjectOfType<Level1Minigame>();
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
        level1.AddPoint();
        Destroy(collision.gameObject);
        
    }
}
