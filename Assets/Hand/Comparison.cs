using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;
//using UnityEngine.UI;
using TMPro;
using System;
using System.IO;

public class Comparison : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI cardNameText;
    [SerializeField]
    private LeapProvider leapProvider;

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
        string info;
        Hand _leftHand = frame.GetHand(Chirality.Left);
        Hand _rightHand = frame.GetHand(Chirality.Right);

        if (_rightHand != null && _leftHand != null)
        {
            info = OnUpdateHand(_rightHand, "Right") + OnUpdateHand(_leftHand, "Left");
            cardNameText.text = info;
        }

        if (_rightHand != null && _leftHand == null)
        {
            info = OnUpdateHand(_rightHand, "Right");
            cardNameText.text = info;
        }

        if (_rightHand == null && _leftHand != null)
        {
            info = OnUpdateHand(_leftHand, "Left");
            cardNameText.text = info;
        }
    }

    string OnUpdateHand(Hand _hand, string _WhichHand)
    {
        //Use _hand to Explicitly get the specified finger and subsequent bone from it
        Finger _thumb = _hand.GetThumb();
        Finger _index = _hand.GetIndex();
        Finger _middle = _hand.GetMiddle();
        Finger _ring = _hand.GetRing();
        Finger _pinky = _hand.GetPinky();

        //Use the _indexFinger to access the bone array, then get the metacarpal by index
        Bone _thumbMetacarpal = _thumb.bones[0];
        Bone _indexMetacarpal = _index.bones[0];
        Bone _middleMetacarpal = _middle.bones[0];
        Bone _ringMetacarpal = _ring.bones[0];
        Bone _pinkyMetacarpal = _pinky.bones[0];

        Bone _thumbProximal = _thumb.bones[1];
        Bone _indexProximal = _index.bones[1];
        Bone _middleProximal = _middle.bones[1];
        Bone _ringProximal = _ring.bones[1];
        Bone _pinkyProximal = _pinky.bones[1];

        Bone _thumbIntermediate = _thumb.bones[2];
        Bone _indexIntermediate = _index.bones[2];
        Bone _middleIntermediate = _middle.bones[2];
        Bone _ringIntermediate = _ring.bones[2];
        Bone _pinkyIntermediate = _pinky.bones[2];

        Bone _thumbDistal = _thumb.bones[3];
        Bone _indexDistal = _index.bones[3];
        Bone _middleDistal = _middle.bones[3];
        Bone _ringDistal = _ring.bones[3];
        Bone _pinkyDistal = _pinky.bones[3];

        string _info = "WhichHand:" + _WhichHand + ", Hand ID: " + _hand.Id + "\n"
            + ", IndexMeta_yBasis: " + _indexMetacarpal.Basis.yBasis + ", hand.PalmNormal:" + _hand.PalmNormal + "\n"
            + ", radial/ulnar(with IndexMeta_yBasis):" + Vector3.SignedAngle(_indexMetacarpal.Direction, _thumbProximal.Direction, _indexMetacarpal.Basis.yBasis)
            + ", radial/ulnar(with hand.PalmNormal):" + Vector3.SignedAngle(_indexMetacarpal.Direction, _thumbProximal.Direction, _hand.PalmNormal) + "\n"
            + ", _indexMetacarpal.Basis.xBasis: " + _indexMetacarpal.Basis.xBasis + ", Vector3.Cross(_hand.Direction, _hand.PalmNormal):" + Vector3.Cross(_hand.Direction, _hand.PalmNormal) + "\n"
            + ", palmar(with IndexMeta_xBasis):" + Vector3.SignedAngle(_indexMetacarpal.Direction, _thumbProximal.Direction, _indexMetacarpal.Basis.xBasis)
            + ", palmar(with Vector3.Cross(_hand.Direction, _hand.PalmNormal)):" + Vector3.SignedAngle(_indexMetacarpal.Direction, _thumbProximal.Direction, Vector3.Cross(_hand.Direction, _hand.PalmNormal)) + "\n"
            + ", _thumbProximal.Basis.xBasis: " + _thumbProximal.Basis.xBasis + ", _thumbIntermediate.Basis.xBasis:" + _thumbIntermediate.Basis.xBasis + "\n"
            + "Thumb_MCP(with _indexMetacarpal.Basis.xBasis):" + Vector3.SignedAngle(_thumbProximal.Direction, _thumbIntermediate.Direction, _thumbMetacarpal.Basis.xBasis)
            + "Thumb_MCP(with _indexProximal.Basis.xBasis):" + Vector3.SignedAngle(_thumbProximal.Direction, _thumbIntermediate.Direction, _thumbIntermediate.Basis.xBasis) + "\n"
             + ", _thumbIntermediate.Basis.xBasis: " + _thumbIntermediate.Basis.xBasis + ", _thumbDistal.Basis.xBasis:" + _thumbDistal.Basis.xBasis + "\n"
            + "Thumb_IP(with _indexMetacarpal.Basis.xBasis):" + Vector3.SignedAngle(_thumbIntermediate.Direction, _thumbDistal.Direction, _thumbIntermediate.Basis.xBasis)
            + "Thumb_IP(with _indexProximal.Basis.xBasis):" + Vector3.SignedAngle(_thumbIntermediate.Direction, _thumbDistal.Direction, _thumbDistal.Basis.xBasis) + "\n"
            + ", _indexMetacarpal.Basis.xBasis: " + _indexMetacarpal.Basis.xBasis + ", _indexProximal.Basis.xBasis:" + _indexProximal.Basis.xBasis + "\n"
            + "Index_MCP(with _indexMetacarpal.Basis.xBasis):" + Vector3.SignedAngle(_indexMetacarpal.Direction, _indexProximal.Direction, _indexMetacarpal.Basis.xBasis)
            + "Index_MCP(with _indexProximal.Basis.xBasis):" + Vector3.SignedAngle(_indexMetacarpal.Direction, _indexProximal.Direction, _indexProximal.Basis.xBasis) + "\n"
            + ", _indexProximal.Basis.xBasis: " + _indexProximal.Basis.xBasis + ", _indexIntermediate.Basis.xBasis:" + _indexIntermediate.Basis.xBasis + "\n"
            + "Index_PIP(with _indexMetacarpal.Basis.xBasis):" + Vector3.SignedAngle(_indexProximal.Direction, _indexIntermediate.Direction, _indexProximal.Basis.xBasis)
            + "Index_PIP(with _indexProximal.Basis.xBasis):" + Vector3.SignedAngle(_indexProximal.Direction, _indexIntermediate.Direction, _indexIntermediate.Basis.xBasis) + "\n"
            + "\n"
            + ", IndexMeta_yBasis: " + _indexMetacarpal.Basis.yBasis + ", hand.PalmNormal:" + _hand.PalmNormal + "\n"
            + ", radial/ulnar(with IndexMeta_yBasis):" + AngleOnPlane(_indexMetacarpal.Direction, _thumbProximal.Direction, _indexMetacarpal.Basis.yBasis)
            + ", radial/ulnar(with hand.PalmNormal):" + AngleOnPlane(_indexMetacarpal.Direction, _thumbProximal.Direction, _hand.PalmNormal) + "\n"
            + ", _indexMetacarpal.Basis.xBasis: " + _indexMetacarpal.Basis.xBasis + ", Vector3.Cross(_hand.Direction, _hand.PalmNormal):" + Vector3.Cross(_hand.Direction, _hand.PalmNormal) + "\n"
            + ", palmar(with IndexMeta_xBasis):" + AngleOnPlane(_indexMetacarpal.Direction, _thumbProximal.Direction, _indexMetacarpal.Basis.xBasis)
            + ", palmar(with Vector3.Cross(_hand.Direction, _hand.PalmNormal)):" + AngleOnPlane(_indexMetacarpal.Direction, _thumbProximal.Direction, Vector3.Cross(_hand.Direction, _hand.PalmNormal)) + "\n"
            + ", _thumbProximal.Basis.xBasis: " + _thumbProximal.Basis.xBasis + ", _thumbIntermediate.Basis.xBasis:" + _thumbIntermediate.Basis.xBasis + "\n"
            + "Thumb_MCP(with _indexMetacarpal.Basis.xBasis):" + AngleOnPlane(_thumbProximal.Direction, _thumbIntermediate.Direction, _thumbMetacarpal.Basis.xBasis)
            + "Thumb_MCP(with _indexProximal.Basis.xBasis):" + AngleOnPlane(_thumbProximal.Direction, _thumbIntermediate.Direction, _thumbIntermediate.Basis.xBasis) + "\n"
            + ", _thumbIntermediate.Basis.xBasis: " + _thumbIntermediate.Basis.xBasis + ", _thumbDistal.Basis.xBasis:" + _thumbDistal.Basis.xBasis + "\n"
            + "Thumb_IP(with _indexMetacarpal.Basis.xBasis):" + AngleOnPlane(_thumbIntermediate.Direction, _thumbDistal.Direction, _thumbIntermediate.Basis.xBasis)
            + "Thumb_IP(with _indexProximal.Basis.xBasis):" + AngleOnPlane(_thumbIntermediate.Direction, _thumbDistal.Direction, _thumbDistal.Basis.xBasis) + "\n"
            + ", _indexMetacarpal.Basis.xBasis: " + _indexMetacarpal.Basis.xBasis + ", _indexProximal.Basis.xBasis:" + _indexProximal.Basis.xBasis + "\n"
            + "Index_MCP(with _indexMetacarpal.Basis.xBasis):" + AngleOnPlane(_indexMetacarpal.Direction, _indexProximal.Direction, _indexMetacarpal.Basis.xBasis)
            + "Index_MCP(with _indexProximal.Basis.xBasis):" + AngleOnPlane(_indexMetacarpal.Direction, _indexProximal.Direction, _indexProximal.Basis.xBasis) + "\n"
            + ", _indexProximal.Basis.xBasis: " + _indexProximal.Basis.xBasis + ", _indexIntermediate.Basis.xBasis:" + _indexIntermediate.Basis.xBasis + "\n"
            + "Index_PIP(with _indexMetacarpal.Basis.xBasis):" + AngleOnPlane(_indexProximal.Direction, _indexIntermediate.Direction, _indexProximal.Basis.xBasis)
            + "Index_PIP(with _indexProximal.Basis.xBasis):" + AngleOnPlane(_indexProximal.Direction, _indexIntermediate.Direction, _indexIntermediate.Basis.xBasis) + "\n";

        return _info;
    }

    float AngleOnPlane(Vector3 from, Vector3 to, Vector3 axis)
    {
        Vector3 fromOnPlane = Vector3.ProjectOnPlane(from, axis);
        Vector3 toOnPlane = Vector3.ProjectOnPlane(to, axis);
        return Vector3.SignedAngle(fromOnPlane, toOnPlane, axis);
    }
}