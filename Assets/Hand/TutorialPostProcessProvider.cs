using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;
//using UnityEngine.UI;
using TMPro;

public class TutorialPostProcessProvider : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI cardNameText;
    [SerializeField]
    private TextMeshProUGUI MinText;
    [SerializeField]
    private TextMeshProUGUI MaxText;

    public LeapProvider leapProvider;

    private void OnEnable()
    {
        leapProvider.OnUpdateFrame += OnUpdateFrame;
    }
    private void OnDisable()
    {
        leapProvider.OnUpdateFrame -= OnUpdateFrame;
    }

    void OnUpdateFrame(Frame frame)
    {
        //cardNameText.text = LeftOrRight();
    }


    //Button Setting
    string MinrightID = "";
    string MinleftID = "";
    string MinID = "";
    List<List<float>> MinrightAngles;
    List<List<float>> MinleftAngles;
    List<List<float>> MinAngles;

    string MaxrightID = "";
    string MaxleftID = "";
    string MaxID = "";
    List<List<float>> MaxrightAngles;
    List<List<float>> MaxleftAngles;
    List<List<float>> MaxAngles;

    string lr = "";

    string FlexionOrExtension = "";
    string Min = "";
    string Max = "";

    List<List<float>> rightROM;
    List<List<float>> leftROM;
    List<List<float>> ROM;

    public void Flexion()
    {
        FlexionOrExtension = "flexion";

        List<string> IDList = LeftOrRight().IDList;
        List<string> LRList = LeftOrRight().LRList;
        List<List<List<float>>> angleList = LeftOrRight().angleList;

        if (IDList.Count == 2)
        {
            MinrightID = IDList[0];
            MinleftID = IDList[1];
            MinrightAngles = angleList[0];
            MinleftAngles = angleList[1];

            Min = "RIGHT ID:" + MinrightID + "Angles:" + MinrightAngles[0][0] + "\n"
            + "LEFT ID:" + MinleftID + "Angles:" + MinleftAngles[0][0] + "\n";
        }
        else
        {
            MinID = IDList[0];
            lr = LRList[0];
            MinAngles = angleList[0];

            Min = lr + "ID:" + MinID + "Angles:" + MinAngles[0][0] + "\n";
        }
        MinText.text = "Min:" + Min;
    }

    public void Save()
    {
        List<string> IDList = LeftOrRight().IDList;
        List<string> LRList = LeftOrRight().LRList;
        List<List<List<float>>> angleList = LeftOrRight().angleList;

        if (IDList.Count == 2)
        {
            MaxrightID = IDList[0];
            MaxleftID = IDList[1];
            MaxrightAngles = angleList[0];
            MaxleftAngles = angleList[1];

            rightROM = CalculateROM(MaxrightAngles, MinrightAngles);
            leftROM = CalculateROM(MaxleftAngles, MinleftAngles);

            Max = "RIGHT ID:" + MaxrightID + "Angles:" + MaxrightAngles[0][0] + "ROM:" + rightROM[0][0] + "\n"
            + "LEFT ID:" + MaxleftID + "Angles:" + MaxleftAngles[0][0] + "ROM:" + leftROM[0][0] + "\n";
        }
        else
        {
            MaxID = IDList[0];
            lr = LRList[0];
            MaxAngles = angleList[0];

            ROM = CalculateROM(MaxAngles, MinAngles);

            Max = lr + "ID:" + MaxID + "Angles:" + MaxAngles[0][0] + "ROM:" + ROM[0][0] + "\n";
        }
        MaxText.text = "Min:" + Min + "Max:" + Max;
    }

    List<List<float>>  CalculateROM(List<List<float>> MaxAngles, List<List<float>> MinAngles)
    {
        List<List<float>> ROM = new List<List<float>>();
        for (int i = 0; i < MaxAngles.Count; i++)
        {
            List<float> row = new List<float>();
            for (int j = 0; j < MaxAngles[0].Count; j++)
            {
                row.Add(0.0f);
            }
            ROM.Add(row);
        }
        
        for (int i = 0; i < MaxAngles.Count; i++)
        {
            for (int j = 0; j < MaxAngles[0].Count; j++)
            {
                ROM[i][j] = MaxAngles[i][j] - MinAngles[i][j];
            }
        }
        return ROM;
    }

    //Recognize Left or Right
    (List<string> IDList, List<string> LRList, List<List<List<float>>> angleList) LeftOrRight()
    {
        string info;
        Hand _rightHand = Hands.Provider.GetHand(Chirality.Right);
        Hand _leftHand = Hands.Provider.GetHand(Chirality.Left);

        List<string> _IDList = new List<string>();
        List<string> _LRList = new List<string>();
        List<List<List<float>>> _angleList = new List<List<List<float>>>();

        if (_rightHand != null && _leftHand != null)
        {
            _IDList.Add(_rightHand.Id.ToString());
            _IDList.Add(_leftHand.Id.ToString());

            _LRList.Add("Right");
            _LRList.Add("Left");

            _angleList.Add(GetAngle(_rightHand));
            _angleList.Add(ChangeToMinus(GetAngle(_leftHand)));

            return (_IDList, _LRList, _angleList);
        }

        if (_rightHand != null && _leftHand == null)
        {
            _IDList.Add(_rightHand.Id.ToString());
            _LRList.Add("Right");
            _angleList.Add(GetAngle(_rightHand));
            return (_IDList, _LRList, _angleList);
        }

        if (_rightHand == null && _leftHand != null)
        {
            _IDList.Add(_leftHand.Id.ToString());
            _LRList.Add("Left");
            _angleList.Add(ChangeToMinus(GetAngle(_leftHand)));
            return (_IDList, _LRList, _angleList);
        }

        else//if (_rightHand == null && _leftHand == null)
        {
            return (_IDList, _LRList, _angleList);
        }
    }

    //Get Angles
    List<List<float>> GetAngle(Hand _hand)
    {
        List<List<float>> angleList = new List<List<float>>();

        foreach (Finger finger in _hand.Fingers)
        {
            List<float> angles = new List<float>();
            if (finger.Type == 0)//_hand.Finger(finger.Id).FingerType.TYPE_THUMB)
            {
                Finger _thumb = _hand.Fingers[0];
                Finger _index = _hand.Fingers[1];
                angles.Add(Vector3.SignedAngle(_index.bones[0].Direction, _thumb.bones[1].Direction, _hand.PalmNormal));
                angles.Add(Vector3.SignedAngle(_index.bones[0].Direction, _thumb.bones[1].Direction, Vector3.Cross(_hand.Direction, _hand.PalmNormal)));
                for (int i = 1; i < 3; i++)
                {
                    angles.Add(Vector3.SignedAngle(finger.bones[i].Direction, finger.bones[i + 1].Direction, Vector3.Cross(_hand.Direction, _hand.PalmNormal)));
                }
            }
            else
            {
                for (int i = 0; i < 3; i++)
                {
                    angles.Add(Vector3.SignedAngle(finger.bones[i].Direction, finger.bones[i + 1].Direction, Vector3.Cross(_hand.Direction, _hand.PalmNormal)));
                }
                angles.Add(0.0f);
            }
            angleList.Add(angles); //the size of this list is [5][4]
        }
        return angleList;
    }

    //change left angles to minus
    List<List<float>> ChangeToMinus(List<List<float>> angleList)
    {
        for (int i = 0; i < angleList.Count; i++)
        {
            for (int j = 0; j < angleList[0].Count; j++)
            {
                angleList[i][j] = angleList[i][j] * (-1);
            }
        }
        return angleList;
    }
}