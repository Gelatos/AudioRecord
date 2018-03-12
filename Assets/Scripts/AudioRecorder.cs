using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class AudioRecorder : MonoBehaviour
{
	[SerializeField]
	private Dropdown dropdownOptions;
	[SerializeField]
	private Text recordButtonText;
	[SerializeField]
	private Text statusText;
	[SerializeField]
	private InputField input;
	[SerializeField]
	private AudioSource aSource;
	[SerializeField]
	private Animator savePopup;

	private string defaultName = "MyAudio_";
	private int defaultNameIncrementer = 1;

	private string fileName;
	private string chosenAudioDevice = "";
	private AudioClip recordingClip;
	private bool isRecording;

	#region Mono Functions

	private void Start()
	{
		List<string> options = new List<string> ();
		foreach (string option in Microphone.devices) {
			options.Add (option);
		}
		dropdownOptions.ClearOptions();
		dropdownOptions.AddOptions (options);
	}

	private void Update()
	{
		if (Input.GetKeyDown (KeyCode.Space)) {
			if (isRecording) 
			{
				StopRecording ();
			} else 
			{
				StartRecording ();
			}
		}
	}

	#endregion

	#region Recording Functions

	private void StartRecording()
	{
		// create a new clip
		recordingClip = new AudioClip();
		recordingClip = Microphone.Start(chosenAudioDevice, true, 600, 44100);
		isRecording = true;

		// set ui
		statusText.text = "Recording";
		recordButtonText.text = "Stop";

		// stop audio
		aSource.Stop();
	}

	private void StopRecording()
	{
		if (isRecording) 
		{
			Microphone.End (chosenAudioDevice);
			isRecording = false;

			// set ui
			statusText.text = "Waiting";
			recordButtonText.text = "Start Recording";
		}
	}

	private void PlayAudio()
	{
		if (!isRecording) {
			aSource.clip = recordingClip;
			aSource.Play ();

			// set ui
			statusText.text = "Playing";
		}
	}

	private void SaveAudio()
	{
		if (!isRecording) {
			defaultNameIncrementer++;
			SavWav.Save (input.text, recordingClip);

			// set ui
			statusText.text = "Waiting";
		}
	}

	#endregion

	#region On Click Functions

	public void OnClickStartRecordingBTN()
	{
		if (isRecording) 
		{
			StopRecording ();
		} else 
		{
			StartRecording ();
		}
	}

	public void OnClickStopRecordingBTN()
	{
		StopRecording ();
	}

	public void OnClickPlayBTN()
	{
		PlayAudio ();
	}

	public void OnClickSaveBTN()
	{
		input.text = defaultName + defaultNameIncrementer;
		savePopup.SetBool ("active", true);
	}

	public void OnClickSaveAudioBTN()
	{
		SaveAudio ();
		savePopup.SetBool ("active", false);
	}

	public void OnClickClosePopupBTN()
	{
		savePopup.SetBool ("active", false);
	}

	#endregion
}
