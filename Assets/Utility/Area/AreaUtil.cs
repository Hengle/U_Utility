using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaUtil {

    public static Vector2 GetV2(Transform trans)
    {   
        //if (trans == null) return Vector2.zero;
        return new Vector2(trans.position.x, trans.position.z);
    }   

   // declay member of poly and init  Vector2[];
   // [ rect ]
    public static bool IsInsideRect(Vector2[] poly, Transform[] trans, Vector2 target)
    {   
        poly[0] = AreaUtil.GetV2(trans[0]);
        poly[1] = AreaUtil.GetV2(trans[1]);
        poly[2] = AreaUtil.GetV2(trans[2]);
        poly[3] = AreaUtil.GetV2(trans[3]);
        //Vector2 t = AreaUtil.GetV2(target);
        return AreaUtil.IsInsidePoly(poly, target); 
    }

    public static bool IsInsidePoly(Vector2[] p, Vector2 v)
    {
        int j = p.Length - 1;
        bool c = false;
        for (int i = 0; i < p.Length; j = i++) c ^= p[i].y > v.y ^ p[j].y > v.y && v.x < (p[j].x - p[i].x) * (v.y - p[i].y) / (p[j].y - p[i].y) + p[i].x;
        return c;
    }


    public static bool isFront(  Transform target, Transform origin)
    {   
        Vector3 forward = origin.TransformDirection(Vector3.forward);
        Vector3 toOther = target.position - origin.position;
        return Vector3.Dot(forward, toOther) > 0;
    }

    public static bool isWithinDistance(Vector3 target, Transform origin, float radius)
    {   
         return Vector3.Distance( target, origin.position) <= radius;
    }

    // [ Rag ]
    // angle 180 is Cirle
    // angle 90 is half cirle
    public static bool isWithinAngle(Vector3 target, Transform origin, float angle)
    {       
        Vector3 targetDir = target - origin.position;
        float _val = Vector3.Angle(targetDir, origin.forward);
        return (_val <= angle);
    }

    // [ Rag ]
    public static bool isWithinCircle(Vector3 target, Transform origin, float angle, float radius)
    {   
        return isWithinAngle(target, origin, angle) && isWithinDistance(target,origin, radius);
    }

}
