using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class BarChart : MonoBehaviour
{
    public RectTransform graphContainer, graphContainerBar, labelXRect, labelYRect, UIContainer;
    public GameObject radarChart, calander;
    public Text dateInfo;
    string posture;

    GameObject box;
    List<GameObject> gameObjectList = new List<GameObject>();

    private void Start()
    {
        //CreateChart();
    }
    public void DestroyChart()
    {
    
        graphContainerBar.DetachChildren();
    }
    public void CreateChart(List<float> valList, List<string> labelList, string post)
    {
        posture = post;
        DestroyChart();
        ShowGraph(valList, labelList);
    }

    void ShowInfo(string date)
    {
        Debug.Log("바 클릭");
        StartCoroutine(SearchCorrectInfo(date));
    }

    private void ShowGraph(List<float> valueList, List<string> labelList)
    {


        float graphWidth = graphContainer.sizeDelta.x;
        float graphHeight = graphContainer.sizeDelta.y;
        int maxVisibleValueAmout = 3;
        maxVisibleValueAmout = Mathf.Min(maxVisibleValueAmout, valueList.Count);
        float yMaximum = 55f;
        float xSize = graphWidth / (maxVisibleValueAmout + 1) * 2;

        int xIndex = 0;
        for (int i = 0; i < valueList.Count; i++)
        {
            box = new GameObject("box", typeof(RectTransform));
            box.transform.SetParent(graphContainerBar, false);
            box.GetComponent<RectTransform>().sizeDelta = new Vector2(xSize, box.GetComponent<RectTransform>().sizeDelta.y);
     
         
            box.SetActive(true);
            float xPosition = xSize / 2;
            float yPosition = (valueList[i] / yMaximum) * graphHeight - 1;
            CreateBar(new Vector2(xPosition, yPosition), xSize / 2, labelList[i]);

            RectTransform labelX = Instantiate(labelXRect);
            labelX.SetParent(box.transform, false);
            labelX.gameObject.SetActive(true);
            labelX.anchoredPosition = new Vector2(xPosition, -2.8f);
            labelX.GetComponent<Text>().text = labelList[i].ToString();
            labelX.GetComponent<Text>().fontSize = 40;
            xIndex++;
        }
        if (!UIContainer.FindChild("labelY(Clone)"))
        {
            int separatorCount = 11;
            for (int i = 0; i < separatorCount; i++)
            {
                RectTransform labelY = Instantiate(labelYRect);
                labelY.gameObject.SetActive(true);
                labelY.SetParent(UIContainer, false);
                float normalizedValue = i * 1f / separatorCount;
                labelY.anchoredPosition = new Vector2(6.5f, normalizedValue * (graphHeight - 20) + 20);
                labelY.GetComponent<Text>().text = Mathf.RoundToInt(normalizedValue * yMaximum).ToString();
                labelY.GetComponent<Text>().color = Color.black;
                labelY.GetComponent<Text>().fontStyle = FontStyle.Bold;
            }
        }
        graphContainerBar.GetComponent<ContentSizeFitter>().horizontalFit = ContentSizeFitter.FitMode.MinSize;

    }

    private GameObject CreateBar(Vector2 graphPosition, float barWidth, string pDate)
    {
        GameObject gameObject = new GameObject("bar", typeof(Image));
        gameObject.transform.SetParent(box.transform, false);
        gameObject.AddComponent<Button>();
        gameObject.GetComponent<Button>().onClick.AddListener(() => ShowInfo(pDate));
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
   
        rectTransform.anchoredPosition = new Vector2(graphPosition.x, 0f);
        rectTransform.sizeDelta = new Vector2(barWidth, graphPosition.y);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.pivot = new Vector2(.5f, 0f);
        return gameObject;
    }

    IEnumerator SearchCorrectInfo(string date)
    {
        Debug.Log("자세 : " + posture + ", 날짜 : " + date);
        string url = DataManager.GetInstance().GetAWSURL();
        WWWForm form = new WWWForm();
       
        form.AddField("posture", posture, System.Text.Encoding.UTF8);
        form.AddField("date", date);


        WWW webRequest = new WWW(url + "/OutputPerformResult.php", form);
        yield return webRequest; //정보가 올 때까지 기다림

        Debug.Log("점수 얻기 : " + webRequest.text);
        string[] webstr = webRequest.text.Split('\n');
        string[] score = webstr[1].Split(',');
        dateInfo.text = date;

        OnClickedBar();
        GameObject.Find("RecordCanvas/UI/Panel/RecordChartMaker").GetComponent<Testing2>().SetScore(score);

    }

    public void OnClickedBar()
    {
        calander.SetActive(false);
        radarChart.SetActive(true);
    }
}