using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutputErrorData : MonoBehaviour
{
    DataManager dataManager;
    
    IEnumerator Start()
    {
        dataManager = DataManager.GetInstance();
        //int itemIdex = dataManager.GetItemIndex(); //0:목, 1:어깨, 2:허리, 3:다리
        //List<double[]> animationTimeInfo = dataManager.GetAnimtaionTimes(); //double[], 0:머리 누적 오차 시간. 1:왼손, 2:오른손, 3:왼발: 4:오른발, 5:전체 수행 시간
        //animationTimeInfo[0] : itemIndex항목에서 첫번째 애니메이션
        //animationTimeInfo[1] : itemIndex항목에서 두번째 애니메이션
        List<double[]> errorData = new List<double[]>();
        List<string> errorDate = new List<string>();

        WWW webRequest = new WWW("http://ec2-3-35-168-80.ap-northeast-2.compute.amazonaws.com/OutputErrorData.php");
        yield return webRequest;//정보가 올 때까지 기다림
        string itemDataString = webRequest.text;
        char sp = ',';
        string[] spstring = itemDataString.Split(sp);

        for (int i = 0; i < spstring.Length-1;)
        {
            double[] tempdata = new double[5];
            errorDate.Add(spstring[i]);
            tempdata[0] = (Convert.ToDouble(spstring[i + 1]) / Convert.ToDouble(spstring[i+6]))*100;
            tempdata[1] = (Convert.ToDouble(spstring[i + 2]) / Convert.ToDouble(spstring[i+6]))*100;
            tempdata[2] = (Convert.ToDouble(spstring[i + 3]) / Convert.ToDouble(spstring[i+6]))*100;
            tempdata[3] = (Convert.ToDouble(spstring[i + 4]) / Convert.ToDouble(spstring[i+6]))*100;
            tempdata[4] = (Convert.ToDouble(spstring[i + 5]) / Convert.ToDouble(spstring[i+6]))*100;
            errorData.Add(tempdata);
            i = i + 7;
        }

        dataManager.SetDateErrorOfServer(errorData, errorDate);
        GameObject.Find("Menus/Canvas/Menu/RecordPanel").GetComponent<DrawChart>().SetDateData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
