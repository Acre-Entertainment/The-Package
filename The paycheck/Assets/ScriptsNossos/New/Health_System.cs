using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health_System : MonoBehaviour
{
    public int max_Health;
    [SerializeField] int current_Health;
    public int Current_Health 
    {
        get{
            return current_Health;
        }
    }

    public bool is_Invincible;
    [SerializeField] float invincibility_Time;
    public string[] damage_Tags;
    [Space]
    public UnityEvent death_Effect;
    [SerializeField]
    private UnityEvent onTakeDamage;

    // Events
    public event System.Action onDeath;
    public event System.Action<int> onHurt;
    public event System.Action<int> onHeal;


    private void Awake() 
    {
        if(current_Health <= 0)
            current_Health = max_Health;    
    }

    public void Hurt(string tag_Name, int damage)
    {
        if(is_Invincible)
            return;

        if(Check_Damage_Tag(tag_Name) == false)
            return;

        current_Health -= Mathf.Abs(damage);
        current_Health = Mathf.Clamp(current_Health, 0, max_Health);

        onTakeDamage.Invoke();
        onHurt?.Invoke(Mathf.Abs(damage));

        if(!Check_Death())
            StartCoroutine(Become_Invincible());
    }

    public void Heal(int heal)
    {
        current_Health += Mathf.Abs(heal);
        current_Health = Mathf.Clamp(current_Health, 0, max_Health);

        onHeal?.Invoke(Mathf.Abs(heal));
    }

    bool Check_Death()
    {
        if(current_Health == 0)
        {
            onDeath?.Invoke();
            death_Effect.Invoke();
            return true;
        }

        return false;
    }

    bool Check_Damage_Tag(string tag_Name)
    {
        foreach(string tag in damage_Tags)
        {
            if(tag == tag_Name)
                return true;
        }

        return false;
    }

    public IEnumerator Become_Invincible()
    {
        is_Invincible = true;

        yield return new WaitForSeconds(invincibility_Time);

        is_Invincible = false;
    }

}