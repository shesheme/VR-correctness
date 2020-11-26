using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultPanelEvent : MonoBehaviour
{
    public GameObject resultPanel;
    
    public void ClosePanel()
    {
        resultPanel.SetActive(false);
    }
}
