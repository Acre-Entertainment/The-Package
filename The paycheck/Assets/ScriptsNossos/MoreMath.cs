using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MoreMath
{
    public static float From_Vector_To_Angle(Vector2 dir)
    {
        // Pego o inverso da tangente do vetor, e multiplico pela constante de conversão entre Rad e Deg
        return Mathf.Atan2(dir.y, dir.x) * 57.29578f;
    }

    public static Vector2 FromAngleToVector(float radian_angle)
    {
        return new Vector2(Mathf.Cos(radian_angle), Mathf.Sin(radian_angle));
    }
}
