using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEditor.PackageManager;
using System.CodeDom.Compiler;
using System.Data;

class DataFileList
{
    List<string> bodyFileList;
    List<string> coreFileList;
    List<string> revitalizeFileList;
    List<string> stretchFileList;


    public void Init()
    {
        bodyFileList = new List<string>();
        coreFileList = new List<string>();
        revitalizeFileList = new List<string>();
        stretchFileList = new List<string>();

        bodyFileList.Add("Extract_body01");
        bodyFileList.Add("Extract_body02");
        bodyFileList.Add("Extract_body03");
        bodyFileList.Add("Extract_body04");

        coreFileList.Add("Extract_core01");
        coreFileList.Add("Extract_core02");
        coreFileList.Add("Extract_core03");
        coreFileList.Add("Extract_core04");

        revitalizeFileList.Add("Extract_revitalize01");
        revitalizeFileList.Add("Extract_revitalize02");
        revitalizeFileList.Add("Extract_revitalize03");
        revitalizeFileList.Add("Extract_revitalize04");

        stretchFileList.Add("Extract_stretch01");
        stretchFileList.Add("Extract_stretch02");
        stretchFileList.Add("Extract_stretch03");
        stretchFileList.Add("Extract_stretch04");


    }

    public List<string> GetFileList(int itemIndex)
    {

        List<string> itemFileList = new List<string>();
        switch(itemIndex)
        {
            case 0:
                itemFileList = bodyFileList;
                break;
            case 1:
                itemFileList = coreFileList;
                break;
            case 2:
                itemFileList = revitalizeFileList;
                break;
            case 3:
                itemFileList = stretchFileList;
                break;
        }
        return itemFileList;
    }
}
public class DataManager : MonoBehaviour
{
    public static DataManager dataManager = null;
    static DataFileList dataFileList;

    static List<Vector3[,]> positionList;
    static List<Vector3[,]> rotationList;


    List<double[]> errorTimes;
    List<double> playTimes;
    List<double> totalTimes;
    List<double[]> animationErrorTimes;
    List<string> itemNames;
    float[] userLengthInfo;
    int maxDevice = 5;
    int partionNum;
    int itemIndex;
    string url;

    List<double[]> dateDataOfServer;
    List<string> dateStrOfSever;
    double[] netDataOfServer;

    /// 호성이꺼 시작
    All_Knn_Results all_knn_results;    // knn결과 종합
    /// 호성이꺼 끝
    private void Awake()
    {
        if(dataManager == null)
        {
            dataManager = this;
            url = "http://ec2-3-35-168-80.ap-northeast-2.compute.amazonaws.com";
            dataFileList = new DataFileList();
            dataFileList.Init();
        }
        else if(dataManager != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        this.all_knn_results = new All_Knn_Results();   //호성
    }

    public static DataManager GetInstance()
    {
        return dataManager;
    }

    public void LoadFile(int itemIndex)
    {
        List<string> fileNameList = dataFileList.GetFileList(itemIndex);
        positionList = new List<Vector3[,]>();
        rotationList = new List<Vector3[,]>();
        errorTimes = new List<double[]>();
        totalTimes = new List<double>();
        playTimes = new List<double>();
        animationErrorTimes = new List<double[]>();
        this.itemIndex = itemIndex;

        foreach (string fileName in fileNameList)
        {
            string dataFile = File.ReadAllText(Application.dataPath + "/Resources/Transforms/LocalData/" + fileName + ".txt");
            string[] dataSet = dataFile.Split('\n');
            partionNum = (dataSet.Length - 1) / maxDevice; // 한 동작에 존재하는 구분점 수, 첫줄은 개요
            Vector3[,] position = new Vector3[maxDevice, partionNum]; // [5개의 트래커 장비][구분 지점 좌표]
            Vector3[,] rotation = new Vector3[maxDevice, partionNum]; // [5개의 트래커 장비][구분 지점 기울기]

            int dataIndex = 0;
            int deviceIndex = 0;

            for (int i = 1; i < dataSet.Length; i++)
            {
                string[] text = dataSet[i].Split('\t');

                position[deviceIndex, dataIndex].Set(float.Parse(text[1]), float.Parse(text[2]), float.Parse(text[3]));
                rotation[deviceIndex, dataIndex].Set(float.Parse(text[4]), float.Parse(text[5]), float.Parse(text[6]));
                 
                dataIndex++;
                if (i % partionNum == 0)
                {
                    dataIndex = 0;
                    deviceIndex++;
                }
            }

            positionList.Add(position);
            rotationList.Add(rotation);
        }
    }
    public string GetAWSURL()
    {
        return url;
    }

    public int GetItemIndex()
    {
        return itemIndex;
    }

    public Vector3[,] GetPositionData(int index)
    {
        return positionList[index];
    }
    public Vector3[,] GetRotationData(int index)
    {
        return rotationList[index];
    }
    public void SaveTimeData(double[] errorTime, double totalTime)
    {
        //CorrectnesAnalysis.cs에서 호출하는 함수
        //한 동작의 클립이 모두 끝났을 때 실행
        //서버에 애니메이션별 전체 동작 시간과 에러 발생 시간 기록
        //날짜, (수행한 항목 ex) 허리, 다리), 애니메이션 이름, 머리 에러 시간, 양손 에러 시간, 양발 에러 시간, 전체 수행 시간

        errorTimes.Add(errorTime);
        totalTimes.Add(totalTime);
    }

    public double GetNetErrorTime(int deviceIndex)
    {
        double result = 0;
        foreach(double[] error in errorTimes)
        {
            result += error[deviceIndex];
        }
        return result;
    }

    public double GetNetTotalTime()
    {
        double result = 0;
        foreach(double total in totalTimes)
        {
            result += total;
        }
        return result;
    }

    public void SaveAnimationInfo(int clipCount)
    {
        double[] tempTime = new double[6];
        for(int i = errorTimes.Count - clipCount; i<errorTimes.Count;i++)
        {
            for (int j = 0; j < 5;j++)
            {
                tempTime[j] += errorTimes[i][j];
            }
            tempTime[5] += totalTimes[i];
        }
        animationErrorTimes.Add(tempTime);
    }
    public void SetPlayTime(double time)
    {
        playTimes.Add(time);
        Debug.Log("실행시간 저장 : " + time);
    }
    public double GetLastPlayTime()
    {
        return playTimes[playTimes.Count-1];
    }
    public List<double> GetPlayTimes()
    {
        return playTimes;
    }

    public void ResetPlayTimes()
    {
        playTimes.Clear();
    }
    public List<double[]> GetAnimationTimes()
    {
        return animationErrorTimes;
    }
    //호성이 KNN사용시작
    public string GetLastItemName()
    {
        return itemNames[animationErrorTimes.Count - 1];
    }
    public double[] GetLastAnimationTimes()
    {
        return animationErrorTimes[animationErrorTimes.Count - 1];
    }
    public double GetLastAnimationTotalTime()
    {
        return totalTimes[animationErrorTimes.Count - 1];
    }
    public void AddKnnResults(Knn_Results knn_result)
    {
        Debug.LogError("all에 result 추가");
        this.all_knn_results.Add_Result(knn_result);
    }
    public void ResetKnnInstance()
    {
        this.all_knn_results = new All_Knn_Results();
    }
    public All_Knn_Results GetAll_Knn_Results()
    {
        return this.all_knn_results;
    }
    //호성이 KNN사용끝
    public double[] GetNetDataOfServer()
    {
        return netDataOfServer;
    }

    public List<double[]> GetDateDataOfServer()
    {
        return dateDataOfServer;
    }

    public List<string> GetDateStrOfServer()
    {
        return dateStrOfSever;
        
    }
    public List<double[]> GetItemDataOfServer()
    {
        List<double[]> temp = new List<double[]>();
        return temp;
    }

    public List<string> GetItemStrOfServer()
    {
        List<string> temp = new List<string>();
        return temp;
    }

    public void SetDateErrorOfServer(List<double[]> datalist, List<string> strlist)
    {
        dateDataOfServer = datalist;
        dateStrOfSever = strlist;
    }

    public void SetNetDataOfServer(double[] datalist)
    {
        netDataOfServer = datalist;
    }

    public void SetUserLengthInfo(float headLenght, float LHLenght, float RHLenght, float LFLenght, float RFLenght)
    {
        if(userLengthInfo == null)
        {
            userLengthInfo = new float[5];
        }
        userLengthInfo[0] = headLenght;
        userLengthInfo[1] = LHLenght;
        userLengthInfo[2] = RHLenght;
        userLengthInfo[3] = LFLenght;
        userLengthInfo[4] = RFLenght;
    }

    public float[] GetUserLengthInfo()
    {
        return userLengthInfo;
    }

    public void SetItemNameList(List<string> nameList)
    {
        itemNames = nameList;
    }

    public List<string> GetItemNameList()
    {
        return itemNames;
    }

  
}
