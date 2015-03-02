using UnityEngine;
using System.Collections;
using UnityEditor;
using FMOD.Studio;

[CustomEditor(typeof(FMOD_EventScript))]
public class EventScriptEditor : Editor {
	
	public override void OnInspectorGUI()
	{
		FMOD_EventScript myFMOD_EventScript = (FMOD_EventScript)target; 
		bool showPosition = true;

		int Row1 = 10;
		int Row2 = 20;
		int Row3 = 30;
		int Row4 = 40;

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

		if(myFMOD_EventScript.ObjectStrange != null)
		{
			////////////////////////////////////////////////////// General Properties
			/// START AUTOMATICALLY
			GUILayout.Space(Row1);

			GUILayout.BeginHorizontal ();
			{	
				GUILayout.Space(Row1);
				myFMOD_EventScript.StartAutomatically = GUILayout.Toggle(myFMOD_EventScript.StartAutomatically, "Start Automatically");
			}
			GUILayout.EndHorizontal ();

			/// UPDATE LOCATION OF SOUND
			GUILayout.Space(Row1);
			GUILayout.BeginHorizontal ();
			{	
				GUILayout.Space(Row1);
				myFMOD_EventScript.UpdatePositionConstantly = GUILayout.Toggle(myFMOD_EventScript.UpdatePositionConstantly, "Update Location constantly");
			}
			GUILayout.EndHorizontal ();

			/// LOCATE SOUND TO OTHER
			GUILayout.Space(Row1);
			GUILayout.BeginHorizontal ();
			{	
				GUILayout.Space(Row1);
				myFMOD_EventScript.UpdatePositionToOther = GUILayout.Toggle(myFMOD_EventScript.UpdatePositionToOther, "Localize to another object");
			}
			GUILayout.EndHorizontal ();
			if(myFMOD_EventScript.UpdatePositionToOther){
				GUILayout.BeginHorizontal ();
				{	
					GUILayout.Space(Row2);
					myFMOD_EventScript.OtherPositionObject = (GameObject)EditorGUILayout.ObjectField (myFMOD_EventScript.OtherPositionObject, typeof(GameObject), false);
				}
				GUILayout.EndHorizontal ();
			}


			////////////////////////////////////////////////////// PARAMETERS  
			GUILayout.Space(Row1);
			
			GUILayout.BeginHorizontal ();
			{	
				GUILayout.Space(Row1);
				myFMOD_EventScript.UseParameterChange = GUILayout.Toggle(myFMOD_EventScript.UseParameterChange, "Enable Parameter control");
			}
			GUILayout.EndHorizontal ();

			if(myFMOD_EventScript.UseParameterChange){

				GUILayout.BeginHorizontal ();
				{	
					GUILayout.Space(Row2);
					GUILayout.Label ("Parameter Controls");

				}
				GUILayout.EndHorizontal ();
				//foreach(FMOD.Studio.ParameterInstance paramInst in myFMOD_EventScript.parameterInstance){
				for(int i=0; i<myFMOD_EventScript.numOfParams; i++){
					GUILayout.BeginHorizontal ();
					{	
						GUILayout.Space(Row2);//OLD MADS STUFF BELOW:
											//myFMOD_EventScript.ParameterName = GUILayout.TextField(myFMOD_EventScript.ParameterName);
											//myFMOD_EventScript.NewParameterValue = EditorGUILayout.Slider(myFMOD_EventScript.NewParameterValue, 0.0f, 1.0f);
						myFMOD_EventScript.parameterDescription[i].name = GUILayout.TextField(myFMOD_EventScript.parameterDescription[i].name);
						//I think this might need revision possibly
						myFMOD_EventScript.NewParameterValue = EditorGUILayout.Slider(myFMOD_EventScript.NewParameterValue, myFMOD_EventScript.parameterDescription[i].minimum, myFMOD_EventScript.parameterDescription[i].maximum);
					}
					GUILayout.EndHorizontal ();
				//}
				}

				GUILayout.Label ("Note: Parameter is changed through calling the XXX(); function");
				GUILayout.Space(Row1);

			}
		}
	}

}
