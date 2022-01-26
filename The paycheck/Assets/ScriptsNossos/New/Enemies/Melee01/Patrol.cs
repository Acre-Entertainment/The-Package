using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MeleeEnemyFSM
{
    [System.Serializable]
    public class Patrol : State<MeleeEnemyFSM>
    {
        [SerializeField]
        float patrolSpeed;
        [SerializeField]
        float distToWait;
        [SerializeField]
        float restTime;

        int spotsIterator;
        int waypoint;
        float timeWaiting = 0;

        public override void Enter(MeleeEnemyFSM fsm)
        {
            if (fsm.spots.Length < 2)
            {
                Debug.Log("NOTE: Esse objeto PRECISA de 2 move Spots");
                fsm.gameObject.SetActive(false);
                return;
            }

            timeWaiting = 0;
            spotsIterator = 1;
            waypoint = Find_Closest(fsm.transform.position, fsm.spots);
        }

        public override void Update(MeleeEnemyFSM fsm) {}

        public override void FixedUpdate(MeleeEnemyFSM fsm)
        {
            if (fsm.Detect())
            {
                fsm.EnterState(fsm.chase);
                return;
            }

            if (fsm.waitingAnimationEnd)
                return;

            // WAIT
            if (Mathf.Abs(fsm.m_Rb.position.x - fsm.spots[waypoint].x) < distToWait)
            {
                fsm.m_Anim.Play("Idle");

                if (timeWaiting > restTime)
                {
                    timeWaiting = 0;
                    GetNextSpot(fsm);
                }

                timeWaiting += Time.fixedDeltaTime;
                return;
            }

            // PATROL
            fsm.Move(fsm.spots[waypoint], patrolSpeed);
        }

        int Find_Closest(Vector2 from, Vector2[] pos_To)
        {
            int closest_index = 0;
            float closest_Dist = Mathf.Infinity;
            float dist = 0;

            for (int i = 0; i < pos_To.Length; i++)
            {
                dist = Vector2.Distance(from, pos_To[i]);
                if (dist < closest_Dist)
                {
                    closest_Dist = dist;
                    closest_index = i;
                }
            }

            return closest_index;
        }

        void GetNextSpot(MeleeEnemyFSM fsm)
        {
            if (waypoint + spotsIterator < 0 || waypoint + spotsIterator == fsm.spots.Length)
                spotsIterator *= -1;

            waypoint += spotsIterator;
        }
    }
}