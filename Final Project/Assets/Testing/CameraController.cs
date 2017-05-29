using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TrackID { None, BoatToOcean, OceanToBoat, OceanToHook, HookToOcean }
public class CameraController : MonoBehaviour {
    private Camera _camera;

    /*[SerializeField] private bool _loop = true;
    [SerializeField] private bool _mirror = false;
    [SerializeField] private bool _flipped = false;
    [SerializeField] [Range(0, 10)] private int _rail;*/

    [SerializeField] private List<Rail> _rails;
    [HideInInspector] public Dictionary<TrackID, Rail> _actualRails = new Dictionary<TrackID, Rail>();
    private Rail _selected = null;

    // Use this for initialization
    void Start () {
        _camera = Camera.main;

        _actualRails.Clear();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.U)) SetRail(TrackID.BoatToOcean);
        if (Input.GetKeyDown(KeyCode.I)) SetRail(TrackID.OceanToHook);
        if (Input.GetKeyDown(KeyCode.O)) SetRail(TrackID.HookToOcean);

        if (_selected) _selected.Run(ref _camera);
    }
    public void SetRail(TrackID pTrack)
    {
        if (_selected) _selected.Refresh();
        _selected = _actualRails[pTrack];
        _selected.Activate();
    }
    private void InitializeActualRails()
    {
        _actualRails.Clear();
        _actualRails[TrackID.None] = null;
        for (int i = 0; i < _rails.Count; i++)
        {
            _actualRails[(TrackID)(i + 1)] = _rails[i];
        }
    }
    public void AddTrail(TrackID pTrack, Rail pRail)
    {
        _actualRails[pTrack] = pRail;
        if (pTrack == TrackID.BoatToOcean) _selected = _actualRails[TrackID.BoatToOcean];
    }
}
