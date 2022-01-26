using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class Swinging_State : State<Player_FSM>
{
    public bool rope_Attached;
    public GameObject rope_Hinge_Anchor;
    public Rigidbody2D rope_Hinge_Anchor_Rb;
    public bool isColliding;                                                    // Will be used as a flag to determine whether or not the rope's distance joint distance property can be increased or decreased.
    public DistanceJoint2D rope_Joint;

    // Fire
    [SerializeField] private LineRenderer rope_Renderer;
    [SerializeField] private LayerMask rope_Layer_Mask;
    [SerializeField] private float rope_Max_Cast_Dist;
    private List<Vector2> rope_Positions = new List<Vector2>();

    private bool distance_Set;
    private Dictionary<Vector2, int> wrap_Points_Lookup = new Dictionary<Vector2, int>();

    public float climbSpeed = 3f;
    public float swing_Force = 4f;
    float hor_Input = 0f;

    public override void Enter(Player_FSM player)
    {
        // Verificar se pode fazer o grappling
        RaycastHit2D hitObstacle = Physics2D.Raycast(player.m_rb.position, player.mouseFacingDir, rope_Max_Cast_Dist);
        RaycastHit2D hitGrapplePoint = Physics2D.Raycast(player.m_rb.position, player.mouseFacingDir, rope_Max_Cast_Dist, rope_Layer_Mask);

        Debug.Log(hitObstacle.collider.name);
        Debug.Log(hitGrapplePoint.collider.name);

        bool hasObstacle = hitObstacle.collider == hitGrapplePoint.collider ? false : true;

        if (!hasObstacle && hitGrapplePoint.collider != null)
        {
            Debug.Log("The grapple hit the " + hitGrapplePoint.collider.gameObject.name + " GameObject");
            rope_Attached = true;
            rope_Renderer.enabled = true;
            player.m_rb.constraints = RigidbodyConstraints2D.None;
            player.m_rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            player.anim_Handler.PlayAnim(AnimationsPlayer.SWINGING);
            Debug.Log("Played animation");

            if (!rope_Positions.Contains(hitGrapplePoint.point))
            {
                player.m_rb.GetComponent<Rigidbody2D>().AddForce(new Vector2(0,5), ForceMode2D.Impulse);
                rope_Positions.Add(hitGrapplePoint.point);
                rope_Joint.distance = Vector2.Distance(player.m_rb.position, hitGrapplePoint.point);
                rope_Joint.enabled = true;
                rope_Hinge_Anchor.GetComponent<SpriteRenderer>().enabled = true;
            }
        }
        else
        {
            rope_Renderer.enabled = false;
            rope_Attached = false;
            rope_Joint.enabled = false;
            player.Switch_State(player.idle_State);
        }
    }

    public override void Exit(Player_FSM player)
    {
        Reset_Rope();
        player.m_rb.constraints = RigidbodyConstraints2D.FreezePositionX;
        player.m_rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public override void Update(Player_FSM player)
    {
        if (player.m_Input.GrappleShoot())
        {
            if (rope_Attached)
            {
                if (player.grounded)
                    player.Switch_State(player.idle_State);
                else
                    player.Switch_State(player.air_State);
            }
        }

        Handle_Rope_Length(player);
    }

    public override void FixedUpdate(Player_FSM player)
    {
        player.m_Input.Horizontal(out hor_Input);
        float x_Input = hor_Input;

        if (x_Input < 0f || x_Input > 0f)
        {
            // Flip
            //player.m_Sr.flipX = player.horizontal_Input < 0f;
            player.Flip();

            // Add perpendicular Force
            Vector2 perp_Dir = Get_Perpendicular(player.m_rb.position, player.rope_Hook, x_Input);

            Vector2 perp_Pos = player.m_rb.position + perp_Dir * 2f;
            Debug.DrawLine(player.m_rb.position, perp_Pos, Color.green, 0f);

            Vector2 force = perp_Dir * swing_Force;
            player.m_rb.AddForce(force, ForceMode2D.Force);
        }



        // Rope Settings
        player.rope_Hook = rope_Positions.Last();

        player.Crosshair(false);
        // If the ropePositions list has any positions stored, then...
        if (rope_Positions.Count > 0)
        {
            // Fire a raycast out from the player's position, in the direction of the player looking at the last rope position in the list — the pivot point where the grappling hook is hooked into the rock — with a raycast distance set to the distance between the player and rope pivot position.
            var lastRopePoint = rope_Positions.Last();
            var playerToCurrentNextHit = Physics2D.Raycast(player.m_rb.position, (lastRopePoint - player.m_rb.position).normalized, Vector2.Distance(player.m_rb.position, lastRopePoint) - 0.1f, rope_Layer_Mask);

            // If the raycast hits something, then that hit object's collider is safe cast to a PolygonCollider2D. As long as it's a real PolygonCollider2D, then the closest vertex position on that collider is returned as a Vector2, using that handy-dandy method you wrote earlier.
            if (playerToCurrentNextHit)
            {
                var colliderWithVertices = playerToCurrentNextHit.collider as PolygonCollider2D;
                if (colliderWithVertices != null)
                {
                    var closestPointToHit = Get_Closest_Collider_Point_From_Raycast_Hit(playerToCurrentNextHit, colliderWithVertices);

                    // The wrapPointsLookup is checked to make sure the same position is not being wrapped again. If it is, then it'll reset the rope and cut it, dropping the player.
                    if (wrap_Points_Lookup.ContainsKey(closestPointToHit))
                    {
                        player.Switch_State(player.air_State);
                    }

                    // The ropePositions list is now updated, adding the position the rope should wrap around, and the wrapPointsLookup dictionary is also updated. Lastly the distanceSet flag is disabled, so that UpdateRopePositions() method can re-configure the rope's distances to take into account the new rope length and segments.
                    rope_Positions.Add(closestPointToHit);
                    wrap_Points_Lookup.Add(closestPointToHit, 0);
                    distance_Set = false;
                }
            }
        }

        Update_Rope_Positions(player);
    }

    public override void OnTriggerStay2D(Collider2D other, Player_FSM player)
    {
        if (other.isTrigger == false)
            isColliding = true;
    }

    public override void OnTriggerExit2D(Collider2D other, Player_FSM player)
    {
        isColliding = false;
    }

    #region Grappling 

    void Reset_Rope()
    {
        rope_Joint.enabled = false;
        rope_Attached = false;
        rope_Renderer.positionCount = 2;
        rope_Renderer.SetPosition(0, Vector2.zero);
        rope_Renderer.SetPosition(1, Vector2.zero);
        rope_Positions.Clear();
        rope_Hinge_Anchor.GetComponent<SpriteRenderer>().enabled = false;
        wrap_Points_Lookup.Clear();
    }

    void Update_Rope_Positions(Player_FSM player)
    {
        // 1
        if (!rope_Attached)
            return;

        // Set the rope's line renderer vertex count (positions) to whatever number of positions are stored in ropePositions, plus 1 more (for the player's position).
        rope_Renderer.positionCount = rope_Positions.Count + 1;

        // Loop backwards through the ropePositions list, and for every position (except the last position), set the line renderer vertex position to the Vector2 position stored at the current index being looped through in ropePositions.
        for (var i = rope_Renderer.positionCount - 1; i >= 0; i--)
        {
            if (i != rope_Renderer.positionCount - 1) // if not the Last point of line renderer
            {
                rope_Renderer.SetPosition(i, rope_Positions[i]);

                // Set the rope anchor to the second-to-last rope position where the current hinge/anchor should be, or if there is only one rope position, then set that one to be the anchor point. This configures the ropeJoint distance to the distance between the player and the current rope position being looped over.
                if (i == rope_Positions.Count - 1 || rope_Positions.Count == 1)
                {
                    var ropePosition = rope_Positions[rope_Positions.Count - 1];
                    if (rope_Positions.Count == 1)
                    {
                        rope_Hinge_Anchor_Rb.position = ropePosition;
                        if (!distance_Set)
                        {
                            rope_Joint.distance = Vector2.Distance(player.m_rb.position, ropePosition);
                            distance_Set = true;
                        }
                    }
                    else
                    {
                        rope_Hinge_Anchor_Rb.position = ropePosition;
                        if (!distance_Set)
                        {
                            rope_Joint.distance = Vector2.Distance(player.m_rb.position, ropePosition);
                            distance_Set = true;
                        }
                    }
                }
                // Set the rope anchor to the second-to-last rope position where the current hinge/anchor should be, or if there is only one rope position, then set that one to be the anchor point. This configures the ropeJoint distance to the distance between the player and the current rope position being looped over.
                else if (i - 1 == rope_Positions.IndexOf(rope_Positions.Last()))
                {
                    Debug.Log("1");
                    var ropePosition = rope_Positions.Last();
                    Debug.Log("2");
                    rope_Hinge_Anchor_Rb.position = ropePosition;
                    if (!distance_Set)
                    {
                        rope_Joint.distance = Vector2.Distance(player.m_rb.position, ropePosition);
                        distance_Set = true;
                    }
                }
            }
            else
            {
                // This else block handles setting the rope's last vertex position to the player's current position.
                rope_Renderer.SetPosition(i, player.m_rb.position);
            }
        }
    }

    private void Handle_Rope_Length(Player_FSM player)
    {
        // This if..elseif block looks for vertical axis input (up/down or W/S on the keyboard), and depending on the ropeAttached iscColliding flags will either increase or decrease the ropeJoint distance, having the effect of lengthening or shortening the rope.
        float ver_Input = 0f;
        player.m_Input.Vertical(out ver_Input);

        if (ver_Input >= 1f && rope_Attached && !isColliding)
        {
            rope_Joint.distance -= Time.deltaTime * climbSpeed;
        }
        else if (ver_Input < 0f && rope_Attached)
        {
            rope_Joint.distance += Time.deltaTime * climbSpeed;
        }
    }

    Vector2 Get_Closest_Collider_Point_From_Raycast_Hit(RaycastHit2D hit, PolygonCollider2D poly_Collider)
    {
        // Here be LINQ query magic! This converts the polygon collider's collection of points, into a dictionary of Vector2 positions (the value of each dictionary entry is the position itself), and the key of each entry, is set to the distance that this point is to the player's position (float value). Something else happens here: the resulting position is transformed into world space (by default a collider's vertex positions are stored in local space - i.e. local to the object the collider sits on, and we want the world space positions).
        var distance_Dictionary = poly_Collider.points.ToDictionary<Vector2, float, Vector2>(
            position => Vector2.Distance(hit.point, poly_Collider.transform.TransformPoint(position)),
            position => poly_Collider.transform.TransformPoint(position));

        // The dictionary is ordered by key. In other words, the distance closest to the player's current position, and the closest one is returned, meaning that whichever point is returned from this method, is the point on the collider between the player and the current hinge point on the rope!
        var ordered_Dictionary = distance_Dictionary.OrderBy(e => e.Key);
        return ordered_Dictionary.Any() ? ordered_Dictionary.First().Value : Vector2.zero;
    }
    
    #endregion

    Vector2 Get_Perpendicular(Vector2 m_Pos, Vector2 target_Pos, float hor_Speed)
    {
        // 1 - Get a normalized direction vector from the player to the target
        Vector2 dir_To_Target = (target_Pos - m_Pos).normalized;

        // 2 - Inverse the direction to get a perpendicular direction
        Vector2 perp_Dir;
        if (hor_Speed < 0)
            perp_Dir = new Vector2(-dir_To_Target.y, dir_To_Target.x);
        else
            perp_Dir = new Vector2(dir_To_Target.y, -dir_To_Target.x);

        return perp_Dir;
    }

}