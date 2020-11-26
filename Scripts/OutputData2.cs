using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutputData2 : MonoBehaviour
{
    DataManager dataManager;

    IEnumerator Start()
    {
        dataManager = DataManager.GetInstance();
        //int itemIdex = dataManager.GetItemIndex(); //0:목, 1:어깨, 2:허리, 3:다리
        //List<double[]> animationTimeInfo = dataManager.GetAnimtaionTimes(); //double[], 0:머리 누적 오차 시간. 1:왼손, 2:오른손, 3:왼발: 4:오른발, 5:전체 수행 시간
        //animationTimeInfo[0] : itemIndex항목에서 첫번째 애니메이션
        //animationTimeInfo[1] : itemIndex항목에서 두번째 애니메이션
        double[] netData = new double[6];

        WWW webRequest = new WWW("http://ec2-13-125-126-232.ap-northeast-2.compute.amazonaws.com/OutputErrorData.php");
        yield return webRequest;//정보가 올 때까지 기다림
        string itemDataString = webRequest.text;
        char sp = ',';
        string[] spstring = itemDataString.Split(sp);

            netData[0] = (Convert.ToDouble(spstring[0]) / Convert.ToDouble(spstring[5])) * 100;
            netData[1] = (Convert.ToDouble(spstring[1]) / Convert.ToDouble(spstring[5])) * 100;
            netData[2] = (Convert.ToDouble(spstring[2]) / Convert.ToDouble(spstring[5])) * 100;
            netData[3] = (Convert.ToDouble(spstring[3]) / Convert.ToDouble(spstring[5])) * 100;
            netData[4] = (Convert.ToDouble(spstring[4]) / Convert.ToDouble(spstring[5])) * 100;
        

        dataManager.SetNetDataOfServer(netData);
        GameObject.Find("Menus/Canvas/Menu/RecordPanel").GetComponent<DrawChart>().SetNetData();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
