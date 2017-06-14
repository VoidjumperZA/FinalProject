using UnityEngine;
using System.Collections;

public class Scannable : MonoBehaviour
{
    [SerializeField] private general GeneralScript;
	//public Animator UIAnim;
    private bool locked;
    private float scanTime;
    private float timeLeft;
    private float timeOutTime;
    void Start()
    {
        locked = true;
    }

	public void Ping()
	{
            Debug.Log("Ping");

            GeneralScript.FishRenderer.enabled = true;
            GeneralScript.FishOutliner.enabled = true;
            GeneralScript.Visible = true;
            timeOutTime = scanTime;
            timeLeft = scanTime;
            //StartCoroutine(TimeOutOutline());   
    }

    void Update()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0)
        {
            GeneralScript.FishRenderer.enabled = false;
            GeneralScript.FishOutliner.enabled = false;
            GeneralScript.Visible = false;
        }
    }

    IEnumerator TimeOutOutline()
    {
        yield return new WaitForSeconds(scanTime);
        GeneralScript.FishRenderer.enabled = false;
        GeneralScript.FishOutliner.enabled = false;
        GeneralScript.Visible = false;
    }

    public void SetLockState(bool pState)
    {
        locked = pState;
    }

    public bool IsLocked()
    {
        return locked;
    }

    public void SetScanTime(float pTimeAsSeconds)
    {
        scanTime = pTimeAsSeconds;
    }
}
