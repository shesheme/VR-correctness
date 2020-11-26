using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTC.UnityPlugin.Vive;
using UnityEngine.UI;
using UnityEditorInternal;
using UnityEngine.Analytics;
using UnityEditor;
using TMPro;
using System.IO;
using Valve.VR;

[System.Serializable]
public class VRDevice
{
    public GameObject correctSphere;
    public GameObject infomation;
    public Slider slider;
    public Material sphereColor;
    int deviceIndex, state;
    double correctTime, errorTime, deltaTime;
    float standardSize;
    float userSize = 0;
    public void Init(int index, float standSize)
    {
        standardSize = standSize;
        if (userSize == 0)
            userSize = standardSize;
        deviceIndex = index;
        if (index != 6)
        {
            infomation.SetActive(true);
            slider.value = 0;
        }
        correctSphere.SetActive(true);
        sphereColor.color = Color.white;

        SetCorrectTime(0);
        SetErrorTime(0);
        SetDeltaTime(Time.time);
    }

    public int GetDeviceIndex()
    {
        return deviceIndex;
    }

    public Vector3 GetPosition()
    {
        Vector3 position;
        switch (deviceIndex)
        {
            case 1:
                position = VivePose.GetPoseEx(DeviceRole.Hmd).pos;
                break;
            case 2:
                position = VivePose.GetPoseEx(HandRole.LeftHand).pos;
                break;
            case 3:
                position = VivePose.GetPoseEx(HandRole.RightHand).pos;
                break;
            case 4:
                position = VivePose.GetPoseEx(TrackerRole.Tracker1).pos;
                break;
            case 5:
                position = VivePose.GetPoseEx(TrackerRole.Tracker2).pos;
                break;
            case 6:
                position = VivePose.GetPoseEx(TrackerRole.Tracker3).pos;
                break;
            default:
                position = new Vector3(0, 0, 0);
                break;
        }
        return position;
    }

    public Vector3 GetPosition(int index)
    {
        Vector3 position;
        switch (index)
        {
            case 1:
                position = VivePose.GetPoseEx(DeviceRole.Hmd).pos;
                break;
            case 2:
                position = VivePose.GetPoseEx(HandRole.LeftHand).pos;
                break;
            case 3:
                position = VivePose.GetPoseEx(HandRole.RightHand).pos;
                break;
            case 4:
                position = VivePose.GetPoseEx(TrackerRole.Tracker1).pos;
                break;
            case 5:
                position = VivePose.GetPoseEx(TrackerRole.Tracker2).pos;
                break;
            case 6:
                position = VivePose.GetPoseEx(TrackerRole.Tracker3).pos;
                break;
            default:
                position = new Vector3(0, 0, 0);
                break;
        }
        return position;
    }

    public Vector3 GetRotation()
    {
        Quaternion rotation;
        switch (deviceIndex)
        {
            case 1:
                rotation = VivePose.GetPoseEx(DeviceRole.Hmd).rot;
                break;
            case 2:
                rotation = VivePose.GetPoseEx(HandRole.LeftHand).rot;
                break;
            case 3:
                rotation = VivePose.GetPoseEx(HandRole.RightHand).rot;
                break;
            case 4:
                rotation = VivePose.GetPoseEx(TrackerRole.Tracker1).rot;
                break;
            case 5:
                rotation = VivePose.GetPoseEx(TrackerRole.Tracker2).rot;
                break;
            case 6:
                rotation = VivePose.GetPoseEx(TrackerRole.Tracker3).rot;
                break;
            default:
                rotation = new Quaternion(0, 0, 0, 0);
                break;
        }
        Vector3 normal = rotation.eulerAngles;

        normal.x -= 180;
        normal.x = Mathf.Abs(normal.x);
        normal.y -= 180;
        normal.y = Mathf.Abs(normal.y);
        normal.z -= 180;
        normal.z = Mathf.Abs(normal.z);

        return normal;
    }

    public Vector3 GetRotation(int index)
    {
        Quaternion rotation;
        switch (index)
        {
            case 1:
                rotation = VivePose.GetPoseEx(DeviceRole.Hmd).rot;
                break;
            case 2:
                rotation = VivePose.GetPoseEx(HandRole.LeftHand).rot;
                break;
            case 3:
                rotation = VivePose.GetPoseEx(HandRole.RightHand).rot;
                break;
            case 4:
                rotation = VivePose.GetPoseEx(TrackerRole.Tracker1).rot;
                break;
            case 5:
                rotation = VivePose.GetPoseEx(TrackerRole.Tracker2).rot;
                break;
            case 6:
                rotation = VivePose.GetPoseEx(TrackerRole.Tracker3).rot;
                break;
            default:
                rotation = new Quaternion(0, 0, 0, 0);
                break;
        }
        Vector3 normal = rotation.eulerAngles;

        normal.x -= 180;
        normal.x = Mathf.Abs(normal.x);
        normal.y -= 180;
        normal.y = Mathf.Abs(normal.y);
        normal.z -= 180;
        normal.z = Mathf.Abs(normal.z);

        return normal;
    }

    public void SetState(int value)
    {
        state = value;
    }

    public void SetSphereVisible(bool b)
    {
        correctSphere.SetActive(b);
    }

    public void SetSphereColor(Color color)
    {
        sphereColor.color = color;
    }

    public void SetInfoVIsible(bool b)
    {
        infomation.SetActive(b);
    }
    public void SetCorrectTime(double time)
    {
        correctTime = time;
    }
    public void SetErrorTime(double time)
    {
        errorTime = time;
    }
    public void SetDeltaTime(double time)
    {
        deltaTime = time;
    }
    public void AddCorrectTime(double time)
    {
        correctTime += time;
    }
    public void AddErrorTime(double time)
    {
        errorTime += time;
    }
    public double GetCorrectTime()
    {
        return correctTime;
    }
    public double GetErrorTime()
    {
        return errorTime;
    }
    public double GetDeltaTime()
    {
        return deltaTime;
    }
    public void SetSliderValue(double value)
    {
        value -= 0.5;
        if (value < 0)
            value = 0;
        else if (value > 1)
            value = 1;
        slider.value = 1 - (float)value;
    }

    public void SetStandardSize(float size)
    {
        standardSize = size;
    }

    public float GetStandardSize()
    {
        return standardSize;
    }

    public void SetUserSize(float size)
    {
        userSize = size;
    }
    public float GetUserSize()
    {
        return userSize;
    }
}

public class CorrectnessAnalysis : MonoBehaviour
{
    public VRDevice head, leftHand, rightHand, leftFoot, rightFoot, waist;
    public GameObject resultPanel;
    public GameObject lazer;
    public GameObject inputErrorData;
    public GameObject slider;

    int animationIndex, clipIndex, level, threshold;
    double weight, startTime;
    double playTime;
    DataManager dataManager;
    Vector3[,] positionList;
    Vector3[,] rotationList;
    List<double[]> correctTimeList;
    List<double[]> errorTimeList;
    List<double> totalTimeList;
    List<Vector3> lengthOfHead, lengthOfLH, lengthOfRH, lengthOfLF, lengthOfRF;
    List<Vector3> rotationOfHead, rotationOfLH, rotationOfRH, rotationOfLF, rotationOfRF;
    bool playFlag, isSync;
    void Update()
    {
       

    }
    public float Pytha(float x, float y)
    {
        return Mathf.Sqrt(Mathf.Pow(x, 2) + Mathf.Pow(y, 2));
    }
    public void Init(int itemLevel)
    {
        level = itemLevel;
        animationIndex = 0;
        dataManager = DataManager.GetInstance();
        positionList = dataManager.GetPositionData(animationIndex);
        rotationList = dataManager.GetRotationData(animationIndex);
        threshold = 35;
        //threshold = dataManager.GetThresholdData(itemIndex);
        correctTimeList = new List<double[]>();
        errorTimeList = new List<double[]>();
        totalTimeList = new List<double>();
        resultPanel.SetActive(false);
        inputErrorData.SetActive(false);

        switch(level)
        {
            case 1://중
                weight = 0.8;
                break;
            case 2://상
                weight = 0.5;
                break;
            default:
                weight = 1;
                break;
        }

        head.Init(1, 63.5f);
        leftHand.Init(2, 91f);
        rightHand.Init(3, 86.5f);
        leftFoot.Init(4, 93f);
        rightFoot.Init(5, 92.5f);
        waist.Init(6, 1f);
        clipIndex = 0;
        startTime = Time.time;
        playTime = Time.time;
        slider.SetActive(true);
        slider.GetComponent<Slider>().maxValue = 5;
        slider.GetComponent<Slider>().value = 0;
        playFlag = true;



        StartCoroutine(Calculate(clipIndex, head));
        StartCoroutine(Calculate(clipIndex, leftHand));
        StartCoroutine(Calculate(clipIndex, rightHand));
        StartCoroutine(Calculate(clipIndex, leftFoot));
        StartCoroutine(Calculate(clipIndex, rightFoot));
        StartCoroutine(PlayCorretness());
    }
    IEnumerator RecordSize()
    {
        while(isSync)
        {
            yield return null;
            if (ViveInput.GetTriggerValue(HandRole.LeftHand) == 1 && ViveInput.GetTriggerValue(HandRole.RightHand) == 1 && Time.time - startTime < 3)
            {
                lengthOfHead.Add((waist.GetPosition(6) - leftHand.GetPosition(1)) * 100);
                lengthOfLH.Add((waist.GetPosition(6) - leftHand.GetPosition(2)) * 100);
                lengthOfRH.Add((waist.GetPosition(6) - leftHand.GetPosition(3)) * 100);
                lengthOfLF.Add((waist.GetPosition(6) - leftHand.GetPosition(4)) * 100);
                lengthOfRF.Add((waist.GetPosition(6) - leftHand.GetPosition(5)) * 100);

                Vector3 temp = waist.GetRotation(6) - leftHand.GetRotation(1);
                temp.x = Mathf.Abs(temp.x);
                temp.y = Mathf.Abs(temp.y);
                temp.z = Mathf.Abs(temp.z);
                rotationOfHead.Add(temp);

                temp = waist.GetRotation(6) - leftHand.GetRotation(2);
                temp.x = Mathf.Abs(temp.x);
                temp.y = Mathf.Abs(temp.y);
                temp.z = Mathf.Abs(temp.z);
                rotationOfLH.Add(temp);

                temp = waist.GetRotation(6) - leftHand.GetRotation(3);
                temp.x = Mathf.Abs(temp.x);
                temp.y = Mathf.Abs(temp.y);
                temp.z = Mathf.Abs(temp.z);
                rotationOfRH.Add(temp);

                temp = waist.GetRotation(6) - leftHand.GetRotation(4);
                temp.x = Mathf.Abs(temp.x);
                temp.y = Mathf.Abs(temp.y);
                temp.z = Mathf.Abs(temp.z);
                rotationOfLF.Add(temp);

                temp = waist.GetRotation(6) - leftHand.GetRotation(5);
                temp.x = Mathf.Abs(temp.x);
                temp.y = Mathf.Abs(temp.y);
                temp.z = Mathf.Abs(temp.z);
                rotationOfRF.Add(temp);

            }
            else if (ViveInput.GetTriggerValue(HandRole.LeftHand) == 1 && ViveInput.GetTriggerValue(HandRole.RightHand) == 1 && Time.time - startTime >= 3)
            {
                
                Vector3 avgOfHead = new Vector3(0, 0, 0);
                foreach (Vector3 value in lengthOfHead)
                {
                    avgOfHead += value;
                }
                avgOfHead /= lengthOfHead.Count;

                Vector3 avgOfLH = new Vector3(0, 0, 0);
                foreach (Vector3 value in lengthOfLH)
                {
                    avgOfLH += value;
                }
                avgOfLH /= lengthOfLH.Count;

                Vector3 avgOfRH = new Vector3(0, 0, 0);
                foreach (Vector3 value in lengthOfRH)
                {
                    avgOfRH += value;
                }
                avgOfRH /= lengthOfRH.Count;

                Vector3 avgOfLF = new Vector3(0, 0, 0);
                foreach (Vector3 value in lengthOfLF)
                {
                    avgOfLF += value;
                }
                avgOfLF /= lengthOfLF.Count;

                Vector3 avgOfRF = new Vector3(0, 0, 0);
                foreach (Vector3 value in lengthOfRF)
                {
                    avgOfRF += value;
                }
                avgOfRF /= lengthOfRF.Count;



                //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
                Vector3 avgOfRoHead = new Vector3(0, 0, 0);
                foreach (Vector3 value in rotationOfHead)
                {
                    avgOfRoHead += value;
                }
                avgOfRoHead /= rotationOfHead.Count;

                Vector3 avgOfRoLH = new Vector3(0, 0, 0);
                foreach (Vector3 value in rotationOfLH)
                {
                    avgOfRoLH += value;
                }
                avgOfRoLH /= rotationOfLH.Count;

                Vector3 avgOfRoRH = new Vector3(0, 0, 0);
                foreach (Vector3 value in rotationOfRH)
                {
                    avgOfRoRH += value;
                }
                avgOfRoRH /= rotationOfRH.Count;

                Vector3 avgOfRoLF = new Vector3(0, 0, 0);
                foreach (Vector3 value in rotationOfLF)
                {
                    avgOfRoLF += value;
                }
                avgOfRoLF /= rotationOfLF.Count;

                Vector3 avgOfRoRF = new Vector3(0, 0, 0);
                foreach (Vector3 value in rotationOfRF)
                {
                    avgOfRoRF += value;
                }
                avgOfRoRF /= rotationOfRF.Count;
                //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

                Debug.Log("호성이 머리좌표 : " + avgOfHead + ", 머리 기울기 : " + avgOfRoHead);
                Debug.Log("호성이 왼손좌표 : " + avgOfLH + ", 왼손 기울기 : " + avgOfRoLH);
                Debug.Log("호성이 오른손좌표 : " + avgOfRH + ", 오른손 기울기 : " + avgOfRoRH);
                Debug.Log("호성이 왼발좌표 : " + avgOfLF + ", 왼발 기울기 : " + avgOfRoLF);
                Debug.Log("호성이 오른발좌표 : " + avgOfRF + ", 오른발 기울기 : " + avgOfRoRF);

                string sh, slh, srh, slf, srf;
                sh = "head\t" + avgOfHead.x.ToString("0.00") + "\t" + avgOfHead.y.ToString("0.00") + "\t" + avgOfHead.z.ToString("0.00") + "\t"
    + avgOfRoHead.x.ToString("0.00") + "\t" + avgOfRoHead.y.ToString("0.00") + "\t" + avgOfRoHead.z.ToString("0.00");

                slh = "lh\t" + avgOfLH.x.ToString("0.00") + "\t" + avgOfLH.y.ToString("0.00") + "\t" + avgOfLH.z.ToString("0.00") + "\t"
  + avgOfRoLH.x.ToString("0.00") + "\t" + avgOfRoLH.y.ToString("0.00") + "\t" + avgOfRoLH.z.ToString("0.00");

                srh = "rh\t" + avgOfRH.x.ToString("0.00") + "\t" + avgOfRH.y.ToString("0.00") + "\t" + avgOfRH.z.ToString("0.00") + "\t"
  + avgOfRoRH.x.ToString("0.00") + "\t" + avgOfRoRH.y.ToString("0.00") + "\t" + avgOfRoRH.z.ToString("0.00");

                slf = "lf\t" + avgOfLF.x.ToString("0.00") + "\t" + avgOfLF.y.ToString("0.00") + "\t" + avgOfLF.z.ToString("0.00") + "\t"
  + avgOfRoLF.x.ToString("0.00") + "\t" + avgOfRoLF.y.ToString("0.00") + "\t" + avgOfRoLF.z.ToString("0.00");

                srf = "rf\t" + avgOfRF.x.ToString("0.00") + "\t" + avgOfRF.y.ToString("0.00") + "\t" + avgOfRF.z.ToString("0.00") + "\t"
  + avgOfRoRF.x.ToString("0.00") + "\t" + avgOfRoRF.y.ToString("0.00") + "\t" + avgOfRoRF.z.ToString("0.00");

                if (!Directory.Exists(Application.dataPath + "/Resources/Transforms/RecordData/"))
                    Directory.CreateDirectory(Application.dataPath + "/Resources/Transforms/RecordData/");

                // File.Create(Application.dataPath + "/Resources/Transforms/RecordData/" + fileName + ".txt");

                FileStream fs = new FileStream(Application.dataPath + "/Resources/Transforms/RecordData/record.txt", FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter writer = new StreamWriter(fs, System.Text.Encoding.Unicode);
                writer.WriteLine("Device\tIndex\tPosX\tPosY\tPosZ\tRotX\tRotY\tRotZ\tTime");
                writer.WriteLine(sh);
                writer.WriteLine(slh);
                writer.WriteLine(srh);
                writer.WriteLine(slf);
                writer.WriteLine(srf);
                writer.Close();


                dataManager = DataManager.GetInstance();
                dataManager.SetUserLengthInfo(Mathf.Abs(avgOfHead.y), Pytha(avgOfLH.x, avgOfLH.z), Pytha(avgOfRH.x, avgOfRH.z), Mathf.Abs(avgOfLF.y), Mathf.Abs(avgOfRF.y));
                float[] tempLength = dataManager.GetUserLengthInfo();
                head.SetUserSize(tempLength[0]);
                leftHand.SetUserSize(tempLength[1]);
                rightHand.SetUserSize(tempLength[2]);
                leftFoot.SetUserSize(tempLength[3]);
                rightFoot.SetUserSize(tempLength[4]);

                ExitVRSync();
                isSync = false;
            }
            else
            {
                lengthOfHead.Clear();
                lengthOfLH.Clear();
                lengthOfRH.Clear();
                lengthOfLF.Clear();
                lengthOfRF.Clear();
                startTime = Time.time;
            }
            slider.GetComponent<Slider>().value = (float)(Time.time - startTime);
        }
    }
    public void SavePlayTime()
    {
        dataManager.SetPlayTime(Time.time - playTime);
        playTime = Time.time;
    }

    IEnumerator VibrationController(int num)
    {
        yield return null;
        for(int i = 0;i<num;i++)
        {
            ViveInput.TriggerHapticVibration(HandRole.LeftHand);
            ViveInput.TriggerHapticVibration(HandRole.RightHand);
        }
    }

    IEnumerator PlayCorretness()
    {
        while (playFlag)
        {
            yield return null;
            slider.GetComponent<Slider>().value = (float)GetAverageTime();

            if (GetAverageTime() >= 5)      ////////////////////////////////////////////
            {
                StartCoroutine(VibrationController(30));
                SetNextData();

                clipIndex = GameObject.Find("AnimationManager").GetComponent<AnimationManager>().PlayNext();
                if (clipIndex == -1)
                {
                    
                    slider.SetActive(false);
                    head.SetInfoVIsible(false);
                    leftHand.SetInfoVIsible(false);
                    rightHand.SetInfoVIsible(false);
                    leftFoot.SetInfoVIsible(false);
                    rightFoot.SetInfoVIsible(false);
                    resultPanel.SetActive(true);
                    GameObject.Find("Result/ResultCanvas").GetComponent<ResultPanelScript>().Init();
                    inputErrorData.SetActive(true);
                    GameObject.Find("Result/ResultCanvas/Panel/CharMaker").GetComponent<Testing>().Init();
                    playFlag = false;
                }
                else if(clipIndex == 0)
                {
                   
                    animationIndex = GameObject.Find("AnimationManager").GetComponent<AnimationManager>().GetAnimationIndex();
                    positionList = dataManager.GetPositionData(animationIndex);
                    rotationList = dataManager.GetRotationData(animationIndex);
              
                }
            }
        }
    }
    IEnumerator Calculate(int subItemIndex, VRDevice device)
    {
        while (playFlag || clipIndex!=-1)
        {
            yield return null;
            float valueOfPosition = CalcPosition(clipIndex, device);
            float valueOfRotation = CalcRotation(clipIndex, device);
            float value = (4 * valueOfPosition + valueOfRotation) / 5;
            device.SetSliderValue(value / (threshold * weight));
            switch(device.GetDeviceIndex())
            {
                case 1:
                    Debug.Log("머리 사용자 값 : " + value + ", 기준 값 : " + (threshold * weight) + ", 슬라이더 값 : " + (value / (threshold * weight)) + ", 거리 : " + valueOfPosition +", 기울기 : " + valueOfRotation);
                    break;
                case 2:
                    Debug.Log("왼손 사용자 값 : " + value + ", 기준 값 : " + (threshold * weight) + ", 슬라이더 값 : " + (value / (threshold * weight)) + ", 거리 : " + valueOfPosition + ", 기울기 : " + valueOfRotation);
                    break;
                case 3:
                    Debug.Log("오른손 사용자 값 : " + value + ", 기준 값 : " + (threshold * weight) + ", 슬라이더 값 : " + (value / (threshold * weight)) + ", 거리 : " + valueOfPosition + ", 기울기 : " + valueOfRotation);
                    break;
                case 4:
                    Debug.Log("왼발 사용자 값 : " + value + ", 기준 값 : " + (threshold * weight) + ", 슬라이더 값 : " + (value / (threshold * weight)) + ", 거리 : " + valueOfPosition + ", 기울기 : " + valueOfRotation);
                    break;
                case 5:
                    Debug.Log("오른발 사용자 값 : " + value + ", 기준 값 : " + (threshold * weight) + ", 슬라이더 값 : " + (value / (threshold * weight)) + ", 거리 : " + valueOfPosition + ", 기울기 : " + valueOfRotation);
                    break;
            }

            if (value < threshold * weight)
            {
                device.SetState(0);
                device.SetSphereColor(Color.green);
                device.AddCorrectTime(Time.time - device.GetDeltaTime());
                device.SetDeltaTime(Time.time);
            }
            else if (value < threshold * weight * 2)
            {
                device.SetState(1);
                device.SetSphereColor(Color.yellow);
                device.AddCorrectTime((Time.time - device.GetDeltaTime()) / 2);
                device.AddErrorTime((Time.time - device.GetDeltaTime()) / 2);
                device.SetDeltaTime(Time.time);
            }
            else
            {
                device.SetState(2);
                device.SetSphereColor(Color.red);
                device.AddErrorTime(Time.time - device.GetDeltaTime());
                device.SetDeltaTime(Time.time);
            }
        }
    }

    public float CalcPosition(int index, VRDevice device)
    {
        Debug.Log("신체 배율 : " + device.GetUserSize() / device.GetStandardSize());
        Vector3 distanceOfDevice = (((waist.GetPosition() - device.GetPosition()) * 100) / device.GetStandardSize()) * device.GetUserSize();
        Vector3 distanceOfStandard = positionList[device.GetDeviceIndex() - 1, index] - distanceOfDevice;
        float distanceOfPosition = distanceOfStandard.magnitude;

        return distanceOfPosition;
    }

    public float CalcRotation(int index, VRDevice device)
    {
 
        Vector3 distanceOfDevice = (waist.GetRotation() - device.GetRotation());
        distanceOfDevice.x = Mathf.Abs(distanceOfDevice.x);
        distanceOfDevice.y = Mathf.Abs(distanceOfDevice.y);
        distanceOfDevice.z = Mathf.Abs(distanceOfDevice.z);
        Vector3 distanceOfStandard = rotationList[device.GetDeviceIndex()-1, index];
        Vector3 multipleOfRotation = new Vector3(distanceOfDevice.x * distanceOfStandard.x, distanceOfDevice.y * distanceOfStandard.y, distanceOfDevice.z * distanceOfStandard.z);
        float distanceOfRotation = Mathf.Acos((multipleOfRotation.x + multipleOfRotation.y + multipleOfRotation.z) / (distanceOfDevice.magnitude * distanceOfStandard.magnitude));


        Debug.Log("인덱스 : " + device.GetDeviceIndex() + " 기울기 테스트, 허리 : " + waist.GetRotation() + ", 왼손 : " + device.GetRotation() + ", 두 값의 차 : " + distanceOfDevice + ", 기준값 : " + distanceOfStandard + ", 두 벡터 곱 : " + multipleOfRotation +  ", 사잇각 : " + distanceOfRotation);


        return distanceOfRotation*180;
    }
    public void SetNextData()
    {
        SaveCorrectTime();
        SaveErrorTime();
        SaveTotalTime();
        dataManager.SaveTimeData(errorTimeList[errorTimeList.Count - 1], totalTimeList[totalTimeList.Count - 1]);
    }

    public void SaveCorrectTime()
    {
        double[] correctTimeArray = new double[5];
        correctTimeArray[0] = head.GetCorrectTime();
        correctTimeArray[1] = leftHand.GetCorrectTime();
        correctTimeArray[2] = rightHand.GetCorrectTime();
        correctTimeArray[3] = leftFoot.GetCorrectTime();
        correctTimeArray[4] = rightFoot.GetCorrectTime();
        correctTimeList.Add(correctTimeArray); //서버 연동시 삭제 고려

        head.SetCorrectTime(0);
        leftHand.SetCorrectTime(0);
        rightHand.SetCorrectTime(0);
        leftFoot.SetCorrectTime(0);
        rightFoot.SetCorrectTime(0);

    }
    public void SaveErrorTime()
    {
        double[] errorTimeArray = new double[5];
        errorTimeArray[0] = head.GetErrorTime();
        errorTimeArray[1] = leftHand.GetErrorTime();
        errorTimeArray[2] = rightHand.GetErrorTime();
        errorTimeArray[3] = leftFoot.GetErrorTime();
        errorTimeArray[4] = rightFoot.GetErrorTime();
        errorTimeList.Add(errorTimeArray); //서버 연동시 삭제 고려
        head.SetErrorTime(0);
        leftHand.SetErrorTime(0);
        rightHand.SetErrorTime(0);
        leftFoot.SetErrorTime(0);
        rightFoot.SetErrorTime(0);
    }
    public void SaveTotalTime()
    {
        totalTimeList.Add(Time.time - startTime);
        startTime = Time.time;
    }

    public double GetAverageTime()
    {
        double netCorrectime = (head.GetCorrectTime() + leftHand.GetCorrectTime() + rightHand.GetCorrectTime() + leftFoot.GetCorrectTime() + rightFoot.GetCorrectTime()) / 5;
        double netErrorTime = (head.GetErrorTime() + leftHand.GetErrorTime() + rightHand.GetErrorTime() + leftFoot.GetErrorTime() + rightFoot.GetErrorTime()) / 20;
        return netCorrectime + netErrorTime;
        //return (head.GetCorrectTime() + leftHand.GetCorrectTime() + rightHand.GetCorrectTime() + leftFoot.GetCorrectTime() + rightFoot.GetCorrectTime()) / 5;
    }


    public void setLazer()
    {
        lazer.SetActive(!lazer.active);
    }

    public void VRSynchronise()
    {

        lengthOfHead = new List<Vector3>();
        lengthOfLH = new List<Vector3>();
        lengthOfRH = new List<Vector3>();
        lengthOfLF = new List<Vector3>();
        lengthOfRF = new List<Vector3>();

        rotationOfHead = new List<Vector3>();
        rotationOfLH = new List<Vector3>();
        rotationOfRH = new List<Vector3>();
        rotationOfLF = new List<Vector3>();
        rotationOfRF = new List<Vector3>();

        GameObject.Find("AnimationManager").GetComponent<AnimationManager>().Sync(true);
        slider.SetActive(true);
        slider.GetComponent<Slider>().maxValue = 3;
        slider.GetComponent<Slider>().value = 0;
        isSync = true;
        StartCoroutine(RecordSize());
    }

    public void ExitVRSync()
    {
        slider.SetActive(false);
        GameObject.Find("AnimationManager").GetComponent<AnimationManager>().Sync(false);

        GameObject.Find("Helps").GetComponent<Helps>().ShowBodyRecordCanvas(head.GetUserSize() + ((leftFoot.GetUserSize() + rightFoot.GetUserSize()) / 2) + 15, (leftHand.GetUserSize() + rightHand.GetUserSize())/2, (leftFoot.GetUserSize()+rightFoot.GetUserSize())/2 - 10);
    }
}
