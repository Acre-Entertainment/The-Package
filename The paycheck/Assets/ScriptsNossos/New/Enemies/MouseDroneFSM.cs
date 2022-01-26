using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseDroneFSM : MonoBehaviour
{
    [Header("Moviment")]
    [SerializeField] private LookingDir lookingDir;
    [SerializeField]
    private float speed = 2f;

    [Header("Detection")]
    [SerializeField] public Transform pointOfView;
    [SerializeField] private Vector2 detectionField;
    [SerializeField] private float detectionDist;

    [HideInInspector] public PointsOfDetection player;

    [Header("Chase")]
    [SerializeField]
    private float distToStopChase = 3f;
    [SerializeField]
    private float distToExplode = 3f;

    [SerializeField]
    private GameObject explosionFX;
    private Rigidbody2D rb;
    private Animator anim;
    bool chasing;
    bool exploding;

    private void Start()
    {
        Physics2D.queriesStartInColliders = false;

        EnemiesManager manager = GetComponentInParent<EnemiesManager>();
        if (manager != null)
            player = manager.playerDetection;
        else
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<PointsOfDetection>();

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (exploding)
            return;

        Flip();

        if(!chasing)
        {
            anim.Play("Idle");

            if (Detect())
                chasing = true;

            return;
        }

        Chase();
    }
    
    void Flip()
    {
        if(player.GetMain().position.x > transform.position.x)
        {
            lookingDir = LookingDir.Right;
            GetComponent<SpriteRenderer>().flipX = false;
            return;
        }

        lookingDir = LookingDir.Left;
        GetComponent<SpriteRenderer>().flipX = true;
    }

    void Chase()
    {
        float distToPlayer = player.GetClosestDist(transform.position);
        if (distToPlayer > distToStopChase)
        {
            chasing = false;
            return;
        }

        if(distToPlayer < distToExplode)
        {
            StartExplosion();
        }

        if(!IsThereGroundAhead())
        {
            // TODO: Idle Animation
            anim.Play("Idle");
            return;
        }

        anim.Play("Running");
        Vector2 dir = player.GetMain().position.x > transform.position.x ? Vector2.right : -Vector2.right;
        
        rb.position += dir * (speed * Time.deltaTime);        
    }

    void StartExplosion()
    {
        exploding = true;
        anim.Play("Explode");
    }

    public void Explode()
    {
        Instantiate(explosionFX, transform.position, Quaternion.identity);
        Destroy(gameObject, 0.2f);
    }

    public bool IsThereGroundAhead(bool checkForGroundAhead = true)
    {
        RaycastHit2D hit;
        Vector2 posOffset;

        if (checkForGroundAhead)
            posOffset = lookingDir == LookingDir.Left ? Vector2.left : Vector2.right;
        else
            posOffset = lookingDir == LookingDir.Left ? Vector2.right : Vector2.left;

        hit = Physics2D.Raycast(rb.position + posOffset, Vector2.down, 5f);

        if (hit.collider != null)
            return true;

        return false;
    }

    #region Detection

    public bool Detect()
    {
        // Distancia
        if (player.GetClosestDist(pointOfView.position) > detectionDist)
            return false;

        // Vetor de visão
        Vector2 lookDir = lookingDir == LookingDir.Right ? Vector2.right : Vector2.left;
        Debug.DrawRay(pointOfView.position, lookDir * detectionDist, Color.black);

        // Tem que checar cada um dos pontos do player
        foreach (Transform point in player.Points)
        {
            // Vetor até alvo
            Vector2 toPlayerDir = point.position - pointOfView.position;
            Debug.DrawRay(pointOfView.position, toPlayerDir * detectionDist, Color.black);

            // Angulo entre os dois
            float angle = Vector2.Angle(lookDir, toPlayerDir);

            // Ver se enquadra no limite de detecção
            if (point.position.y > pointOfView.position.y)
            {
                if (angle < detectionField.x)
                    return RaycastCheck(pointOfView.position, toPlayerDir);
            }

            if (angle < Mathf.Abs(detectionField.y))
                return RaycastCheck(pointOfView.position, toPlayerDir);
        }

        return false;
    }

    bool RaycastCheck(Vector2 origin, Vector2 dir)
    {
        ContactFilter2D filter = new ContactFilter2D().NoFilter();
        RaycastHit2D hit = Physics2D.Raycast(origin, dir, detectionDist);

        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Player"))
                return true;
        }

        return false;
    }

    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, distToExplode);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, distToStopChase);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(pointOfView.position, detectionDist);

        if (lookingDir == LookingDir.Right)
        {
            // FOV
            Gizmos.DrawLine(pointOfView.position, pointOfView.position + Vector3.right * detectionDist);

            Gizmos.color = Color.yellow;

            // UP
            Vector3 dir = MoreMath.FromAngleToVector(detectionField.x * Mathf.Deg2Rad);
            Gizmos.DrawLine(pointOfView.position, pointOfView.position + dir * detectionDist);

            // Down
            dir = MoreMath.FromAngleToVector(detectionField.y * Mathf.Deg2Rad);
            Gizmos.DrawLine(pointOfView.position, pointOfView.position + dir * detectionDist);
        }
        else
        {
            // FOV
            Gizmos.DrawLine(pointOfView.position, pointOfView.position + Vector3.left * detectionDist);

            Gizmos.color = Color.yellow;

            // UP
            Vector3 dir = MoreMath.FromAngleToVector((180 - detectionField.x) * Mathf.Deg2Rad);
            Gizmos.DrawLine(pointOfView.position, pointOfView.position + dir * detectionDist);

            // Down
            dir = MoreMath.FromAngleToVector((-180 - detectionField.y) * Mathf.Deg2Rad);
            Gizmos.DrawLine(pointOfView.position, pointOfView.position + dir * detectionDist);
        }
    }
}