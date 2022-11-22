using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Locator
{
	static AudioPlayer audio;


	public static AudioPlayer GetAudioPlayer()
	{
		if (audio == null)
			audio = Object.FindObjectOfType<AudioPlayer>();

		return audio;
	}
}
