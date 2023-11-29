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

        LeapTransform _basis = _indexDistal.Basis;
        Vector3 xBasis = _basis.xBasis;
        string xBasisString = xBasis.ToString();

        string _info = "WhichHand:" + _WhichHand + ", Hand ID: " + _hand.Id +   ", Angle" + Vector3.Angle(_indexProximal.Direction, _indexMetacarpal.Direction) + "\n"
            + ", Rotation:" + _indexIntermediate.Rotation.eulerAngles + "\n";

        return _info;
    }
}