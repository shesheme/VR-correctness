using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    [SerializeField] private UI_StatsRaderChart uiStatsRaderChart;
    Stats stats;
    DataManager dataManager;
    All_Knn_Results all_knn_results;
    string url;

    List<double[]> animationTimes;
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

    private void Start()
    {

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
        animationTimes = dataManager.GetAnimationTimes();
        all_knn_results = dataManager.GetAll_Knn_Results();
        CalculateScore(animationTimes, all_knn_results);
        netTime = new double[6];

        foreach (double[] time in animationTimes)
        {
            for (int i = 0; i < 6; i++)
                netTime[i] += time[i];
        }
        times = new double[6];
        times = netTime;
        temp = new double[5];
        for (int i = 0; i < 5; i++)
        {
            temp[i] = ((times[i] / times[5]) * 100);
            temp[i] = 100 - temp[i];
        
        }
        chartIndex = 0;
    }

    public void AddIndex(int value)
    {
        chartIndex += value;

        if (chartIndex < 0)
        {
            chartIndex = 0;
            return;
        }
        if (chartIndex > animationTimes.Count)
        {
            chartIndex = animationTimes.Count;
            return;
        }
        reset();


        if (chartIndex == 0)
        {
            times = netTime;
        }
        else
        {
            times = animationTimes[chartIndex - 1];
        }
        for (int i = 0; i < 5; i++)
        {
            temp[i] = ((times[i] / times[5]) * 100);
            temp[i] = 100 - temp[i];

          
        }
    }

    void CalculateScore(List<double[]> aniTimes, All_Knn_Results all_Knn)
    {

        double[] totalTime = new double[6];
        List<double[]> performScore = new List<double[]>();

        foreach (double[] time in aniTimes)
        {
            double[] score = new double[5];
            for (int i = 0; i < 6; i++)
            {
                totalTime[i] += time[i];
                if (i != 5)
                {
                    score[i] = (time[i] / time[5]) * 100;
                    score[i] = 100 - score[i];
                }
            }
            performScore.Add(score);
        }
        double[] totalScore = new double[5];
        for (int i = 0; i < 5; i++)
        {
            totalScore[i] = ((totalTime[i] / totalTime[5]) * 100);
            totalScore[i] = 100 - totalScore[i];
        }

        List<string> performname = dataManager.GetItemNameList();
        Debug.Log("이름수 : " + performname.Count + ", 부위별점수개수 : " + performScore.Count + ", 평균점수개수 : " + all_Knn.Count());
        for(int i=0;i<performScore.Count;i++)
            StartCoroutine(PostScoreToAWS(performname[i], performScore[i], all_Knn.GetEachResult(i).GetScore()));
    }

    IEnumerator PostScoreToAWS(string name, double[] scores, double avgScore)
    {
        WWWForm form = new WWWForm();
        form.AddField("ani_name", name, System.Text.Encoding.UTF8);
        form.AddField("score", avgScore.ToString());
        form.AddField("head", scores[0].ToString());
        form.AddField("lh", scores[1].ToString());
        form.AddField("rh", scores[2].ToString());
        form.AddField("lf", scores[3].ToString());
        form.AddField("rf", scores[4].ToString());


        WWW webRequest = new WWW(url + "/InputPerformResult.php", form);
        yield return webRequest; //정보가 올 때까지 기다림

        if (webRequest.text.Equals("\n1\n"))
        {
            Debug.Log("수행 점수 전송 성공!");
        }
        else
        {
            Debug.Log("수행 점수 전송 실패!");
        }
    }
}
