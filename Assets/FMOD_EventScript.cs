using UnityEngine;
using System.Collections;
using FMOD.Studio;

/// <summary>
/// Enum used for Stopping the FMOD Asset Event.
/// </summary>
public enum StopModeMethod
{
	StopInstantly, AllowFadeOut, StopAndForget
}

public enum PlayModeMethod
{
	PlayAtPosition, PlayAndFollow
}

/// <summary>
/// Used for start a FMOD studio Event. Only one can be used at one gameobject.
/// </summary>
public class FMOD_EventScript : MonoBehaviour {
	
	public FMODAsset ObjectStrange;
	FMOD.Studio.EventInstance ObjectStrangeEvent;
	FMOD.Studio.ParameterInstance ObjectStrangeParameter;
	
	public bool StartAutomatically = false;
	public bool UseParameterChange = false;
	public string ParameterName = "Write the name here";
	public float NewParameterValue = 0.0f;
	public bool UpdatePositionConstantly = false;
	public bool UpdateOtherPositionConstantly = false;
	public bool UpdatePositionToOther = false;
	public GameObject OtherPositionObject;

	//Jacoberino stuffs that's not finished
	public int numOfParams;
	public PARAMETER_DESCRIPTION[] parameterDescription;
	public FMOD.Studio.ParameterInstance[] parameterInstance;
	//End of danger zone

	private float originalParameterValue;
	private FMOD.Studio.ATTRIBUTES_3D position;
	
	private Vector3 EmitterPosition;

	void Start(){
		// INSTANTIATE THE EVENTS
		if (ObjectStrange != null) {
			this.ObjectStrangeEvent = FMOD_StudioSystem.instance.GetEvent (this.ObjectStrange);
		}
		
		if(StartAutomatically){
			if(UpdatePositionConstantly){
				FMOD_Play(PlayModeMethod.PlayAndFollow);
			}else{
				FMOD_Play(PlayModeMethod.PlayAtPosition);
			}

		}
		if(UseParameterChange){

			ObjectStrangeEvent.getParameterCount(out numOfParams);//Get the number of parameters in the event instance
			parameterInstance = new ParameterInstance[numOfParams];
			parameterDescription = new PARAMETER_DESCRIPTION[numOfParams];

			for(int i=0; i<numOfParams; i++){//run through all parameters and get descriptions of them
				ObjectStrangeEvent.getParameterByIndex(i, out parameterInstance[i]);
				parameterInstance[i].getDescription(out parameterDescription[i]);
				//print ("Param: '"+parameterDescription[i].name+"', minVal: "+parameterDescription[i].minimum+", maxVal: "+parameterDescription[i].maximum);//this should be rendered in the editor mads pls
			}
			//OLD MADS STUFF
			//ObjectStrangeEvent.getParameter(ParameterName, out ObjectStrangeParameter);
			//ObjectStrangeParameter.getValue(out originalParameterValue);
		}
		
	}
	
	void Update(){


		// CONTROLLING THE POSITION OF THE SOUND
		if (UpdatePositionToOther) {
			EmitterPosition = this.OtherPositionObject.transform.position;
		} else {
			EmitterPosition = this.transform.position;
		}

		if (UpdatePositionConstantly) { 
			ObjectStrangeEvent.set3DAttributes(FMOD.Studio.UnityUtil.to3DAttributes(this.EmitterPosition));
		}


//		if(Input.GetKeyDown(KeyCode.H)){
//			ObjectStrangeParameter.setValue(NewParameterValue);
//		}
//		
//		if (Input.GetKeyUp (KeyCode.H)) {
//			ObjectStrangeParameter.setValue(originalParameterValue);
//		}
//		
//		if(Input.GetKeyDown(KeyCode.E)){
//			FMOD_Stop(StopModeMethod.AllowFadeOut);
//		}
	}
	
	/// <summary>
	/// If this script is not started automatically, this function starts the FMOD Asset event connected to it. Can only be started once.
	/// </summary>
	public void FMOD_Play(PlayModeMethod playAction){
		if(playAction == PlayModeMethod.PlayAtPosition){
			this.ObjectStrangeEvent.start ();
		}
		if(playAction == PlayModeMethod.PlayAndFollow){
			this.ObjectStrangeEvent.start ();
			UpdatePositionConstantly = true;
		}
	}
	
	/// <summary>
	/// Stops the FMOD Asset Event. 
	/// Modes: 
	/// StopInstantly just stops the event. 
	/// StopAndForget stops and discards the event so you wont be able to start it again. 
	/// AllowFadeOut: Allows you to fade it out with e.g. the AHDSR. 
	/// </summary>
	/// <param name="stopAction">Stop action.</param>
	public void FMOD_Stop(StopModeMethod stopAction){
		
		if(stopAction == StopModeMethod.StopInstantly || stopAction == StopModeMethod.StopAndForget){
			this.ObjectStrangeEvent.stop (STOP_MODE.IMMEDIATE);
		}
		if(stopAction == StopModeMethod.AllowFadeOut){
			this.ObjectStrangeEvent.stop (STOP_MODE.ALLOWFADEOUT);
		}
		if(stopAction == StopModeMethod.StopAndForget){
			this.ObjectStrangeEvent.release();
		}
	}
}
