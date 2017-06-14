using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    [SerializeField]
    private Transform[] links;
    [SerializeField]
    private float distance;
    private LineRenderer lineRenderer;
    private GameObject activeCube;
    private int activeLink;
    // Use this for initialization
    void Start()
    {
        activeLink = 0; 
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = links.Length;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (activeLink == 0)
            {
                activeLink = links.Length - 1;
            }
            else
            {
                activeLink = 0;
            }
            Debug.Log("Switching link to " + activeLink);
        }

        /*foreach (Transform link in links)
        {
            
        }*/
        Vector3 previousPoint = links[activeLink].gameObject.transform.position;
        if (activeLink == 0)
        {
            for (int i = 0; i < links.Length; i++)
            {
                calcuate(links[i], previousPoint);
            }
            float time = Time.time;
            for (int i = 0; i < links.Length; i++)
            {
                lineRenderer.SetPosition(i, links[i].transform.position);
            }
        }
        else
        {
            for (int i = links.Length; i > 0; i--)
            {
                calcuate(links[i], previousPoint);
            }
            float time = Time.time;
            for (int i = 0; i < links.Length; i++)
            {
                lineRenderer.SetPosition(i, links[i].transform.position);
            }
        }
        

    }

    private void calcuate(Transform pLink, Vector3 pPrevPoint)
    {
        
        Vector3 direction = (pPrevPoint - pLink.position).normalized;
        pLink.position = pPrevPoint - direction * distance;
        pPrevPoint = pLink.position;



    }
}
