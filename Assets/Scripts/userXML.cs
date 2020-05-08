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
    string[] formSet;
    Vector3[] positions;
    //Vector3[,] positions2;
    int j=0;

    public void init()
    {
        string headForms = File.ReadAllText(Application.dataPath + "/Resources/Transforms/Spine2.txt");
        formSet = headForms.Split('\n');

        positions = new Vector3[formSet.Length];
        //positions2 = new Vector3[6, (formSet.Length-1)/6];

        for(int i=1;i<formSet.Length;i++)
        {
            Debug.Log(formSet[i]);
            string[] text = formSet[i].Split('\t');

            positions[i - 1].Set(float.Parse(text[1]), float.Parse(text[2]), float.Parse(text[3]));
            Debug.Log(positions[i - 1]);

            //int k = (i - 1) - (j * 6);
            //positions2[j, k].Set(float.Parse(text[1]), float.Parse(text[2]), float.Parse(text[3]));
            //if(i - (j*6) >= (formSet.Length-1)/6)  
            //    j++;
           
        }
    }

    public Vector3[] getPos()
    {
        return positions;
    }

    //public Vector3[] getPos2(int body)
    //{
    //    Vector3[] temp = new Vector3[positions2.GetLength(body)];
    //    for (int i = 0; i < positions2.GetLength(body); i++)
    //    {
    //        temp[i] = positions2[body, i];
    //    }
    //    return temp;
    //}
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
        //debugText.GetComponent<TextMesh>().text = "3";
    }
}

[System.Serializable]
//VR 기기 오차율 계산
public class VRDev
{
    public Material mat;
    public GameObject sphere;

    Vector3[] positions2;
    Vector3[] position;
    float startTime;
    float th1, th2;
    Vector3 uPosOff, distance;
    Quaternion uRotOff;

    CorXml corXml;
    int mNum;


    List<string> dataSet;
    int dataIndex;
    string hPosX, hPosY, hPosZ, hRotX, hRotY, hRotZ, hRotW;
    Vector3 gPosOff;
    Quaternion gRotOff;

    public void init(int body)//인덱스로 기기와 좌표값 매핑
    {
        dataSet = new List<string>();
        mNum = body;
        corXml = new CorXml();
        corXml.init();
        positions2 = corXml.getPos();
        //position = corXml.getPos2(body);
        Debug.Log(positions2);

        startTime = 0;
        th1 = 16;
        th2 = 100;
        //sphere.GetComponent<MeshRenderer>().enabled = true; //교정 오브젝트 렌더링
        mat.color = Color.white;

     
    }

    bool calcCor(int state)
    {
        Vector3 dis = positions2[state] - distance;//기준 좌표와 사용자 좌표간 차이
        //Vector3 dis = position[state] - distance;
        float value = dis.sqrMagnitude;
       // Debug.Log(dis+ ", " +value);


        //value = ((positions[state, 0] - distance.x)* (positions[state, 0] - distance.x))
        //    + ((positions[state, 1] - distance.y) * (positions[state, 1] - distance.y))
        //    + ((positions[state, 2] - distance.z) * (positions[state, 2] - distance.z));
        //Debug.Log(value);
        //기울기 분석 추가

        //가우시안 필터 적용 추가
        if (value < th1)
        {
            mat.color = Color.green;
            return true;
        } else if(value < th2)
        {
            mat.color = Color.yellow;
            return false;
        }
        mat.color = Color.red;
        return false;
        //횟수 및 누적 오차 시간 추가
    }
    public bool checkCor(int state)
    {
        //Vector3 head_position = VivePose.GetPoseEx(DeviceRole.Hmd).pos;
        Vector3 mPosition = getPosition(mNum);
        //Hmd : -1, leftHand : 1, rightHand : 2, tracker1 : 3, tracker2 : 4, tracker3 : 5
        Quaternion head_rotation = VivePose.GetPoseEx(DeviceRole.Hmd).rot;
        gPosOff = getPosition(6);
        gRotOff = getRotation(6);

        //if (uPosOff.x == 0)
        //    gPosOff.x - mPosition.x;
        //    //uPosOff.x = head_position.x;

        //if (uPosOff.y == 0)
        //    uPosOff.y = head_position.y;
        ////uPosOff.x = mPosition.y - waist_position.y
        //if (uPosOff.z == 0)
        //    uPosOff.z = head_position.z;
        ////uPosOff.x = mPosition.z - waist_position.z
        //if (uRotOff.x == 0)
        //    uRotOff.x = head_rotation.x;
        //if (uRotOff.y == 0)
        //    uRotOff.y = head_rotation.y;
        //if (uRotOff.z == 0)
        //    uRotOff.z = head_rotation.z;

        //distance = (uPosOff - head_position)*100; //시작 좌표와 움직인 좌표 차이
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

        // Vector3 head_position = VivePose.GetPoseEx(0).pos;
        // Quaternion head_rotation = VivePose.GetPoseEx(0).rot;
        hPosX = ((gPosOff.x - mPosition.x) * 100).ToString();
        hPosY = ((gPosOff.y - mPosition.y) * 100).ToString();
        hPosZ = ((gPosOff.z - mPosition.z) * 100).ToString();
        hRotX = ((gRotOff.x - mRotation.x) * 100).ToString();
        hRotY = ((gRotOff.y - mRotation.y) * 100).ToString();
        hRotZ = ((gRotOff.z - mRotation.z) * 100).ToString();
        hRotW = ((gRotOff.w - mRotation.w) * 100).ToString();

        string item = mNum.ToString() + "\t" + hPosX + "\t" + hPosY + "\t" + hPosZ + "\t" + hRotX + "\t" + hRotY + "\t" + hRotZ + "\t" + state.ToString();
        dataSet.Add(item);
    }
}

public class userXML : MonoBehaviour
{
    string fileName = "UserTransforms.xml";
    public GameObject debugText, timeText, posX, posY, posZ, rotX, rotY, rotZ;
    public TextMesh hPosX, hPosY, hPosZ, hRotX, hRotY, hRotZ, hRotW;
    Vector3 gPosOff;
    Quaternion gRotOff;
    string[] formSet;
    public int state;

    List<string> mDataSet;
    public int deviceNum;
    int tempNum;


    bool h;
    public VRDev head;
    public VRDev lh, rh, lf, rf, waist;
    public int stateNum;

    // Start is called before the first frame update
    void Awake()
    {
        //text.GetComponent<TextMesh>().text = "0";
        //if (!LoadXml(Application.dataPath + "/XML/" + fileName))
        //{
        //    text.GetComponent<TextMesh>().text = "1";
        //    CreateXml(Application.dataPath + "/Resources/XML/" + fileName);
        //} else
        //{
        //    text.GetComponent<TextMesh>().text = "2";
        //}



        //Load
        //TextAsset textXml = (TextAsset)Resources.Load(fileName, typeof(TextAsset));
        //XmlDocument xml = new XmlDocument();
        //xml.LoadXml(textXml.text);

        //Read
        //XmlNode root = xml.FirstChild;
        //foreach(XmlNode node in root.ChildNodes)
       // {
        //    if (node.FirstChild.NodeType == XmlNodeType.Text)
        //        Debug.Log(node.InnerText);
       // }

        //Save
        //xml.Save(AssetDatabase.GetAssetPath(textXml));
    }
    
    Vector3 getPosition(int num)
    {
        Vector3 mPos;
        switch(num)
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


    void Start()
    {
        mDataSet = new List<string>();
        deviceNum = 1;
        tempNum = deviceNum;

        state = 0;
        head.init((int)body.head);
        lh.init((int)body.leftHand);
        rh.init((int)body.rightHand);
        lf.init((int)body.leftFoot);
        rf.init((int)body.rightFoot);
        //waist.init((int)body.waist);

    }

    private void Update()
    {

        h = head.checkCor(state) && lh.checkCor(state);

        if(h)
        {
            state++;
            if (state >= 5)
                state = 0;
            debugText.GetComponent<TextMesh>().text = state.ToString();
        }
  
        //Vector3 head_position = VivePose.GetPoseEx(DeviceRole.Hmd).pos;
        //Quaternion head_rotation = VivePose.GetPoseEx(DeviceRole.Hmd).rot;
        Vector3 mPosition = getPosition(deviceNum);
        Quaternion mRotation = getRotation(deviceNum);
        gPosOff = getPosition(6);
        gRotOff = getRotation(6);

        // Vector3 head_position = VivePose.GetPoseEx(0).pos;
        // Quaternion head_rotation = VivePose.GetPoseEx(0).rot;
        hPosX.text = ((gPosOff.x - mPosition.x)*100).ToString();
        hPosY.text = ((gPosOff.y - mPosition.y)*100).ToString();
        hPosZ.text = ((gPosOff.z - mPosition.z)*100).ToString();
        hRotX.text = ((gRotOff.x - mRotation.x)*100).ToString();
        hRotY.text = ((gRotOff.y - mRotation.y)*100).ToString();
        hRotZ.text = ((gRotOff.z - mRotation.z)*100).ToString();
        hRotW.text = ((gRotOff.w - mRotation.w)*100).ToString();

        head.saveData(stateNum);
        lh.saveData(stateNum);
        rh.saveData(stateNum);
        lf.saveData(stateNum);
        rf.saveData(stateNum);

    }
    //Update is called once per frame
    void LateUpdate()
    {

    }

    void OnDisable()
    {
        StreamWriter writer;
        string path = "C:/Users/han/Desktop/dataSet/test.txt";
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
      
            Debug.Log("file start, " + mDataSet.Count);
            foreach (string temp in mDataSet)
            {
                writer.Write(temp);
                writer.Write("\n");
                Debug.Log("file input : " + temp);
            }
            
        }
        writer.Close();

        Debug.Log("finish");
    }

    public int getState()
    {
        return state;
    }
}
