using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LongRangeEnemy
{
    [System.Serializable]
    public class Chase : State<LongRangeEnemyFSM>
    {
        [SerializeField]
        float chaseSpeed;
        [SerializeField]
        float moveAwaySpeed;
        [SerializeField]
        float distToStopChase;
        [SerializeField]
        float distToMoveAway;
        [SerializeField]
        float timeWaitingForPlayer;
        [SerializeField]
        float timeBlindlyChasing = 5f;
        [SerializeField]
        Vector2 greyZone;

        float timeWaiting = 0;
        float blindChaseTime;

        public override void Enter(LongRangeEnemyFSM fsm)
        {
            if (fsm.findTarget)
            {
                blindChaseTime = timeBlindlyChasing;
                fsm.findTarget = false;
            }

            timeWaiting = 0;
        }

        public override void Update(LongRangeEnemyFSM fsm) {}

        public override void FixedUpdate(LongRangeEnemyFSM fsm)
        {
            if (fsm.waitingAnimationEnd)
                return;

            // STOP CHASE
            if (StopChase(fsm))
                return;

            // ATTACK
            if (fsm.TryToAttack())
                return;

            float distToPlayer = fsm.player.GetClosestDist(fsm.pointOfView.position);

            // GREY ZONE
            if (IsInGreyZone(distToPlayer))
            {
                fsm.m_Anim.Play("Idle");
                return;
            }

            // MOVE AWAY
            if (distToPlayer < distToMoveAway)
            {
                fsm.MoveAway(fsm.player.GetMain().position, chaseSpeed);
                return;
            }

            // MOVE TOWARDS
            if (fsm.IsThereGroundAhead())
            {
                fsm.MoveTowards(fsm.player.GetMain().position, chaseSpeed);
            }
        }

        bool StopChase(LongRangeEnemyFSM fsm)
        {
            if (blindChaseTime <= 0)
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

        public void GoToPatrol(LongRangeEnemyFSM fsm)
        {
            fsm.EnterState(fsm.patrol);
        }

        bool IsInGreyZone(float value)
        {
            if (value > greyZone.x && value < greyZone.y)
                return true;

            return false;
        }

        public override void DrawGizmos(LongRangeEnemyFSM fsm)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(fsm.transform.position, distToStopChase);

            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(fsm.transform.position, distToMoveAway);
        }
    }
}