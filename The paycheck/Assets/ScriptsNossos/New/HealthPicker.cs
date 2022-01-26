using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class HealthPicker : MonoBehaviour
{
    [SerializeField]
    private int heal;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.GetComponent<Player_FSM>())
        {
            Health_System health = other.GetComponent<Health_System>();

            health.Heal(heal);

            Destroy(this.gameObject);
        }
    }
}