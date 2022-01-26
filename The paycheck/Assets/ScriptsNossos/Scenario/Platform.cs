using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Platform : MonoBehaviour
{
    public Transform pos1, pos2;
    public float speed;
    public Transform startPos;

    Vector3 nextPos;

    // Glue
    Rigidbody2D playerRb;

    Vector3 lastFramePos;
    Vector2 posDelta;
    BoxCollider2D boxColl;
    public LookingDir lookDir;

    public static Platform currentPlatform;


    void Start()
    {
        nextPos = startPos.position;

        lastFramePos = transform.position;
        boxColl = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if(transform.position == pos1.position)
        {
            nextPos = pos2.position;
        }
        if(transform.position == pos2.position)
        {
            nextPos = pos1.position;
        }

        transform.position = Vector3.MoveTowards(transform.position, nextPos, speed * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        posDelta = transform.position - lastFramePos;
        SetLookingDir(posDelta.x);

        if (playerRb != null)
            playerRb.position += posDelta;

        lastFramePos = transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (IsCollisionContactInsideGlueArea(collision.GetContact(0).point))
            {
                playerRb = collision.rigidbody;
                currentPlatform = this;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerRb = null;
            currentPlatform = null;
        }
    }

    bool IsCollisionContactInsideGlueArea(Vector2 contactPos)
    {
        if (contactPos.y < transform.position.y)
            return false;

        float leftColliderEdge = transform.position.x - (boxColl.size.x * 0.5f * transform.localScale.x);
        float rightColliderEdge = transform.position.x + (boxColl.size.x * 0.5f * transform.localScale.x);

        if (contactPos.x > leftColliderEdge && contactPos.x < rightColliderEdge)
            return true;

        return false;
    }

    void SetLookingDir(float xDelta)
    {
        if (xDelta > 0)
            lookDir = LookingDir.Right;
        else
            lookDir = LookingDir.Left;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(pos1.position, pos2.position);
    }
}
