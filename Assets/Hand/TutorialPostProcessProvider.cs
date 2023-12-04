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
    [SerializeField]
    private TextMeshProUGUI ROMText;

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
        int ID;
        string LR;
        List<List<float>> Angles;
        string info;

        List<int> IDList = LeftOrRight().IDList;
        List<string> LRList = LeftOrRight().LRList;
        List<List<List<float>>> angleList = LeftOrRight().angleList;

        if (IDList.Count == 2)
        {
            if (IDList[0] < IDList[1])
            {
                ID = IDList[0];
                LR = LRList[0];
                Angles = angleList[0];

                info = "<" + LR + "> ID:" + ID + "\nAngles:\n" + LineUpAngles(Angles) + "\n";
            }
            else
            {
                ID = IDList[1];
                LR = LRList[1];
                Angles = angleList[1];

                info = "<" + LR + "> ID:" + ID + "\nAngles:\n" + LineUpAngles(Angles) + "\n";
            }
        }
        if(IDList.Count == 1)
        {
            ID = IDList[0];
            LR = LRList[0];
            Angles = angleList[0];
           
            info = "<" + LR + "> ID:" + ID + "\nAngles:\n" + LineUpAngles(Angles) + "\n";
        }
        else
        {
            info = "NO HAND";
        }
        cardNameText.text = info;
    }


    //Button Setting
    int MinID;
    int MaxID;
    string lr;
    List<List<float>> MinAngles;
    List<List<float>> MaxAngles;
    string Min;
    string Max;
    string FlexionOrExtension = "flexion";
    List<List<float>> ROM;

    //Flexion Button
    public void Flexion()
    {
        FlexionOrExtension = "flexion";

        List<int> IDList = LeftOrRight().IDList;
        List<string> LRList = LeftOrRight().LRList;
        List<List<List<float>>> angleList = LeftOrRight().angleList;

        if (IDList.Count == 2)
        {
            if(IDList[0] < IDList[1])
            {
                MinID = IDList[0];
                lr = LRList[0];
                MinAngles = angleList[0];

                Min = "<" + lr + "> ID:" + MinID + "\nAngles:\n" + LineUpAngles(MinAngles) + "\n";
            }
            else
            {
                MinID = IDList[1];
                lr = LRList[1];
                MinAngles = angleList[1];

                Min = "<" + lr + "> ID:" + MinID + "\nAngles:\n" + LineUpAngles(MinAngles) + "\n";
            }
        }
        if (IDList.Count == 1)
        {
            MinID = IDList[0];
            lr = LRList[0];
            MinAngles = angleList[0];

            Min = "<" + lr + "> ID:" + MinID + "\nAngles:\n" + LineUpAngles(MinAngles) + "\n";
        }
        else
        {
            Min = "NO HAND";
        }
        MinText.text = "Min:" + Min;
    }

    //Save Button
    public void Save()
    {
        string ROMValue;
        List<int> IDList = LeftOrRight().IDList;
        List<string> LRList = LeftOrRight().LRList;
        List<List<List<float>>> angleList = LeftOrRight().angleList;

        if (IDList.Count == 2)
        {
            if (IDList[0] < IDList[1])
            {
                MaxID = IDList[0];
                lr = LRList[0];
                MaxAngles = angleList[0];

                Max = "<" + lr + "> ID:" + MaxID + "\nAngles:\n" + LineUpAngles(MaxAngles) + "\n";
            }
            else
            {
                MaxID = IDList[1];
                lr = LRList[1];
                MaxAngles = angleList[1];

                Max = "<" + lr + "> ID:" + MaxID + "\nAngles:\n" + LineUpAngles(MaxAngles) + "\n";
            }

            ROM = CalculateROM(MaxAngles, MinAngles);

            Max = "<" + lr + "> ID:" + MaxID + "\nAngles:\n" + LineUpAngles(MaxAngles) + "\n";
            ROMValue = "<" + lr + "> ID:" + MaxID + "\nAngles:\n" + LineUpAngles(ROM) + "\n";
        }
        if (IDList.Count == 1)
        {
            MaxID = IDList[0];
            lr = LRList[0];
            MaxAngles = angleList[0];

            ROM = CalculateROM(MaxAngles, MinAngles);

            Max = "<" + lr + "> ID:" + MaxID + "\nAngles:\n" + LineUpAngles(MaxAngles) + "\n";
            ROMValue = "<" + lr + "> ID:" + MaxID + "\nAngles:\n" + LineUpAngles(ROM) + "\n";
        }
        else
        {
            Max = "NO HAND";
            ROMValue = "NO ROM";
        }
        MaxText.text = "Max:" + Max;
        ROMText.text = "ROM:" + ROMValue;
    }

    //Line up angles as string
    string LineUpAngles(List<List<float>> Angles)
    {
        string AngleText = "";
        List<string> FourFingersName = new List<string>() { "Index: ", "Middle: ", "Ring: ", "Pinky: "};
        List<string> ThumbAngle = new List<string>() {"radial/ulnar: ", ", palmar: ", ", MCP: ", ", IP: "};
        List<string> FourFingersAngle = new List<string>() {"MCP: ", ", PIP: ", ", DIP: "};

        AngleText += "Thumb: ";
        for (int j = 0; j < Angles[0].Count; j++)
        {
            AngleText += ThumbAngle[j] + Angles[0][j];
        }
        AngleText += "\n";
        for (int i = 1; i < Angles.Count; i++)
        {
            AngleText += FourFingersName[i-1];
            for (int j = 0; j < Angles[0].Count - 1 ; j++)
            {
                AngleText += FourFingersAngle[j] + Angles[i][j];
            }
            AngleText += "\n";
        }

        return AngleText;
    }

    //Calculate ROM
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
    (List<int> IDList, List<string> LRList, List<List<List<float>>> angleList) LeftOrRight()
    {
        string info;
        Hand _rightHand = Hands.Provider.GetHand(Chirality.Right);
        Hand _leftHand = Hands.Provider.GetHand(Chirality.Left);

        List<int> _IDList = new List<int>();
        List<string> _LRList = new List<string>();
        List<List<List<float>>> _angleList = new List<List<List<float>>>();

        if (_rightHand != null && _leftHand != null)
        {
            _IDList.Add(_rightHand.Id);
            _IDList.Add(_leftHand.Id);

            _LRList.Add("Right");
            _LRList.Add("Left");

            _angleList.Add(GetAngle(_rightHand));
            _angleList.Add(ChangeToMinus(GetAngle(_leftHand)));

            return (_IDList, _LRList, _angleList);
        }

        if (_rightHand != null && _leftHand == null)
        {
            _IDList.Add(_rightHand.Id);
            _LRList.Add("Right");
            _angleList.Add(GetAngle(_rightHand));
            return (_IDList, _LRList, _angleList);
        }

        if (_rightHand == null && _leftHand != null)
        {
            _IDList.Add(_leftHand.Id);
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
                    angles.Add(Vector3.SignedAngle(finger.bones[i].Direction, finger.bones[i + 1].Direction, finger.bones[i].Basis.xBasis));
                }
            }
            else
            {
                for (int i = 0; i < 3; i++)
                {
                    angles.Add(Vector3.SignedAngle(finger.bones[i].Direction, finger.bones[i + 1].Direction, finger.bones[i].Basis.xBasis));
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
        angleList[0][0] = angleList[0][0] * (-1);
        return angleList;
    }
}