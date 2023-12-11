using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;
//using UnityEngine.UI;
using TMPro;
using System;
using System.IO;

public class FingerAROMProvider : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI LiveText;
    [SerializeField]
    private TextMeshProUGUI MinText;
    [SerializeField]
    private TextMeshProUGUI MaxText;
    [SerializeField]
    private TextMeshProUGUI ROMText;
    [SerializeField]
    private TextMeshProUGUI FlexionOrExtensionText;
    [SerializeField]
    private TMP_InputField inputField;

    public LeapProvider leapProvider;

    string FlexionOrExtension;
    string LR;
    string patientID;

    List<List<float>> MinAngles;
    List<List<float>> MaxAngles;
    List<List<float>> ROM;

    //Initialize Min and Max
    void Start()
    {
        FlexionOrExtension = "flexion";
        FlexionOrExtensionText.text = FlexionOrExtension;

        MinAngles = new List<List<float>>();
        MaxAngles = new List<List<float>>();

        for (int i = 0; i < 5; i++)
        {
            List<float> row = new List<float>();
            for (int j = 0; j < 4; j++)
            {
                row.Add(180.0f);
            }
            MinAngles.Add(row);
        }
        for (int i = 0; i < 5; i++)
        {
            List<float> row = new List<float>();
            for (int j = 0; j < 4; j++)
            {
                row.Add(-180.0f);
            }
            MaxAngles.Add(row);
        }
    }

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
        List<List<float>> Angles;
        string info;
        string Min;
        string Max;
        string ROMValue;

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
                MinAngles = GetMinAngles(Angles);
                MaxAngles = GetMaxAngles(Angles);
                ROM = CalculateROM();

                info = "<" + LR + "> ID:" + ID + "\nAngles:\n" + LineUpAngles(Angles) + "\n";
                Min = "<" + LR + "> ID:" + ID + "\nAngles:\n" + LineUpAngles(MinAngles) + "\n";
                Max = "<" + LR + "> ID:" + ID + "\nAngles:\n" + LineUpAngles(MaxAngles) + "\n";
                ROMValue = "<" + LR + "> ID:" + ID + "\nAngles:\n" + LineUpAngles(ROM) + "\n";
            }
            else
            {
                ID = IDList[1];
                LR = LRList[1];
                Angles = angleList[1];
                MinAngles = GetMinAngles(Angles);
                MaxAngles = GetMaxAngles(Angles);
                ROM = CalculateROM();

                info = "<" + LR + "> ID:" + ID + "\nAngles:\n" + LineUpAngles(Angles) + "\n";
                Min = "<" + LR + "> ID:" + ID + "\nAngles:\n" + LineUpAngles(MinAngles) + "\n";
                Max = "<" + LR + "> ID:" + ID + "\nAngles:\n" + LineUpAngles(MaxAngles) + "\n";
                ROMValue = "<" + LR + "> ID:" + ID + "\nAngles:\n" + LineUpAngles(ROM) + "\n";
            }
        }
        if(IDList.Count == 1)
        {
            ID = IDList[0];
            LR = LRList[0];
            Angles = angleList[0];
            MinAngles = GetMinAngles(Angles);
            MaxAngles = GetMaxAngles(Angles);
            ROM = CalculateROM();

            info = "<" + LR + "> ID:" + ID + "\nAngles:\n" + LineUpAngles(Angles) + "\n";
            Min = "<" + LR + "> ID:" + ID + "\nAngles:\n" + LineUpAngles(MinAngles) + "\n";
            Max = "<" + LR + "> ID:" + ID + "\nAngles:\n" + LineUpAngles(MaxAngles) + "\n";
            ROMValue = "<" + LR + "> ID:" + ID + "\nAngles:\n" + LineUpAngles(ROM) + "\n";
        }
        else
        {
            info = "NO HAND";
            Min = "NO Min";
            Max = "NO Max";
            ROMValue = "NO ROM";
        }
        LiveText.text = "Live:" + info;
        MinText.text = "Min:" + Min;
        MaxText.text = "Max:" + Max;
        ROMText.text = "ROM:" + ROMValue;
    }

    

    //Flexion Button
    public void Flexion()
    {
        Start();
        FlexionOrExtension = "flexion";
        FlexionOrExtensionText.text = FlexionOrExtension;
    }

    public void Extension()
    {
        Start();
        FlexionOrExtension = "extension";
        FlexionOrExtensionText.text = FlexionOrExtension;
    }

    //Save Button
    public void Save()
    {
        string path = @"C:\HandtrackerUnity\" + patientID + ".csv";//Application.dataPath + @"\HandtrackerUnity\Sample.csv";

        //create a directory if it does not exist
        string directory = Path.GetDirectoryName(path);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
        //create a path if it does not exist
        if (!File.Exists(path))
        {
            using (File.Create(path)) {
            }
            string row = "Patient ID:" + patientID + ", handedness, flex./ext., , ";
            List<string> FingersName = new List<string>() { "Thumb", "Index", "Middle", "Ring", "Pinky" };
            List<string> ThumbAngle = new List<string>() { "radial/ulnar", "palmar", "MCP", "IP" };
            List<string> FourFingersAngle = new List<string>() { "MCP", "PIP", "DIP" };
            List<string> ROMMinMax = new List<string>() { "ROM", "Min", "Max" };
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 3; k++)
                {
                    row += FingersName[0] + " " + ThumbAngle[j] + " " + ROMMinMax[k] + ",";
                }

            }
            for (int i = 1; i < 5; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    for (int k = 0; k < 3; k++)
                    {
                        row += FingersName[i] + " " + FourFingersAngle[j] + " " + ROMMinMax[k] + ",";
                    }
                }

            }
            row += "\n";
            File.AppendAllText(path, row);
        }
        //add angles data
        DateTime dt = DateTime.Now;
        string dataRow = dt + "," + LR + "," + FlexionOrExtension + ", ,";
        for (int j = 0; j < 4; j++) // "radial/ulnar", "palmar", "MCP", "IP" only in Thumb
        {
            dataRow += ROM[0][j] + "," + MinAngles[0][j] + "," + MaxAngles[0][j] + ",";

        }
        for (int i = 1; i < 5; i++) // "Index", "Middle", "Ring", "Pinky"
        {
            for (int j = 0; j < 3; j++) //"MCP", "PIP", "DIP"
            {
                dataRow += ROM[i][j] + "," + MinAngles[i][j] + "," + MaxAngles[i][j] + ",";
            }

        }
        dataRow += "\n";
        File.AppendAllText(path, dataRow);

        if (FlexionOrExtension == "flexion")
        {
            Flexion();
        }
        if(FlexionOrExtension == "extension")
        {
            Extension();
        }
    }

    //Reset Button
    public void Reset()
    {
        if (FlexionOrExtension == "flexion")
        {
            Flexion();
        }
        if (FlexionOrExtension == "extension")
        {
            Extension();
        }
    }

    public void OnValueChanged()
    {
        patientID = inputField.GetComponent<TMP_InputField>().text;
    }

    //Get Max Angles
    List<List<float>> GetMaxAngles(List<List<float>> Angles)
    {
        for (int i = 0; i < Angles.Count; i++)
        {
            for (int j = 0; j < Angles[0].Count; j++)
            {
                if(MaxAngles[i][j] < Angles[i][j])
                {
                    MaxAngles[i][j] = Angles[i][j];
                }
            }
        }
        return MaxAngles;
    }

    //Get Min Angles
    List<List<float>> GetMinAngles(List<List<float>> Angles)
    {
        for (int i = 0; i < Angles.Count; i++)
        {
            for (int j = 0; j < Angles[0].Count; j++)
            {
                if (Angles[i][j] < MinAngles[i][j])
                {
                    MinAngles[i][j] = Angles[i][j];
                }
            }
        }
        return MinAngles;
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
    List<List<float>> CalculateROM()
    {
        string _FlexionOrExtension = FlexionOrExtension;
        List<List<float>> _MinAngles = new List<List<float>>(MinAngles);
        List<List<float>> _MaxAngles = new List<List<float>>(MaxAngles);
        List<List<float>> _ROM = new List<List<float>>();

        for (int i = 0; i < 5; i++)
        {
            List<float> row = new List<float>();
            for (int j = 0; j < 4; j++)
            {
                row.Add(0.0f);
            }
            _ROM.Add(row);
        }
        

        if (_FlexionOrExtension == "flexion")
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    _ROM[i][j] = _MinAngles[i][j] * (-1);
                }
            }
            return _ROM;
        }
        if (_FlexionOrExtension == "extension")
        {
            //_ROM = new List<List<float>>(_MaxAngles);
            for (int j = 2; j < 4; j++)
            {
                _ROM[0][j] = _MaxAngles[0][j];
            }
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    _ROM[i][j] = _MaxAngles[i][j];
                }
            }
            _ROM[0][0] = _MinAngles[0][0] * (-1);
            _ROM[0][1] = _MinAngles[0][1] * (-1);
            
            return _ROM;
        }
        else
        {
            return _ROM;
        }
    }

    //Recognize Left or Right(Main Calculation)
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
        string _FlexionOrExtension = FlexionOrExtension;

        foreach (Finger finger in _hand.Fingers)
        {
            List<float> angles = new List<float>();
            if (finger.Type == 0)//_hand.Finger(finger.Id).FingerType.TYPE_THUMB)
            {
                Finger _thumb = _hand.Fingers[0];
                Finger _index = _hand.Fingers[1];
                angles.Add(-AngleOnPlane(_index.bones[0].Direction, _thumb.bones[1].Direction, _hand.PalmNormal));
                angles.Add(-AngleOnPlane(_index.bones[0].Direction, _thumb.bones[1].Direction, Vector3.Cross(_hand.Direction, _hand.PalmNormal)));
                for (int i = 1; i < 3; i++)
                {
                    angles.Add(-AngleOnPlane(finger.bones[i].Direction, finger.bones[i + 1].Direction, finger.bones[i+1].Basis.xBasis));
                }
            }
            else
            {
                for (int i = 0; i < 3; i++)
                {
                    angles.Add(-AngleOnPlane(finger.bones[i].Direction, finger.bones[i + 1].Direction, finger.bones[i+1].Basis.xBasis));
                }
                angles.Add(0.0f);
            }
            angleList.Add(angles); //the size of this list is [5][4]
        }
        return angleList;
    }

    //change left radial/ulnar angles to minus
    List<List<float>> ChangeToMinus(List<List<float>> angleList)
    {
        angleList[0][0] = angleList[0][0] * (-1);
        return angleList;
    }

    //Calculate angle on plane
    float AngleOnPlane(Vector3 from, Vector3 to, Vector3 axis)
    {
        Vector3 fromOnPlane = Vector3.ProjectOnPlane(from, axis);
        Vector3 toOnPlane = Vector3.ProjectOnPlane(to, axis);
        return Vector3.SignedAngle(fromOnPlane, toOnPlane, axis);
    }
}