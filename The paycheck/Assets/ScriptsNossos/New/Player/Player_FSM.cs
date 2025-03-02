using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Health_System))]
[RequireComponent(typeof(AnimationsPlayer))]
[RequireComponent(typeof(Player_Input))]
[RequireComponent(typeof(Inventory))]
[RequireComponent(typeof(AudioPlayer))]

public class Player_FSM : MonoBehaviour
{
    public State<Player_FSM> currentState;
    [SerializeField] private string current_State_Name;

    [Header("JUMP")]
    public bool can_Jump;
    public bool grounded;
    [SerializeField] private LayerMask ground_Layer;							// A mask determining what is ground to the character
    [SerializeField] private float jump_Force;							        // Amount of force added when the player jumps.
    public float ground_Check_Radius;                                           // Radius of the overlap circle to determine if grounded
    public Transform ground_Check_Pos;						                    // A position marking where to check if the player is grounded.

    public Vector2 bodyFacingDir;
    LookingDir lookDir;
    [HideInInspector] public Vector2 mouseFacingDir;
    [HideInInspector] public Vector2 rope_Hook;

    [Header("SHOOT")]
    [SerializeField] Transform muzzle;
    [SerializeField] GameObject bulletPrefab;

    [Header("OBJECTS")]
    [SerializeField] private Transform posToCheckForObj;
    [SerializeField] private float detectionRadius;
    [HideInInspector] public Rigidbody2D box;

    [Header("REFERENCES ")]
    public Transform obj_To_Flip;
    [HideInInspector] public AnimationsPlayer anim_Handler;
    [HideInInspector] public Rigidbody2D m_rb;
    public Transform crosshair;
    public SpriteRenderer crosshair_Sprite;
    Health_System health_System;
    [HideInInspector] public Player_Input m_Input;
    [HideInInspector] public Inventory inv;
    [HideInInspector] public AudioPlayer audioPlayer;

    bool can_Move = true;
    bool on_Dialogue = false;

    [Space(20)]
    [HideInInspector] public Idle_State idle_State;
    public Crouch_State crouch_State;
    public Run_State run_State;
    public Air_State air_State;
    public Swinging_State swinging_State;
    public PushingBox pushingBox;
    public Climb climb;

    public Vector2 LastFramePos { get; private set; }

    private float speedMod;

    #region Items Name
    public const string GUN = "Gun";
    public const string MAGNET = "Magnet";
    public const string LIGHTER = "Lighter";
    public const string ROPE = "Rope";
    public const string GLOVE = "Glove";
    public const string CLOTH = "Cloth";
    #endregion

    #region Audio Name
    public const string clipJump = "Jump";
    public const string clipFootsteps = "Footstep";
    public const string clipKick = "Kick";
    public const string clipDamaged = "Damaged";
    public const string clipDeath = "Death";
    public const string clipTurnOnLighter = "TurnOnLighter";
    public const string clipCrouch = "Crouch";
    public const string clipLand = "Land"; 
    public const string clipShot = "Shot";
    public const string clipShotCharge = "ShotCharge";
    #endregion

    void Awake()
    {
        m_rb = GetComponent<Rigidbody2D>();
        currentState = run_State;
        health_System = GetComponent<Health_System>();
        anim_Handler = GetComponent<AnimationsPlayer>();
        m_Input = GetComponent<Player_Input>();
        inv = GetComponent<Inventory>();
        audioPlayer = GetComponent<AudioPlayer>();

        // Listening
        health_System.onHurt += Update_Healt_GUI;
        health_System.onHurt += PlayHurtClip;
        health_System.onDeath += PlayDeathClip;
        Global_Events.Dialogue_Changed_State_Event += Dialogue_State;

        Can_Move_Update();

        //transform.position = CheckpointManager.LoadCheckpointPos(transform);
    }

    void Start()
    {
        Update_Healt_GUI(health_System.Current_Health);
        health_System.onDeath += Death;
    }

    void Update()
    {
        // Handle Crosshair
        Crosshair_Update();

        grounded = Is_Grounded();

        currentState.Update(this);
    }

    void FixedUpdate()
    {
        if (currentState != swinging_State) // Each state could do this by himself
            m_rb.velocity = new Vector2(0, m_rb.velocity.y);

        //FELIPE: CONSERTAR O TAL BUG DA CORDA
        try
        {
            currentState.FixedUpdate(this);
        }
        catch(Exception e)
        {
            if(!grounded)
            {
                Switch_State(air_State);
            }
            else
            {
                Switch_State(idle_State);
            }

            Debug.Log("ERROR IN STATE");
        }
        current_State_Name = currentState.ToString();
        LastFramePos = transform.position;
    }

    public void Switch_State(State<Player_FSM> new_State, int animToWait = -1)
    {
        if(animToWait != -1)
        {
            StartCoroutine(Switch_After_Animation(new_State, animToWait));
            return;
        }
        
        currentState.Exit(this);
        currentState = new_State;
        currentState.Enter(this);        
    }

    IEnumerator Switch_After_Animation(State<Player_FSM> new_State, int animToWait)
    {
        m_Input.readInput = false;

        while (anim_Handler.bodyAnim.GetCurrentAnimatorStateInfo(0).shortNameHash != animToWait)
            yield return null;

        while(anim_Handler.bodyAnim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            yield return null;

        currentState.Exit(this);
        currentState = new_State;
        currentState.Enter(this);
        m_Input.readInput = true;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        currentState.OnTriggerStay2D(other, this);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        currentState.OnTriggerExit2D(other, this);
    }

    public void Move(Rigidbody2D rb, Vector2 dir, float speed)
    {
        if (Platform.currentPlatform != null)
        {
            if (Platform.currentPlatform.lookDir == lookDir)
                speedMod = -Platform.currentPlatform.speed;
            else
                speedMod = Platform.currentPlatform.speed;
        }
        else
            speedMod = 0;

        rb.position += dir * ((speed + speedMod) * Time.deltaTime);
    }

    public void Flip()
    {
        float hor_Mov = 0;
        m_Input.Horizontal(out hor_Mov);

        if (hor_Mov > 0)
        {
            obj_To_Flip.rotation = Quaternion.Euler(0, 0, 0);
            bodyFacingDir = Vector2.right;
            lookDir = LookingDir.Right;
        }
        else if (hor_Mov < 0)
        {
            obj_To_Flip.rotation = Quaternion.Euler(0, 180, 0);
            bodyFacingDir = -Vector2.right;
            lookDir = LookingDir.Left;
        }
    }

    bool Is_Grounded()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(ground_Check_Pos.position, ground_Check_Radius, ground_Layer);

        if (colliders.Length > 0)
            return true;
        else
            return false;
    }

    public void Jump()
    {
        anim_Handler.PlayAnim(AnimationsPlayer.JUMP);
        m_rb.velocity = new Vector2(m_rb.velocity.x, 0f);
        m_rb.AddForce(new Vector2(0f, jump_Force), ForceMode2D.Impulse);
        audioPlayer.PlayClip(clipJump, false);
    }

    #region Crosshair

    private void Crosshair_Update()
    {
        Vector3 world_Mouse_Pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
        mouseFacingDir = world_Mouse_Pos - transform.position;

        // Crosshair position
        if (!swinging_State.rope_Attached)
        {
            float aim_Angle = Mathf.Atan2(mouseFacingDir.y, mouseFacingDir.x);
            // Isso é pra arrumar o tipo de angulo que o Unity trabalha
            if (aim_Angle < 0f)
                aim_Angle = Mathf.PI * 2 + aim_Angle;

            Vector3 aim_Dir = Quaternion.Euler(0, 0, aim_Angle * Mathf.Rad2Deg) * Vector2.right;

            Set_Crossair_Pos(aim_Angle);
        }
    }

    private void Set_Crossair_Pos(float aim_Angle)
    {
        // * This method will position the crosshair based on the aimAngle that you pass in (a float value you calculated in Update()) in a way that it circles around you in a radius of 1 unit. It'll also ensure the crosshair sprite is enabled if it isn’t already.
        if (!crosshair_Sprite.enabled)
            crosshair_Sprite.enabled = true;

        float x = transform.position.x + 5f * Mathf.Cos(aim_Angle);
        float y = transform.position.y + 5f * Mathf.Sin(aim_Angle);

        Vector3 crosshair_Pos = new Vector3(x, y, 0f);
        crosshair.transform.position = crosshair_Pos;
    }

    public void Crosshair(bool enable)
    {
        if (enable)
            crosshair_Sprite.enabled = true;
        else
            crosshair_Sprite.enabled = false;
    }

    #endregion

    #region Health

    void Update_Healt_GUI(int damage)
    {
        Global_Events.UpdateHealthGUI(health_System.Current_Health);
    }

    public void Death()
    {
        StopAllCoroutines();
        m_Input.readInput = false;
        GetComponent<Collider2D>().enabled = false;
        m_rb.constraints = RigidbodyConstraints2D.FreezePositionX;
        //m_rb.bodyType = RigidbodyType2D.Kinematic;
        Invoke("ReloadScene", 3f);
        Global_Events.PlayerDied();
        anim_Handler.PlayDeathAnim();
    }

    void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    #endregion

    #region Dialogue

    void Dialogue_State(bool is_Open)
    {
        on_Dialogue = is_Open;
        Can_Move_Update();
    }

    void Can_Move_Update()
    {
        if (on_Dialogue)
        {
            can_Move = false;
            return;
        }

        can_Move = true;
    }

    #endregion

    public bool CheckItemEquipped(Hand hand, string itemName)
    {
        if(hand == Hand.Right)
        {
            if (inv.rightHandEquippedItem.name == itemName)
                return true;

            return false;
        }

        if (inv.leftHandEquippedItem.name == itemName)
            return true;

        return false;
    }

    public void Shoot()
    {
        Bullet_Movement bullet = PoolManager.SpawnObject(bulletPrefab, muzzle.position, Quaternion.identity).GetComponent<Bullet_Movement>();
        audioPlayer.PlayClip(clipShot, false);

        if (bullet != null)
            bullet.Initialize(bodyFacingDir);
    }

    public float Displacement()
    {
        return Mathf.Abs(transform.position.x - LastFramePos.x);
    }

    public bool HasBox()
    {
        Debug.Log("Checking box");
        Collider2D[] hits = Physics2D.OverlapCircleAll(posToCheckForObj.position, detectionRadius);

        foreach(Collider2D hit in hits)
        {
            if(hit.CompareTag("Pushable"))
            {
                box = hit.GetComponent<Rigidbody2D>();
                return true;
            }
        }

        return false;
    }

    public bool HasWall()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(posToCheckForObj.position, detectionRadius);

        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Metal Wall"))
                return true;
        }

        return false;
    }

    public void PlayStepSound()
    {
        audioPlayer.PlayClip(clipFootsteps, false, .5f);
    }

    public void PlayKickClip()
    {
        audioPlayer.PlayClip(clipKick, false);
    }

    void PlayHurtClip(int hurt)
    {
        audioPlayer.PlayClip(clipDamaged, false, .5f + hurt * .1f);
    }

    void PlayDeathClip()
    {
        audioPlayer.PlayClip(clipDeath, false, 1f);
    }

    public void PlayShotChargeClip()
    {
        audioPlayer.PlayClip(clipShotCharge, false);
    }

    private void OnDrawGizmos()
    {
        if (ground_Check_Pos != null)
            Gizmos.DrawWireSphere(ground_Check_Pos.position, ground_Check_Radius);

        if(posToCheckForObj != null)
        {
            Gizmos.color = new Color(0, 0, 1, .5f);
            Gizmos.DrawSphere(posToCheckForObj.position, detectionRadius);
        }
        //if(ceiling_Check_Pos != null)
        //Gizmos.DrawWireSphere(jump_State.ceiling_Check_Pos.position, jump_State.ceiling_Check_Radius);
    }
}

public enum Hand
{
    Right,
    Left
}