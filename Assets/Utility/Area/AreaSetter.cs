using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EAreaType
{   
    Rect,Triangle,Arc,Circle,Line,Length
}   

[System.Serializable]
public class AreaParam
{   
    public AreaParam() { }
    public AreaParam( bool random  )
    {   
        AreaType = ((EAreaType)(Random.Range(0, (int)EAreaType.Length)));
        Range = Random.Range(1, 10);
        Width = Random.Range(0, 4);
        Angle = Random.Range(0, 75);
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




    const float baseScaleFracter = 0.1f;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnDrawGizmos()
    {
        Debug.DrawRay( transform.position,transform.position + transform.forward * this.AreaParam.Range  );
    }

    //=============    edit func ==================

    private void RandomizeParam()
    {
        AreaParam = new AreaParam(true);
    }

    [ContextMenu("Apply Area Setter Setting")]
    public void ApplyTypeDraw()
    {
        Debug.LogError("Apply Area Setter Setting");
    }

    [ContextMenu("Save Data To SObj")]
    public void SaveDataToSObj()
    {   
        Debug.LogError("Save Data To SObj");
    }   
}
