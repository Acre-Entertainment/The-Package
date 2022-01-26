using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Simple_Enemy_FSM : MonoBehaviour
{
    private enum States
    {
        Patrol,
        Chase
    }

    [SerializeField]
    States currentState;
    Vector2 dir_To_Move;
    LookingDir lookingDir;
    [SerializeField]
    private float flipOffset = 1f; // Quanto na frente do inimigo o alvo tem que estar para ele vira na direção do alvo


    [Header("Patrol")]
    [SerializeField] float patrol_Speed;
    [SerializeField] float dist_To_Wait;
    [SerializeField] float restTime;
    public Vector2[] spots;
    float wait_Time;
    int spots_Interator;
    Vector2 waypoint;
    private bool isMovimentLocked = false;
    private bool waitingMovimentEnd = false;

    [Header("Chase")]
    [SerializeField] float chase_Speed;
    [SerializeField] float dist_To_Attack;
    [SerializeField] float dist_To_Stop_Chase;
    [SerializeField] float start_time_Waiting_For_Player;
    [SerializeField] LayerMask ground_Mask;
    float time_Waiting_For_Player;

    [SerializeField]
    private Transform[] objsToRotateTowardsTarget;
    [SerializeField]
    private Animator m_Anim;
    private Rigidbody2D m_Rb;
    private Transform player;

    #region Animation Methods

    public void StartAttackAnimation()
    {
        //LockMoviment(true);
    }

    public void EndAttackAnimation()
    {
        //LockMoviment(false);
        //Enter_Chase();
    }

    #endregion


    
}