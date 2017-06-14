using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class mouse {
    private static RaycastHit _hitInfo;
    private static Vector3 _previousWorldPoint;

    public static Vector3 GetWorldPoint()
    {
        if (GetRaycastHit().HasValue) return GetRaycastHit().Value.point;
        Debug.Log("No RayCastHit Value");
        return Vector3.zero;
    }
    /* public static Vector2 GetTouch(int pFinger = 0)
     {
         if (Touching()) return Input.GetTouch(pFinger).position;
         return new Vector2(0, 0);

     }
   */
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
        if (Input.touchCount == 1)
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.GetTouch(0).position), out _hitInfo, 1 << 8))
            {
                //Debug.Log("RAYCAST HIT SUCCESSFUL");
                return _hitInfo;
            }
        }
        else if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out _hitInfo, 1 << 8))
        {
            return _hitInfo;
        }
        return null;
    }
    public static bool GameObjectTagIs(string pTag)
    {
        return GetGeneral() ? GetGeneral().gameObject.CompareTag(pTag) : false;
    }
    public static bool Touching()
    {
        return (Input.touchCount != 0 || Input.GetMouseButton(0));
    }
}
