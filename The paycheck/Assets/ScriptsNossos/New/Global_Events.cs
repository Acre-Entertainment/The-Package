using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public static class Global_Events
{
    #region Update GUI

    public static event System.Action<int> Update_Health_GUI_Event;
    public static event System.Action<Item[]> Update_Inventory_GUI_Event;
    public static event System.Action<Item> SelectItemEvent;

    public static void UpdateHealthGUI(int current_Health)
    {
        Update_Health_GUI_Event?.Invoke(current_Health);
    }

    public static void UpdateInventoryGUI(Item[] items)
    {
        if(items != null)
            Update_Inventory_GUI_Event?.Invoke(items);
    }

    public static void SelectItem(Item item)
    {
        SelectItemEvent?.Invoke(item);
    }

    #endregion

    #region Dialogue

    public static event System.Action<Dialogue_Localization, UnityEvent> Open_Dialogue_Event;
    public static event System.Action<bool> Dialogue_Changed_State_Event;

    public static void OpenDialogue(Dialogue_Localization dialogue, UnityEvent close_Event)
    {
        Open_Dialogue_Event?.Invoke(dialogue, close_Event);
        Dialogue_Changed_State_Event?.Invoke(true);
    }

    public static void CloseDialogue()
    {
        Dialogue_Changed_State_Event?.Invoke(false);
    }

    #endregion

    public static event System.Action onPlayerDeath;

    public static void PlayerDied()
    {
        onPlayerDeath?.Invoke();
    }

}