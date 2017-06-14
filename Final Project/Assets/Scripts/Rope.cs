using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    [SerializeField]
    private Transform[] links;
    [SerializeField]
    private float distance;
    private const int lengthOfLineRenderer = 20;
    private LineRenderer lineRenderer;
    // Use this for initialization
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = links.Length;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 previousPoint = transform.position;
        foreach (Transform link in links)
        {
            Vector3 direction = (previousPoint - link.position).normalized;
            link.position = previousPoint - direction * distance;
            previousPoint = link.position;
        }
        float time = Time.time;
        for (int i = 0; i < links.Length; i++)
        {
            lineRenderer.SetPosition(i, links[i].transform.position);
        }
    }
}
