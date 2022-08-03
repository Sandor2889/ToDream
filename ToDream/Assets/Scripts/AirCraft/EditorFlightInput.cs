using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FlightInput))]
public class EditorFlightInput : Editor
{
	private FlightInput _targetInput;
	
	private void OnEnable()
	{
		_targetInput = target as FlightInput;
	}
	
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		
		//Custom editor changes
		string debugInfo = "";
		debugInfo += "Speed : " + _targetInput._Speed;
		debugInfo += "\nRoll : " + _targetInput._Roll;
		debugInfo += "\nYaw : " + _targetInput._Yaw;
		debugInfo += "\nPitch : " + _targetInput._Pitch;

		GUILayout.Space(20);
		EditorGUILayout.TextArea(debugInfo, GUILayout.Height(100));
		GUILayout.Space(20);
		Repaint();
	}
}
