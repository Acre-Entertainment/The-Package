using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Generic_State
{
    // When called, check if everything is alright then Update_State
    public abstract Generic_State Enter();

    // Where the code of the state runs
    public abstract Generic_State Do_State();

    // Clean variables
    public abstract Generic_State Exit();
}