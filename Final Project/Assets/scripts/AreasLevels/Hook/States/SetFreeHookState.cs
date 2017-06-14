using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetFreeHookState : AbstractHookState {

    public SetFreeHookState(hook pHook) : base(pHook)
    {
    }
    public override void Start()
    {
        Debug.Log("entered setfree state");
        GameManager.Scorehandler.BankScore();
        //basic.Scorehandler.ToggleHookScoreUI(false);
        for (int i = 0; i < _hook.FishOnHook.Count; i++)
        {

            GameManager.Fishspawner.QueueFishAgain(_hook.FishOnHook[i], false, true, true); //_hook.FishOnHook[i].SetState(fish.FishState.PiledUp);
        }
        _hook.FishOnHook.Clear();
        /*for (int i = 0; i < _hook.TrashOnHook.Count; i++) _hook.TrashOnHook[i].SetState(trash.TrashState.PiledUp);
        _hook.TrashOnHook.Clear();*/

        /*if (basic.GlobalUI.GetInTutorial() == true)
        {
            basic.Tempfishspawn._boatSetUp = false;
            basic.ClearFishList(true);
            if (basic.GlobalUI.GetReelUpCompleted() == true)
            {
                basic.Tempfishspawn._boatSetUp = true;
                basic.Seafloorspawning.SpawnTrash();
                basic.Seafloorspawning.SpawnSpecialItems();
            }
        }*/

        /*if ((!basic.GlobalUI.MoveBoatCompleted && basic.GlobalUI.DropHookCompleted && !basic.GlobalUI.ReelUpHookCompleted) || basic.GlobalUI.MoveBoatCompleted || !basic.GlobalUI.InTutorial)
        {
            basic.GlobalUI.SwitchHookButtons();
        }*/
        GameManager.Camerahandler.SetViewPoint(CameraHandler.FocusPoint.Ocean);
        GameManager.Boat.SetState(boat.BoatState.Stationary);
        //GameManager.Radar.SetState(radar.RadarState.Pulse);
        SetState(hook.HookState.None);
    }
    public override void Update()
    {
    }
    public override void Refresh()
    {
        _hook.FishOnHook = null;
        _hook.FishOnHook = new List<fish>();
        _hook.TrashOnHook = null;
        _hook.TrashOnHook = new List<trash>();
    }
    public override hook.HookState StateType()
    {
        return hook.HookState.SetFree;
    }
}
