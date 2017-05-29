using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlotOfThingsScript : MonoBehaviour {
    public GameObject _object;
    public Renderer _renderer;
    private Material _material;
    [Range(0, 1.0f)]
    public float _slider;
    private Color col;
	// Use this for initialization
	void Start () {
        _material = _renderer.material;
        col = _material.color;
	}
	
	// Update is called once per frame
	void Update () {
        col.a = _slider;
        _material.color = col;
	}
}
