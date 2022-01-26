using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Long_Range_Enemy_FSM : MonoBehaviour
{
    // Patrolling
    // Chasing
    public string current_State;
    Vector2 dir_To_Move;
    Vector2 target;
    [SerializeField] SpriteRenderer m_Sr;
    [SerializeField] Transform detection_Obj;

    // States
    const string Patrol_State = "Patrol";
    const string Chase_State = "Chase";
    const string Wait_State = "Wait";
    const string Attack_State = "Attack";


    [Header("Patrol")]
    [SerializeField] float patrol_Speed;
    [SerializeField] float dist_To_Wait;
    [SerializeField] float start_Wait_Time;
    public Vector2[] spots;
    float wait_Time;
    int spots_Interator;

    [Header("Chase")]
    [SerializeField] float chase_Speed;
    [SerializeField] float dist_To_Attack;
    [SerializeField] float dist_To_Stop_Chase;
    [SerializeField] float start_time_Waiting_For_Player;
    [SerializeField] LayerMask ground_Mask;
    float time_Waiting_For_Player;

    [Header("Attack")]
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform shoot_Pos;
    [SerializeField] float time_Btw_Shots;
    float t_Btw_Shots;

    Rigidbody2D rb;
    Transform player;

    #region Mono

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        wait_Time = start_Wait_Time;

        Enter_Patrol();
    }

    private void FixedUpdate() 
    {
        rb.velocity = new Vector2(0, rb.velocity.y);

        switch (current_State)
        {
            case Patrol_State:
                Patrol();
                break;
            case Chase_State:
                Chase();
                break;
            case Wait_State:
                Wait();
                break;
            case Attack_State:
                Attack();
                break;
            default:
                Debug.LogError("Current state has no valid value");
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        // Se o player entrar nesse trigger
        // Chase    
        if(other.CompareTag("Player") && current_State != Attack_State)
        {
            if(player == null)
                player = other.transform;
            
            Enter_Chase();
        }
    }

    #endregion

    #region Patrol

    void Enter_Patrol()
    {
        current_State = Patrol_State;

        if (spots.Length < 2)
        {
            Debug.Log("NOTE: Esse objeto PRECISA de 2 move Spots");
            gameObject.SetActive(false);
            return;
        }

        spots_Interator = 1;
        target = Find_Closest(transform.position, spots);
        Flip(target, transform.position);
    }

    void Patrol()
    {
        if(Mathf.Abs(rb.position.x - target.x) < dist_To_Wait)
        {
            if(wait_Time < 0)
            {
                wait_Time = start_Wait_Time;

                // Take the next spot

                for(int i = 0; i < spots.Length; i++)
                {
                    if(target == spots[i])
                    {
                        if(i + spots_Interator < 0 || i + spots_Interator == spots.Length)
                            spots_Interator *= -1;
                        
                        target = spots[i + spots_Interator];
                        Flip(target, transform.position);
                        break;
                    }
                } 
            }
            else
            {
                wait_Time -= Time.deltaTime; 
            }
        }
        else
        {
            Move(target, patrol_Speed);
        }
    }

    Vector2 Find_Closest(Vector2 from, Vector2[] pos_To)
    {
        int closest_index = 0;
        float closest_Dist = Mathf.Infinity;
        float dist = 0;

        for(int i = 0; i < pos_To.Length; i++)
        {
            dist = Vector2.Distance(from, pos_To[i]);
            if(dist < closest_Dist)
            {
                closest_Dist = dist;
                closest_index = i;
            }
        }

        return pos_To[closest_index];
    }

    #endregion

    #region Chase

    void Enter_Chase()
    {
        current_State = Chase_State;
    }

    void Chase()
    {
        target = player.position;
        float dist_To_Target = Vector2.Distance(transform.position, target);

        // Check for ground
        RaycastHit2D hit;

        if(m_Sr.flipX)
            hit = Physics2D.Raycast(rb.position + Vector2.left, Vector2.down, 5f, ground_Mask);
        else
            hit = Physics2D.Raycast(rb.position + Vector2.right, Vector2.down, 5f, ground_Mask);

        if(hit.collider == null)
        {
            Debug.Log("Stop chase");
            Enter_Wait();
            return;
        }

        Flip(target, transform.position);

        // STOP CHASE
        if (dist_To_Target > dist_To_Stop_Chase)
            Enter_Wait();
        // ATTACK
        else if(dist_To_Target < dist_To_Attack)
            Enter_Attack();
        // CHASE
        else
            Move(target, chase_Speed);
    }

    #endregion

    #region Wait

    void Enter_Wait()
    {
        current_State = Wait_State;
        time_Waiting_For_Player = start_time_Waiting_For_Player;
    }

    void Wait()
    {
        if(time_Waiting_For_Player <= 0)
            Enter_Patrol();
        else
            time_Waiting_For_Player -= Time.fixedDeltaTime;
    }

    #endregion

    #region Attack

    void Enter_Attack()
    {
        // Iniciar a animação de ataque
        current_State = Attack_State;
        Shoot();
        t_Btw_Shots = time_Btw_Shots;
    }

    void Attack()
    {   
        if(t_Btw_Shots <= 0)
            Enter_Chase();
        else
            t_Btw_Shots -= Time.fixedDeltaTime;
    }

    public void Shoot()
    {
        Bullet_Movement bullet = PoolManager.SpawnObject(bulletPrefab, shoot_Pos.position, Quaternion.identity).GetComponent<Bullet_Movement>();

        if(bullet != null)
            bullet.Initialize(new Vector2(shoot_Pos.localScale.x, 0));
        else
        {
            Debug.Log(gameObject.name + " can not shoot the bullet prefab, because it doesn't have the Bullet_Moviment component");
            return;
        }
    }

    #endregion

    void Flip(Vector2 target_Pos, Vector2 my_Pos)
    {
        if(target_Pos.x > my_Pos.x)
        {
            m_Sr.flipX = false;
            detection_Obj.localScale = new Vector3(1f,1f,1f);
            shoot_Pos.localScale = new Vector3(1f,1f,1f);
        }
        else
        {
            m_Sr.flipX = true;
            detection_Obj.localScale = new Vector3(-1f,1f,1f);
            shoot_Pos.localScale = new Vector3(-1f,1f,1f);
        }
    }

    void Move(Vector2 target, float speed)
    {
        dir_To_Move = new Vector2(target.x, rb.position.y) - rb.position;
        dir_To_Move.Normalize();

        rb.position += dir_To_Move * speed * Time.deltaTime;  
    }

    private void OnDrawGizmosSelected() 
    {
        foreach(Vector2 spot in spots)
        {
            Gizmos.DrawIcon(spot, "Target.png", true);
        }    

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, dist_To_Stop_Chase);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, dist_To_Attack);
    }
}