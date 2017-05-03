using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class basic : MonoBehaviour {
    [SerializeField] private GameObject _boatPrefab;
    [SerializeField] private Transform _boatSpawn;

    private general _selected = null;
    private List<general> _generals = new List<general>();

    void Start()
    {
        SpawnBoat();
        Debug.Log(_generals.Count + " generals");
    }
    void Update()
    {
        SelectNewGeneral();
        DeselectPreviousGeneral();
    }
    private void SelectNewGeneral()
    {
        // On Left mouse button click
        if (!Input.GetMouseButtonDown(0)) return;
        // Deselect() previously selected object
        if (_selected)
        {
            _selected.Deselect();
            _selected = null;
        }
        // Raycast to get new object with general component
        _selected = mouse.Instance.GetGeneral();
        // Select() new object
        if (_selected)
        {
            _selected.Select();
        }
    }
    private void DeselectPreviousGeneral()
    {
        if (Input.GetMouseButtonUp(0) && _selected) _selected.Deselect();
    }
    private void SpawnBoat()
    {
        _generals.Add(Instantiate(_boatPrefab, _boatSpawn.position, Quaternion.identity).GetComponent<boat>());
        _generals.Add(((boat)_generals[0]).SpawnHook());
    }
    /*[SerializeField] private boat _boat;
    [SerializeField] private hook _hook;
    [SerializeField] private Camera _cam;
    private RaycastHit _hitInfo;

    private bool _selectedBoat = false;
    private bool _selectedHook = false;

    private bool _actionRecognised = false;
    private bool _moveBoat = false;
    private bool _dropHook = false;

    private Vector3 _initialMousePos = Vector3.zero;
    [SerializeField] private float _holdTime;
    private float _passedHoldTime = 0;

	void Start ()
    {
    }
	void Update ()
    {
        _selectedBoat = Select("Boat");
        _selectedHook = Select("Hook");
        // ---


        // ---
        Deselect(ref _selectedBoat);
        Deselect(ref _selectedHook);
    }

    private bool Select(string pTag)
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(new Ray(Camera.main.ScreenToWorldPoint(Input.mousePosition), gameObject.transform.forward), out _hitInfo))
            {
                return _hitInfo.collider.gameObject.CompareTag(pTag);
            }
        }
        return false;
    }
    private void Deselect(ref bool pSelectedObject)
    {
        if (Input.GetMouseButtonUp(0) && pSelectedObject)
        {
            pSelectedObject = false;
        }
    }

    /*private void HandleBoat()
    {
        SelectBoat();
        if (_selectedBoat)
        {
            if (!_actionRecognised)
            {
                if (_passedHoldTime < _holdTime)
                {
                    _passedHoldTime += Time.deltaTime;
                }
                else
                {
                    _moveBoat = DraggingSideways();
                    _dropHook = !DraggingSideways();

                    _actionRecognised = true;
                    _passedHoldTime = 0;
                }
            }
            else
            {
                if (_moveBoat) MoveBoat2D();
                else if (_dropHook) DropHook();
                Debug.Log("I WAS HERE");
            }
        }
        DeselectBoat();
    }
    private void HandleHook()
    {
        SelectHook();
        MoveHook2D();
        DeselectHook();
    }
    private bool DraggingSideways()
    {
        return Mathf.Abs(Input.mousePosition.x - _initialMousePos.x) > Mathf.Abs(Input.mousePosition.y - _initialMousePos.y);
    }
    private bool SelectBoat()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(new Ray(Camera.main.ScreenToWorldPoint(Input.mousePosition), gameObject.transform.forward), out _hitInfo))
            {
                _selectedBoat = _hitInfo.collider.gameObject.CompareTag("Boat") ? true : false;
                _initialMousePos = Input.mousePosition;
                return true;
            }
        }
        return false;
    }
    private void MoveBoat2D()
    {
        if (Input.GetMouseButton(0) && _selectedBoat)
        {
            if (Physics.Raycast(new Ray(Camera.main.ScreenToWorldPoint(Input.mousePosition), gameObject.transform.forward), out _hitInfo))
            {
                _boat.SetDestination(new Vector3(_hitInfo.point.x, _boat.gameObject.transform.position.y, _boat.gameObject.transform.position.z));
            }
        }
    }
    private void DeselectBoat()
    {
        if (Input.GetMouseButtonUp(0) && _selectedBoat)
        {
            _selectedBoat = false;
            _actionRecognised = false;
        }
    }
    private void DropHook()
    {
        Debug.Log("Dropping hook");
        _dropHook = false;
        _hook.Appear();
    }
    private void SelectHook()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(new Ray(Camera.main.ScreenToWorldPoint(Input.mousePosition), gameObject.transform.forward), out _hitInfo))
            {
                _selectedHook = _hitInfo.collider.gameObject.CompareTag("Hook") ? true : false;
                _initialMousePos = Input.mousePosition;
            }
        }
    }
    private void MoveHook2D()
    {
        if (Input.GetMouseButton(0) && _selectedHook)
        {
            if (Physics.Raycast(new Ray(Camera.main.ScreenToWorldPoint(Input.mousePosition), gameObject.transform.forward), out _hitInfo))
            {
                _hook.SetDestination(new Vector3(_hitInfo.point.x, 0,  _hook.gameObject.transform.position.z));
            }
        }
    }
    private void DeselectHook()
    {
        if (Input.GetMouseButtonUp(0) && _selectedHook)
        {
            _selectedHook = false;
        }
    }*/
}
