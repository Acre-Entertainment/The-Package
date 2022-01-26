using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Localize_On_Enable : MonoBehaviour
{
    public Simple_Localization simple_Localization;

    private void OnEnable() 
    {
        Translate();
    }

    public void Translate()
    {
        if (GetComponent<TextMeshProUGUI>() != null)
        {
            GetComponent<TextMeshProUGUI>().text = Global_Localization.Translated_Text(simple_Localization);
        }
        else
        if (GetComponent<TextMeshPro>() != null)
        {
            GetComponent<TextMeshPro>().text = Global_Localization.Translated_Text(simple_Localization);
        }
        else
        if (GetComponent<Text>() != null)   
            GetComponent<Text>().text = Global_Localization.Translated_Text(simple_Localization);
    }
}