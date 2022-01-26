using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Patroling : MonoBehaviour
{
    public float speed;
    public float distance;
    private bool movingRight = true;
    public float dist_To_Wait;
    public float start_Wait_Time;
    float wait_Time;
    public Transform[] move_Spot;
    Vector2[] spots;
    Vector2 target;
    Rigidbody2D rb;
    

    private void Start() 
    {
        rb = GetComponent<Rigidbody2D>();
        wait_Time = start_Wait_Time;

        if(move_Spot.Length < 2)
        {
            Debug.Log("NOTE: Esse objeto PRECISA de 2 move Spots");
            gameObject.SetActive(false);
            return;
        }

        spots = new Vector2[move_Spot.Length];

        for(int i = 0; i < move_Spot.Length; i++)
        {
            spots[i] = new Vector2(move_Spot[i].position.x, move_Spot[i].position.y);
        }

        target = spots[0];
    }

    //void Update()
    //{
     //transform.Translate(Vector2.right * speed * Time.deltaTime);
        //if(movingRight == true)
        //{
            //transform.eulerAngles = new Vector3(0, -180, 0);
            //movingRight = false;
        //} else {
            //transform.eulerAngles = new Vector3(0, 0, 0);
            //movingRight = true;
        //}
    //}

    void Flip(Vector2 target)
    {
        if(target.x > transform.position.x)
           transform.rotation = Quaternion.Euler(0f,180f,0f);
        else
           transform.rotation = Quaternion.Euler(0f,0f,0f);
    }


    private void FixedUpdate() 
    {

        if(Vector2.Distance(rb.position, target) < dist_To_Wait)
        {
            if(wait_Time < 0)
            {
                wait_Time = start_Wait_Time;
                foreach(Vector2 spot in spots)
                {
                    if(spot != target)
                    {
                        target = spot;
                        Flip(target);
                        break;
                    }
                }   
            }
            else
            {
                wait_Time -= Time.deltaTime;
                return;
            }
        }

        Vector2 dir = new Vector2(target.x, target.y) - rb.position;
        dir.Normalize();

        rb.position += dir * speed * Time.deltaTime;
    }
}