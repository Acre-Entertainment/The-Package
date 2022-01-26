using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Save_Localization_Data : MonoBehaviour
{
    public void Change_Language(string language)
    {
        Global_Localization.Change_Current_Language(language);

        Localization_Data data = new Localization_Data();
        data.current_Language = Global_Localization.Current_Language;

        Global_Localization.Save(data);

        Translate_Active_Objects();
    }

    void Translate_Active_Objects()
    {
        Localize_On_Enable[] localize_On_Enable = FindObjectsOfType<Localize_On_Enable>();

        foreach (Localize_On_Enable localize in localize_On_Enable)
            localize.Translate();
    }
}
