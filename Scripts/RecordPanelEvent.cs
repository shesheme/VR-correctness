using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RecordPanelEvent : MonoBehaviour
{
    DataManager dataManager;
    public Text posture, startDate, endDate;
    public GameObject radarChart, calander, errorInfo;

    string url;
    private void Start()
    {

    }

    public void Init()
    {
        dataManager = DataManager.GetInstance();
        url = dataManager.GetAWSURL();
        string preDate = System.DateTimeOffset.Now.AddDays(-30).ToString("yyyy-MM-dd");
        string nowDate = System.DateTime.Now.ToString("yyyy-MM-dd");
        endDate.text = nowDate;
        startDate.text = preDate;
        StartCoroutine(GetRecordDataFromAWS());
    }

    IEnumerator GetRecordDataFromAWS()
    {
        if(radarChart.activeSelf)
            radarChart.SetActive(false);
        string str = "종합";
        if (!posture.text.Equals("종합"))
        {
            string[] strarr = posture.text.Split(' ');
            str = strarr[2];
        }

        WWWForm form = new WWWForm();
        Debug.Log("자세 : " + str + ", 시작 : " + startDate.text + ", 종료 : " + endDate.text);
        form.AddField("posture", str);
        form.AddField("start_date", startDate.text);
        form.AddField("end_date", endDate.text);


        WWW webRequest = new WWW(url + "/OutputTotalPerformResult.php", form);
        yield return webRequest; //정보가 올 때까지 기다림

        Debug.Log("수신 테스트 : " + webRequest.text);

        List<float> scores = new List<float>();
        List<string> dateLabels = new List<string>();

        string[] webstr = webRequest.text.Split('\n');
        Debug.Log("수신 테스트2 : " + webstr[1]);

        errorInfo.SetActive(false);
        if(webstr[1] == "")
        {
            errorInfo.SetActive(true);
        }
        webstr = webstr[1].Split('!');
        for (int i = 0; i < webstr.Length - 1; i++)
        {
            string temp = webstr[i];
            string[] value = temp.Split(',');
            //value[0] = value[0].Remove(0, 5);
            dateLabels.Add(value[0]);
            scores.Add(float.Parse(value[1]));
        }
        GameObject.Find("RecordCanvas/Panel/Graph").GetComponent<BarChart>().CreateChart(scores, dateLabels, str);
       
       



    }

    public void Search()
    {
        StartCoroutine(GetRecordDataFromAWS());
    }


    public void DestroyRecordCanvas()
    {
        radarChart.SetActive(false);
    }

    public void OnClickedDate()
    {
        radarChart.SetActive(false);
        calander.SetActive(true);
    }


}