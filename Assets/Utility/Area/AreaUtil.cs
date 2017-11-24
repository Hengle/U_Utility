using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaUtil {

    public static Vector2 GetV2(Transform trans)
    {   
        return new Vector2(trans.position.x, trans.position.z);
    }   

   // declay member of poly and init  Vector2[];
   // [ rect ]
   public static bool IsInsideRect(Vector2[] poly ,Transform[] trans, Transform target)
    {   
        poly[0] = AreaUtil.GetV2(trans[0]);
        poly[1] = AreaUtil.GetV2(trans[1]);
        poly[2] = AreaUtil.GetV2(trans[2]);
        poly[3] = AreaUtil.GetV2(trans[3]);
        Vector2 t = AreaUtil.GetV2(target);
        return AreaUtil.IsInsidePoly(poly, t); 
    }   

    public static bool IsInsidePoly(Vector2[] poly, Vector2 target)
    {   
        int i, j;
        int nvert = poly.Length;
        bool c = false;
        for (i = 0, j = nvert - 1; i < nvert; j = i++)
        {
            if (((poly[i].y > target.y) != (poly[j].y > target.y)) &&
             (target.x < (poly[j].x - poly[i].x) * (target.y - poly[i].y) / (poly[j].y - poly[i].y) + poly[i].x))
                c = !c;
        }
        return c;
    }

    public static bool isFront(  Transform target, Transform origin)
    {   
        Vector3 forward = origin.TransformDirection(Vector3.forward);
        Vector3 toOther = target.position - origin.position;
        return Vector3.Dot(forward, toOther) > 0;
    }

    public static bool isWithinDistance(Transform target, Transform origin, float radius )
    {   
         return Vector3.Distance( target.position, origin.position) <= radius;
    }

    // [ Rag ]
    // angle 180 is Cirle
    // angle 90 is half cirle
    public static bool isWithinAngle(Transform target, Transform origin, float angle)
    {       
        Vector3 targetDir = target.position - origin.position;
        float _val = Vector3.Angle(targetDir, origin.forward);
        return (_val <= angle);
    }

    // [ Rag ]
    public static bool isWithinCircle(Transform target, Transform origin, float angle, float radius)
    {   
        return isWithinAngle(target, origin, angle) && isWithinDistance(target,origin, radius);
    }

}
