using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

            Game_UI_Controller ui = FindObjectOfType<Game_UI_Controller>();
            ui.GetComponent<Game_UI_Controller>().Update_Health_GUI(health.Current_Health);

            Destroy(this.gameObject);
        }
    }
}