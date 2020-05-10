using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menu : MonoBehaviour
{
    public Canvas canvas;
    public GameObject target;

    Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        offset.Set((float)0.5, (float)0.5, (float)1);
        canvas.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        canvas.transform.position = target.transform.position + offset;
    }

    public void showMenu()
    {
        canvas.enabled = !canvas.enabled;
    }

    //private void ShowLaser(RaycastHit hit)
    //{
    //    laser.SetActive(true);
    //    laser.transform.position = Vector3.Lerp(controllerPose.transform.position, hitPoint, 0.5f);
    //    laserTransform.LookAt(hitPoint);
    //    laserTransform.localScale = new Vector3(laserTransform.localScale.x, laserTransform.localScale.y, hit.distance);
    //}
}
