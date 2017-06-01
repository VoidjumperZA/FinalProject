using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class hook : general
{
    // States
    private Dictionary<HookState, AbstractHookState> _stateCache = new Dictionary<HookState, AbstractHookState>();
    private AbstractHookState _abstractState = null;
    public enum HookState { None, FollowBoat, Fish, Reel, SetFree }
    [SerializeField] private HookState _hookState = HookState.None;

    [HideInInspector] public List<fish> FishOnHook = new List<fish>();
    [HideInInspector] public List<trash> TrashOnHook = new List<trash>();

    // Class references
    private GameObject _manager;
    private InputTimer _inputTimer;
   
    private GlobalUI _globalUI;
    private Camera _mainCamera;
    // Components
    [SerializeField] private Rigidbody _rigidBody;
    
    // Fishing
    private boat _boat;
    List<GameObject> fishAttachedToHook = new List<GameObject>();
    float fishRotationAngle = 25.0f;

    // Movement
    [SerializeField] private float _sideSpeed;
    [SerializeField] private float _downSpeed;
    [SerializeField] private float _fallSpeed;
    [SerializeField] private float _reelSpeed;


    private Vector3 _xyOffset;
    private Vector3 _velocity;
    // X Velocity damping


    public Transform HookTip;
	public ParticleSystem JellyAttackEffect;

    private bool valid;
    public override void Start()
    {
        valid = true;
        InitializeStateMachine();
        //basic.GlobalUI.GetOceanCleanUpBar().gameObject.GetComponent<OceanCleanUpUIAnimation>().SetBarPosition();
    }

    //
    public override void Update()
    {
        _abstractState.Update(); 
    }
    public void SetState(HookState pState)
    {
        if (_abstractState != null) _abstractState.Refresh();
        _abstractState = _stateCache[pState];
        _abstractState.Start();
    }
    private void InitializeStateMachine()
    {
        _stateCache.Clear();
        _stateCache[HookState.None] = new NoneHookState(this);
        _stateCache[HookState.FollowBoat] = new FollowBoatHookState(this, basic.Boat);
        _stateCache[HookState.Fish] = new FishHookState(this, _sideSpeed, _downSpeed, _fallSpeed);
        _stateCache[HookState.Reel] = new ReelHookState(this, _reelSpeed);
        _stateCache[HookState.SetFree] = new SetFreeHookState(this);
        SetState(_hookState);
    }
    
 
    private void OnTriggerEnter(Collider other)
    {
        if (other && _abstractState != null) _abstractState.OnTriggerEnter(other);
        if (_hookState == HookState.Fish)
        { 

            //On contact with a fish
            if (other.gameObject.CompareTag("Fish"))
            {
                //ATTACH FISH TO HOOK
                //Rotate the fish by a small degree
                float fishAngle = Random.Range(-fishRotationAngle, fishRotationAngle);
                other.gameObject.transform.Rotate(fishAngle, 0.0f, 0.0f);
            }
        }
    }
    public void AssignBoat(boat pBoat)
    {
        _boat = pBoat;
    }
	public void EnableJellyAttackEffect() 
	{
		StartCoroutine(JellyAttackCoroutine());
	}

	private IEnumerator JellyAttackCoroutine()
    {
        JellyAttackEffect.gameObject.SetActive(true);


        yield return new WaitForSeconds (0.95f);
		JellyAttackEffect.gameObject.SetActive (false);
	}
}
