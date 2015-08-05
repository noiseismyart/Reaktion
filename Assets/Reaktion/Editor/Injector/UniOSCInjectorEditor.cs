//
// Reaktion - An audio reactive animation toolkit for Unity.
//
// Copyright (C) 2013, 2014 Keijiro Takahashi
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of
// this software and associated documentation files (the "Software"), to deal in
// the Software without restriction, including without limitation the rights to
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
// the Software, and to permit persons to whom the Software is furnished to do so,
// subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
// FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
// IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
// CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
using UnityEngine;
using UnityEditor;
using System.Collections;

namespace Reaktion {

[CustomEditor(typeof(UniOSCInjector)), CanEditMultipleObjects]
public class UniOSCInjectionEditor : Editor
{
    SerializedProperty propOSCAddress;
    SerializedProperty propOSCValue;
    SerializedProperty propOSCenabled;
	SerializedProperty propOSCsmoothing;
	SerializedProperty propOSCsmoothingamt;
	SerializedProperty propOSCcurve;
	SerializedProperty propCurve;
    


    void OnEnable()
    {
        propOSCAddress = serializedObject.FindProperty("OSCAddress");
        propOSCenabled = serializedObject.FindProperty("OSCenabled");
        propOSCValue = serializedObject.FindProperty("OSCvalue");
		propOSCsmoothing = serializedObject.FindProperty("OSCSmoothing");
		propOSCsmoothingamt = serializedObject.FindProperty("OSCSmoothingAmt");
		propOSCcurve = serializedObject.FindProperty("OSCValueCurve");
		propCurve = serializedObject.FindProperty("curve");

    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

		EditorGUILayout.PropertyField(propOSCenabled, new GUIContent ("OSC Inc Enable"));
		EditorGUILayout.PropertyField(propOSCAddress, new GUIContent ("OSC Address"));
		EditorGUILayout.PropertyField(propOSCValue, new GUIContent ("OSC Value"));
		EditorGUILayout.PropertyField(propOSCsmoothing, new GUIContent ("OSC Value Smoothing"));
		EditorGUILayout.PropertyField(propOSCsmoothingamt, new GUIContent ("OSC Smoothing Amount"));
		EditorGUILayout.PropertyField(propOSCcurve, new GUIContent ("Use Value Curve"));
		EditorGUILayout.PropertyField(propCurve);


        //EditorGUILayout.Space();


        serializedObject.ApplyModifiedProperties();
    }
}

} // namespace Reaktion
