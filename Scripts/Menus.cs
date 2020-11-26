using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;




public class Menus : MonoBehaviour
{
    public Canvas canvas;
    public GameObject target;

    public GameObject optionPanel;
    public GameObject correctPanel;
    public GameObject recordPanel;
    public GameObject videoPanel;
    public GameObject lazer;

    Vector3 offset;

    private void Start()
    {
        offset.Set((float)0.25, (float)0.5, (float)3);
        canvas.enabled = false;

        callCorrect();
    }



    void Update()
    {
        canvas.transform.position = target.transform.position + offset;
    }

    public void allPanelDestroy()
    {
        optionPanel.SetActive(false);
        correctPanel.SetActive(false);
        recordPanel.SetActive(false);
        videoPanel.SetActive(false);
    }
    public void callMenu()
    {
        canvas.enabled = !canvas.enabled;
        if(canvas.enabled)
        {
            callCorrect();
        }
        setLazer(canvas.enabled);
    }

    public void callMenu(bool b)
    {
        canvas.enabled = b;
    }

    public void callOption()
    {
        allPanelDestroy();
        optionPanel.SetActive(true);
    }

    public void callCorrect()
    {
        allPanelDestroy();
        correctPanel.SetActive(true);
    }

    public void callRecord()
    {
        allPanelDestroy();
        recordPanel.SetActive(true);
    }
    public void callVideo()
    {
        allPanelDestroy();
        videoPanel.SetActive(true);
    }
    public void setLazer()
    {
        lazer.SetActive(!lazer.active);
    }

    public void setLazer(bool b)
    {
        lazer.SetActive(b);
    }

    public void LogOut()
    {
        StartCoroutine(SendLotOutMessageToAWS());
    }

    public void SuccessLogOut()
    {
        SceneManager.LoadScene(0);
    }

    public void FailLogOut()
    {

    }

    IEnumerator SendLotOutMessageToAWS()
    {
        string url = DataManager.GetInstance().GetAWSURL();
        WWW webRequest = new WWW(url + "/LogOut.php");
        yield return webRequest; //정보가 올 때까지 기다림
        Debug.Log("로그아웃 : " + webRequest.text);
        if (webRequest.text.Equals("\n1"))
        {
            SuccessLogOut();
        }
        else
        {
            FailLogOut();
        }

    }
}
