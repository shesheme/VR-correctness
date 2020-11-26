using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CorrectPanelEvent : MonoBehaviour
{
    public Text levelValue;
    int level;
    public void Start()
    {
        level = 0;
    }

    public void selectRoutine(int i)
    {
        GameObject.Find("Helps").GetComponent<Helps>().SetStartCanvasActive(false);
        startRoutine(i);
    }

    void startRoutine(int i)
    {
        DataManager dataManager = DataManager.GetInstance();
        dataManager.LoadFile(i);
        GameObject.Find("user/CorrectnessAnalysis").GetComponent<CorrectnessAnalysis>().Init(level);
        GameObject.Find("AnimationManager").GetComponent<AnimationManager>().init(i);
    }

    public void setLevel(int i)
    {
        level += i;
        if(level < 0)
        {
            level = 0;
        } else if(level > 2)
        {
            level = 2;
        }
        switch (level)
        {
            case 0:
                levelValue.text = "하";
                break;
            case 1:
                levelValue.text = "중";
                break;
            case 2:
                levelValue.text = "상";
                break;
        }
    }
}
