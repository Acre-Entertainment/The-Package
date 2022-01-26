using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Air_State : State<Player_FSM>
{
	[SerializeField] private bool air_Control = false;                          // Whether or not a player can steer while jumping;
    [SerializeField] private float air_Mov_Speed;
    float hor_Input = 0f;

    public override void Enter(Player_FSM player)
    {
    }

    public override void Update(Player_FSM player)
    {
        if (player.CheckItemEquipped(Hand.Right, Player_FSM.GUN))
        {
            if (player.m_Input.Shoot())
                player.anim_Handler.PlayArmAnim(AnimationsPlayer.SHOOT, Arm.Right);
        }

        if (player.CheckItemEquipped(Hand.Right, Player_FSM.ROPE))
        {
            if (player.m_Input.GrappleShoot())
            {
                player.Switch_State(player.swinging_State);
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
    }

    public override void FixedUpdate(Player_FSM player)
    {
        if(player.m_rb.velocity.y <= 0)
        {
            player.anim_Handler.PlayAnim(AnimationsPlayer.FALL);

            // Check if landed
            if (player.grounded)
            {
                //Debug.Log("NOTE: Player landed");
                player.audioPlayer.PlayClip(Player_FSM.clipLand, false);
                player.Switch_State(player.idle_State);
            }
        }


        if (air_Control)
        {
            player.m_Input.Horizontal(out hor_Input);
            player.Move(player.m_rb, new Vector2(hor_Input, 0), air_Mov_Speed);
        }

        player.Flip();
    }
}