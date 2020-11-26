using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Xml;
using System.IO;
using HTC.UnityPlugin.Vive;

enum body { head =1 , leftHand, rightHand, leftFoot, rightFoot, waist}
//머리, 왼손, 오른손, 왼발, 오른발, 허리
class CorXml
{
    static CorXml corXml;

    string[] formSet;
    Vector3[,] positions;
    int j, maxDevice;
    int partionNum;

    private CorXml() { }
    public static CorXml getInstance()
    {
        if(corXml == null)
        {
            corXml = new CorXml();
        }
        return corXml;
    }
    public void loadFile(string fileName)
    {
        string headForms = File.ReadAllText(Application.dataPath + "/Resources/Transforms/" + fileName);
        formSet = headForms.Split('\n');

        j = 0;
        maxDevice = 5;

        positions = new Vector3[maxDevice, (formSet.Length-1)/ maxDevice];

        partionNum = (formSet.Length - 1) / maxDevice;
        int k = 0;
        for (int i=1;i<formSet.Length;i++)
        {
            string[] text = formSet[i].Split('\t');

            positions[j, k].Set(float.Parse(text[1]), float.Parse(text[2]), float.Parse(text[3]));
            k++;
            if (i % partionNum == 0)
            {
                k = 0;
                j++;
            } 
        }
    }

    public int getPartNum()
    {
        return partionNum;
    }

    public Vector3[] getPos(int body)
    {

        Vector3[] temp = new Vector3[positions.GetLength(1)];
        for (int i = 0; i < positions.GetLength(1); i++)
        {
            temp[i] = positions[body-1, i];
        }
        return temp;
    }

    bool LoadXml(string filePath)
    {
        TextAsset textAsset = (TextAsset)Resources.Load(filePath, typeof(TextAsset));
        if (textAsset == null)
            return false;
        XmlDocument xml = new XmlDocument();
        xml.LoadXml(textAsset.text);

        return true;
    }

    void CreateXml(string filePath)
    {
        XmlDocument xml = new XmlDocument();
        XmlDeclaration xmlDeclaration = xml.CreateXmlDeclaration("1.0", "UTF-8", null);
        xml.AppendChild(xmlDeclaration);
        XmlElement root = xml.CreateElement("Transforms");
        xml.AppendChild(root);
        XmlElement item = (XmlElement)root.AppendChild(xml.CreateElement("field"));
        item.SetAttribute("Head", "head");
        item.SetAttribute("Waist", "waist");
        XmlElement item2 = (XmlElement)root.AppendChild(xml.CreateElement("field3"));
        item2.SetAttribute("Head", "head");
        item2.SetAttribute("Waist", "waist");

        xml.Save(filePath);
    }
}

[System.Serializable]
//VR 기기 오차율 계산
public class VRDev
{
    public Material mat;
    public GameObject sphere;
    public TextMesh mText, timeText;

    Vector3[] position;
    float startTime;
    public float netTime, errorTime;
    float th1, th2;
    Vector3 distance;

    CorXml mCorXml;
    List<string> dataSet;
    int mNum;

    Vector3 gPosOff;
    Quaternion gRotOff;
    string mPosX, mPosY, mPosZ, mRotX, mRotY, mRotZ, mRotW;

    public void init(int body)//인덱스로 기기와 좌표값 매핑
    {

        dataSet = new List<string>();
        mNum = body;
       

        startTime = 0;
        netTime = 0;
        errorTime = Time.time;
        th1 = 250;
        th2 = 500;
        //sphere.GetComponent<MeshRenderer>().enabled = true; //교정 오브젝트 렌더링
        mat.color = Color.white;
    }

    bool calcCor(int state)
    {
        mCorXml = CorXml.getInstance();
        position = mCorXml.getPos(mNum);

        Vector3 dis = position[state] - distance;
       // Debug.Log(mNum);
        float value = dis.sqrMagnitude;
        //기울기 분석 추가

        //가우시안 필터 적용 추가

        mText.text = value.ToString();
        timeText.text = netTime.ToString();

        if (value < th1)
        {
            mat.color = Color.green;
            errorTime = Time.time;
            return true;
        } else if(value < th2)
        {
            mat.color = Color.yellow;
            netTime = netTime + ((Time.time - errorTime) / 2);
            errorTime = Time.time;
            return false;
        }
        mat.color = Color.red;
        netTime = netTime + (Time.time - errorTime);
        errorTime = Time.time;
        return false;
    }
    public bool checkCor(int state)
    {
        Vector3 mPosition = getPosition(mNum);
        Quaternion mRotation = getRotation(mNum);
        gPosOff = getPosition(6);
        gRotOff = getRotation(6);
        distance = (gPosOff - mPosition)*100;

        //오차율 계산
        if (calcCor(state))
        {
            if (startTime == 0)
            {
                startTime = Time.time;
            }
            if (Time.time - startTime > 2)
            {
                return true;
            }
            return false;
        }
        startTime = Time.time;
        return false;
    }
    public float getErrorTime()
    {
        return netTime;
    }
    Vector3 getPosition(int num)
    {
        Vector3 mPos;
        switch (num)
        {
            case 1:
                mPos = VivePose.GetPoseEx(DeviceRole.Hmd).pos;
                break;
            case 2:
                mPos = VivePose.GetPoseEx(HandRole.LeftHand).pos;
                break;
            case 3:
                mPos = VivePose.GetPoseEx(HandRole.RightHand).pos;
                break;
            case 4:
                mPos = VivePose.GetPoseEx(TrackerRole.Tracker1).pos;
                break;
            case 5:
                mPos = VivePose.GetPoseEx(TrackerRole.Tracker2).pos;
                break;
            case 6:
                mPos = VivePose.GetPoseEx(TrackerRole.Tracker3).pos;
                break;
            default:
                mPos = new Vector3(0, 0, 0);
                break;

        }
        return mPos;
    }

    Quaternion getRotation(int num)
    {
        Quaternion mRot;
        switch (num)
        {
            case 1:
                mRot = VivePose.GetPoseEx(DeviceRole.Hmd).rot;
                break;
            case 2:
                mRot = VivePose.GetPoseEx(HandRole.LeftHand).rot;
                break;
            case 3:
                mRot = VivePose.GetPoseEx(HandRole.RightHand).rot;
                break;
            case 4:
                mRot = VivePose.GetPoseEx(TrackerRole.Tracker1).rot;
                break;
            case 5:
                mRot = VivePose.GetPoseEx(TrackerRole.Tracker2).rot;
                break;
            case 6:
                mRot = VivePose.GetPoseEx(TrackerRole.Tracker3).rot;
                break;
            default:
                mRot = new Quaternion(0, 0, 0, 0);
                break;

        }
        return mRot;
    }

    public List<string> getDataSet()
    {
        return dataSet;
    }
    public void saveData(int state)
    {
        Vector3 mPosition = getPosition(mNum);
        Quaternion mRotation = getRotation(mNum);
        gPosOff = getPosition(6);
        gRotOff = getRotation(6);

        mPosX = ((gPosOff.x - mPosition.x) * 100).ToString();
        mPosY = ((gPosOff.y - mPosition.y) * 100).ToString();
        mPosZ = ((gPosOff.z - mPosition.z) * 100).ToString();
        mRotX = ((gRotOff.x - mRotation.x) * 100).ToString();
        mRotY = ((gRotOff.y - mRotation.y) * 100).ToString();
        mRotZ = ((gRotOff.z - mRotation.z) * 100).ToString();
        mRotW = ((gRotOff.w - mRotation.w) * 100).ToString();

        string item = mNum.ToString() + "\t" + mPosX + "\t" + mPosY + "\t" + mPosZ + "\t" + mRotX + "\t" + mRotY + "\t" + mRotZ + "\t" + state.ToString();
        dataSet.Add(item);
    }
}

public class userXML : MonoBehaviour
{
    string fileName = "UserTransforms.xml";
    public GameObject debugText;
    public TextMesh wholeTime;
    Vector3 gPosOff;
    Quaternion gRotOff;
    string[] formSet;
    public int state;
    CorXml corXml;

    List<string> mDataSet;
    public int deviceNum;
    int tempNum;
    float startTime;
    public float[] errorTimes;
    float testTime;
    public int count;

    bool h;
    public VRDev head;
    public VRDev lh, rh, lf, rf, waist;
    public int stateNum;


    void Start()
    {
        mDataSet = new List<string>();
        corXml = CorXml.getInstance();
        
        //corXml.loadFile("test.txt");
        errorTimes = new float[5];

        head.init((int)body.head);
        lh.init((int)body.leftHand);
        rh.init((int)body.rightHand);
        lf.init((int)body.leftFoot);
        rf.init((int)body.rightFoot);


    }
    public void init(string str)
    {
        h = false;
        corXml.loadFile(str + ".txt");
        //mDataSet = new List<string>();
        deviceNum = 1;
        tempNum = deviceNum;
        count = 0;

        startTime = Time.time;
        state = 0;

        head.errorTime = Time.time;
        lh.errorTime = Time.time;
        rh.errorTime = Time.time;
        lf.errorTime = Time.time;
        rf.errorTime = Time.time;

        head.netTime = 0;
        lh.netTime = 0;
        rh.netTime = 0;
        lf.netTime = 0;
        rf.netTime = 0;


        //waist.init((int)body.waist);

        testTime = Time.time;
    }

    private void Update()
    {
        head.saveData(stateNum);
        lh.saveData(stateNum);
        rh.saveData(stateNum);
        lf.saveData(stateNum);
        rf.saveData(stateNum);

        bool mIsPlay = GameObject.Find("MenuCanvas/ButtonScript").GetComponent<ButtonScript>().isPlay;
        if (mIsPlay)
            StartCoroutine("check");
        else
        {
            Debug.Log("머리 에러시간 : " + head.getErrorTime());
            Debug.Log("전체 에러시간 : " + (Time.time - startTime));

            errorTimes[0] = (head.getErrorTime() / (Time.time - startTime)) * 100;
            errorTimes[1] = (lh.getErrorTime() / (Time.time - startTime)) * 100;
            errorTimes[2] = (rh.getErrorTime() / (Time.time - startTime)) * 100;
            errorTimes[3] = (lf.getErrorTime() / (Time.time - startTime)) * 100;
            errorTimes[4] = (rf.getErrorTime() / (Time.time - startTime)) * 100;
            Debug.Log("에러시간1 : " + errorTimes[0]);
            Debug.Log("에러시간2 : " + errorTimes[1]);
            Debug.Log("에러시간3 : " + errorTimes[2]);
            Debug.Log("에러시간4 : " + errorTimes[3]);
            Debug.Log("에러시간5 : " + errorTimes[4]);
        }

        //wholeTime.text = (Time.time - startTime).ToString();
        //bool b1 = head.checkCor(state);
        //bool b2 = lh.checkCor(state);
        //bool b3 = rh.checkCor(state);
        //bool b4 = lf.checkCor(state);
        //bool b5 = rf.checkCor(state);
        ////h = b1 && b2 && b3 && b4 && b5;
        //// h = head.checkCor(state) && lh.checkCor(state) && rh.checkCor(state) && lf.checkCor(state) && rf.checkCor(state);


        //if(Time.time-testTime > 3)
        //{
        //    h = true;
        //    testTime = Time.time;
        //} else
        //{
        //    h = false;
        //}
        //Debug.Log(h);

        //if (h && mIsPlay)
        //{
        //    state++;
        //    if (state >= corXml.getPartNum())
        //        state = 0;
        //    debugText.GetComponent<TextMesh>().text = state.ToString();
        //}

        //head.saveData(stateNum);
        //lh.saveData(stateNum);
        //rh.saveData(stateNum);
        //lf.saveData(stateNum);
        //rf.saveData(stateNum);
    }

    IEnumerator check() {
        yield return null;
        wholeTime.text = (Time.time - startTime).ToString();
        bool b1 = head.checkCor(state);
        bool b2 = lh.checkCor(state);
        bool b3 = rh.checkCor(state);
        bool b4 = lf.checkCor(state);
        bool b5 = rf.checkCor(state);
        h = b1 && b2 && b3 && b4 && b5;
        // h = head.checkCor(state) && lh.checkCor(state) && rh.checkCor(state) && lf.checkCor(state) && rf.checkCor(state);


        //if (Time.time - testTime > 3)
        //{
        //    h = true;
        //    testTime = Time.time;
        //}
        //else
        //{
        //    h = false;
        //}

        if (h)
        {
            state++;
            if (state >= corXml.getPartNum())
            {
                count++;
                state = 0;

            }
            debugText.GetComponent<TextMesh>().text = state.ToString();
        }

     

        //head.saveData(stateNum);
        //lh.saveData(stateNum);
        //rh.saveData(stateNum);
        //lf.saveData(stateNum);
        //rf.saveData(stateNum);

       
    }

    void OnDisable()
    {
        StreamWriter writer;
        string path = "C:/Users/han/Desktop/졸업작품/20.05.11/dataSet/5_11_text.txt";
        writer = File.CreateText(path);

        for (int i = 1; i < 6; i++)
        {
            switch(i)
            {
                case 1:
                    mDataSet = head.getDataSet();
                    break;
                case 2:
                    mDataSet = lh.getDataSet();
                    break;
                case 3:
                    mDataSet = rh.getDataSet();
                    break;
                case 4:
                    mDataSet = lf.getDataSet();
                    break;
                case 5:
                    mDataSet = rf.getDataSet();
                    break;
            }

            foreach (string temp in mDataSet)
            {
                writer.Write(temp);
                writer.Write("\n");
            }
            
        }
        writer.Close();

       
    }

    public int getState()
    {
        return state;
    }
}
