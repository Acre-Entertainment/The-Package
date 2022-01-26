using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAProjectile : MonoBehaviour
{
    public Vector2 dir;
public float speed;
 public float angleOffSet;
void Update()
{
    Move();
}
void Move()
{
    Vector2 position = transform.position;
    transform.position = position + dir * speed * Time.deltaTime;
}

public void Change_Dir(Vector2 dir)
{
   this.dir = dir.normalized;
   RotateObject(transform.up, dir);
}

 void RotateObject(Vector2 frontDir, Vector2 targetDir)
    {
       float angle = Vector2.SignedAngle(frontDir, targetDir);
       transform.Rotate(0, 0, angle + angleOffSet);
    }
}

