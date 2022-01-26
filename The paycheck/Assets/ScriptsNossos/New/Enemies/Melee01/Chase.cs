using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MeleeEnemyFSM
{
    [System.Serializable]
    public class Chase : State<MeleeEnemyFSM>
    {
        [SerializeField]
        float chaseSpeed;
        [SerializeField]
        float distToStopChase;
        [SerializeField]
        float timeWaitingForPlayer;
        [SerializeField]
        float timeBlindlyChasing = 5f;

        float timeWaiting = 0;
        float blindChaseTime;

        public override void Enter(MeleeEnemyFSM fsm)
        {
            if(fsm.findTarget)
            {
                blindChaseTime = timeBlindlyChasing;
                fsm.findTarget = false;
            }

            timeWaiting = 0;
        }

        public override void Update(MeleeEnemyFSM fsm) {}

        public override void FixedUpdate(MeleeEnemyFSM fsm)
        {
            if (fsm.waitingAnimationEnd)
                return;

            // STOP CHASE
            if (StopChase(fsm))
                return;

            // ATTACK
            if (fsm.TryToAttack())
                return;

            // CHASE
            fsm.Move(fsm.player.GetMain().position, chaseSpeed);
        }

        bool StopChase(MeleeEnemyFSM fsm)
        {
            if(blindChaseTime <= 0)
            {
                if (fsm.player.GetClosestDist(fsm.pointOfView.position) > distToStopChase)
                {
                    fsm.m_Anim.Play("Idle");

                    if (timeWaiting > timeWaitingForPlayer)
                        fsm.EnterState(fsm.patrol);

                    timeWaiting += Time.fixedDeltaTime;
                    return true;
                }

                return false;
            }

            blindChaseTime -= Time.fixedDeltaTime;
            return false;
        }

        public override void DrawGizmos(MeleeEnemyFSM fsm)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(fsm.transform.position, distToStopChase);
        }
    }
}