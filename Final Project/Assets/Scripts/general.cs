using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class general : MonoBehaviour
{
    [HideInInspector]
    public bool Visible = false;
    [HideInInspector]
    public bool Revealed = false;



    public virtual void Start()
    {

    }
    public virtual void Update()
    {

    }
    public virtual void FixedUpdate()
    {

    }
    public string GetTag()
    {
        return gameObject.tag;
    }
    public virtual void ToggleOutliner(bool pBool)
    {

    }
    public virtual void ToggleRenderer(bool pBool)
    {
        Visible = pBool;
    }
    public virtual void Reveal()
    {

    }
    public virtual void Hide()
    {

    }
}
