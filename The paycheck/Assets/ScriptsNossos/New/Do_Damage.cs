using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Do_Damage : MonoBehaviour
{
    [Header("Damage")]
    public int damage = 1;
    [SerializeField]
    private bool continuouslyDamage;
    [SerializeField]
    private float timeToDamageAgain = 1f;

    [Header("Collider Info")]
    [SerializeField]
    private Transform coll_Pos;
    [SerializeField]
    private float radius = 1f;
    [SerializeField]
    private LayerMask damageable_Layers;
    [SerializeField]
    private Collider2D[] collidersToIgnore;

    private Collider2D[] hits;
    public List<Health_System> healthDamaged = new List<Health_System>();

    [Header("Debug")]
    [SerializeField]
    private Color debug_Color;

    private void FixedUpdate()
    {
        if(continuouslyDamage)
            Damage();
    }

    private void Damage()
    {
        if (coll_Pos == null || radius <= 0 || damage == 0)
            return;

        Vector2 pos = new Vector2(coll_Pos.position.x, coll_Pos.position.y);

        hits = Physics2D.OverlapCircleAll(pos, radius, damageable_Layers);
        //Debug.Log(hits.Length);

        foreach (Collider2D coll in hits)
        {
            Health_System otherHealth = coll.transform.GetComponent<Health_System>();

            if (otherHealth == null)
                continue;

            if (IsInsideIgnoreList(coll))
                continue;

            if (healthDamaged.Contains(otherHealth))
                continue;

            otherHealth.Hurt(gameObject.tag, damage);
            healthDamaged.Add(otherHealth);
            StartCoroutine(RemoveFromDamagedList(otherHealth));
        }
    }

    public void ActivateContinuousDamage()
    {
        continuouslyDamage = true;
    }

    public void DeactivateContinuousDamage()
    {
        continuouslyDamage = false;
    }

    IEnumerator RemoveFromDamagedList(Health_System health)
    {
        yield return new WaitForSeconds(timeToDamageAgain);

        healthDamaged.Remove(health);
    }

    private bool IsInsideIgnoreList(Collider2D coll)
    {
        foreach(Collider2D ignoreColl in collidersToIgnore)
        {
            if (coll == ignoreColl)
                return true;
        }

        return false;
    }

    private void OnEnable()
    {
        healthDamaged.Clear();
    }

    private void OnDrawGizmos()
    {
        if(coll_Pos != null)
        {
            Gizmos.color = debug_Color;
            Gizmos.DrawWireSphere(coll_Pos.position, radius);
        }
    }
}