using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using System.Diagnostics;

public class VideoManager : MonoBehaviour
{
    public GameObject screen;
    public VideoClip body1;         // 0 
    public VideoClip body4;         // 1
    public VideoClip core1;         // 2
    public VideoClip core2;         // 3
    public VideoClip stretch1;      // 4
    public VideoClip core4;         // 5
    public VideoClip core3;         // 6
    public VideoClip revitalize1;   // 7
    public VideoClip revitalize2;   // 8
    public VideoClip loadingVideo;       // 로딩화면
    Dictionary<int, VideoClip> videoDict;
    bool onOff = false; //  true - on      false - off
    int nowPlaying = -1;        // -1 이면 현재 비디오 영상 안 나오는 중
    void Start()
    {
        this.updateDict();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void updateDict()
    {
        this.videoDict = new Dictionary<int, VideoClip>();
        this.videoDict.Add(0, this.body1);
        this.videoDict.Add(1, this.body4);
        this.videoDict.Add(2, this.core1);
        this.videoDict.Add(3, this.core2);
        this.videoDict.Add(4, this.core3);
        this.videoDict.Add(5, this.core4);
        this.videoDict.Add(6, this.stretch1);
        this.videoDict.Add(7, this.revitalize1);
        this.videoDict.Add(8, this.revitalize2);
        this.screen.GetComponent<VideoPlayer>().isLooping = true;
    }
    public void stopVideo()
    {
        UnityEngine.Debug.Log("stopVideo");
        this.screen.GetComponent<VideoPlayer>().Stop();
        this.turnOff();
    }
    public void playVideo(int videoNum)
    {
        this.turnOn();
        this.loading(videoNum);
    }
    public void playVideo2(int videoNum)
    {
        if (this.nowPlaying == -1 && this.onOff == false)
            this.turnOn();
        UnityEngine.Debug.Log("PlayVideo : " + videoNum.ToString());
        this.nowPlaying = videoNum;
        this.screen.GetComponent<VideoPlayer>().clip = videoDict[this.nowPlaying];
        this.screen.GetComponent<VideoPlayer>().Play();
    }
    public void turnOn()
    {
        UnityEngine.Debug.Log("TurnOn");
        this.onOff = true;
        this.screen.SetActive(true);
    }
    public void turnOff()
    {
        UnityEngine.Debug.Log("TurnOff");
        this.nowPlaying = -1;
        this.onOff = false;
        this.screen.GetComponent<VideoPlayer>().Stop();
        this.screen.SetActive(false);
    }
    public void loading(int videoNum)
    {
        UnityEngine.Debug.Log("Loading Video");
        float randomTime = Random.Range(3.0f, 5.0f);
        this.screen.GetComponent<VideoPlayer>().clip = this.loadingVideo;
        this.screen.GetComponent<VideoPlayer>().Play();
        StartCoroutine(DelayTime(randomTime, videoNum));
    }
    public IEnumerator DelayTime(float randomTime, int videoNum)
    {
        yield return new WaitForSeconds(randomTime);
        this.screen.GetComponent<VideoPlayer>().Stop();
        this.playVideo2(videoNum);
    }
}