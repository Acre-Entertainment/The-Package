using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableBox : MonoBehaviour
{
    private Rigidbody2D rb;
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        gameObject.transform.eulerAngles = new Vector3(0, 0, 90);
    }
    void OnTriggerStay2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "Gloves")
        {
            rb.constraints = RigidbodyConstraints2D.None;
        }
    }
    void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "Gloves")
        {
            rb.constraints = RigidbodyConstraints2D.FreezePositionX;
        }
    }
}
