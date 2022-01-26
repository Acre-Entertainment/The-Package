using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Input : MonoBehaviour
{
    [HideInInspector] public bool readInput = true;   

    [SerializeField] private KeyCode[] jump;
    [SerializeField] private KeyCode[] crouch;
    [SerializeField] private KeyCode[] uncrouch;
    [SerializeField] private KeyCode[] kick;
    [SerializeField] private KeyCode[] shoot;
    [SerializeField] private KeyCode[] inventory;
    [SerializeField] private KeyCode[] grapple_Hook;
    [SerializeField] private KeyCode[] interact;

    public bool CanProcessInput()
    {
        return readInput;
    }

    public bool Horizontal(out float horInput)
    {
        horInput = 0;
        if (CanProcessInput())
        {
            horInput = Input.GetAxis("Horizontal");            
            return true;
        }
        return false;
    }

    public bool Vertical(out float verInput)
    {
        verInput = 0;
        if (CanProcessInput())
        {
            verInput = Input.GetAxisRaw("Vertical");
            return true;
        }

        return false;
    }

    public bool Jump()
    {
        if (CanProcessInput())
        {
            foreach (KeyCode code in jump)
            {
                if (Input.GetKeyDown(code))
                    return true;
            }
        }

        return false;
    }

    public bool Crouch()
    {
        if (CanProcessInput())
        {
            foreach (KeyCode code in crouch)
            {
                if (Input.GetKeyDown(code))
                    return true;
            }
        }

        return false;
    }

    public bool Uncrouch()
    {
        if (CanProcessInput())
        {
            foreach (KeyCode code in uncrouch)
            {
                if (Input.GetKeyDown(code))
                    return true;
            }
        }

        return false;
    }

    public bool Kick()
    {
        if (CanProcessInput())
        {
            foreach (KeyCode code in kick)
            {
                if (Input.GetKeyDown(code))
                    return true;
            }
        }

        return false;
    }

    public bool Shoot()
    {
        if (CanProcessInput())
        {
            foreach (KeyCode code in shoot)
            {
                if (Input.GetKeyDown(code))
                    return true;
            }
        }

        return false;
    }

    public bool Interact()
    {
        if(CanProcessInput())
        {
            foreach (KeyCode code in interact)
            {
                if (Input.GetKeyDown(code))
                    return true;
            }
        }

        return false;
    }

    public bool InventorySpace(out int key_Index)
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            if (Input.GetKeyDown(inventory[i]))
            {
                key_Index = i;
                return true;
            }
        }

        key_Index = -1;
        return false;
    }

    public bool GrappleShoot()
    {
        if (CanProcessInput())
        {
            foreach (KeyCode code in grapple_Hook)
            {
                if (Input.GetKeyDown(code))
                    return true;
            }
        }

        return false;
    }

}