using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Load_Localization_Data : MonoBehaviour
{
    private void Awake()
    {
        Localization_Data data = new Localization_Data();

        data = Global_Localization.Load();

        if(data != null)
            Global_Localization.Change_Current_Language(data.current_Language);
    }
}
