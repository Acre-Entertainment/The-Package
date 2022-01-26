using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet_Movement : MonoBehaviour
{
    public float speed;
    Rigidbody2D m_rb;
    [SerializeField] float angleOffset = 0f;
    [SerializeField]
    //private bool autoDestroy;

    private void Awake() 
    {
        m_rb = GetComponent<Rigidbody2D>();    
    }

    void FixedUpdate()
    {
        Vector2 velocity = transform.right * speed * Time.deltaTime;
        m_rb.MovePosition(m_rb.position + velocity);
    }

    private void OnBecameInvisible() 
    {
        PoolManager.ReleaseObject(this.gameObject);
    }

    public void Initialize(Vector2 targetDir)
    {
        Flip(targetDir);
    }

    void Flip(Vector2 targetDir)
    {
        float angle = Vector2.SignedAngle(transform.right, targetDir);

       transform.Rotate(new Vector3(0, 0, angle + angleOffset));
    }
}