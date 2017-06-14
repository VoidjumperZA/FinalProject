using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
[ExecuteInEditMode]
public class Sonar : MonoBehaviour
{
    [SerializeField] private Transform ScannerOrigin;
    [SerializeField] private Transform ScannerBoundary;
    [SerializeField] private Material EffectMaterial;
    [SerializeField] private float scanSpeed;
    [SerializeField] private float outlinerVisableTimeAsSeconds;
    private float ScanDistance;
    [SerializeField]
    private bool linearTrueExponentialFalse;

    private Camera _camera;
    private bool pingLock;

    //Is scanning and scannable objects
    bool _scanning;
    Scannable[] _scannables;

    void Start()
    {
        pingLock = false;
        Debug.Log("Scanning = " + _scanning);
    }

    void Update()
    {
        _scannables = FindObjectsOfType<Scannable>();
        //Debug.Log("Scannable Size: " + _scannables.Length);
        if (_scanning)
        {
            checkOrigin();
            //progress distance linearly or exponentially
            if (linearTrueExponentialFalse == true)
            {
                ScanDistance += Time.deltaTime * scanSpeed;
            }
            else
            {
                ScanDistance += ScanDistance * (Time.deltaTime * scanSpeed);
            }

            foreach (Scannable objects in _scannables)
            {
                //if our items are inside our scan range
                if (objects.IsLocked() == false && Vector3.Distance(ScannerOrigin.position, objects.transform.position) <= ScanDistance)
                {
                    //send a call to switch on their outline shaders
                    objects.Ping();
                    objects.SetLockState(true);
                }

            }
            if (IsInput() == true && Vector3.Distance(ScannerOrigin.position, ScannerBoundary.transform.position) <= ScanDistance)
            {
                FirePulse();
            }
           
        }


        if (IsInput() == true && _scanning == false)
        {
            FirePulse();
        }
        //test expand from origin
        if (Input.GetKeyDown(KeyCode.C))
        {
            FirePulse();
            /*
            _scanning = true;
            ScanDistance = 0;
            foreach (Scannable objects in _scannables)
            {
                objects.SetLockState(false);
                objects.SetScanTime(outlinerVisableTimeAsSeconds);
            }*/
        }

        //scan from ray click
        /*if (Input.GetMouseButtonDown(0))
        {
            //fire ray
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                //start a scan from our hitpoint
                _scanning = true;
                ScanDistance = 0;
                ScannerOrigin.position = hit.point;
            }
        }*/
    }

    private bool IsInput()
    {
        if (Input.GetMouseButton(0) || mouse.Touching())
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private void checkOrigin()
    {
        Vector3 position;
        if (GameManager.Boat.GetAbstractState().StateType() != (boat.BoatState.Fish))
        {
            position = GameManager.Boat.transform.position;
        }
        else
        {
            position = GameManager.Hook.transform.position;
        }
        ScannerOrigin.position = position;
    }

    /// <summary>
    /// Fire a sonar pulse, given point acts as scan origin.
    /// </summary>
    /// <param name="pPulseOrigin"></param>
    public void FirePulse(/*Vector3 pPulseOrigin*/)
    {
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            foreach (Scannable objects in _scannables)
            {
                objects.SetLockState(false);
                objects.SetScanTime(outlinerVisableTimeAsSeconds);
            }
            _scanning = true;
            ScanDistance = 0;
        }
      
       // ScannerOrigin.position = pPulseOrigin;
       
    }
    void OnEnable()
    {
        _camera = Camera.main;
        _camera.depthTextureMode = DepthTextureMode.Depth;
    }

    //[ImageEffectOpaque]
    void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        //feed transform to shasder as material property
        EffectMaterial.SetVector("_WorldSpaceScannerPos", ScannerOrigin.position);
        EffectMaterial.SetFloat("_ScanDistance", ScanDistance);
        RaycastCornerBlit(src, dst, EffectMaterial);
    }

    void RaycastCornerBlit(RenderTexture source, RenderTexture dest, Material mat)
    {
        // Compute Frustum Corners
        float camFar = _camera.farClipPlane;
        float camFov = _camera.fieldOfView;
        float camAspect = _camera.aspect;

        float fovWHalf = camFov * 0.5f;

        Vector3 toRight = _camera.transform.right * Mathf.Tan(fovWHalf * Mathf.Deg2Rad) * camAspect;
        Vector3 toTop = _camera.transform.up * Mathf.Tan(fovWHalf * Mathf.Deg2Rad);

        Vector3 topLeft = (_camera.transform.forward - toRight + toTop);
        float camScale = topLeft.magnitude * camFar;

        topLeft.Normalize();
        topLeft *= camScale;

        Vector3 topRight = (_camera.transform.forward + toRight + toTop);
        topRight.Normalize();
        topRight *= camScale;

        Vector3 bottomRight = (_camera.transform.forward + toRight - toTop);
        bottomRight.Normalize();
        bottomRight *= camScale;

        Vector3 bottomLeft = (_camera.transform.forward - toRight - toTop);
        bottomLeft.Normalize();
        bottomLeft *= camScale;

        // Custom Blit, encoding Frustum Corners as additional Texture Coordinates
        RenderTexture.active = dest;

        mat.SetTexture("_MainTex", source);

        GL.PushMatrix();
        GL.LoadOrtho();

        mat.SetPass(0);

        GL.Begin(GL.QUADS);

        GL.MultiTexCoord2(0, 0.0f, 0.0f);
        GL.MultiTexCoord(1, bottomLeft);
        GL.Vertex3(0.0f, 0.0f, 0.0f);

        GL.MultiTexCoord2(0, 1.0f, 0.0f);
        GL.MultiTexCoord(1, bottomRight);
        GL.Vertex3(1.0f, 0.0f, 0.0f);

        GL.MultiTexCoord2(0, 1.0f, 1.0f);
        GL.MultiTexCoord(1, topRight);
        GL.Vertex3(1.0f, 1.0f, 0.0f);

        GL.MultiTexCoord2(0, 0.0f, 1.0f);
        GL.MultiTexCoord(1, topLeft);
        GL.Vertex3(0.0f, 1.0f, 0.0f);

        GL.End();
        GL.PopMatrix();
    }
}
