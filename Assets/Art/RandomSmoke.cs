using System;
using UnityEngine;
using Random = UnityEngine.Random;
public class RandomSmoke : MonoBehaviour
{
    private Material mtrl;
    public float fadeDelay;
    public float fadeTime;
    Renderer rendererComp;
    //private Action<float> onUpdate;
    bool goFade = false;
    void Awake()
    {
        
        if (rendererComp == null)
            rendererComp = GetComponent<Renderer>();
        if (rendererComp == null)
            return;

        //mtrl = rendererComp.material = Instantiate(rendererComp.material);
        mtrl = rendererComp.material;
        //onUpdate = x => { mtrl.SetFloat("_Alpha", x); };
        //if(onUpdate == null)
        //    onUpdate = this.onUpdateFX;
    }   

    void onUpdateFX(float value )
    {       
        mtrl.SetFloat("_Alpha", value);
    }

    void OnEnable()
    {   
        if (mtrl == null)
            return;

        mtrl.SetFloat("_Offset", Random.Range(0.0f, 1.0f));
        mtrl.SetFloat("_Alpha", 1.0f);
        goFade = true;
    }

    private void Update()
    {
        if (this.goFade)
        {
            var val = mtrl.GetFloat("_Alpha");

            val -= Time.deltaTime / fadeTime;

            onUpdateFX(val);
        }
    }

    private void OnDisable()
    {
        goFade = false;
    }
}
