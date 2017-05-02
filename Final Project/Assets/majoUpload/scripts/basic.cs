using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class basic : MonoBehaviour {
    [SerializeField] private boat _boat;
    [SerializeField] private Camera _cam;
    private RaycastHit _hitInfo;

    private bool _selectedBoat = false;
	void Start () {
		
	}
	void Update () {
        SelectBoat();
        MoveBoat2D();
        DeselectBoat();
    }
    private void SelectBoat()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(new Ray(Camera.main.ScreenToWorldPoint(Input.mousePosition), gameObject.transform.forward), out _hitInfo))
            {
                _selectedBoat = _hitInfo.collider.gameObject.CompareTag("Boat") ? true : false;
            }
        }
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
        }
    }
}
