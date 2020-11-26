using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Helps : MonoBehaviour
{
    public GameObject startCanvas, bodySizeCanvas, bodyRecordCanvas;
    public GameObject nextButton;
    public Text heightText, armLengthText, legLengthText;
    public Image image;
    public Sprite[] tutorialImages;

    DataManager dataManager;
    string url;

    int stepNumber;
    // Start is called before the first frame update
    void Start()
    {
        dataManager = DataManager.GetInstance();
        url = dataManager.GetAWSURL();
        DestroyAllCanvas();
        startCanvas.SetActive(true);
        stepNumber = 1;
        SetStartText(stepNumber);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DestroyAllCanvas()
    {
        startCanvas.SetActive(false);
        bodySizeCanvas.SetActive(false);
        bodyRecordCanvas.SetActive(false);
    }
    public void SetStartCanvasActive(bool b)
    {
        startCanvas.SetActive(b);
    }
    public bool IsActivedStartCanvas()
    {
        return startCanvas.activeSelf;
    }

    public void SetStartCanvasEnabled(bool b)
    {
        startCanvas.GetComponent<Canvas>().enabled = b;
    }
    public void SetStartText(int step)
    {
        if(startCanvas.activeSelf || step==6 || step == 9) {
            switch (step)
            {
                case 0: 
                    break;
                case 1:
                    break;
                case 2:
                    if (stepNumber == 1)
                    {
                        nextButton.SetActive(true);
                        image.sprite = tutorialImages[1];
                        stepNumber = step;
                    }
                    break;
                case 3:
                    if (stepNumber == 2)
                    {
                   
                        image.sprite = tutorialImages[2];
                        stepNumber = step;
                        nextButton.SetActive(false);
                    }
                    break;
                case 4:
                    if (stepNumber == 3)
                    {
                        image.sprite = tutorialImages[3];
                        stepNumber = step;
                    }
                    break;
                case 5:
                    if (stepNumber == 4)
                    {
                        image.sprite = tutorialImages[4];
                        stepNumber = step;
                    }
                    break;
                case 6:
                    if (stepNumber == 5)
                    {
                        startCanvas.SetActive(true);
                        image.sprite = tutorialImages[6];
                        stepNumber = step;
                    }
                    break;
                default:
                    SetStartCanvasActive(false);
                    break;
            }
        }
    }

    public void ShowBodyRecordCanvas(float userHeight, float userArmLength, float userLegLength)
    {
        StartCoroutine(SaveUserSizeToAWS(userHeight, userArmLength, userLegLength));
        SetBodyRecordCanvasActive(false);
        SetBodySizeCanvasActive(true);
        SetBodySizeText(userHeight, userArmLength, userLegLength);

    }
    public void SetBodySizeCanvasActive(bool b)
    {
        bodySizeCanvas.SetActive(b);
    }

    public void SetBodyRecordCanvasActive(bool b)
    {
        bodyRecordCanvas.SetActive(b);
    }

    public void SetBodySizeText(float userHeight, float userArmLength, float userLegLength)
    {
        heightText.text = "키 : " + userHeight.ToString();
        armLengthText.text = "팔 : " + (userArmLength-15).ToString();
        legLengthText.text = "다리 : " + (userLegLength+8).ToString();
    }

    IEnumerator SaveUserSizeToAWS(float height, float armLength, float legLength)
    {
        WWWForm form = new WWWForm();
        form.AddField("height", height.ToString());
        form.AddField("arm_length", armLength.ToString());
        form.AddField("leg_length", legLength.ToString());

        WWW webRequest = new WWW(url + "/InputUserSize.php", form);
        yield return webRequest; //정보가 올 때까지 기다림

        if (webRequest.text.Equals("\n1\n"))
        {
            Debug.Log("사용자 신체크기 저장 완료");
        }
        else
        {
            Debug.Log("사용자 신체크기 저장 실패");
        }
    
    }
}
