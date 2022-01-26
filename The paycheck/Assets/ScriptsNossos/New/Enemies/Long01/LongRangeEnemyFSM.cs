using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace LongRangeEnemy
{
    public class LongRangeEnemyFSM : MonoBehaviour
    {
        [Header("FSM Basic")]
        [SerializeField] public State<LongRangeEnemyFSM> currentState;
        [SerializeField] private string currentStateName;
        [SerializeField] public List<IEnumerator> routineBeingPlayed = new List<IEnumerator>();

        [Header("Moviment")]
        [SerializeField] private LookingDir lookingDir;
        [SerializeField] private float minDisplacement;
        [SerializeField] public Vector2[] spots;
        [SerializeField] private float flipOffset = 1f;                                             // Quanto na frente do inimigo o alvo tem que estar para ele vira na direção do alvo
        [SerializeField] private LayerMask groundMask;
        [SerializeField] private Transform[] objsToRotateTowardsTarget;

        [SerializeField] public Vector2 LastFramePos { get; private set; }
        [HideInInspector] public bool waitingAnimationEnd = false;

        [Header("Shoot")]
        [SerializeField] private float distToAttack;
        [SerializeField] private Transform[] muzzlePoints;
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] [Range(0, 10)] private float timeBtwShots;
        [HideInInspector] private float lastShotTime;
        [HideInInspector] public bool findTarget;

        [Header("Detection")]
        [SerializeField] public Transform pointOfView;
        [SerializeField] private Vector2 detectionField;
        [SerializeField] private float detectionDist;

        // References
        [SerializeField] public Animator m_Anim;
        [HideInInspector] public Rigidbody2D m_Rb;
        [HideInInspector] public PointsOfDetection player;
        [HideInInspector] public AudioPlayer audioPlayer;

        public Patrol patrol = new Patrol();
        public Chase chase = new Chase();

        [Header("Debug")]
        [SerializeField] private DebugState showDebug;

        #region Mono

        private void Start()
        {
            Physics2D.queriesStartInColliders = false;

            m_Rb = GetComponent<Rigidbody2D>();

            EnemiesManager manager = GetComponentInParent<EnemiesManager>();
            if (manager != null)
                player = manager.playerDetection;
            else
                player = GameObject.FindGameObjectWithTag("Player").GetComponent<PointsOfDetection>();

            lastShotTime = Time.time;
            audioPlayer = GetComponent<AudioPlayer>();

            Health_System health = GetComponent<Health_System>();

            health.onDeath += Death;
            health.onHurt += ForceChase;
            health.onHurt += PlayHurtClip;

            EnterState(patrol);
        }

        private void FixedUpdate()
        {
            m_Rb.velocity = new Vector2(0, m_Rb.velocity.y);

            currentStateName = currentState.ToString();
            currentState.FixedUpdate(this);

            LastFramePos = transform.position;
        }

        #endregion

        #region FSM Base

        public void EnterState(State<LongRangeEnemyFSM> new_State)
        {
            if (currentState != null)
                currentState.Exit(this);

            StopRoutines();

            currentState = new_State;
            currentState.Enter(this);
        }

        public void PlayRoutine(IEnumerator routine)
        {
            StartCoroutine(routine);

            routineBeingPlayed.Add(routine);
        }

        #endregion

        #region Detection

        public bool Detect()
        {
            // Distancia
            if (player.GetClosestDist(pointOfView.position) > detectionDist)
                return false;

            // Vetor de visão
            Vector2 lookDir = lookingDir == LookingDir.Right ? Vector2.right : Vector2.left;
            Debug.DrawRay(pointOfView.position, lookDir * detectionDist, Color.black);

            // Tem que checar cada um dos pontos do player
            foreach (Transform point in player.Points)
            {
                // Vetor até alvo
                Vector2 toPlayerDir = point.position - pointOfView.position;
                Debug.DrawRay(pointOfView.position, toPlayerDir * detectionDist, Color.black);

                // Angulo entre os dois
                float angle = Vector2.Angle(lookDir, toPlayerDir);

                // Ver se enquadra no limite de detecção
                if (point.position.y > pointOfView.position.y)
                {
                    if (angle < detectionField.x)
                        return RaycastCheck(pointOfView.position, toPlayerDir);
                }

                if (angle < Mathf.Abs(detectionField.y))
                    return RaycastCheck(pointOfView.position, toPlayerDir);
            }

            return false;
        }

        bool RaycastCheck(Vector2 origin, Vector2 dir)
        {
            ContactFilter2D filter = new ContactFilter2D().NoFilter();
            RaycastHit2D hit = Physics2D.Raycast(origin, dir, detectionDist);

            if (hit.collider != null)
            {
                Debug.Log("Collider name = " + hit.collider.name);

                if (hit.collider.CompareTag("Player"))
                    return true;
            }

            return false;
        }

        #endregion

        public void MoveTowards(Vector2 target, float speed)
        {
            if (!IsThereGroundAhead())
            {
                m_Anim.Play("Idle");
                return;
            }

            Vector2 dir_To_Move = new Vector2(target.x, m_Rb.position.y) - m_Rb.position;
            dir_To_Move.Normalize();

            Flip(target);
            m_Rb.position += dir_To_Move * speed * Time.deltaTime;

            if (Displacement() > minDisplacement)
                m_Anim.Play("Run");
            else
                m_Anim.Play("Idle");
        }

        public void MoveAway(Vector2 target, float speed)
        {
            if (!IsThereGroundAhead(false))
            {
                m_Anim.Play("Idle");
                return;
            }

            Vector2 dir_To_Move = new Vector2(target.x, m_Rb.position.y) - m_Rb.position;
            dir_To_Move.Normalize();
            m_Rb.position -= dir_To_Move * speed * Time.deltaTime;

            if (Displacement() > minDisplacement)
                m_Anim.Play("MoveAway");
            else
                m_Anim.Play("Idle");
        }

        public float Displacement()
        {
            return Mathf.Abs(transform.position.x - LastFramePos.x);
        }

        public void Flip(Vector2 target)
        {
            Vector3 rotation = Vector3.zero;

            if (Mathf.Abs(target.x - transform.position.x) < flipOffset)
                return;

            if (target.x > transform.position.x)
            {
                lookingDir = LookingDir.Right;
                rotation = new Vector3(0f, 0f, 0f);
            }
            else if (target.x < transform.position.x)
            {
                lookingDir = LookingDir.Left;
                rotation = new Vector3(0f, 180f, 0f);
            }

            foreach (Transform obj in objsToRotateTowardsTarget)
                obj.rotation = Quaternion.Euler(rotation);
        }

        public bool IsThereGroundAhead(bool checkForGroundAhead = true)
        {
            RaycastHit2D hit;
            Vector2 posOffset;

            if (checkForGroundAhead)
                posOffset = lookingDir == LookingDir.Left ? Vector2.left : Vector2.right;
            else
                posOffset = lookingDir == LookingDir.Left ? Vector2.right : Vector2.left;

            hit = Physics2D.Raycast(m_Rb.position + posOffset, Vector2.down, 5f, groundMask);

            if (hit.collider != null)
                return true;

            return false;
        }

        public IEnumerator WaitEndOfAnimation(string animToWait)
        {
            m_Anim.Play(animToWait, 0, 0);
            waitingAnimationEnd = true;

            while (!m_Anim.GetCurrentAnimatorStateInfo(0).IsName(animToWait))
                yield return null;

            while (m_Anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                yield return null;

            waitingAnimationEnd = false;
        }

        public bool TryToAttack()
        {
            float distToTarget = player.GetClosestDist(pointOfView.position);

            if (distToTarget <= distToAttack)
            {
                if(lastShotTime + timeBtwShots < Time.time)
                {
                    Flip(player.GetMain().position);
                    lastShotTime = Time.time;
                    m_Anim.StopPlayback();
                    PlayRoutine(WaitEndOfAnimation("Shoot"));

                    return true;
                }
            }

            return false;
        }

        public void Shoot()
        {
            foreach (Transform muzzlePoint in muzzlePoints)
            {
                Bullet_Movement bullet = PoolManager.SpawnObject(bulletPrefab, muzzlePoint.position, Quaternion.identity).GetComponent<Bullet_Movement>();

                audioPlayer.PlayClip("Shoot", false, 1f);

                if (bullet != null)
                    bullet.Initialize(muzzlePoint.transform.right);
            }
        }

        private void ForceChase(int damage = 0)
        {
            if (currentState != chase)
            {
                StopRoutines();
                findTarget = true;
                EnterState(chase);
            }
        }

        void Death()
        {
            GetComponent<Collider2D>().enabled = false;
            audioPlayer.PlayClip("Death", false, 1f);
            m_Rb.constraints = RigidbodyConstraints2D.FreezePositionX;
            m_Anim.Play("Death");
            this.enabled = false;
        }

        void PlayHurtClip(int hurt)
        {
            audioPlayer.PlayClip("Hurt", false, 1f);
        }

        public void PlayFootstepClip()
        {
            audioPlayer.PlayClip("Footstep", false, 1f);
        }

        private void StopRoutines()
        {
            waitingAnimationEnd = false;
            StopAllCoroutines();
            routineBeingPlayed.Clear();
        }

        private void OnDisable()
        {
            StopRoutines();
        }

        private void OnDrawGizmos()
        {
            if (showDebug == DebugState.None)
                return;

            if (showDebug == DebugState.Ranges || showDebug == DebugState.All)
            {
                foreach (Vector2 spot in spots)
                {
                    Gizmos.DrawIcon(spot, "Target.png", true);
                }

                chase.DrawGizmos(this);

                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(transform.position, distToAttack);
            }

            if (showDebug == DebugState.Detection || showDebug == DebugState.All)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(pointOfView.position, detectionDist);

                if (lookingDir == LookingDir.Right)
                {
                    // FOV
                    Gizmos.DrawLine(pointOfView.position, pointOfView.position + Vector3.right * detectionDist);

                    Gizmos.color = Color.yellow;

                    // UP
                    Vector3 dir = MoreMath.FromAngleToVector(detectionField.x * Mathf.Deg2Rad);
                    Gizmos.DrawLine(pointOfView.position, pointOfView.position + dir * detectionDist);

                    // Down
                    dir = MoreMath.FromAngleToVector(detectionField.y * Mathf.Deg2Rad);
                    Gizmos.DrawLine(pointOfView.position, pointOfView.position + dir * detectionDist);
                }
                else
                {
                    // FOV
                    Gizmos.DrawLine(pointOfView.position, pointOfView.position + Vector3.left * detectionDist);

                    Gizmos.color = Color.yellow;

                    // UP
                    Vector3 dir = MoreMath.FromAngleToVector((180 - detectionField.x) * Mathf.Deg2Rad);
                    Gizmos.DrawLine(pointOfView.position, pointOfView.position + dir * detectionDist);

                    // Down
                    dir = MoreMath.FromAngleToVector((-180 - detectionField.y) * Mathf.Deg2Rad);
                    Gizmos.DrawLine(pointOfView.position, pointOfView.position + dir * detectionDist);
                }
            }
        }
    }
}