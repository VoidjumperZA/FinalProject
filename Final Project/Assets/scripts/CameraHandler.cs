using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CameraHandler {
    //public static CameraHandler Instance;
    private static Camera _camera { get { return Camera.main; } }
    public static void SetParent(Transform pTransform)
    {
        _camera.transform.SetParent(pTransform);
    }
}
