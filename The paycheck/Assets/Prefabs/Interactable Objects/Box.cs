using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    public GameObject box;
    public Vector3 startingPosition;
    private bool didItOnce = false;
    void Start()
    {
        startingPosition = box.transform.position;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "PlayerMeleeAttack" && didItOnce == false)
        {
            didItOnce = true;
            Instantiate(box, startingPosition, Quaternion.identity);
            Destroy(box);
        }
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "PlayerMeleeAttack" && didItOnce == false)
        {
            didItOnce = true;
            Instantiate(box, startingPosition, Quaternion.identity);
            Destroy(box);
        }
    }
}
