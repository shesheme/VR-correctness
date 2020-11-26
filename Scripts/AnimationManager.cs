using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor.Animations;
using UnityEditor;

[System.Serializable]
public class AnimationGuide
{
    public Animator animationGuide1;
    public Animator animationGuide2;
    public Animator animationGuide3;
    public Animator animationGuide4;

    List<int> clipCount;

    public void Init()
    {
        clipCount = new List<int>();
        //클립 개수 세는 함수
    }

    public List<int> GetClipCountList()
    {
        return clipCount;
    }
}

public class AnimationManager : MonoBehaviour
{
    public GameObject frontGuide, backGuide;
    public Text countDownText, animationText, clipText, postureNameText; //동작 시작시 나오는 카운트다운 텍스트

    Animator guideAnimator, guideAnimator2;
    List<string> itemNameList;
    List<int> itemClipList;
    DataManager dataManager;


    int count; //카운트 다운 숫자
    int animationIndex;
    int clipIndex;
    int itemIndex;

    bool countFlag = false;

    public void init(int itemIndex)
    {
        frontGuide.SetActive(false);
        backGuide.SetActive(false);
        itemClipList = new List<int>();
        itemNameList = new List<string>();

        switch(itemIndex)
        {
            case 0: //맨 몸 운동
                itemClipList.Add(5); itemNameList.Add("풍차돌리기");
                itemClipList.Add(3); itemNameList.Add("기지개펴기");
                itemClipList.Add(6); itemNameList.Add("만세펴기");
                itemClipList.Add(4); itemNameList.Add("슈퍼맨");
                break;
            case 1: // 코어 운동
                itemClipList.Add(5); itemNameList.Add("스쿼트");
                itemClipList.Add(8); itemNameList.Add("런지");
                itemClipList.Add(8); itemNameList.Add("변형플랭크");
                itemClipList.Add(2); itemNameList.Add("플랭크");
                break;
            case 2: //재활 운동
                itemClipList.Add(7); itemNameList.Add("쇄골압수");
                itemClipList.Add(4); itemNameList.Add("손들어꼼짝마");
                itemClipList.Add(4); itemNameList.Add("올인원");
                itemClipList.Add(5); itemNameList.Add("부채꼴돌리기");
                break;
            case 3: //스트레칭
                itemClipList.Add(9); itemNameList.Add("거북목교정");
                itemClipList.Add(2); itemNameList.Add("쉼호흡자세");
                itemClipList.Add(6); itemNameList.Add("골리앗자세");
                itemClipList.Add(3); itemNameList.Add("발등터치");
                break;
        }

        frontGuide.SetActive(true);
        backGuide.SetActive(true);
        guideAnimator = frontGuide.GetComponent<Animator>();
        guideAnimator2 = backGuide.GetComponent<Animator>();
        animationIndex = 0;
        clipIndex = 0;
        this.itemIndex = itemIndex;
        dataManager = DataManager.GetInstance();
        dataManager.SetItemNameList(itemNameList);

        animationText.text = "Posture : " + (animationIndex + 1).ToString() + " / " + itemClipList.Count.ToString();
        clipText.text = "Step : " + (clipIndex + 1).ToString() + " / " + itemClipList[animationIndex].ToString();
        postureNameText.text = itemNameList[animationIndex];
    }

    public int GetAnimationIndex()
    {
        return animationIndex;
    }

    public int PlayNext()
    {
        guideAnimator.SetInteger("itemIndex", itemIndex);
        guideAnimator2.SetInteger("itemIndex", itemIndex);
        clipIndex++;
        if(clipIndex < itemClipList[animationIndex]) //한 애니메이션의 다음 스탭
        {
            guideAnimator.SetInteger("clipIndex", clipIndex);
            guideAnimator2.SetInteger("clipIndex", clipIndex);
            clipText.text = "Step : " + (clipIndex + 1).ToString() + " / " + itemClipList[animationIndex].ToString();
        } else if(animationIndex < itemClipList.Count) // 다음 애니메이션
        {
            GameObject.Find("user/CorrectnessAnalysis").GetComponent<CorrectnessAnalysis>().SavePlayTime();
            dataManager.SaveAnimationInfo(clipIndex);
            clipIndex = 0;
            animationIndex++;

            //knn실행 코루틴
            StartCoroutine(GetLastKnnEvaluation());

            if (animationIndex >= itemClipList.Count)
            {
                guideAnimator.SetBool("isFinish", true);
                guideAnimator2.SetBool("isFinish", true);
                frontGuide.SetActive(false);
                backGuide.SetActive(false);
                clipIndex = -1;
                animationText.text = "";
                clipText.text = "";
                postureNameText.text = "";
            } else
            {
                guideAnimator.SetInteger("clipIndex", clipIndex);
                guideAnimator2.SetInteger("clipIndex", clipIndex);
                clipText.text = "Step : " + (clipIndex + 1).ToString() + " / " + itemClipList[animationIndex].ToString();
                animationText.text = "Posture : " + (animationIndex + 1).ToString() + " / " + itemClipList.Count.ToString();
                postureNameText.text = itemNameList[animationIndex];
            }
        }


        return clipIndex;
    }


    public void Sync(bool isSync)
    {

        frontGuide.SetActive(false);
        backGuide.SetActive(false);

        if (isSync)
        {
            frontGuide.SetActive(true);
            backGuide.SetActive(true);
            guideAnimator = frontGuide.GetComponent<Animator>();
            guideAnimator2 = backGuide.GetComponent<Animator>();
            guideAnimator.SetBool("sync", true);
            guideAnimator2.SetBool("sync", true);
        }
    }
    //KNN 코루틴 호성이 시작
    IEnumerator GetLastKnnEvaluation()
    {
        Knn_Calculate knn_instance = new Knn_Calculate();
        knn_instance.Init();
        Knn_Results result = knn_instance.Knn_Execute();
        this.dataManager.AddKnnResults(result);
        yield return null;
    }


    ////KNN 코루틴 호성이 끝
    ///
}
