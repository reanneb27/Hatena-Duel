using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

/*
This script will find all of the animation curves under "From root", possibly
negate them, and copy the resultant animation curves to "To root".
 
It will operate on the default animation clip of the currently selected
object. This may be different from the clip you have selected in the
animation editor, so be sure to change the "Animation" setting of the
animation you want to operate on in the Inspector.
 
To then use the script, select "Mirror Animation Curves" in the "Window"
menu of Unity and fill in the options and click one of the buttons to
activate the script.
 
When naming the new curves, it will remove the "From prefix" from the start
of the original curve's name if it is present, and replace it with the the
"To prefix". It will also remove the "From appendage" from the end of the
original curve's name if present and append the "To appendage".
 
For example, if you had an AnimationClip which animated the Right_arm in the
Body child of whatever it was attached to, and you wanted to mirror the curves
so that they applied to the Left_arm, your original curves would be named like
this (using the hand as an example):
 
Body/Right_arm/Right_forearm/Right_hand
 
And your new curves would be named like this:
 
Body/Left_arm/Left_forearm/Left_hand
 
In this case, you would set the "From root" to "Body/Left_arm" and the
"To root" to "Body/Right_arm" because you want to mirror the entire left
arm and all of its children and write the curves to the right arm.
The "From prefix" should be set to "Right" and the "To prefix" should be set to
"Left" because you want to change "Right" to "Left" at the start of all of the
names. Note that the root names are not changed because you already set the new
root name. The appendages (From appendage and To appendage) should be set to
nothing because you don't want to change anything at the end of the names.
 
The script will then use these rules to change "Right_forearm" to "Left_forearm"
and "Right_hand" to "Left_hand" when it is copying the animation curves
for the hand.
 
Also, you may want to copy animation curves from one object to a mirror image
of that object. In this case, you have to set up your mirrored object so that
two axis preserve their original meaning and one axis is inverted. For example,
your right thumb may be oriented so that the X axis points towards the arm,
the Y axis points towards the index finger, and the Z axis points
towards the palm. When you mirror it, you have to change the meaning of
one of these axis because you have to end up with a left-handed coordinate
system. For example, you might have the mirrored thumb's X axis continue to
point towards the mirrored arm and the Y axis continue to point
towards the mirrored index finger, but the Z axis would now point away from the
palm because the coordinate system has to be left handed. In this case you
would use the "Mirror Z" setting. Note that your mirrored armature has
to have only the Z axis inverted like this for every bone you are mirroring
or the script won't work (i.e. you have to be consistent).
 
If you have a different animation for your mirrored object, you can still use
this script. First make a copy of the animation clip and change the mirrored
object to use the copy, then run this script on the mirrored object with the
from root and to root both set to nothing. If your children are named the
same in each object, you can also set the prefixes and appendages to nothing,
otherwise you will be asked to clean up animation curves which you may do.
*/

public class mirror_animation_curves : EditorWindow
{

    [MenuItem("Window/Mirror Animation Curves")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(mirror_animation_curves));
    }

    string GameObjectName;
    string FromAnimationClip;
    string ToAnimationClip;
    void OnGUI()
    {
        GUILayout.Label("Game Object Name:");
        GameObjectName = GUILayout.TextField(GameObjectName);
        GUILayout.Label("Input Animation Clip Name:");
        FromAnimationClip = GUILayout.TextField(FromAnimationClip);
        GUILayout.Label("Output Animation Clip Name:");
        ToAnimationClip = GUILayout.TextField(ToAnimationClip);
        var mirror_x = GUILayout.Button("Mirror X");

        if (mirror_x) 
        {
            if (GameObject.Find(GameObjectName) == null)
            {
                Debug.Log("Game Object \"" + GameObjectName + "\" doesn't exist");
                return;
            }
            if (GetAnimationClip(FromAnimationClip) == null)
            {
                Debug.Log("Input Animation Clip \"" + FromAnimationClip + "\" doesn't exist in " + GameObjectName);
                return;
            }
            if (GetAnimationClip(ToAnimationClip) == null)
            {
                Debug.Log("Output Animation Clip \"" + ToAnimationClip + "\" doesn't exist in " + GameObjectName);
                return;
            }

            Mirror(FromAnimationClip, ToAnimationClip);
            Debug.Log("Successfully Mirrored!");
        }
        
    }

    /// <summary>
    /// Mirroring Strategy: Get curve data of the input clip. Set first curve frame of output clip same as first curve frame of input clip. Iterate to remaining keys and set the value mirroring on the x-axis with first curve frame as the x axis offset. Set keyframe in-tangent and out-tangent of output clip as the negative of keyframe in-tangent and out-tangent of input clip.
    /// </summary>
    /// <param name="animClip1"></param>
    /// <param name="animClip2"></param>
    private void Mirror(string animClip1, string animClip2)
    {
        AnimationClip a = GetAnimationClip(animClip1);
        AnimationClip mirroredClip = GetAnimationClip(animClip2);
        AnimationCurve mirroredCurve = new AnimationCurve();

        foreach (var x in UnityEditor.AnimationUtility.GetCurveBindings(a))
        {
            if (x.propertyName == "m_LocalPosition." + "x")
            {
                AnimationCurve originalCurve = UnityEditor.AnimationUtility.GetEditorCurve(a, x);

                if (originalCurve.length > 1)
                {
                    mirroredCurve.AddKey(originalCurve[0]);
                    Debug.Log("1st key: " + originalCurve[0].inWeight + ", " + originalCurve[0].outWeight);

                    for (int i = 1; i < originalCurve.length; i++)
                    {
                        float dist = Mathf.Abs(originalCurve[i].value - originalCurve[0].value);
                        if (originalCurve[i].value != originalCurve[0].value)
                        {
                            Debug.Log(originalCurve[i].inTangent + ", " + originalCurve[i].outTangent);
                            if (originalCurve[i].value < originalCurve[0].value)
                            {
                                Keyframe k = new Keyframe();
                                k.time = originalCurve[i].time;
                                k.value = originalCurve[0].value + dist;
                                k.inTangent = -originalCurve[i].inTangent;
                                k.outTangent = -originalCurve[i].outTangent;
                                //mirroredCurve.AddKey(originalCurve[i].time, originalCurve[0].value + dist);
                                mirroredCurve.AddKey(k);

                                Debug.Log("Mirrored: " + mirroredCurve[i].inTangent + ", " + mirroredCurve[i].outTangent);
                            }
                            else
                            {
                                Keyframe k = new Keyframe();
                                k.time = originalCurve[i].time;
                                k.value = originalCurve[0].value - dist;
                                k.inTangent = -originalCurve[i].inTangent;
                                k.outTangent = -originalCurve[i].outTangent;
                                //mirroredCurve.AddKey(originalCurve[i].time, originalCurve[0].value - dist);
                                mirroredCurve.AddKey(k);
                                Debug.Log("Mirrored: " + mirroredCurve[i].inTangent + ", " + mirroredCurve[i].outTangent);
                            }
                        }
                    }
                }

                mirroredClip.SetCurve(x.path, x.type, x.propertyName, mirroredCurve);
            }
            else
            {
                AnimationCurve originalCurve = UnityEditor.AnimationUtility.GetEditorCurve(a, x);
                mirroredClip.SetCurve(x.path, x.type, x.propertyName, originalCurve);
            }
        }
    }

    public AnimationClip GetAnimationClip(string name)
    {
        foreach (AnimationClip clip in UnityEditor.AnimationUtility.GetAnimationClips(GameObject.Find(GameObjectName)))
        {
            if (clip.name == name)
            {
                return clip;
            }
        }
        return null; // no clip by that name
    }

};