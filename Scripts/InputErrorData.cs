using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputErrorData : MonoBehaviour
{
    DataManager dataManager;
    public GameObject inputErrorData;

    IEnumerator Start()
    {
        dataManager = DataManager.GetInstance();
        int itemIdex = dataManager.GetItemIndex(); //0:목, 1:어깨, 2:허리, 3:다리
        List<double[]> animationTimeInfo = dataManager.GetAnimationTimes(); //double[], 0:머리 누적 오차 시간. 1:왼손, 2:오른손, 3:왼발: 4:오른발, 5:전체 수행 시간
                                                                            //animationTimeInfo[0] : itemIndex항목에서 첫번째 애니메이션
                                                                            //animationTimeInfo[1] : itemIndex항목에서 두번째 애니메이션

        for (int i = 0; i < animationTimeInfo.Count; i++)
        {
            string category = null;
            switch (itemIdex)
            {
                case 0:
                    category = "neck";
                    break;
                case 1:
                    category = "shoulder";
                    break;
                case 2:
                    category = "waist";
                    break;
                case 3:
                    category = "leg";
                    break;
            }
            string ani_name = category + (i + 1).ToString();

            double head_error = animationTimeInfo[i][0];
            double lh_error = animationTimeInfo[i][1];
            double rh_error = animationTimeInfo[i][2];
            double lf_error = animationTimeInfo[i][3];
            double rf_error = animationTimeInfo[i][4];
            double net_error = animationTimeInfo[i][5];

            WWWForm form = new WWWForm();
            form.AddField("ani_name", ani_name);
            form.AddField("head_error", head_error.ToString());
            form.AddField("lh_error", lh_error.ToString());
            form.AddField("rh_error", rh_error.ToString());
            form.AddField("lf_error", lf_error.ToString());
            form.AddField("rf_error", rf_error.ToString());
            form.AddField("net_error", net_error.ToString());

            WWW webRequest = new WWW("http://ec2-13-125-126-232.ap-northeast-2.compute.amazonaws.com/InputErrorData.php", form);
            string err_msg = webRequest.error;
            yield return webRequest; //정보가 올 때까지 기다림

            string response = webRequest.text;

            inputErrorData.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
 
    }

}
