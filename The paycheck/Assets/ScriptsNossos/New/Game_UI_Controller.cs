using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game_UI_Controller : MonoBehaviour
{
    [Header("Health_GUI")]
    [SerializeField] Sprite full_Heart;
    [SerializeField] Sprite half_Heart;
    [SerializeField] Sprite empty_Heart;
    [SerializeField] Image[] health_Images;
    [SerializeField] Image fadeIn;
    [SerializeField] float fadeDuration = 2f;
    [Space]
    [Header("Inventory_GUI")]
    [SerializeField] GameObject[] storage_Space;
    string[] item_Name;
    string right_Arm_Item_Name;
    string left_Arm_Item_Name;



    private void Awake() 
    {
        Global_Events.Update_Health_GUI_Event += Update_Health_GUI;
        Global_Events.Update_Inventory_GUI_Event += Update_Inventory_GUI;
        Global_Events.SelectItemEvent += Update_Item_Selection;
        Global_Events.onPlayerDeath += PlayerDeath;
    }

    private void OnDestroy()
    {
        Global_Events.Update_Health_GUI_Event -= Update_Health_GUI;
        Global_Events.Update_Inventory_GUI_Event -= Update_Inventory_GUI;
        Global_Events.SelectItemEvent -= Update_Item_Selection;
        Global_Events.onPlayerDeath -= PlayerDeath;
    }

    void Update_Health_GUI(int current_Health)
    {
        if(current_Health > health_Images.Length * 2)
            current_Health = health_Images.Length * 2;

        int current_Gui_Health = 0;

        for(int i = 0; i < health_Images.Length; i++)
        {
            if(current_Gui_Health + 2 <= current_Health)
            {
                current_Gui_Health += 2;
                health_Images[i].sprite = full_Heart;
            }
            else if(current_Gui_Health + 1 == current_Health)
            {
                current_Gui_Health += 1;
                health_Images[i].sprite = half_Heart;
            }
            else
                health_Images[i].sprite = empty_Heart;
        }

    }

    void Update_Inventory_GUI(Item[] items)
    {
        item_Name = new string[items.Length];
        for(int i = 0; i < storage_Space.Length; i++)
        {
            if(i < items.Length)
            {
                // Mostrat item no storage
                storage_Space[i].SetActive(true);
                storage_Space[i].transform.GetChild(0).GetComponent<Image>().sprite = items[i].sprite;
                item_Name[i] = items[i].name;
            }
            else
            {
                // Esconder esse espaço do storage
                storage_Space[i].SetActive(false);
            }
        }
    }

    void Update_Item_Selection(Item item_To_Select)
    {
        // Ver se o item é da mão direita (Verde) pega item da direita e deseleciona e seleciona esse
        // Ver se é o da esquerda /////
        // Se for de duas mãos deseleciona tudo e seleciona esse

        if (item_To_Select.animators.itemType == ItemType.BothHands)
        {
            for (int i = 0; i < item_Name.Length; i++)
            {
                if (item_Name[i] == item_To_Select.name)
                    storage_Space[i].GetComponent<Image>().color = Color.red;
                else
                    storage_Space[i].GetComponent<Image>().color = Color.white;
            }
            right_Arm_Item_Name = left_Arm_Item_Name = item_To_Select.name;
        }
        else if (item_To_Select.animators.itemType == ItemType.RightHandOnly)
        {
            for (int i = 0; i < item_Name.Length; i++)
            {
                if (item_Name[i] == item_To_Select.name)
                    storage_Space[i].GetComponent<Image>().color = Color.green;
                else if (item_Name[i] == right_Arm_Item_Name)
                    storage_Space[i].GetComponent<Image>().color = Color.white;
            }
            right_Arm_Item_Name = item_To_Select.name;
        }
        else

        {
            for (int i = 0; i < item_Name.Length; i++)
            {
                if (item_Name[i] == item_To_Select.name)
                    storage_Space[i].GetComponent<Image>().color = Color.blue;
                else if (item_Name[i] == left_Arm_Item_Name)
                    storage_Space[i].GetComponent<Image>().color = Color.white;
            }
            left_Arm_Item_Name = item_To_Select.name;
        }
    }

    void PlayerDeath()
    {
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        float fade = 0f;

        while(fade < 1)
        {
            Color fadeColor = fadeIn.color;
            fadeColor.a = fade;

            fadeIn.color = fadeColor;

            fade += Time.deltaTime / fadeDuration;

            yield return null;
        }
    }

}