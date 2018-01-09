using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EAreaType
{   
    Rect,
    //Triangle,
    Arc,
    //Circle,
    //Line,
    Length
}   

[System.Serializable]
public class AreaParam
{   
    public AreaParam() { }
    public AreaParam( bool random  )
    {   
        AreaType = ((EAreaType)(Random.Range(0, (int)EAreaType.Length)));
        Range = Random.Range(1, 10);
        Width = Random.Range(1, 4);
        Angle = Random.Range(5, 75);
    }   
    [Header("EAreaType")]
    public EAreaType AreaType = EAreaType.Rect;
    public float Range;
    public float Width;
    public float Angle;
}   


public class AreaSetter : MonoBehaviour {


    [Header("AreaParam")]
    [ContextMenuItem("Randomize AreaParam", "RandomizeParam")]
    public AreaParam AreaParam;
        
    const float baseScaleFacter = 0.1f;
    Vector2[] poly = new Vector2[4];
    Transform rectL;
    Transform rectR;
    Transform rectLF;
    Transform rectRF;
    Transform[] rects;
    //======debug obj===========
    Transform _arcTrans;
    Transform arcTrans { get { if (_arcTrans == null) _arcTrans = transform.Find("Arc"); return _arcTrans; } }
    Transform _rectTrans;
    Transform rectTrans { get { if (_rectTrans == null) _rectTrans = transform.Find("Rect"); return _rectTrans; } }
    Transform _debugL;
    Transform _debugR;

    Transform debugLeft { get { if (_debugL == null) _debugL = transform.Find("_debugL"); _debugL.transform.parent = transform; return _debugL; } }
    Transform debugRight { get { if (_debugR == null) _debugR = transform.Find("_debugR"); _debugR.transform.parent = transform; return _debugR; } }

    public bool ChechIsInside(AreaCollider t)
    {   
        if (AreaParam.AreaType == EAreaType.Rect)
            return t.IsInsideRect(poly, rects);
        else if (AreaParam.AreaType == EAreaType.Arc)
            return t.isWithinCircle(transform, AreaParam.Angle,AreaParam.Range);
        return false;
    }   

    // Use this for initialization
    void Start () {
        InitRectTrans();
        rects = new Transform[4];
        rects[0] = rectL;
        rects[1] = rectR;
        rects[2] = rectRF;
        rects[3] = rectLF;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnEnable()
    {
        InitRectTrans();
    }

    private void InitRectTrans()
    {
        if (!rectL) rectL = transform.Find("l");
        if (!rectR) rectR = transform.Find("r");
        if (!rectLF) rectLF = transform.Find("lf");
        if (!rectRF) rectRF = transform.Find("rf");
    }   

    private void OnDrawGizmos()
    {   
        if (Application.isPlaying) return;
        //fwd
        Debug.DrawRay( transform.position,transform.position + transform.forward * this.AreaParam.Range ,new Color( 1,1,1,0.1f ) );
        if (this.AreaParam.AreaType == EAreaType.Arc)
        {   
            // left
            debugLeft.transform.localEulerAngles = new Vector3( 0,-AreaParam.Angle,0 );
            debugRight.transform.localEulerAngles = new Vector3(0, AreaParam.Angle, 0);
            debugLeft.transform.localPosition = debugRight.transform.localPosition = Vector3.zero;
            Debug.DrawRay(debugLeft.transform.position, debugLeft.transform.position + debugLeft.transform.forward * this.AreaParam.Range, new Color(1, 1, 1, 0.1f));
            Debug.DrawRay(debugRight.transform.position, debugRight.transform.position + debugRight.transform.forward * this.AreaParam.Range, new Color(1, 1, 1, 0.1f));
        }   
        else if (this.AreaParam.AreaType == EAreaType.Rect)
        {
            
        }   
    }

    //=============   Edit Func ==================
    [ContextMenu("RandomizeParam")]
    private void RandomizeParam()
    {   
        AreaParam = new AreaParam(true);
        ApplyTypeDraw();
    }

    [ContextMenu("Apply Area Setter Setting")]
    public void ApplyTypeDraw()
    {   
        Debug.LogError("Apply Area Setter Setting");
        InitRectTrans();
        rectTrans.gameObject.SetActive(AreaParam.AreaType == EAreaType.Rect);
        arcTrans.gameObject.SetActive(AreaParam.AreaType == EAreaType.Arc);

        if (this.AreaParam.AreaType == EAreaType.Arc)
        {   
            arcTrans.localScale = this.AreaParam.Range * baseScaleFacter * Vector3.one * 2;
            var m = arcTrans.GetComponent<Renderer>();
            m.material.SetFloat("_Angle", AreaParam.Angle);

            rectL.transform.localPosition = rectR.transform.localPosition = rectLF.transform.localPosition = rectRF.transform.localPosition = Vector3.zero;
        }   
        else if (this.AreaParam.AreaType == EAreaType.Rect)
        {   
            rectTrans.localScale = baseScaleFacter * new Vector3(this.AreaParam.Width, 0, this.AreaParam.Range);
            rectTrans.localPosition = new Vector3(0, 0, this.AreaParam.Range/2);
            rectL.transform.localPosition = new Vector3( -AreaParam.Width/2,0,0 );
            rectR.transform.localPosition = new Vector3(AreaParam.Width / 2, 0, 0);
            rectLF.transform.localPosition = new Vector3(-AreaParam.Width / 2, 0, AreaParam.Range);
            rectRF.transform.localPosition = new Vector3(AreaParam.Width / 2, 0, AreaParam.Range);
        }   

    }

}
