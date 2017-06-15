using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class radar : general
{
    // States
    private Dictionary<RadarState, AbstractRadarState> _stateCache = new Dictionary<RadarState, AbstractRadarState>();
    private AbstractRadarState _abstractState = null;
    public enum RadarState { None, Pulse }
    [SerializeField] private RadarState _radarState = RadarState.None;
    // Fish detector
    [SerializeField] private float _scrollSpeed;
    [SerializeField] private float _fadeOutDuration;
    [SerializeField] private int _collectableStaysVisibleRange;
    [SerializeField] private GameObject _radarAngleSlider;
    [SerializeField] private Renderer _renderer; [HideInInspector] public Renderer Renderer { get { return _renderer; } }
	public override void Start() {
        InitializeStateMachine();
	}
    public override void FixedUpdate()
    {
        _abstractState.FixedUpdate();
        Debug.Log(_abstractState.StateType());
    }
    public void SetState(RadarState pState)
    {
        if (_abstractState != null) _abstractState.Refresh();
        _abstractState = _stateCache[pState];
        _abstractState.Start();
    }
    private void InitializeStateMachine()
    {
        _stateCache.Clear();
        _stateCache[RadarState.None] = new NoneRadarState(this);
        _stateCache[RadarState.Pulse] = new PulseRadarState(this, GetRadarAngle(), _scrollSpeed, _fadeOutDuration, _collectableStaysVisibleRange);
        SetState(_radarState);
    }
    public float GetRadarAngle()
    {
        //return Vector3.Dot(gameObject.transform.right, -gameObject.transform.up);
        return Vector3.Dot((_radarAngleSlider.transform.position - gameObject.transform.position).normalized, -gameObject.transform.up);
    }
}