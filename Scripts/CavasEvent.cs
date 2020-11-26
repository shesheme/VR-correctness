using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

public class CavasEvent : MonoBehaviour
{
    public InputField IDField, PWField;
    public GameObject alertCanvas, LoginPanel, SignUpPanel;
    public Text alertText;
    public InputField S_IDField, S_PWField, S_PWField2, S_AgeField;
    public ToggleGroup toggleGroup;

    string url;

    // Start is called before the first frame update
    void Start()
    {
        url = "http://ec2-3-35-168-80.ap-northeast-2.compute.amazonaws.com";
        alertCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowSignUpPanel()
    {
        LoginPanel.SetActive(false);
        SignUpPanel.SetActive(true);
    }

    public void CancelSignUpPanel()
    {
        S_IDField.text = "";
        S_PWField.text = "";
        S_PWField2.text = "";
        S_AgeField.text = "";
        toggleGroup.gameObject.transform.Find("0").GetComponent<Toggle>().isOn = true;
        toggleGroup.gameObject.transform.Find("1").GetComponent<Toggle>().isOn = false;
        SignUpPanel.SetActive(false);
        LoginPanel.SetActive(true);
    }
    /*
     * 회원 가입 절차
     * 프로젝트 내부에서 ID와 PW를 POST방식으로 php에 전달
     * php에서 아이디 중복 검사 수행
     * ID가 중복일 경우 SignUpError함수 실행
     * ID가 중복이 아닐 SignUpSuccess함수 실행
     */
    public void SignUp()
    {
        if (!S_PWField.text.Equals(S_PWField2.text))
        {
            alertCanvas.SetActive(true);
            alertText.text = "비밀번호가\n일치하지 않습니다.";
        } else if(S_IDField.text.Equals("") || S_PWField.text.Equals("") || S_PWField2.text.Equals(""))
        {
            alertCanvas.SetActive(true);
            alertText.text = "ID 혹은 비밀번호를\n입력해 주십시오.";
        }
        else
        {
            string sex = toggleGroup.ActiveToggles().FirstOrDefault().name;
            StartCoroutine(SignUpCheck(S_IDField.text, S_PWField.text, sex, S_AgeField.text));
        }
    }
    
    void SignUpError()
    {
        alertCanvas.SetActive(true);
        alertText.text = "이미 존재하는 ID입니다.";
    }
    void SignUpSuccess()
    {
        CancelSignUpPanel();
        alertCanvas.SetActive(true);
        alertText.text = "회원 가입 성공!";
    }

    /*
     * 로그인 절차
     * 프로젝트 내부에서 ID와 PW를 POST방식으로 php에 전달
     * php에서 아이디와 비밀번호가 일치하는지 검사
     * 아이디와 비밀번호가 일치할 경우 SignIn함수 실행 후 php세션에 아이디 등록
     * 아이디와 비밀번호가 불일치할 경우 LoginError함수 실행
     */
    public void Login()
    {
        StartCoroutine(LoginCheck(IDField.text, PWField.text));
    }

    //로그인 성공 후 자세교정 씬으로 전환
    void SignIn()
    {
        SceneManager.LoadScene(1);
    }

    void LoginError()
    {
        alertCanvas.SetActive(true);
        alertText.text = "아이디와 비밀번호를\n확인해주세요.";
    }

    public void CloseAlert()
    {
        alertCanvas.SetActive(false);
    }

    IEnumerator SignUpCheck(string userID, string userPW, string userSex, string userAge)
    {
        WWWForm form = new WWWForm();
        form.AddField("id", userID);
        form.AddField("password", userPW);
        form.AddField("sex", userSex);
        form.AddField("age", userAge);


        WWW webRequest = new WWW(url + "/SignUp.php", form);
        yield return webRequest; //정보가 올 때까지 기다림
        Debug.Log("회원 : " + webRequest.text + "test");
        Debug.Log("회원 가입 : " + webRequest.MoveNext() + "test");
        //SOF + 1 + EOF
        Debug.Log("회원 가입 에러 : " + webRequest.error + "test");

        if (webRequest.text.Equals("\n1\n"))
        {
            SignUpSuccess();
        } else
        {
            SignUpError();
        }
    }

    IEnumerator LoginCheck(string userID, string userPW)
    {
        WWWForm form = new WWWForm();
        form.AddField("id", userID);
        form.AddField("password", userPW);


        WWW webRequest = new WWW(url + "/LogIn.php", form);
        yield return webRequest; //정보가 올 때까지 기다림
        Debug.Log("로그인 : " + webRequest.text);
        if (webRequest.text.Equals("\n1\n"))
        {
            SignIn();
        }
        else
        {
            LoginError();
        }
    }
}
