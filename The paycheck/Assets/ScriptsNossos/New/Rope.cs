using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Rope : MonoBehaviour
{
    public GameObject rope_Hinge_Anchor;
    private Rigidbody2D rope_Hinge_Anchor_Rb;
    private SpriteRenderer rope_Hinge_Anchor_Sprite;
    
    public DistanceJoint2D rope_Joint;
    public Transform crosshair;
    public SpriteRenderer crosshair_Sprite;
    public Player_FSM player;

    private bool rope_Attached;
    private Vector2 player_Pos;

    // Fire
    [SerializeField] private LineRenderer rope_Renderer;
    [SerializeField] private LayerMask rope_Layer_Mask;
    [SerializeField] private float rope_Max_Cast_Dist;
    private List<Vector2> rope_Positions = new List<Vector2>();

    private bool distance_Set;
    private Dictionary<Vector2, int> wrap_Points_Lookup = new Dictionary<Vector2, int>();

    public float climbSpeed = 3f;
    private bool isColliding; // Will be used as a flag to determine whether or not the rope's distance joint distance property can be increased or decreased.

    private void Awake() 
    {
        rope_Hinge_Anchor_Rb = rope_Hinge_Anchor.GetComponent<Rigidbody2D>();
        rope_Hinge_Anchor_Sprite = rope_Hinge_Anchor.GetComponent<SpriteRenderer>();

        rope_Joint.enabled = false;   
        player_Pos = transform.position;
    }

    private void Update() 
    {
        Vector3 world_Mouse_Pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));

        Vector2 facing_Dir = world_Mouse_Pos - transform.position;   
        float aim_Angle = Mathf.Atan2(facing_Dir.y, facing_Dir.x);
        // If the angle is negative, take it positive side
        // Isso é pra arrumar o tipo de angulo que o Unity trabalha
        if(aim_Angle < 0f)
            aim_Angle = Mathf.PI * 2 + aim_Angle;

        Vector3 aim_Dir = Quaternion.Euler(0,0, aim_Angle * Mathf.Rad2Deg) * Vector2.right;

        player_Pos = transform.position;

        if(rope_Attached)
        {
            //player.isSwinging = true;
            player.rope_Hook = rope_Positions.Last();

            crosshair_Sprite.enabled = false;
            // If the ropePositions list has any positions stored, then...
            if (rope_Positions.Count > 0)
            {
                // Fire a raycast out from the player's position, in the direction of the player looking at the last rope position in the list — the pivot point where the grappling hook is hooked into the rock — with a raycast distance set to the distance between the player and rope pivot position.
                var lastRopePoint = rope_Positions.Last();
                var playerToCurrentNextHit = Physics2D.Raycast(player_Pos, (lastRopePoint - player_Pos).normalized, Vector2.Distance(player_Pos, lastRopePoint) - 0.1f, rope_Layer_Mask);
                
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
                            Reset_Rope();
                            return;
                        }

                        // The ropePositions list is now updated, adding the position the rope should wrap around, and the wrapPointsLookup dictionary is also updated. Lastly the distanceSet flag is disabled, so that UpdateRopePositions() method can re-configure the rope's distances to take into account the new rope length and segments.
                        rope_Positions.Add(closestPointToHit);
                        wrap_Points_Lookup.Add(closestPointToHit, 0);
                        distance_Set = false;
                    }
                }
            }
        }
        else
        {
            //player.isSwinging = false;
            Set_Crossair_Pos(aim_Angle);
        }

        Handle_Input(facing_Dir);
        Update_Rope_Positions();
        HandleRopeLength();
    }

    // * This method will position the crosshair based on the aimAngle that you pass in (a float value you calculated in Update()) in a way that it circles around you in a radius of 1 unit. It'll also ensure the crosshair sprite is enabled if it isn’t already.
    private void Set_Crossair_Pos(float aim_Angle)
    {
        if(!crosshair_Sprite.enabled)
            crosshair_Sprite.enabled = true;

        float x = transform.position.x + 5f * Mathf.Cos(aim_Angle);
        float y = transform.position.y + 5f * Mathf.Sin(aim_Angle);

        Vector3 crosshair_Pos = new Vector3(x,y,0f);
        crosshair.transform.position = crosshair_Pos;
    }

    private void Handle_Input(Vector2 aim_Dir)
    {
        if(Input.GetMouseButton(0))
        {
            if(rope_Attached)
                return;
        
            rope_Renderer.enabled = true;

            RaycastHit2D hit = Physics2D.Raycast(player_Pos, aim_Dir, rope_Max_Cast_Dist, rope_Layer_Mask);

            if(hit.collider != null)
            {
                Debug.Log("The grapple hit the " + hit.collider.gameObject.name + " GameObject");
                rope_Attached = true;
                if(!rope_Positions.Contains(hit.point))
                {
                    transform.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 5f), ForceMode2D.Impulse);
                    rope_Positions.Add(hit.point);
                    rope_Joint.distance = Vector2.Distance(player_Pos, hit.point);
                    rope_Joint.enabled = true;
                    rope_Hinge_Anchor_Sprite.enabled = true;
                }
            }   
            else
            {
                rope_Renderer.enabled = false;
                rope_Attached = false;
                rope_Joint.enabled = false;
            }
        }

        if (Input.GetMouseButton(1))
        {
            if(rope_Attached)
                Reset_Rope();
        }
    }

    void Reset_Rope()
    {
        rope_Joint.enabled = false;
        rope_Attached = false;
        //player.isSwinging = false;
        rope_Renderer.positionCount = 2;
        rope_Renderer.SetPosition(0, transform.position);
        rope_Renderer.SetPosition(1, transform.position);
        rope_Positions.Clear();
        rope_Hinge_Anchor_Sprite.enabled = false;
        wrap_Points_Lookup.Clear();
    }

    void Update_Rope_Positions()
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
                        rope_Hinge_Anchor_Rb.transform.position = ropePosition;
                        if (!distance_Set)
                        {
                            rope_Joint.distance = Vector2.Distance(transform.position, ropePosition);
                            distance_Set = true;
                        }
                    }
                    else
                    {
                        rope_Hinge_Anchor_Rb.transform.position = ropePosition;
                        if (!distance_Set)
                        {
                            rope_Joint.distance = Vector2.Distance(transform.position, ropePosition);
                            distance_Set = true;
                        }
                    }
                }
                // Set the rope anchor to the second-to-last rope position where the current hinge/anchor should be, or if there is only one rope position, then set that one to be the anchor point. This configures the ropeJoint distance to the distance between the player and the current rope position being looped over.
                else if (i - 1 == rope_Positions.IndexOf(rope_Positions.Last()))
                {
                    var ropePosition = rope_Positions.Last();
                    rope_Hinge_Anchor_Rb.transform.position = ropePosition;
                    if (!distance_Set)
                    {
                        rope_Joint.distance = Vector2.Distance(transform.position, ropePosition);
                        distance_Set = true;
                    }
                }
            }
            else
            {
                // This else block handles setting the rope's last vertex position to the player's current position.
                rope_Renderer.SetPosition(i, transform.position);
            }
        }
    }

    // This method takes in two parameters, a RaycastHit2D object, and a PolygonCollider2D. All the rocks in the level have PolygonCollider2D colliders, so this will work well as long as you're always using PolygonCollider2D shapes.
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

    private void HandleRopeLength()
    {
        // This if..elseif block looks for vertical axis input (up/down or W/S on the keyboard), and depending on the ropeAttached iscColliding flags will either increase or decrease the ropeJoint distance, having the effect of lengthening or shortening the rope.
        if (Input.GetAxis("Vertical") >= 1f && rope_Attached && !isColliding)
        {
            rope_Joint.distance -= Time.deltaTime * climbSpeed;
        }
        else if (Input.GetAxis("Vertical") < 0f && rope_Attached)
        {
            rope_Joint.distance += Time.deltaTime * climbSpeed;
        }
    }

    //If a Collider is currently touching another physics object in the game, the OnTriggerStay2D method will continuously fire, setting the isColliding flag to true. This means whenever the slug is touching a rock, the isColliding flag is being set to true.
    //The OnTriggerExit2D method will fire when one collider leaves another's collider area, setting the flag to false.
    //Just be warned: the OnTriggerStay2D method can be very performance-intensive, so be careful with its use.

    void OnTriggerStay2D(Collider2D colliderStay)
    {
        //Debug.Log(colliderStay.gameObject.name);
        if(colliderStay.isTrigger == false)
            isColliding = true; 
    }

    private void OnTriggerExit2D(Collider2D colliderOnExit)
    {
        isColliding = false;
    }

}