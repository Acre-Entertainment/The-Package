using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Idle_State : State<Player_FSM>
{
    public override void Enter(Player_FSM player)
    {
        // Play idle animation
        player.anim_Handler.PlayAnim(AnimationsPlayer.IDLE);
    }

    public override void Update(Player_FSM player)
    {
        #region Input

        if(player.CheckItemEquipped(Hand.Right, Player_FSM.GUN))
        {
            if(player.m_Input.Shoot())
                player.anim_Handler.PlayArmAnim(AnimationsPlayer.SHOOT, Arm.Right);
        }

        if (player.CheckItemEquipped(Hand.Right, Player_FSM.GLOVE))
        {
            if (player.m_Input.Interact() && player.HasBox())
            {
                player.Switch_State(player.pushingBox);
                return;
            }
        }

        if (player.CheckItemEquipped(Hand.Right, Player_FSM.MAGNET))
        {
            if (player.m_Input.Interact() && player.HasWall())
            {
                player.Switch_State(player.climb);
                return;
            }
        }

        if (player.m_Input.Horizontal(out float hor_Input))
        {
            if (hor_Input != 0)
                    player.Switch_State(player.run_State);
        }

        if (player.m_Input.Jump())
        {
            player.Jump();
            player.Switch_State(player.air_State);
        }

        if(player.CheckItemEquipped(Hand.Right, Player_FSM.ROPE))
        {
            if (player.m_Input.GrappleShoot())
            {
                player.Switch_State(player.swinging_State);
                return;
            }
        }

        if (player.m_Input.Crouch())
        {
            if (player.CheckItemEquipped(Hand.Right, Player_FSM.ROPE))
                player.inv.SelectItem(Player_FSM.GUN);

            player.audioPlayer.PlayClip(Player_FSM.clipCrouch, false);
            player.anim_Handler.PlayAnim(AnimationsPlayer.CROUCH, true);
            player.Switch_State(player.crouch_State, AnimationsPlayer.CROUCH);
        }

        if (player.m_Input.Kick())
        {
            player.audioPlayer.PlayClip(Player_FSM.clipKick, false);
            player.anim_Handler.PlayAnim(AnimationsPlayer.KICK);
            player.Switch_State(player.idle_State, AnimationsPlayer.KICK);
        }

        #endregion
    }

    public override void FixedUpdate(Player_FSM player)
    {
    }
}