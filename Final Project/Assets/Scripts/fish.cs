using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fish : general {
    [SerializeField]
    private float _speed;
    private GameObject _hook = null;
    public enum FishType { Small, Medium, Large, Hunted };
    public FishType fishType;

    private bool _caught = false;
    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (!_caught) gameObject.transform.Translate(Vector3.forward * _speed);
        else FollowHook();
        //if (gameObject.transform.position.x < -9) Destroy(gameObject);
    }
    private void FollowHook()
    {
        if (!_hook) return;
        gameObject.transform.position = _hook.transform.position;
        //gameObject.transform.rotation = _hook.transform.rotation;
        //gameObject.transform.Rotate(0.0f, 90.0f, 0.0f);
        /*Vector3 differenceVector = _hook.transform.position - gameObject.transform.position;
        if (differenceVector.magnitude >= _speed)
        {
            gameObject.transform.Translate(differenceVector.normalized * _speed);
        }*/
    }
    public void Catch(GameObject pHook)
    {
        _hook = pHook;
        _caught = true;
        transform.position = pHook.transform.position;
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
            Destroy(gameObject);
        }
    }
}
