using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Player_Input))]
[RequireComponent(typeof(AnimationsPlayer))]
public class Inventory : MonoBehaviour
{
    public Item rightHandEquippedItem;
    public Item leftHandEquippedItem;

    [Space(10)]
    public List<Item> items = new List<Item>();
    public KeyCode[] keys;

    [HideInInspector] public Player_Input m_Input;
    private AnimationsPlayer animationsPlayer;

    private void Start() 
    {
        m_Input = GetComponent<Player_Input>();
        animationsPlayer = GetComponent<AnimationsPlayer>();

        Global_Events.UpdateInventoryGUI(items.ToArray());
        if(items.Count > 1)
            SelectItem(0);
    }

    private void Update() 
    {
        if(m_Input.InventorySpace(out int i))
                SelectItem(i);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<ItemPicker>())
            AddItem(collision.GetComponent<ItemPicker>().CollectItem());
    }

    public void SelectItem(string itemName)
    {
        int itemIndex = 0;

        for (int i = 0; i < items.Count; i++)
        {
            if(items[i].name == itemName)
            {
                itemIndex = i;
                break;
            }
        }

        SelectItem(itemIndex);
    }

    void SelectItem(int itemToSelect)
    {
        if(itemToSelect >= items.Count)
            return;

        EqupItem(items[itemToSelect]);
    }

    void EqupItem(Item item)
    {
        bool rightBeingUse = rightHandEquippedItem.isBeingUsed;
        bool leftBeingUsed = leftHandEquippedItem.isBeingUsed;
        bool equipItem = false;

        if (item.animators.itemType == ItemType.BothHands)
        {
            if(!rightBeingUse && !leftBeingUsed)
            {
                rightHandEquippedItem.onDisable.Invoke();
                leftHandEquippedItem.onDisable.Invoke();

                rightHandEquippedItem = item;
                leftHandEquippedItem = item;

                equipItem = true;
            }
        }
        else
        if (item.animators.itemType == ItemType.RightHandOnly)
        {
            if (!rightBeingUse)
            {
                rightHandEquippedItem.onDisable.Invoke();
                rightHandEquippedItem = item;

                equipItem = true;

            }
        }
        else
        if(item.animators.itemType == ItemType.LeftHandOnly)
        {
            if(!leftBeingUsed)
            {
                leftHandEquippedItem.onDisable.Invoke();
                leftHandEquippedItem = item;

                equipItem = true;
            }
        }


        if (equipItem == false)
            return;

        if(item.clipId != "")
            GetComponent<AudioPlayer>().PlayClip(item.clipId, false);

        item.onEnable.Invoke();
        Global_Events.SelectItem(item);
    }

    public void AddItem(Item item)
    {
        items.Add(item);
        Global_Events.UpdateInventoryGUI(items.ToArray());
    }

    /// <summary>
    /// 0 = LockRight, 1 LockLeft, 2 LockBoth
    /// </summary>
    public void LockItem(int armToLock)
    {
        if(armToLock == 0)
            rightHandEquippedItem.isBeingUsed = true;

        if (armToLock == 1)
            leftHandEquippedItem.isBeingUsed = true;

        if(armToLock == 2)
        {
            rightHandEquippedItem.isBeingUsed = true;
            leftHandEquippedItem.isBeingUsed = true;
        }
    }

    /// <summary>
    /// 0 = LockRight, 1 LockLeft, 2 LockBoth
    /// </summary>
    public void UnlockItem(int armToUnlock)
    {
        if (armToUnlock == 0)
            rightHandEquippedItem.isBeingUsed = false;

        if (armToUnlock == 1)
            leftHandEquippedItem.isBeingUsed = false;

        if (armToUnlock == 2)
        {
            rightHandEquippedItem.isBeingUsed = false;
            leftHandEquippedItem.isBeingUsed = false;
        }

        animationsPlayer.UpdateArmAnimation();
    }

}


[System.Serializable]
public class Item
{
    public string name;
    public Sprite sprite;
    public bool isBeingUsed;
    public ItemAnimators animators;
    public string clipId;


    [Space]
    public UnityEvent onEnable;
    public UnityEvent onDisable;
}

[System.Serializable]
public class ItemAnimators
{
    public ItemType itemType;
    public Animator rightAnimator;
    public Animator leftAnimator;
}

public enum ItemType
{
    RightHandOnly,
    LeftHandOnly,
    BothHands
}