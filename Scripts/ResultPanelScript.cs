using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;
using System.IO;

public class ResultPanelScript : MonoBehaviour
{
    int itemIndex;
    public UnityEngine.UI.Text PoseNameText;
    public UnityEngine.UI.Text TitleText;
    public UnityEngine.UI.Text ResultText;
    public GameObject panel;
    public GameObject lazer;
    private int ResultGraphIndex = 0; //true 이면 애니메이션 2개 중에 첫번째 애니메이션의 KNN을 보여주고 false이면 두번째꺼
    DataManager dataManager;
    string url;
    List<string> itemNameList;
    List<double[]> animationTimes;
    List<double> palyTimes;
    All_Knn_Results all_knn_results;
    // Start is called before the first frame update
    public void Init()
    {
        dataManager = DataManager.GetInstance();
        url = dataManager.GetAWSURL();
        itemNameList = dataManager.GetItemNameList();
        itemIndex = GameObject.Find("DataManager").GetComponent<DataManager>().GetItemIndex();
        animationTimes = dataManager.GetAnimationTimes();
        palyTimes = dataManager.GetPlayTimes();
        initTitleText(itemIndex);
        all_knn_results = new All_Knn_Results();
        all_knn_results = this.dataManager.GetAll_Knn_Results();
        initPoseNameText("종합");
        this.ResultText.text = all_knn_results.GetEvaluation();
        ResultText.GetComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        ResultGraphIndex = 0;


        if (!Directory.Exists(Application.dataPath + "/Resources/Transforms/RecordData/"))
            Directory.CreateDirectory(Application.dataPath + "/Resources/Transforms/RecordData/");

        // File.Create(Application.dataPath + "/Resources/Transforms/RecordData/" + fileName + ".txt");

        FileStream fs = new FileStream(Application.dataPath + "/Resources/Transforms/RecordData/hou.txt", FileMode.OpenOrCreate, FileAccess.Write);
        StreamWriter writer = new StreamWriter(fs, System.Text.Encoding.Unicode);
        for (int i = 0; i<animationTimes.Count;i++)
        {
            writer.WriteLine("교정운동" + (i+1).ToString());
            for (int j = 0; j<animationTimes[i].Length -1;j++)
            {
                
                writer.WriteLine("디바이스 번호 : " + j.ToString() + ",오류 시간 값 : " + animationTimes[i][j].ToString("0.00"));
            }
            writer.WriteLine("전체 수행 시간 : " + palyTimes[i].ToString("0.00"));
        }
        writer.Close();
    }



    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDisable()
    {
        dataManager.ResetKnnInstance();
    }
    void initTitleText(int itemIndex)
    {
        string title = "";
        switch (itemIndex)
        {
            case 0:
                title = "맨 몸 운동";
                break;
            case 1:
                title = "코어 운동";
                break;
            case 2:
                title = "재활 운동";
                break;
            case 3:
                title = "스트레칭";
                break;
        }
        this.TitleText.text = title;
    }
    void initPoseNameText(string PoseName)
    {
        //임시로 미리 만든 함수임. 나중에 자세들 이름 다 정하고 가져와서 사용해야함.
        this.PoseNameText.text = PoseName;
    }
    public void SetAnimationIndex0()
    {
        this.ResultGraphIndex--;
        if (this.ResultGraphIndex < 0)
            ResultGraphIndex = 0;
        if (this.ResultGraphIndex == 0)
        {
            initPoseNameText("종합");
            ////종합점수
            this.ResultText.text = all_knn_results.GetEvaluation();
            ResultText.GetComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        } else
        {
            initPoseNameText(itemNameList[this.ResultGraphIndex - 1]);
            //개별자세 점수
            this.ResultText.text = all_knn_results.GetEachResult(ResultGraphIndex - 1).GetEvaluation();
            ResultText.GetComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        }
       
    }
    public void SetAnimationIndex1()
    {
        this.ResultGraphIndex++;
        if ((int)this.ResultGraphIndex > animationTimes.Count)
            this.ResultGraphIndex = animationTimes.Count;
        initPoseNameText(itemNameList[ResultGraphIndex - 1]);
        //개별자세 점수
        this.ResultText.text = all_knn_results.GetEachResult(ResultGraphIndex - 1).GetEvaluation();
        ResultText.GetComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
    }
    public void ClosePanel()
    {
        panel.SetActive(false);
    }

    public void setLazer()
    {
        lazer.SetActive(!lazer.active);
    }
}
