﻿using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(ParagonAI.AnimationScript))]
[CanEditMultipleObjects]
public class animationScriptInspector : Editor
{
    SerializedObject animationScript;

    //Stuff
    SerializedProperty myBaseScriptProp;
    SerializedProperty myAIBodyTransformProp;
    SerializedProperty gunScriptProp;
    SerializedProperty animatorProp;
    SerializedProperty rotateGunScriptProp;

    SerializedProperty bodyOffsetProp;

    //Cover
    SerializedProperty minDistToCrouchProp;

    //Speeds
    SerializedProperty maxMovementSpeedProp;
    SerializedProperty animatorSpeedProp;
    SerializedProperty meleeAnimationSpeedProp;

    //Dynamic objects
    SerializedProperty maxAngleDeviationProp;

    //Rotation
    SerializedProperty minAngleToRotateBaseProp;
    SerializedProperty turnSpeedProp;	

    void OnEnable()
    {
        animationScript = new SerializedObject(target);

        myBaseScriptProp = serializedObject.FindProperty("myBaseScript");
        myAIBodyTransformProp = serializedObject.FindProperty("myAIBodyTransform");
        gunScriptProp = serializedObject.FindProperty("gunScript");
        animatorProp = serializedObject.FindProperty("animator");
        rotateGunScriptProp = serializedObject.FindProperty("rotateGunScript");

        bodyOffsetProp = serializedObject.FindProperty("bodyOffset");

        maxMovementSpeedProp = serializedObject.FindProperty("maxMovementSpeed");
        animatorSpeedProp = serializedObject.FindProperty("animatorSpeed");
        meleeAnimationSpeedProp = serializedObject.FindProperty("meleeAnimationSpeed");

        maxAngleDeviationProp = serializedObject.FindProperty("maxAngleDeviation");

        minAngleToRotateBaseProp = serializedObject.FindProperty("minAngleToRotateBase");
        turnSpeedProp = serializedObject.FindProperty("turnSpeed");
    }

    bool showLinks = false;

    public override void OnInspectorGUI()
    {
        animationScript.Update();
        EditorGUI.BeginChangeCheck();

        EditorGUILayout.PropertyField(bodyOffsetProp);
        EditorGUILayout.PropertyField(maxMovementSpeedProp);
        EditorGUILayout.PropertyField(animatorSpeedProp);
        EditorGUILayout.PropertyField(meleeAnimationSpeedProp);
        EditorGUILayout.PropertyField(maxAngleDeviationProp);
        minAngleToRotateBaseProp.floatValue = EditorGUILayout.Slider("Min Angle To Rotate Base", minAngleToRotateBaseProp.floatValue, 0.0f, 90.0f);
        EditorGUILayout.PropertyField(turnSpeedProp);

        //EditorGUILayout.Separator();
        showLinks = EditorGUILayout.Foldout(showLinks, "Show Linked Components");
        if (showLinks)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(myBaseScriptProp);
            EditorGUILayout.PropertyField(myAIBodyTransformProp);
            EditorGUILayout.PropertyField(gunScriptProp);
            EditorGUILayout.PropertyField(animatorProp);
            EditorGUILayout.PropertyField(rotateGunScriptProp);
            EditorGUI.indentLevel--;
        }

        if (EditorGUI.EndChangeCheck())
            serializedObject.ApplyModifiedProperties();
    }

    public void DrawArray(SerializedProperty prop)
    {
        EditorGUIUtility.LookLikeControls();
        EditorGUILayout.PropertyField(prop, true);
    }
}
