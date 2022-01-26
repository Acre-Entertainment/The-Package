using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Crouch_State : State<Player_FSM>
{
    [SerializeField]
    private float crouch_Speed;
    [SerializeField]
    private Collider2D crouching_Collider;
    [SerializeField]
    private Collider2D standing_Collider;
    [SerializeField]
    private float minDisplacement = 0.02f;

    float hor_Input = 0f;

    public override void Enter(Player_FSM player)
    {
        Crouch();
    }

    public override void Update(Player_FSM player)
    {
        #region Input
        if (player.CheckItemEquipped(Hand.Right, Player_FSM.GUN))
        {
            if (player.m_Input.Shoot())
            {
                player.anim_Handler.PlayArmAnim(AnimationsPlayer.SHOOT, Arm.Right);
                return;
            }
        }

        if (player.m_Input.Uncrouch())
        {
            player.audioPlayer.PlayClip(Player_FSM.clipCrouch, false);
            player.anim_Handler.PlayAnim(AnimationsPlayer.STAND, true);
            player.Switch_State(player.idle_State, AnimationsPlayer.STAND);
        }

        if (player.m_Input.Horizontal(out float hor_Input))
        {
            if(player.Displacement() > minDisplacement && hor_Input != 0)
                player.anim_Handler.PlayAnim(AnimationsPlayer.CRAWL);
            else
                player.anim_Handler.PlayAnim(AnimationsPlayer.CROUCHIDLE);
        }

        #endregion
    }

    public override void FixedUpdate(Player_FSM player)
    {
        player.m_Input.Horizontal(out hor_Input);
        player.Move(player.m_rb, new Vector2(hor_Input, 0), crouch_Speed);
        player.Flip();
    }

    public override void Exit(Player_FSM player)
    {
        Stand();
    }

    void Crouch()
    {
        standing_Collider.enabled = false;
        crouching_Collider.enabled = true;
    }

    void Stand()
    {
        crouching_Collider.enabled = false;
        standing_Collider.enabled = true;
    }
}