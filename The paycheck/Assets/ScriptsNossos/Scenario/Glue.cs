using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Glue : MonoBehaviour
{
    public List<Rigidbody2D> rbs = new List<Rigidbody2D>();

    Vector3 lastFramePos;
    public Vector3 posDelta;
    BoxCollider2D boxColl;

    private void Start()
    {
        lastFramePos = transform.position;
        boxColl = GetComponent<BoxCollider2D>();
    }

    private void FixedUpdate()
    {
         posDelta = transform.position - lastFramePos;

        foreach (Rigidbody2D rb in rbs)
        {
            rb.transform.position += posDelta;
        }

        lastFramePos = transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<Rigidbody2D>() != null)
        {
            if (IsCollisionContactInsideGlueArea(collision.GetContact(0).point))
                rbs.Add(collision.rigidbody);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Rigidbody2D>() != null)
            rbs.Remove(collision.rigidbody);
    }

    bool IsCollisionContactInsideGlueArea(Vector2 contactPos)
    {
        if (contactPos.y < transform.position.y)
            return false;

        float leftColliderEdge = transform.position.x - (boxColl.size.x * 0.5f * transform.localScale.x);
        float rightColliderEdge = transform.position.x + (boxColl.size.x  * 0.5f * transform.localScale.x);

        if (contactPos.x > leftColliderEdge && contactPos.x < rightColliderEdge)
            return true;

        return false;
    }
}