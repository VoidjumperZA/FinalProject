using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fish : general {
    [SerializeField]
    private float _speed;
    private GameObject _hook = null;

    private bool _caught = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (!_caught) gameObject.transform.Translate(Vector3.forward*_speed);
        else FollowHook();
        //if (gameObject.transform.position.x < -9) Destroy(gameObject);
	}
    private void FollowHook()
    {
        if (!_hook) return;
        gameObject.transform.position = _hook.transform.position;
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
        GameObject.Find("Manager").GetComponent<ScoreHandler>().AddScore(10, true);
    }

    public void SetDirection(float pPolarity)
    {
        _speed *= pPolarity;
    }

    public void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name == "Fish Despawner")
        {
            Destroy(gameObject);
        }
    }
}
