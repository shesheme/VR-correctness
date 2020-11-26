using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class laserPointer : MonoBehaviour
{
    public GameObject target, line;

    Color color;
    Vector3 startPos, endPos;
    float maxDistance = 30.0f;
    LineRenderer lineRenderer;
    RaycastHit hit;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void createLineRenderer()
    {
        color = Color.green;
        startPos = target.transform.position;
        endPos = startPos * 2;

        lineRenderer = line.GetComponent<LineRenderer>();
        lineRenderer.useWorldSpace = false;
        lineRenderer.receiveShadows = false;

        lineRenderer.positionCount = 2;


        lineRenderer.startWidth = 0.03f;
        lineRenderer.endWidth = 0.005f;
    }
    // Update is called once per frame
    void Update()
    {
        if(Physics.Raycast(transform.position, transform.forward, out hit, maxDistance))
        {
            line.transform.position = target.transform.position;
            //lineRenderer.SetPosition(1, new Vector3(0, 0, hit.distance));
        }
    }
}
