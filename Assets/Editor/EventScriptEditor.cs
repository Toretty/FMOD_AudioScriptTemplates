﻿using UnityEngine;
using System.Collections;
using UnityEditor;
using FMOD.Studio;

[CustomEditor(typeof(FMOD_EventScript))]
public class EventScriptEditor : Editor {

	private bool showInfo = false;
	private int Row1 = 10;
	private int Row2 = 20;
	private Bounds bounds;

	FMOD_EventScript myFMOD_EventScript;

	/// <summary>
	/// Modifies the Inspector for FMOD_EventScript
	/// </summary>
	public override void OnInspectorGUI()
	{
		myFMOD_EventScript = (FMOD_EventScript)target; 
		Display_FmodAsset ();
		Display_AssetControls ();
	}

	/// <summary>
	/// Displays Assets control options
	/// </summary>
	void Display_AssetControls(){
		if (myFMOD_EventScript.ObjectStrange != null) {

			Display_GeneralControls();
			Display_ParameterControl();
		}
	}

	/// <summary>
	/// Checks whether there is an asset or not, and then generates inspector display for it. 
	/// </summary>
	void Display_FmodAsset(){
		GUILayout.BeginHorizontal ();
		{	
			if(myFMOD_EventScript.ObjectStrange == null){
				GUILayout.Label ("Drag your FMOD asset into this field: ");
			}else{
				GUILayout.Label ("FMOD Asset: ");
			}
			
			myFMOD_EventScript.ObjectStrange = (FMODAsset)EditorGUILayout.ObjectField (myFMOD_EventScript.ObjectStrange, typeof(FMODAsset), false);
		}
		
		GUILayout.EndHorizontal ();	
	}

	/// <summary>
	/// Displays the general controls like "Update Location Constantly", "Start Automatically, Localize to an object"
	/// </summary>
	void Display_GeneralControls(){
		GUILayout.BeginHorizontal ();
		{	
			GUILayout.Space (Row1);
			myFMOD_EventScript.UpdatePositionConstantly = GUILayout.Toggle (myFMOD_EventScript.UpdatePositionConstantly, "Update Location Constantly");
			showInfo = GUILayout.Toggle (showInfo, "Info");
		}
		GUILayout.EndHorizontal ();
		
		GUILayout.Space (5);
		GUILayout.BeginHorizontal ();
		if (showInfo) {
			GUILayout.Space (Row2);
			GUILayout.TextArea ("When on, the position of the sound is constantly updated. When off, the sound will only position at the starting location and therefore not follow the object if moved.");
		}
		GUILayout.EndHorizontal ();
		
		/// START AUTOMATICALLY
		GUILayout.Space (Row1);
		
		GUILayout.BeginHorizontal ();
		{	
			GUILayout.Space (Row1);
			myFMOD_EventScript.StartAutomatically = GUILayout.Toggle (myFMOD_EventScript.StartAutomatically, "Start Automatically");
		}
		GUILayout.EndHorizontal ();
		
		GUILayout.Space (5);
		GUILayout.BeginHorizontal ();
		if (showInfo) {
			GUILayout.Space (Row2);
			GUILayout.TextArea ("When on, the position of the sound is constantly updated. When off, the sound will only position at the starting location and therefore not follow the object if moved.");
		}
		GUILayout.EndHorizontal ();
		
		/// LOCATE SOUND TO OTHER
		GUILayout.Space (Row1);
		GUILayout.BeginHorizontal ();
		{	
			GUILayout.Space (Row1);
			myFMOD_EventScript.UpdatePositionToOther = GUILayout.Toggle (myFMOD_EventScript.UpdatePositionToOther, "Localize to another object");
		}
		GUILayout.EndHorizontal ();
		
		GUILayout.Space (5);
		GUILayout.BeginHorizontal ();
		if (showInfo) {
			GUILayout.Space (Row2);
			GUILayout.TextArea ("Use this feature to localize the sound to another GameObject.");
		}
		GUILayout.EndHorizontal ();
		
		if (myFMOD_EventScript.UpdatePositionToOther) {
			GUILayout.BeginHorizontal ();
			{	
				GUILayout.Space (Row2);
				myFMOD_EventScript.OtherPositionObject = (GameObject)EditorGUILayout.ObjectField (myFMOD_EventScript.OtherPositionObject, typeof(GameObject), false);




			}
			GUILayout.EndHorizontal ();
			EditorGUILayout.BeginHorizontal();
			GUILayout.Space (Row2);
			bounds = EditorGUILayout.BoundsField(bounds);
			Mesh mesh = (Mesh)((MeshFilter)myFMOD_EventScript.OtherPositionObject.GetComponent(typeof(MeshFilter))).sharedMesh;
			//MeshFilter meshFilter = Selection.activeTransform.GetComponentInChildren(MeshFilter);
			if (mesh)
			{
				bounds = mesh.bounds;
			}
			EditorGUILayout.EndHorizontal();
		}
	}

	void Display_ParameterControl(){
		GUILayout.Space (Row1);
		
		GUILayout.BeginHorizontal ();
		{	
			GUILayout.Space (Row1);
			myFMOD_EventScript.UseParameterChange = GUILayout.Toggle (myFMOD_EventScript.UseParameterChange, "Enable Parameter control");
		}
		GUILayout.EndHorizontal ();
		
		GUILayout.Space (5);
		GUILayout.BeginHorizontal ();
		if (showInfo) {
			GUILayout.Space (Row2);
			GUILayout.TextArea ("This feature requires programmed activation. When enabled, you can control the parameter name and value from here.");
		}
		GUILayout.EndHorizontal ();
		
		if (myFMOD_EventScript.UseParameterChange) {
			
			GUILayout.BeginHorizontal ();
			{	
				GUILayout.Space (Row2);
				if (Application.isPlaying) {//If the unity is in play mode
					GUILayout.Label ("Parameter Controls");
				} else {
					GUILayout.Label ("(Parameter Controls are only visible while in play mode)");
				}
				
			}
			GUILayout.EndHorizontal ();
			//foreach(FMOD.Studio.ParameterInstance paramInst in myFMOD_EventScript.parameterInstance){
			for (int i=0; i<myFMOD_EventScript.numOfParams; i++) {
				GUILayout.BeginHorizontal ();
				{	
					GUILayout.Space (Row2);
					myFMOD_EventScript.parameterDescriptions [i].name = GUILayout.TextField (myFMOD_EventScript.parameterDescriptions [i].name);
					myFMOD_EventScript.parameterValues [i] = EditorGUILayout.Slider (myFMOD_EventScript.parameterValues [i], myFMOD_EventScript.parameterDescriptions [i].minimum, myFMOD_EventScript.parameterDescriptions [i].maximum);
				}
				GUILayout.EndHorizontal ();
				//}
			}
			GUILayout.Space (5);
			GUILayout.BeginHorizontal ();
			if (showInfo) {
				GUILayout.Space (Row2);
				GUILayout.TextArea ("Parameter is changed through calling the XX(); function");
			}
			GUILayout.EndHorizontal ();
			GUILayout.Space (Row1);
			
		}
	}
}
