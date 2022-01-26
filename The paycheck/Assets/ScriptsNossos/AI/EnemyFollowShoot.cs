using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollowShoot : MonoBehaviour
{
    public float speed;
    public float stoppingDistance;
    public float retreatDistance;
    private float timeBtwShots;
    public float startTimeBtwShots;
    public GameObject EnemyProjectile;
    private Transform player;
    
    void Start () 
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; 
        timeBtwShots = startTimeBtwShots;
    }

    void Update ()
    {
        Debug.DrawLine(transform.position, transform.position + transform.right, Color.red, 1f);
        
        Face_Player(player);

        if(Vector2.Distance(transform.position, player.position) > stoppingDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

        } else if(Vector2.Distance(transform.position, player.position) < stoppingDistance && Vector2.Distance(transform.position, player.position) > retreatDistance){
            
            transform.position = this.transform.position;

        } else if(Vector2.Distance(transform.position, player.position) < retreatDistance){
            
            transform.position = Vector2.MoveTowards(transform.position, player.position, -speed * Time.deltaTime);
        }

        if(timeBtwShots <= 0)
        {
          GameObject bullet = Instantiate(EnemyProjectile, transform.position, Quaternion.identity);
          bullet.GetComponent<IAProjectile>().Change_Dir(-transform.right);
          timeBtwShots = startTimeBtwShots;
        } else {
            timeBtwShots -= Time.deltaTime;
        }
    }

    void Face_Player(Transform player)
    {
        if(player.position.x < transform.position.x)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
    }
}

