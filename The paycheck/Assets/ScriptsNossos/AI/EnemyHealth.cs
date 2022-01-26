using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
   public int health;
   public GameObject deathEffect;

   void Update()
   {
       
       if(health <= 0)
       {
           Destroy(gameObject);
       }
   }

   public void TakeDamage(int damage)
   {
       Instantiate(deathEffect, transform.position, Quaternion.identity);
       health -= damage;
   }
}
