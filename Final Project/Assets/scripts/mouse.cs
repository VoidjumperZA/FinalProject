using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouse : MonoBehaviour {
    public static mouse Instance;

    private RaycastHit _hitInfo;
    private Vector3 _previousWorldPoint;

	void Start () {
        if (Instance == null) Instance = this;
        Debug.Log("Mouse instance created!");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public Vector3 GetWorldPoint()
    {
        if (GetRaycastHit().HasValue) return GetRaycastHit().Value.point;
        return Vector3.zero;
    }
    public general GetGeneral()
    {
        if (GetRaycastHit().HasValue) return GetRaycastHit().Value.collider.gameObject.GetComponent<general>();
        return null;
    }
    public RaycastHit? GetRaycastHit()
    {
        if (Physics.Raycast(new Ray(Camera.main.ScreenToWorldPoint(Input.mousePosition), gameObject.transform.forward), out _hitInfo))
        {
            return _hitInfo;
        }
        return null;
    }
}
