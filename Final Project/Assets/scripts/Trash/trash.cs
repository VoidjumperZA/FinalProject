﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trash : general
{
    // States
    private Dictionary<TrashState, AbstractTrashState> _stateCache = new Dictionary<TrashState, AbstractTrashState>();
    private AbstractTrashState _abstractState = null;
    public enum TrashState { None, Float, FollowHook, PiledUp }
    [SerializeField] private TrashState _trashState = TrashState.None;
    [SerializeField] private int _score;
    [SerializeField] private float _spinsPerSecond;
    [SerializeField] private float _spinRadius;
    [SerializeField] private MeshRenderer _renderer;
    [SerializeField] private cakeslice.Outline _outliner;
    private float _revealDuration;
    public override void Start()
    {
        InitializeStateMachine();
    }
    public override void Update()
    {
        _abstractState.Update();
    }
    public void SetState(TrashState pState)
    {
        if (_abstractState != null) _abstractState.Refresh();
        _abstractState = _stateCache[pState];
        _abstractState.Start();
    }
    private void InitializeStateMachine()
    {
        _stateCache.Clear();
        _stateCache[TrashState.None] = new NoneTrashState(this);
        _stateCache[TrashState.Float] = new FloatTrashState(this, _spinsPerSecond, _revealDuration, _spinRadius);
        _stateCache[TrashState.FollowHook] = new FollowHookTrashState(this);
        _stateCache[TrashState.PiledUp] = new PiledUpTrashState(this);
        SetState(_trashState);
    }
    public int GetScore()
    {
        return _score;
    }
    public override void Reveal(float pRevealDuration)
    {
        _revealDuration = pRevealDuration;
        if (Revealed) return;

        ToggleOutliner(true);
        ToggleRenderer(true);
        Revealed = true;
    }
    public override void Hide()
    {
        ToggleOutliner(false);
        ToggleRenderer(false);
        Revealed = false;
    }
    public override void ToggleOutliner(bool pBool)
    {
        _outliner.enabled = pBool;
    }
    public override void ToggleRenderer(bool pBool)
    {
        Visible = pBool;
        _renderer.enabled = pBool;
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other && _abstractState != null) _abstractState.OnTriggerEnter(other);
    }
}