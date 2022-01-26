using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Inventory))]
public class AnimationsPlayer : MonoBehaviour
{
    public Animator bodyAnim;
    public Animator rightArmAnim;
    public Animator leftArmAnim;

    private int animBeignPlayed;

    //bool rightArmLocked; // If locked , don't change animation
    //bool leftArmLocked;  // If locked, don't chane animation

    Inventory inventory;

    // Const
    public static int JUMP = Animator.StringToHash("Jump");
    public static int FALL = Animator.StringToHash("Fall");
    public static int RUN = Animator.StringToHash("Run");
    public static int CROUCH = Animator.StringToHash("Crouch");
    public static int CRAWL = Animator.StringToHash("Crawl");
    public static int CROUCHIDLE = Animator.StringToHash("Crouch_Idle");
    public static int STAND = Animator.StringToHash("Stand");
    public static int IDLE = Animator.StringToHash("Idle");
    public static int SWINGING = Animator.StringToHash("Swing");
    public static int KICK = Animator.StringToHash("Kick");
    public static int DRAGGINGFORWARD = Animator.StringToHash("DraggingForward");
    public static int DRAGGINGBACKWARD = Animator.StringToHash("DraggingBackward");
    public static int SHOOT = Animator.StringToHash("Shoot");
    public static int DEATH = Animator.StringToHash("Death");
    public static int CLIMB = Animator.StringToHash("Climb");
    public static int CLIMBIDLE = Animator.StringToHash("ClimbIdle");

    private void Awake()
    {
        Physics2D.queriesStartInColliders = false;
        Physics2D.queriesHitTriggers = false;

        Global_Events.SelectItemEvent += ChangeArmAnimator;
        inventory = GetComponent<Inventory>();
    }

    /// <summary>
    /// Plays animation on body and available arm animator
    /// </summary>
    public void PlayAnim(int animID, bool ignoreLockState = false)
    {
        animBeignPlayed = animID;   
        bodyAnim.Play(animBeignPlayed);

        UpdateArmAnimation(ignoreLockState);
    }

    public void UpdateArmAnimation(bool ignoreLockState = false)
    {
        if (inventory.rightHandEquippedItem.isBeingUsed == false || ignoreLockState)
        {
            if(rightArmAnim.HasState(0, animBeignPlayed))
            {
                inventory.rightHandEquippedItem.isBeingUsed = false;
                //rightArmAnim.Play(animBeignPlayed, 0, 0);
                rightArmAnim.Play(animBeignPlayed);
            }
            else
                Debug.LogWarning("Right animator do not have the current body animation");
        }

        if (inventory.leftHandEquippedItem.isBeingUsed == false || ignoreLockState)
        {
            if (leftArmAnim.HasState(0, animBeignPlayed))
            {
                inventory.leftHandEquippedItem.isBeingUsed = false;
                //leftArmAnim.Play(animBeignPlayed, 0, 0);
                leftArmAnim.Play(animBeignPlayed);
            }
            else
                Debug.LogWarning("Left animator do not have the current body animation");
        }
    }

    public void PlayArmAnim(int animID, Arm arm)
    {
        if (arm == Arm.Left && inventory.leftHandEquippedItem.isBeingUsed == false)
        {
            leftArmAnim.Play(animID);
            //leftArmLocked = true;
            //inventory.leftHandEquippedItem.isBeingUsed = true;
            //StartCoroutine(SwitchToBodyAnim(animID, arm));
        }
        if(arm == Arm.Right && inventory.rightHandEquippedItem.isBeingUsed == false)
        {
            rightArmAnim.Play(animID, 0, 0);
            
            //rightArmLocked = true;
            //inventory.rightHandEquippedItem.isBeingUsed = true;
            //StartCoroutine(SwitchToBodyAnim(animID, arm));
        }
    }

    public void ChangeArmAnimator(Item item)
    {
        if(item.animators.itemType == ItemType.BothHands)
        {
            leftArmAnim = ChangeAnimator(leftArmAnim, item.animators.leftAnimator);
            rightArmAnim = ChangeAnimator(rightArmAnim, item.animators.rightAnimator);

            return;
        }

        if (item.animators.itemType == ItemType.RightHandOnly)
        {
            rightArmAnim = ChangeAnimator(rightArmAnim, item.animators.rightAnimator);

            if (leftArmAnim == null)
                leftArmAnim = ChangeAnimator(leftArmAnim, item.animators.leftAnimator);

            return;
        }

        if (item.animators.itemType == ItemType.LeftHandOnly)
        {
            leftArmAnim = ChangeAnimator(leftArmAnim, item.animators.leftAnimator);

            if (rightArmAnim == null)
                rightArmAnim = ChangeAnimator(rightArmAnim, item.animators.rightAnimator);

            return;
        }
    }

    Animator ChangeAnimator(Animator from, Animator to)
    {
        if (from != null)
            from.gameObject.SetActive(false);

        to.gameObject.SetActive(true);
        to.Play(animBeignPlayed, 0, bodyAnim.GetCurrentAnimatorStateInfo(0).normalizedTime);

        return to;
    }

    //public void UnlockArms()
    //{
    //    inventory.leftHandEquippedItem.isBeingUsed = false;
    //    inventory.rightHandEquippedItem.isBeingUsed = false;

    //    UpdateArmAnimation();
    //}

    public void PlayDeathAnim()
    {
        rightArmAnim.gameObject.SetActive(false);
        leftArmAnim.gameObject.SetActive(false);

        bodyAnim.Play(DEATH);
    }

    public void OnDestroy()
    {
        Global_Events.SelectItemEvent -= ChangeArmAnimator;
    }
}

public enum Arm
{
    Left,
    Right
}