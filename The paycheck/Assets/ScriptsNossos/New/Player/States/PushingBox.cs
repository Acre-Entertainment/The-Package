using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PushingBox : State<Player_FSM>
{
    [SerializeField] private float pushSpeed;
    private float hor_Input = 0f;

    public override void Enter(Player_FSM player)
    {
        Debug.Log("Entrou");
    }

    public override void Update(Player_FSM player)
    {
        if (player.m_Input.Interact())
        {
            player.inv.UnlockItem(2);
            player.Switch_State(player.idle_State);
        }
    }

    public override void FixedUpdate(Player_FSM player)
    {
        if(player.HasBox() == false)
        {
            player.inv.UnlockItem(2);
            player.Switch_State(player.idle_State);
            return;
        }


        player.m_Input.Horizontal(out hor_Input);

        player.Move(player.m_rb, new Vector2(hor_Input, 0), pushSpeed);
        player.Move(player.box, new Vector2(hor_Input, 0), pushSpeed);

        if(player.box.position.x > player.transform.position.x)
        {
            if(hor_Input > 0)
            {
                player.anim_Handler.PlayAnim(AnimationsPlayer.DRAGGINGFORWARD);
                return;
            }

            player.anim_Handler.PlayAnim(AnimationsPlayer.DRAGGINGBACKWARD);
        }
        else
        {
            if (hor_Input > 0)
            {
                player.anim_Handler.PlayAnim(AnimationsPlayer.DRAGGINGBACKWARD);
                return;
            }

            player.anim_Handler.PlayAnim(AnimationsPlayer.DRAGGINGFORWARD);
        }
    }
}