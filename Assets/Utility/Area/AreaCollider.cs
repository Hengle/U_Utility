using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ColliderType{
    Box,
    Cirlce,
    None
}   

public class AreaCollider : MonoBehaviour {
    public ColliderType ColliderType = ColliderType.Box;
    public float Radius = 1f;

    Vector3[] pos = new Vector3[4];

    public bool IsInsideRect(Vector2[] poly ,Transform[] trans)
    {  
       GetPos();
       var a = AreaUtil.IsInsideRect(poly, trans, new Vector2(pos[0].x, pos[0].z));
       var b = AreaUtil.IsInsideRect(poly, trans, new Vector2(pos[1].x, pos[1].z));
       var c = AreaUtil.IsInsideRect(poly, trans, new Vector2(pos[2].x, pos[2].z));
       var d = AreaUtil.IsInsideRect(poly, trans, new Vector2(pos[3].x, pos[3].z));
       return a || b || c || d;

    }   

    public bool isWithinCircle( Transform origin, float angle, float radius)
    {   
        GetPos();
        bool b = false;
        for (int i = 0; i < 4; i++)
        {
            if (AreaUtil.isWithinCircle(  pos[i], origin, angle,radius))
                return true;
        }
        return b;
    }

    void GetPos()
    {   
        var t = transform.position;
        t = new Vector3( t.x,0,t.z );
        pos[0] = t + transform.forward * Radius / 2 + transform.right * Radius / 2;
        pos[1] = t + transform.forward * Radius / 2 - transform.right * Radius / 2;
        pos[2] = t - transform.forward * Radius / 2 - transform.right * Radius / 2;
        pos[3] = t - transform.forward * Radius / 2 + transform.right * Radius / 2;
    }   

	// Use this for initialization
	void Start () {
        pos = new Vector3[4];
	}   
	
	// Update is called once per frame
	void Update () {
		
	}

    Color colliderC = new Color(0, 0.6f, 0.2f);
    private void OnDrawGizmos()
    {   
        GetPos();
        Debug.DrawLine(pos[0], pos[1],colliderC);
        Debug.DrawLine(pos[1], pos[2], colliderC);
        Debug.DrawLine(pos[2], pos[3], colliderC);
        Debug.DrawLine(pos[3], pos[0], colliderC);
    }
}
