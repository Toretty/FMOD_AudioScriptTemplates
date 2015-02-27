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
	public bool UpdatePositionToOther = false;
	public GameObject OtherPositionObject;
	
	private float originalParameterValue;
	private FMOD.Studio.ATTRIBUTES_3D position;

	void Start(){
		// INSTANTIATE THE EVENTS
		if (ObjectStrange != null) {
			this.ObjectStrangeEvent = FMOD_StudioSystem.instance.GetEvent (this.ObjectStrange);
		}
		
		if(StartAutomatically){
			FMOD_Play();
		}
		if(UseParameterChange){
			ObjectStrangeEvent.getParameter(ParameterName, out ObjectStrangeParameter);
			ObjectStrangeParameter.getValue(out originalParameterValue);
		}
		
	}
	
	void Update(){

		// CONTROLLING THE POSITION OF THE SOUND
		if (UpdatePositionToOther || UpdatePositionConstantly) {

			if(UpdatePositionConstantly){
				position = FMOD.Studio.UnityUtil.to3DAttributes(this.transform.position);
			}else{
				position = FMOD.Studio.UnityUtil.to3DAttributes(this.OtherPositionObject.transform.position);
			}

			ObjectStrangeEvent.set3DAttributes(position);
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
	public void FMOD_Play(){
		this.ObjectStrangeEvent.start ();
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
