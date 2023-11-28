//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;
//using UnityEngine.UI;
using TMPro;

public class TutorialPostProcessProvider : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI cardNameText;

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
        string info;
        Hand _rightHand = frame.GetHand(Chirality.Right);
        Hand _leftHand = frame.GetHand(Chirality.Left);
        /*
        if (_rightHand == null && _leftHand == null)
        {
            return;
        }
        */
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
        
        //Use the _finger to subsequently get the Metacarpa bone from it using the BoneType Enum
        Bone _thumbMetacarpal = _thumb.Bone(Bone.BoneType.TYPE_METACARPAL);
        Bone _indexMetacarpal = _index.Bone(Bone.BoneType.TYPE_METACARPAL);
        Bone _middleMetacarpal = _middle.Bone(Bone.BoneType.TYPE_METACARPAL);
        Bone _ringMetacarpal = _ring.Bone(Bone.BoneType.TYPE_METACARPAL);
        Bone _pinkyMetacarpal = _pinky.Bone(Bone.BoneType.TYPE_METACARPAL);

        Bone _thumbProximal = _thumb.Bone(Bone.BoneType.TYPE_PROXIMAL);
        Bone _indexProximal = _index.Bone(Bone.BoneType.TYPE_PROXIMAL);
        Bone _middleProximal = _middle.Bone(Bone.BoneType.TYPE_PROXIMAL);
        Bone _ringProximal = _ring.Bone(Bone.BoneType.TYPE_PROXIMAL);
        Bone _pinkyProximal = _pinky.Bone(Bone.BoneType.TYPE_PROXIMAL);

        Bone _thumbIntermediate = _thumb.Bone(Bone.BoneType.TYPE_INTERMEDIATE);
        Bone _indexIntermediate = _index.Bone(Bone.BoneType.TYPE_INTERMEDIATE);
        Bone _middleIntermediate = _middle.Bone(Bone.BoneType.TYPE_INTERMEDIATE);
        Bone _ringIntermediate = _ring.Bone(Bone.BoneType.TYPE_INTERMEDIATE);
        Bone _pinkyIntermediate = _pinky.Bone(Bone.BoneType.TYPE_INTERMEDIATE);

        Bone _thumbDistal = _thumb.Bone(Bone.BoneType.TYPE_DISTAL);
        Bone _indexDistal = _index.Bone(Bone.BoneType.TYPE_DISTAL);
        Bone _middleDistal = _middle.Bone(Bone.BoneType.TYPE_DISTAL);
        Bone _ringDistal = _ring.Bone(Bone.BoneType.TYPE_DISTAL);
        Bone _pinkyDistal = _pinky.Bone(Bone.BoneType.TYPE_DISTAL);

        /*
        //Use the _indexFinger to access the bone array, then get the Metacarpal bone from it using the BoneType Enum cast to an int
        _thumbMetacarpal = _thumb.bones[(int)Bone.BoneType.TYPE_METACARPAL];
        _indexMetacarpal = _index.bones[(int)Bone.BoneType.TYPE_METACARPAL];
        _middleMetacarpal = _middle.bones[(int)Bone.BoneType.TYPE_METACARPAL];
        _ringMetacarpal = _ring.bones[(int)Bone.BoneType.TYPE_METACARPAL];
        _pinkyMetacarpal = _pinky.bones[(int)Bone.BoneType.TYPE_METACARPAL];

        //Use the _indexFinger to access the bone array, then get the metacarpal by index
        LEAP_BONE _indexMetacarpal = _index.bones[0];
        LEAP_BONE _indexProximal = _index.bones[1];
        LEAP_BONE _indexIntermediate = _index.bones[2];
        LEAP_BONE _indexDistal = _index.bones[3];
        */

        Bone _indexDistal1 = _index.bones[3];

        LeapTransform _basis = _indexDistal.Basis;
        Vector3 xBasis = _basis.xBasis;
        string xBasisString = xBasis.ToString();

        LeapTransform _basis1 = _indexDistal1.Basis;
        Vector3 xBasis1 = _basis1.xBasis;
        string xBasisString1 = xBasis1.ToString();

        string _info = "WhichHand:" + _WhichHand + ", Hand ID: " + _hand.Id + ", Palm Position: " + _hand.PalmPosition + ", xBasis" + xBasisString + ", IndexDistal Direction" + _indexDistal.Direction + ", xBasis1" + xBasisString1 + ", IndexDistal1 Direction" + _indexDistal1.Direction + "\n";

        return _info;
    }
}