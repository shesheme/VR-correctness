using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawChart : MonoBehaviour
{
    [SerializeField] private UI_StatsRaderChart uiStatsRaderChart;
    Stats stats;
    DataManager dataManager;
    public GameObject nextButton, preButton, subItemText, outputErrorData;

    List<double[]> animationTimes;
    List<string> animationStr;
    double[] netTime, times, temp;

    int chartIndex;
    public void reset()
    {
        stats.SetStatAmount(Stats.Type.Head, 0);
        stats.SetStatAmount(Stats.Type.RH, 0);
        stats.SetStatAmount(Stats.Type.RF, 0);
        stats.SetStatAmount(Stats.Type.LF, 0);
        stats.SetStatAmount(Stats.Type.LH, 0);
        uiStatsRaderChart.SetStats(stats);
    }

    public void ShowNetData()
    {
        reset();
        nextButton.SetActive(false);
        preButton.SetActive(false);
        subItemText.SetActive(false);
        temp = dataManager.GetNetDataOfServer();

    }

    public void ShowItemData()
    {
        reset();

        nextButton.SetActive(true);
        preButton.SetActive(true);
        subItemText.SetActive(true);

        animationTimes = dataManager.GetItemDataOfServer();
        animationStr = dataManager.GetItemStrOfServer();
        temp = animationTimes[0];
        subItemText.GetComponent<Text>().text = animationStr[0];
    }

    public void ShowDateData()
    {
        reset();

        nextButton.SetActive(true);
        preButton.SetActive(true);
        subItemText.SetActive(true);
        outputErrorData.SetActive(false);
        outputErrorData.SetActive(true);


    
    }
    public void SetNetData()
    {
        animationTimes = new List<double[]>();
        animationTimes.Add(dataManager.GetNetDataOfServer());
    }
    public void SetDateData()
    {
        animationTimes = dataManager.GetDateDataOfServer();
        animationStr = dataManager.GetDateStrOfServer();
    }
    private void Start()
    {
        stats = new Stats(0, 0, 0, 0, 0);
        dataManager = DataManager.GetInstance();
        chartIndex = 0;
       animationTimes = new List<double[]>();
        uiStatsRaderChart.SetStats(stats);
        reset();
    }
    private void Update()
    {
        temp = animationTimes[chartIndex];
        subItemText.GetComponent<Text>().text = animationStr[chartIndex];

        if (stats.GetStatAmount(Stats.Type.Head) < (int)temp[0])
        {
            stats.IncreaseStatAmount(Stats.Type.Head);
            uiStatsRaderChart.SetStats(stats);
        }
        if (stats.GetStatAmount(Stats.Type.RH) < (int)temp[1])
        {
            stats.IncreaseStatAmount(Stats.Type.RH);
            uiStatsRaderChart.SetStats(stats);
        }
        if (stats.GetStatAmount(Stats.Type.RF) < (int)temp[4])
        {
            stats.IncreaseStatAmount(Stats.Type.RF);
            uiStatsRaderChart.SetStats(stats);
        }
        if (stats.GetStatAmount(Stats.Type.LF) < (int)temp[3])
        {
            stats.IncreaseStatAmount(Stats.Type.LF);
            uiStatsRaderChart.SetStats(stats);
        }
        if (stats.GetStatAmount(Stats.Type.LH) < (int)temp[1])
        {
            stats.IncreaseStatAmount(Stats.Type.LH);
            uiStatsRaderChart.SetStats(stats);
        }
    }

    public void AddIndex(int value)
    {
        chartIndex += value;

        if (chartIndex <= 0)
            chartIndex = 0;
        if (chartIndex > animationTimes.Count)
            chartIndex = animationTimes.Count;

        reset();

        temp = animationTimes[chartIndex];
        subItemText.GetComponent<Text>().text = animationStr[chartIndex];
    }
}
