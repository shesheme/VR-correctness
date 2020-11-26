using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System.Text;
using Valve.VR;
using System.Text.RegularExpressions;
using Microsoft.SqlServer.Server;

public class All_Knn_Results
{
    private List<Knn_Results> knn_result_list;  //모든 자세들에 대한 평가 객체 저장
    private string pose_name;   //가장 못한 자세 이름
    private double score;          //평균 점수
    private string evaluation = ""; //평균 점수에 대한 평가

    public All_Knn_Results()
    {
        this.knn_result_list = new List<Knn_Results>();
        this.pose_name = "BasicPose";
        this.score = 0;
    }
    public double GetAvgScore()
    {
        return this.score;
    }

    public int Count()
    {
        return knn_result_list.Count;
    }
    public void Add_Result(Knn_Results result)
    {
        this.knn_result_list.Add(result);
        this.Avg_Calculate();
    }
    public string GetEvaluation()
    {
        UnityEngine.Debug.LogError("all에서 평가 불러오기");
        return this.evaluation;
    }
    public Knn_Results GetEachResult(int num)
    {
        return this.knn_result_list[num];
    }
    public void Avg_Calculate()
    {
        this.evaluation = "";
        double all_score = 0;
        int min_score = 100;
        string min_pose = "BasicPose";
        for(int i=0; i<this.knn_result_list.Count; i++)
        {
            int tmpScore = knn_result_list[i].GetScore();
            all_score += tmpScore;
            if (tmpScore < min_score)
            {
                min_score = tmpScore;
                min_pose = knn_result_list[i].GetPoseName();
            }
        }
        all_score /= knn_result_list.Count;
        this.score = all_score;
        this.evaluation += "평균 점수 : " + this.score.ToString() + "점 / 50점\n";
        this.evaluation += "수행한 운동 개수 : " + this.knn_result_list.Count.ToString()+"개\n\n";
        this.pose_name = min_pose;
        if (this.score >= 10 && this.score < 20)
        {
            this.evaluation += "전체적으로\n숙련도가 부족합니다.\n\n가이드의 자세를\n유심히 관찰하며\n다시 시도해보세요!\n";
        }
        else if (this.score >= 20 && this.score < 30)
        {
            this.evaluation += "많이 아쉬운 점수입니다.\n\n각 자세들의\n부족한 부위를\n잘 확인하며\n다시 시도해보세요!\n";
        }
        else if (this.score >= 30 && this.score < 40)
        {
            this.evaluation += "잘 하셨습니다.\n\n이제 숙련도를 올려\n상위 점수를 노려보세요!\n";
        }
        else if (this.score >= 40 && this.score < 50)
        {
            this.evaluation += "정말 잘 하셨습니다.\n\n부족한 자세가\n무엇인지 확인하여\n좀 더 노력해보세요!\n";
        }
        else if (this.score == 50)
        {
            this.evaluation += "만점입니다!\n정말 완벽하시군요!\n";
        }
        this.evaluation += "\n부족한 자세 : "+this.pose_name;
    }
}
public class Knn_Results
{
    private string pose_name;
    private int very_well;
    private int well;
    private int normal;
    private int bad;
    private int very_bad;
    private int score=0;
    private double totalTime;
    private double minErrorTime;
    private string minErrorTimeName;
    private double maxErrorTime;
    private string maxErrorTimeName;
    private string evaluation = "";
    public Knn_Results(string _pose_name, string results, double _totalTime, double _minErrorTime, string _minErrorTimeName, double _maxErrorTime, string _maxErrorTimeName)
    {
        this.pose_name = _pose_name;
        this.Set_Times(_totalTime, _minErrorTime, _minErrorTimeName, _maxErrorTime, _maxErrorTimeName);
        this.Parsing_String1(results);
        this.Set_Evaluation();
    }
    public int GetScore()
    {
        return this.score;
    }
    public string GetPoseName()
    {
        return this.pose_name;
    }
    private void Set_Times(double _totalTime, double _minErrorTime, string _minErrorTimeName, double _maxErrorTime, string _maxErrorTimeName)
    {
        this.totalTime = _totalTime;
        this.minErrorTime = _minErrorTime;
        this.minErrorTimeName = _minErrorTimeName;
        this.maxErrorTime = _maxErrorTime;
        this.maxErrorTimeName = _maxErrorTimeName;
    }

    private void Set_Evaluation()
    {
        this.Set_Score();
        this.evaluation = "시간 : "+string.Format("{0:0.00}", double.Parse(this.totalTime.ToString()))+"초\n";
        this.evaluation += "점수 : " + this.score.ToString() + "점 / 50점\n\n";
        UnityEngine.Debug.Log("점수점수점수점수:" + this.score.ToString());

        if (this.score >=10 && this.score<20)
        {
            this.evaluation += "전체적으로\n숙련도가 부족합니다.\n\n가이드의 자세를\n유심히 관찰하며\n다시 시도해보세요!\n";
        }
        else if(this.score>=20 && this.score<30)
        {
            this.evaluation += "많이 아쉬운 점수입니다.\n\n부족한 부위를 잘 확인하며\n다시 시도해보세요!\n";
        }
        else if(this.score>=30 && this.score<40)
        {
            this.evaluation += "잘 하셨습니다.\n\n이제 숙련도를 올려\n상위 점수를 노려보세요!\n";
        }
        else if(this.score>=40 && this.score<50)
        {
            this.evaluation += "정말 잘 하셨습니다.\n이제 만점에 도전해보세요!\n";
        }
        else if (this.score == 50)
        {
            this.evaluation += "만점입니다!\n\n이제 당신은\n" + this.pose_name+"의 마스터입니다!\n";
        }
       // this.evaluation += "\n";
        this.evaluation += "가장 훌륭한 부위 : \'"+ this.minErrorTimeName +"\'\n";
        this.evaluation += "가장 부족한 부위 : \'" + this.maxErrorTimeName + "\'\n";
    }
    public string GetEvaluation()
    {
        if (this.evaluation != "")
        {
            return this.evaluation;
        }
        else
        {
            return "evaluation 오류";
        }
    }
    private void Set_Score()
    {
        UnityEngine.Debug.LogError(this.very_well.ToString()+" / " +this.well.ToString() + " / " +this.normal.ToString() + " / " +this.bad.ToString() + " / " +this.very_bad.ToString());
        this.score = 5 * this.very_well + 4 * this.well + 3 * this.normal + 2 * this.bad + 1 * this.very_bad;
    }

    private void Parsing_String1(String results)
    {
        UnityEngine.Debug.LogError(results);
        string[] substr = results.Split('\n');  // split(" : ")
        this.Parsing_String2(substr);
    }
    private void Parsing_String2(string[] substr)
    {
        for(int i=0; i<substr.Length; i++)
        {
            if (substr[i].Contains("완벽함"))
            {
                string[] temp = substr[i].Split(' ');
                this.very_well = Convert.ToInt32(temp[temp.Length-1]);
            }
            if (substr[i].Contains("잘함"))
            {
                string[] temp = substr[i].Split(' ');
                this.well = Convert.ToInt32(temp[temp.Length - 1]);
            }
            if (substr[i].Contains("보통"))
            {
                string[] temp = substr[i].Split(' ');
                this.normal = Convert.ToInt32(temp[temp.Length - 1]);
            }
            if (substr[i].Contains("못함"))
            {
                string[] temp = substr[i].Split(' ');
                this.bad = Convert.ToInt32(temp[temp.Length - 1]);
            }
            if (substr[i].Contains("망함"))
            {
                string[] temp = substr[i].Split(' ');
                this.very_bad = Convert.ToInt32(temp[temp.Length - 1]);
            }
        }
    }
}
public class Knn_Calculate : MonoBehaviour
{
    private DataManager dataManager;
    private double[] ErrorTimes;
    private double totalTime;
    private string PoseName;

    public void Init()
    {
        this.dataManager = DataManager.GetInstance();
        this.ErrorTimes = dataManager.GetLastAnimationTimes();
        this.totalTime = dataManager.GetLastPlayTime();
        PoseName = dataManager.GetLastItemName();

    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    enum AnimationParts : int
    {
        Head = 0,
        LH = 1,
        RH = 2,
        LF = 3,
        RF = 4
    }

    public Knn_Results Knn_Execute()
    {
        UnityEngine.Debug.LogError("knn진입");
        double E_HEAD = ErrorTimes[Convert.ToInt32(AnimationParts.Head)];
        double E_LH = ErrorTimes[Convert.ToInt32(AnimationParts.LH)];
        double E_RH = ErrorTimes[Convert.ToInt32(AnimationParts.RH)];
        double E_LF = ErrorTimes[Convert.ToInt32(AnimationParts.LF)];
        double E_RF = ErrorTimes[Convert.ToInt32(AnimationParts.RF)];
        List<double> sort_list = new List<double>();
        sort_list.Add(E_HEAD);
        sort_list.Add(E_LH);
        sort_list.Add(E_RH);
        sort_list.Add(E_LF);
        sort_list.Add(E_RF);
        sort_list.Sort();
        //KNN 알고리즘 - 파이썬
        string num1 = this.totalTime.ToString(); //전체 실행시간
        string num2 = ((E_HEAD + E_LH + E_RH + E_LF + E_RF) / 5).ToString(); //오류시간 평균
        string num3 = ((sort_list[0] + sort_list[4]) / 2).ToString();  //오류시간 중앙값
        string file_name = PoseName + ".txt";
        string Path = "Knn_code\\" + file_name;
        var psi = new ProcessStartInfo();
        UnityEngine.Debug.Log("file_name=" + file_name);
        UnityEngine.Debug.Log("num1=" + num1);
        UnityEngine.Debug.Log("num2=" + num2);
        UnityEngine.Debug.Log("num3=" + num3);
        psi.FileName = @"python.exe";
        //파이썬 코드 경로
        psi.Arguments = "Knn_code\\knn.py ";
        psi.Arguments += Path + " " + num1 + " " + num2 + " " + num3;
        psi.UseShellExecute = false;
        psi.CreateNoWindow = true;
        psi.RedirectStandardOutput = true;
        psi.RedirectStandardError = true;

        var errors = "";
        var results = "";
        using (var process = Process.Start(psi))
        {
            errors = process.StandardError.ReadToEnd();
            results = process.StandardOutput.ReadToEnd();
        }
        results = results.Replace("Very well", "완벽함").Replace("Well", "잘함").Replace("Normal", "보통").Replace("Bad", "못함").Replace("Very bad", "망함");
        //
        string minErrorTimeName = GetMinErrortime(E_HEAD, E_LH, E_RH, E_LF, E_RF, sort_list[0]);
        string maxErrorTimeName = GetMaxErrortime(E_HEAD, E_LH, E_RH, E_LF, E_RF, sort_list[4]);
        //
        Knn_Results result = new Knn_Results(PoseName, results, this.totalTime, sort_list[0], minErrorTimeName, sort_list[4], maxErrorTimeName);
  

        return result;
    }
    private string GetMinErrortime(double E_HEAD, double E_LH, double E_RH, double E_LF, double E_RF, double MinErrorTime)
    {
        string minErrorTimeName = "";
        if (MinErrorTime == E_HEAD)
        {
            minErrorTimeName = "머리";
        }
        else if (MinErrorTime == E_LH)
        {
            minErrorTimeName = "왼손";
        }
        else if (MinErrorTime == E_RH)
        {
            minErrorTimeName = "오른손";
        }
        else if (MinErrorTime == E_LF)
        {
            minErrorTimeName = "왼발";
        }
        else
        {
            minErrorTimeName = "오른발";
        }
        return minErrorTimeName;
    }
    private string GetMaxErrortime(double E_HEAD, double E_LH, double E_RH, double E_LF, double E_RF, double MaxErrorTime)
    {
        string maxErrorTimeName = "";
        if (MaxErrorTime == E_HEAD)
        {
            maxErrorTimeName = "머리";
        }
        else if (MaxErrorTime == E_LH)
        {
            maxErrorTimeName = "왼손";
        }
        else if (MaxErrorTime == E_RH)
        {
            maxErrorTimeName = "오른손";
        }
        else if (MaxErrorTime == E_LF)
        {
            maxErrorTimeName = "왼발";
        }
        else
        {
            maxErrorTimeName = "오른발";
        }
        return maxErrorTimeName;
    }
}