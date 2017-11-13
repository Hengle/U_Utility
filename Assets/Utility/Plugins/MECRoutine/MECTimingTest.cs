using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MovementEffects;



/// <summary>
///  Unity StartCoroutine 最少有 32b 的 GC ，而且会叠加 
///     用  Timing.RunCoroutine
/// </summary>
public class MECTimingTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
        
        //StartCoroutine( this.Routine() );
        //Timing.RunCoroutine( this.Routine() );
        Timing.RunCoroutine(this.LoadAsync( "obj",  (x) => { GameObject.Instantiate(x); })  );
    }   
	
	// Update is called once per frame
	void Update () {


    }

    IEnumerator<float> LoadAsync(  string path,  System.Action<Object> obj )
    {
        var aysc = Resources.LoadAsync(path);

        while ( !aysc.isDone )
        {   
            yield return Timing.WaitForOneFrame;
        }   
        if (obj != null)
            obj( aysc.asset );
    }   

    IEnumerator<float> Routine( )
    {   
        //Debug.LogError( "start Routine" );
        yield return Timing.WaitForSeconds( 1f );
        //Debug.LogError("Routine end .>>>>>>>>>>>>>>>>>>>");

    }   
}
