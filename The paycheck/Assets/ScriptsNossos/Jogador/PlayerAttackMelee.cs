using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackMelee : MonoBehaviour
{
    private float timeBtwAttack;
    public float startTimeBtwAttack;
    public Transform attackPos;
    public LayerMask whatIsEnemies;
    public Animator camAnim;
    public Animator playerAnim;
    public float attackRangeX;
    public float attackRangeY;
    public int damage;

    void Update()
    {
        if(timeBtwAttack <= 0)
        {
            if(Input.GetButtonDown("Punch"))
            {
              camAnim.SetTrigger("Shake");
              playerAnim.SetTrigger("attack");
              Collider2D[] enemiesToDamage = Physics2D.OverlapBoxAll(attackPos.position, new Vector2(attackRangeX, attackRangeY), 0, whatIsEnemies);
              for (int i = 0; i < enemiesToDamage.Length; i++)
              {
                  enemiesToDamage[i].GetComponent<EnemyHealth >().health -= damage;
              }
            }

            timeBtwAttack = startTimeBtwAttack;
        } else {
            timeBtwAttack -= Time.deltaTime;
        }
    } 

    void OnDrawGizmosSelected() 
    {
      Gizmos.color = Color.red;
      Gizmos.DrawWireCube(attackPos.position, new Vector3(attackRangeX, attackRangeY, 1));  
    }
}
