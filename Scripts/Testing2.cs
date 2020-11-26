using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Testing2 : MonoBehaviour
{
    [SerializeField] private UI_StatsRaderChart uiStatsRaderChart;
    Stats stats;
    DataManager dataManager;
    string url;

    double[] temp = new double[5];
    public void reset()
    {
        if (stats == null)
            Init();
        stats.SetStatAmount(Stats.Type.Head, 0);
        stats.SetStatAmount(Stats.Type.RH, 0);
        stats.SetStatAmount(Stats.Type.RF, 0);
        stats.SetStatAmount(Stats.Type.LF, 0);
        stats.SetStatAmount(Stats.Type.LH, 0);
        uiStatsRaderChart.SetStats(stats);
    }

    private void Update()
    {
    
        if (stats.GetStatAmount(Stats.Type.Head) < (int)temp[0])
        {
            stats.IncreaseStatAmount(Stats.Type.Head);
          //  uiStatsRaderChart.SetStats(stats);
        }
        if (stats.GetStatAmount(Stats.Type.RH) < (int)temp[2])
        {
            stats.IncreaseStatAmount(Stats.Type.RH);
          //  uiStatsRaderChart.SetStats(stats);
        }
        if (stats.GetStatAmount(Stats.Type.RF) < (int)temp[4])
        {
            stats.IncreaseStatAmount(Stats.Type.RF);
          //  uiStatsRaderChart.SetStats(stats);
        }
        if (stats.GetStatAmount(Stats.Type.LF) < (int)temp[3])
        {
            stats.IncreaseStatAmount(Stats.Type.LF);
          //  uiStatsRaderChart.SetStats(stats);
        }
        if (stats.GetStatAmount(Stats.Type.LH) < (int)temp[1])
        {
            stats.IncreaseStatAmount(Stats.Type.LH);
          //  uiStatsRaderChart.SetStats(stats);
        }
    }

    public void Init()
    {
        stats = new Stats(0, 0, 0, 0, 0);
        dataManager = DataManager.GetInstance();
        url = dataManager.GetAWSURL();
        uiStatsRaderChart.SetStats(stats);
        reset();
    }

    public void SetScore(string[] score)
    {
        reset();
        temp[0] = double.Parse(score[0]);
        temp[1] = double.Parse(score[1]);
        temp[2] = double.Parse(score[2]);
        temp[3] = double.Parse(score[3]);
        temp[4] = double.Parse(score[4]);
    }
}
