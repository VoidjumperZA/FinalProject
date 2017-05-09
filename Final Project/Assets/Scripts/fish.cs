using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fish : general {
    [SerializeField] private SkinnedMeshRenderer _renderer;
    [SerializeField] private cakeslice.Outline _outliner;
    [SerializeField] private float _speed;
    private hook _hook = null;
    public enum FishType { Small, Medium, Large };
    public FishType fishType;

    private bool _caught = false;
    // Use this for initialization
    public override void Start() {
        base.Start();
    }

    // Update is called once per frame
    public override void Update() {
        if (!_caught) gameObject.transform.Translate(Vector3.forward * _speed);
        else FollowHook();
    }
    private void FollowHook()
    {
        if (!_hook) return;
        gameObject.transform.position = _hook.transform.position;
    }
    private void Catch(hook pHook)
    {
        _hook = pHook;
        _caught = true;
        transform.position = pHook.gameObject.transform.position;
        _speed = 0.0f;
    }

    public void Release()
    {
        _caught = false;
    }

    public void SetFishType(FishType pType)
    {
        fishType = pType;
    }

    public void SetFishType(int pType)
    {
        fishType = (FishType)pType;
    }

    public FishType GetFishType()
    {
        return fishType;
    }

    public void SetDirection(float pPolarity)
    {
        _speed *= pPolarity;
    }

    public void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name == "Fish Despawner" || col.gameObject.tag == "Floor")
        {
            basic.Generals.Remove(gameObject.GetComponent<fish>());
            Destroy(gameObject);
        }
        if (col.gameObject.tag == "Hook")
        {
            if (!_visible) return;

            if (fishType == FishType.Large ) _hook.ReelUpTheHook();
            Catch(col.gameObject.GetComponent<hook>());
            ToggleOutliner(false);
        }
    }

    public override void ToggleOutliner(bool pBool)
    {
        if (!_caught || !pBool)
        {
            _outliner.enabled = pBool;
        }
    }
    public override void ToggleRenderer(bool pBool)
    {
        _visible = pBool;
        _renderer.enabled = pBool;
    }
}
