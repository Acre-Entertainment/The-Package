using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthSystem : MonoBehaviour
{
    public Image[] healthIcons;
    public HealthSystemAttrCondition health;
    //public HealthSystemAttrCondition health;
    private float previousHealth;

    void Start()
    {
        //previousHealth = health.health;
        //UpdateHealth(health.health);
    }

    void Update()
    {
        /*
        if(previousHealth != health.health)
        {
            UpdateHealth(health.health);
        }
        */
    }

    void UpdateHealth(int health)
    {
        for(int i = 0; i < healthIcons.Length; i++)
        {
            if (i < health)
                healthIcons[i].enabled = true;
            else
                healthIcons[i].enabled = false;
        }

        previousHealth = health;
    }
}
