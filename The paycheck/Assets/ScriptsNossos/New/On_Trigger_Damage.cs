using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class On_Trigger_Damage : MonoBehaviour
{
    [SerializeField] int damage;
    bool isCollisionEnabled = true;
    [SerializeField]
    private GameObject destructionVfx;
    public UnityEvent onDamage;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isCollisionEnabled)
        {
            if (other.GetComponent<Health_System>() != null)
            {
                other.GetComponent<Health_System>().Hurt(gameObject.tag.ToString(), damage);
                onDamage.Invoke();
            }

            if (!other.isTrigger)
                onDamage.Invoke();
        }
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        if(isCollisionEnabled)
        {
            if(other.gameObject.GetComponent<Health_System>() != null)
            {
                other.gameObject.GetComponent<Health_System>().Hurt(gameObject.tag.ToString(), damage);
            }

            onDamage.Invoke();
        }
    }

    private void OnEnable()
    {
        isCollisionEnabled = true;
    }

    private void OnDisable()
    {
        isCollisionEnabled = false;
    }

    public void SelfDestroy()
    {
        if (destructionVfx != null)
            PoolManager.SpawnObject(destructionVfx, transform.position, transform.rotation);

        PoolManager.ReleaseObject(this.gameObject);
    }
}