using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Locator
{
	static AudioPlayer audio;
	static EffectPanel effectPanel;
	static QuestGiver questGiver;

	public static AudioPlayer GetAudioPlayer()
	{
		if (audio == null)
			audio = Object.FindObjectOfType<AudioPlayer>();

		return audio;
	}

	public static EffectPanel GetEffectPanel()
	{
		if (effectPanel == null)
			effectPanel = Object.FindObjectOfType<EffectPanel>();

		return effectPanel;
	}

	public static QuestGiver GetQuestGiver()
	{
		if (questGiver == null)
			questGiver = Object.FindObjectOfType<QuestGiver>();

		return questGiver;
	}
}
