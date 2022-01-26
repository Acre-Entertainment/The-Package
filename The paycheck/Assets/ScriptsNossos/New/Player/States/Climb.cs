using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Climb : State<Player_FSM>
{
    [SerializeField] private float climbSpeed;

    Vector2 moveDir = Vector2.zero;
    float gravity;

    public override void Enter(Player_FSM player)
    {
        player.m_rb.velocity = Vector2.zero;
        gravity = player.m_rb.gravityScale;

        player.m_rb.gravityScale = 0;
    }

    public override void Update(Player_FSM player)
    {
        if (player.m_Input.Vertical(out float verInput))
        {
            moveDir.y = verInput;
        }

        if (player.m_Input.Jump())
        {
            player.Jump();
            player.Switch_State(player.air_State);
            return;
        }
    }

    public override void FixedUpdate(Player_FSM player)
    {
        if (player.HasWall() == false)
        {
            player.Switch_State(player.air_State);
            return;
        }

        if (moveDir.y != 0)
            player.anim_Handler.PlayAnim(AnimationsPlayer.CLIMB);
        else
            player.anim_Handler.PlayAnim(AnimationsPlayer.CLIMBIDLE);

        player.Move(player.m_rb, moveDir, climbSpeed);
    }

    public override void Exit(Player_FSM player)
    {
        player.m_rb.gravityScale = gravity;
    }
}