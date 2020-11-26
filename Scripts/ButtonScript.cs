using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
public class AniClass
{
    public Animation Ani1;      //애니메이션 앞이 보이는 모델
    public Animation Ani2;      //애니메이션 뒤가 보이는 모델
    public string AniStr;       //현재 클립이름
    //public List<string> AniClipList;    //애니의 클립이름들을 저;
    public List<string> clipList;



}
public class ButtonScript : MonoBehaviour
{
    public bool isPlay;
    public GameObject WaistObj;         //
    public GameObject WaistObj2;        //
    public GameObject ShoulderObj;
    public GameObject ShoulderObj2;
    private List<GameObject> AniObjList;
    public Animation WaistAni;
    public Animation WaistAni2;
    public Animation ShoulderAni;
    public Animation ShoulderAni2;
    private List<Animation> AniList;
    private AniClass NowPlayingAnis;        //현재 애니메이션
    private AniClass WaistAniInstance;      //허리 애니메이션
    private AniClass ShoulderAniInstance;    //어깨 애니메이션
    private string NowPlayingAniStr = "";  //현재 애니메이션 이름
    public UnityEngine.UI.Toggle LoopToggle;
    //public UnityEngine.UI.Button BtnPlayPuase;
    public UnityEngine.UI.Button BtnSpeedDown;
    public UnityEngine.UI.Button BtnSpeedUp;
    public Text AniNav;
    private bool isLoop = true;
    //private bool isPause = false;
    //public Sprite PlayImage;
    //public Sprite PauseImage;
    private float AniSpeed = 1.0f;

    GameObject[,] guideAvatar;
    //0 : 허리, 1 : 어깨, 2 : 다리, 3: 목
    int mState;
    public GameObject canvas;
    public GameObject h_sp, lh_sp, rh_sp, lf_sp, rf_sp;

    void Start()
    {
        isPlay = false;
        
        //guideAvatar = new GameObject[4, 2];
        // WaistObj.GetComponent<Animation>.

        //int temp = GameObject.Find("Room/WaistAni_Front").GetComponents<Animation>.
        WaistAniInstance = new AniClass();
        WaistAniInstance.Ani1 = WaistAni;
        WaistAniInstance.Ani2 = WaistAni2;
        WaistAniInstance.AniStr = WaistAniInstance.Ani1.clip.name;
        WaistAniInstance.clipList = new List<string>();
        foreach (AnimationState state in WaistAni)
        {
            WaistAniInstance.clipList.Add(state.clip.name);
        }
        ShoulderAniInstance = new AniClass();
        ShoulderAniInstance.Ani1 = ShoulderAni;
        ShoulderAniInstance.Ani2 = ShoulderAni2;
        ShoulderAniInstance.AniStr = ShoulderAniInstance.Ani1.clip.name;
        ShoulderAniInstance.clipList = new List<string>();
        foreach(AnimationState state in ShoulderAni)
        {
            ShoulderAniInstance.clipList.Add(state.clip.name);
        }

        NowPlayingAniStr = "null";
        //WaistAniInstance.AniClipList = new List<string>();
        //WaistAniInstance.AniClipList.Add("WaistRightKneeClip");
        //WaistAniInstance.AniClipList.Add("WaistStand1Clip");
        //WaistAniInstance.AniClipList.Add("WaistLeftKneeClip");
        //WaistAniInstance.AniClipList.Add("WaistStand2Clip");
        NowPlayingAnis = new AniClass();
        //NowPlayingAnis.AniClipList = new List<string>();

        //일단 애니메이션 한개밖에 없으니 Waist를 현재 애니메이션으로 하자
        NowPlayingAnis = WaistAniInstance;
        //NowPlayingAnis.AniClipList = WaistAniInstance.AniClipList;


        AniObjList = new List<GameObject>();
        AniObjList.Add(WaistObj);
        AniObjList.Add(WaistObj2);
        AniObjList.Add(ShoulderObj);
        AniObjList.Add(ShoulderObj2);
        AniList = new List<Animation>();
        AniList.Add(WaistAni);
        AniList.Add(WaistAni2);
        AniList.Add(ShoulderAni);
        AniList.Add(ShoulderAni2);
        SetActiveFalseAll();

    }

    void Update()
    {
        int mCount = GameObject.Find("user").GetComponent<userXML>().count;
        Debug.Log(mState);
        Debug.Log(NowPlayingAnis.AniStr);

        if (mCount >= 1)
        {
            SetActiveFalseAll();
            canvas.SetActive(true);
            if(isPlay)
                GameObject.Find("Testing").GetComponent<Testing>().reset();
            isPlay = false;
           
                

        }

        if (isPlay)
        {

            StartCoroutine("changeClip");
        }


    }


    IEnumerator changeClip()
    {
        Debug.Log("clip chagne");
        int temp = GameObject.Find("user").GetComponent<userXML>().state;
        if (mState != temp)
        {

            mState = temp;
            Debug.Log("이거 뭐야 :   "+mState);
            NowPlayingAnis.Ani1.Play(NowPlayingAnis.clipList[mState]);
            NowPlayingAnis.Ani2.Play(NowPlayingAnis.clipList[mState]);

            //switch (mState)
            //{
            //    case 1:
            //        anim.clip = playerAnim.right;
            //        NowPlayingAnis.Ani1.Play(NowPlayingAnis.clipList[1]);
            //        anim.Play();
            //        break;
            //    case 2:
            //        anim.clip = playerAnim.stand1;
            //        anim.Play();
            //        break;
            //    case 3:
            //        anim.clip = playerAnim.left;
            //        anim.Play();
            //        break;
            //    case 4:
            //        anim.clip = playerAnim.stand2;
            //        anim.Play();
            //        break;
            //    default:
            //        anim.clip = playerAnim.idle;
            //        anim.Play();
            //        break;
            //}
        }

        yield return new WaitForSeconds(0.5f);
    }

    public void SetActiveFalseAll()     //모두 비활성화
    {
        for (int i = 0; i < AniObjList.Count; i++)
        {
            AniObjList[i].SetActive(false);
        }
        //if(GameObject.Find("testing") != null)
        //    GameObject.Find("testing").GetComponent<Testing>().reset();
        canvas.SetActive(false);
        h_sp.SetActive(false);
        lh_sp.SetActive(false);
        rh_sp.SetActive(false);
        lf_sp.SetActive(false);
        rf_sp.SetActive(false);

    }

    public void SetSphereTrue()
    {
        h_sp.SetActive(true);
        lh_sp.SetActive(true);
        rh_sp.SetActive(true);
        lf_sp.SetActive(true);
        rf_sp.SetActive(true);
    }
    public void AllStopAnis()       //모든 애니메이션 멈추기
    {
        for(int i =0; i<AniList.Count; i++)
        {
            AniList[i].Stop();
        }
    }
    public void RewindAll()
    {
        for (int i = 0; i < AniList.Count; i++)
        {
            AniList[i].Rewind();    //애니메이션의 모든 클립들 rewind
        }
    }
    public void ChangeWrapMode()
    {
        if (LoopToggle.isOn)
        {
            for(int i=0; i<NowPlayingAnis.clipList.Count; i++)
            {
                NowPlayingAnis.Ani1[NowPlayingAnis.clipList[i]].wrapMode = WrapMode.Loop;
                NowPlayingAnis.Ani2[NowPlayingAnis.clipList[i]].wrapMode = WrapMode.Loop;
            }
            //NowPlayingAnis.Ani1.Play(NowPlayingAnis.AniStr);
            //NowPlayingAnis.Ani2.Play(NowPlayingAnis.AniStr);
        }
        else
        {
            for (int i = 0; i < NowPlayingAnis.clipList.Count; i++)
            {
                NowPlayingAnis.Ani1[NowPlayingAnis.clipList[i]].wrapMode = WrapMode.Once;
                NowPlayingAnis.Ani2[NowPlayingAnis.clipList[i]].wrapMode = WrapMode.Once;
            }
            //NowPlayingAnis.Ani1.Play(NowPlayingAnis.AniStr);
            //NowPlayingAnis.Ani2.Play(NowPlayingAnis.AniStr);
        }
    }
    public bool IsPlaying(AniClass ani)
    {
        if (ani.Ani1.isPlaying == true && ani.Ani2.isPlaying == true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void OnClickStartShoulder()
    {
        isPlay = true;
        SetSphereTrue();

        NowPlayingAnis = ShoulderAniInstance;
        NowPlayingAnis.Ani1 = ShoulderAni;
        NowPlayingAnis.Ani2 = ShoulderAni2;
        NowPlayingAniStr = "Shoulder";
        GameObject.Find("user").GetComponent<userXML>().init(NowPlayingAniStr);
     
        NowPlayingAnis.AniStr = NowPlayingAnis.clipList[0];
        AllStopAnis();                 // 애니메이션 모두 stop
        SetActiveFalseAll();        //애니메이션 모두 비활성화
        ShoulderObj.SetActive(true);   //허리1 애니메이션 활성화
        ShoulderObj2.SetActive(true);   //허리2 애니메이션 활성화
        SetSphereTrue();
        RewindAll();
        ChangeWrapMode();
        ShoulderAni.Play(NowPlayingAnis.AniStr);
    }
    public void OnClickStartWaist()
    {
        isPlay = true;

        NowPlayingAnis = WaistAniInstance;
        NowPlayingAnis.Ani1 = WaistAni;
        NowPlayingAnis.Ani2 = WaistAni2;
        NowPlayingAniStr = "Waist";
        GameObject.Find("user").GetComponent<userXML>().init(NowPlayingAniStr);
        NowPlayingAnis.AniStr = NowPlayingAnis.clipList[0];
        AllStopAnis();                 // 애니메이션 모두 stop
        SetActiveFalseAll();        //애니메이션 모두 비활성화
        WaistObj.SetActive(true);   //허리1 애니메이션 활성화
        WaistObj2.SetActive(true);   //허리2 애니메이션 활성화
        SetSphereTrue();
        RewindAll();
        ChangeWrapMode();
        WaistAni.Play(NowPlayingAnis.AniStr);

        
        //if (isPause != true)        //애니메이션이 실행상태여야함
        //{
        //    if (IsPlaying(NowPlayingAnis) == true) return;  //이미 애니메이션이 실행중이니 return
        //    NowPlayingAnis.Ani1 = WaistAni;
        //    NowPlayingAnis.Ani2 = WaistAni2;
        //    NowPlayingAnis.AniStr = NowPlayingAniStr;
        //    //AllStopAnis();                 // 애니메이션 모두 stop
        //    SetActiveFalseAll();        //애니메이션 모두 비활성화
        //    WaistObj.SetActive(true);   //
        //    RewindAll();
        //    ChangeWrapMode();
        //    WaistAni.Play(NowPlayingAnis.AniClipList[0]);
        //    
        //}
    }
    public void OnClickSpeedDown()
    {
        AniSpeed -= 0.1f;
        NowPlayingAnis.Ani1[NowPlayingAnis.AniStr].speed = AniSpeed;
        NowPlayingAnis.Ani2[NowPlayingAnis.AniStr].speed = AniSpeed;
        string msg = "애니메이션 속도 : "+AniSpeed.ToString() + "x";
        SetTextAniNav(msg);
    }
    public void OnClickSpeedUp()
    {
        AniSpeed += 0.1f;
        NowPlayingAnis.Ani1[NowPlayingAnis.AniStr].speed = AniSpeed;
        NowPlayingAnis.Ani2[NowPlayingAnis.AniStr].speed = AniSpeed;
        string msg = "애니메이션 속도 : " + AniSpeed.ToString() + "x";
        SetTextAniNav(msg);
    }
    public void SetTextAniNav(string msg)
    {
        AniNav.text = msg;
    }
}