using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

[RequireComponent(typeof(Rigidbody2D))]
[ExecuteAlways]
public class Drone_FSM : MonoBehaviour
{
    public string current_State;
    Vector2 dir_To_Move;
    Vector2 target;
    [SerializeField] SpriteRenderer m_Sr;
    Light2D detection_Light;
    
    // States
    const string Patrol_State = "Patrol";
    const string Chase_State = "Chase";
    const string Wait_State = "Wait";

    [Header("Detection Settings")]
    [SerializeField] float max_Detection_Angle;
    [SerializeField] float max_detection_Dist;

    [Header("Attack Settings")]
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float max_Shoot_Angle;
    [SerializeField] Transform shoot_Pos;
    [SerializeField] float time_Btw_Shots;
    float t_Btw_Shots;

    [Header("Patrol")]
    [SerializeField] float patrol_Speed;
    [SerializeField] float dist_To_Wait;
    [SerializeField] float start_Wait_Time;
    public Vector2[] spots;
    float wait_Time;
    int spots_Interator;

    [Header("Chase")]
    [SerializeField] float chase_Speed;
    [SerializeField] float dist_To_Stop_Moving;
    [SerializeField] float dist_To_Stop_Chase;
    [SerializeField] float start_time_Waiting_For_Player;
    float time_Waiting_For_Player;

    Rigidbody2D rb;
    [SerializeField] Rigidbody2D player;

    #region Mono

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        detection_Light = GetComponentInChildren<Light2D>();
        wait_Time = start_Wait_Time;

        Enter_Patrol();
    }

    private void Update()
    {
        if (Application.IsPlaying(gameObject)) // Play logic
        {

        }
        else // Editor logic
        {

            GameObject player_GO = GameObject.FindGameObjectWithTag("Player");
            if(player_GO != null)
                player = player_GO.GetComponent<Rigidbody2D>();

            detection_Light = GetComponentInChildren<Light2D>();
            detection_Light.transform.rotation = Quaternion.Euler(0, 0, 180f);
            detection_Light.pointLightInnerRadius = max_detection_Dist;
            detection_Light.pointLightOuterRadius = max_detection_Dist + 2f;
            detection_Light.pointLightInnerAngle = 2 * max_Detection_Angle;
            detection_Light.pointLightOuterAngle = 2 * max_Detection_Angle + 20f;
        }
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
            default:
                Debug.LogError("Current state has no valid value");
                break;
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
        detection_Light.enabled = true;
    }

    void Patrol()
    {
        if (Check_If_Detected(player.position, rb.position, max_Detection_Angle, max_detection_Dist))
        {
            Enter_Chase();
            return;
        }

        if (Mathf.Abs(rb.position.x - target.x) < dist_To_Wait)
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
        detection_Light.enabled = false;
        t_Btw_Shots = time_Btw_Shots;
        current_State = Chase_State;
    }

    void Chase()
    {
        target = player.position;

        // STOP CHASE IF PLAYER IS ABOVE MY POSITION
        if (target.y > rb.position.y)
        {
            Enter_Wait();
            return;
        }

        float dist_To_Target = Vector2.Distance(transform.position, target);

        // STOP CHASE BY DISTANCE
        if(dist_To_Target > dist_To_Stop_Chase)
        {
            Enter_Wait();
        }
        // CHASE
        else
        {
            float x_Dist = Mathf.Abs(target.x - transform.position.x);

            if(x_Dist > dist_To_Stop_Moving)
                Move(target, chase_Speed);
        }

        Flip(target, transform.position);

        // SHOOT
        if (t_Btw_Shots <= 0)
        {
            if (Check_If_Detected(target, rb.position, max_Shoot_Angle))
            {
                Shoot();
                t_Btw_Shots = time_Btw_Shots;
            }
        }
        else
            t_Btw_Shots -= Time.deltaTime;
    }

    #endregion

    #region Wait

    void Enter_Wait()
    {
        current_State = Wait_State;
        time_Waiting_For_Player = start_time_Waiting_For_Player;
        detection_Light.enabled = true;
    }

    void Wait()
    {
        if (Check_If_Detected(player.position, rb.position, max_Detection_Angle, max_detection_Dist))
        {
            Enter_Chase();
            return;
        }

        if (time_Waiting_For_Player <= 0)
            Enter_Patrol();
        else
            time_Waiting_For_Player -= Time.fixedDeltaTime;
    }

    #endregion

    void Flip(Vector2 target_Pos, Vector2 my_Pos)
    {
        if(target_Pos.x > my_Pos.x)
            m_Sr.flipX = false;
        else
            m_Sr.flipX = true;
    }

    void Move(Vector2 target, float speed)
    {
        dir_To_Move = new Vector2(target.x, rb.position.y) - rb.position;
        dir_To_Move.Normalize();

        rb.position += dir_To_Move * speed * Time.deltaTime;  
    }

    bool Check_If_Detected(Vector2 target_Pos, Vector2 m_Pos, float max_Angle, float max_Dist = 0f)
    {
        if (max_Dist == 0f || Vector2.Distance(m_Pos, target_Pos) < max_Dist)
        {
            Vector2 dir_To_Player = target_Pos - m_Pos;
            dir_To_Player.Normalize();
            //Debug.DrawLine(m_Pos, m_Pos + Vector2.down);
            //Debug.DrawLine(m_Pos, m_Pos + dir_To_Player * max_Dist);
            float angle = Vector2.Angle(Vector2.down, dir_To_Player);

            if (angle < max_Angle)
            {
                return true;
            }
        }

        return false;
    }

    public void Shoot()
    {
        Bullet_Movement bullet = PoolManager.SpawnObject(bulletPrefab, shoot_Pos.position, Quaternion.identity).GetComponent<Bullet_Movement>();

        if (bullet != null)
            bullet.Initialize((player.position - rb.position).normalized);
        else
        {
            Debug.Log(gameObject.name + " can not shoot the bullet prefab, because it doesn't have the Bullet_Moviment component");
            return;
        }
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

        Gizmos.color = Color.red;
        float right_Angle = (max_Detection_Angle - 90) * Mathf.Deg2Rad;
        Vector2 right_Angle_Vector = new Vector2(Mathf.Cos(right_Angle), Mathf.Sin(right_Angle));
        float left_Angle = (-max_Detection_Angle - 90) * Mathf.Deg2Rad;
        Vector2 left_Angle_vector = new Vector2(Mathf.Cos(left_Angle), Mathf.Sin(left_Angle));


        Gizmos.DrawLine(rb.position, rb.position + right_Angle_Vector * max_detection_Dist);
        Gizmos.DrawLine(rb.position, rb.position + left_Angle_vector * max_detection_Dist);

    }
}