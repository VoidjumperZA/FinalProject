using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class general : MonoBehaviour
{
    //public Scannable ScannableScript;
    //public cakeslice.Outline FishOutliner;
   // public Renderer FishRenderer;
    [HideInInspector]
    public bool Visible = false;
    [HideInInspector]
    public bool Revealed = false;

    public virtual void Start()
    {
        //FishRenderer = ScannableScript.GetComponent<Renderer>();
    }
    public virtual void Update()
    {

    }
    public virtual void FixedUpdate()
    {

    }
  /*  public virtual void ToggleOutliner(bool pBool)
    {

    }
    public virtual void ToggleRenderer(bool pBool)
    {
        Visible = pBool;
    }*/
    public virtual void Reveal(float pFadeOutDuration, int pCollectableStaysVisibleRange)
    {

    }
    public virtual void Hide()
    {

    }
    public string GetTag()
    {
        return gameObject.tag;
    }
    public virtual void FinalizeInitialization()
    {

    }
}
