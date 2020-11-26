using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionPanelEvent : MonoBehaviour
{
    public Text heightText, floorText, sizeText, guideVisibleText;
    public GameObject frontGuide, backGuide;

    Vector3 floorPosition;
    Vector3 userScale;


    public void start()
    {
        userScale = GameObject.Find("user").GetComponent<Transform>().localScale;
        floorPosition = GameObject.Find("Room").GetComponent<Transform>().position;

    }

    public void setHeight(float value)
    {
        float height = float.Parse(heightText.text);
        GameObject.Find("user").GetComponent<Transform>().localScale = new Vector3(userScale.x, height+value-(float)0.8, userScale.z);
        heightText.text = (height+value).ToString();
    }

    public void setFloorPosition(float value)
    {
        float position = float.Parse(floorText.text);
        GameObject.Find("Room").GetComponent<Transform>().position = new Vector3(floorPosition.x, position+value, floorPosition.z);
        floorText.text = (position).ToString();
    }

    public void ShowGuide()
    {
      
        if(frontGuide.active && backGuide.active)
        {
            frontGuide.SetActive(false);
            backGuide.SetActive(false);
            guideVisibleText.text = "가이드 표시";
        } else
        {
            frontGuide.SetActive(true);
            backGuide.SetActive(true);
            guideVisibleText.text = "가이드 비표시";
        }
    }
    public void SetGuideSize(float value)
    {
        if (!frontGuide.active && !backGuide.active)
        {
            frontGuide.SetActive(true);
            backGuide.SetActive(true);
            guideVisibleText.text = "가이드 비표시";
        }
        float size = float.Parse(sizeText.text);
        GameObject.Find("Room/BackGuide").GetComponent<Transform>().localScale = new Vector3(size + value, size + value, size + value);
        GameObject.Find("Room/FrontGuide").GetComponent<Transform>().localScale = new Vector3(size + value, size + value, size + value);
        sizeText.text = (size + value).ToString();
    }
}
