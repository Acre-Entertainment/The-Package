using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ItemPicker : MonoBehaviour
{
    [SerializeField]
    private Item item;

    [SerializeField]
    private UnityEvent _onPick;

    public Item CollectItem()
    {
        _onPick.Invoke();
        return item;
    }
}