using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class general : MonoBehaviour
{
    protected bool _selected;



	public virtual void Start () {
        _selected = false;
	}
	public virtual void Update () {
	}
    public virtual void Select()
    {
        _selected = true;
    }
    public virtual void Deselect()
    {
        _selected = false;
    }
    public string GetTag()
    {
        return gameObject.tag;
    }
    public virtual void Text(string pText)
    {
        Debug.Log(pText);
    }
}
