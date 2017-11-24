using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour {
    public Transform target;
    public Transform[] cubes;

    public Transform origin;

    public bool ret;
    Vector2[] poly = new Vector2[4];

    void Start()
    {
        StartRet();
    }

        // Use this for initialization
      void StartRet () {
        ret = AreaUtil.IsInsideRect(poly, cubes, target);
    }

    void startReg()
    {   
        Debug.LogError(AreaUtil.isWithinAngle(   target,origin,180 ));
    }   
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.F1))
            startReg();

    }
}
