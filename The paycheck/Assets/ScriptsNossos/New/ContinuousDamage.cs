using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinuousDamage : MonoBehaviour
{
    [SerializeField]
    private int damage;
    [SerializeField]
    private float timeToDamageAgain;

    [SerializeField]
    List<Health_System> healths = new List<Health_System>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Health_System health = collision.GetComponent<Health_System>();

        if (health != null)
        {
            healths.Add(health);
            StartCoroutine(TimeToDamage(health));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Health_System>() != null)
        {
            healths.Remove(collision.GetComponent<Health_System>());
        }
    }

    IEnumerator TimeToDamage(Health_System health)
    {
        if (healths.Contains(health))
            health.Hurt(this.gameObject.tag, damage);

        yield return new WaitForSeconds(timeToDamageAgain);

        if (healths.Contains(health))
            StartCoroutine(TimeToDamage(health));
    }

}
