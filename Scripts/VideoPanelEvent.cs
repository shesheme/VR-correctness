using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VideoPanelEvent : MonoBehaviour
{
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void playVideo(int videoNum)
    {
        this.stopVideo();
        GameObject.Find("VideoManager").GetComponent<VideoManager>().playVideo(videoNum);
    }
    public void stopVideo()
    {
        GameObject.Find("VideoManager").GetComponent<VideoManager>().stopVideo();

    }
    public void turnOn()
    {
        GameObject.Find("VideoManager").GetComponent<VideoManager>().turnOn();
    }
    public void turnOff()
    {
        GameObject.Find("VideoManager").GetComponent<VideoManager>().turnOff();
    }
}
