using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class mouse {
    private static RaycastHit _hitInfo;
    private static Vector3 _previousWorldPoint;
    public static Vector3 GetWorldPoint()
    {
        if (GetRaycastHit().HasValue) return GetRaycastHit().Value.point;
        return Vector3.zero;
    }
    public static general GetGeneral()
    {
        if (GetRaycastHit().HasValue)
        {
            //Debug.Log(GetRaycastHit().Value.collider.gameObject.tag + " :Name");
            return GetRaycastHit().Value.collider.gameObject.GetComponent<general>();
        }
            return null;
    }
    public static RaycastHit? GetRaycastHit()
    { 
        
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out _hitInfo, 1 << 8))
        {
            Debug.Log("RAYCAST HIT SUCCESSFUL");
            return _hitInfo;
        }
        return null;
    }
    public static bool GameObjectTagIs(string pTag)
    {
        return GetGeneral() ? GetGeneral().gameObject.CompareTag(pTag) : false;
    }
}
