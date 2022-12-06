using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    [SerializeField] AudioSource ingredient_mix;
    [SerializeField] AudioSource ingredient_drop;
    [SerializeField] AudioSource condenser_complete;
	[SerializeField] AudioSource quest_complete;
	[SerializeField] AudioSource quest_fail;

    public void PlayMix()
	{
		if (!ingredient_mix.isPlaying)
			ingredient_mix.Play();
	}

    public void PlayDrop()
	{
		if (!ingredient_drop.isPlaying)
			ingredient_drop.Play();
	}

    public void PlayCondenser()
	{
		if (!condenser_complete.isPlaying)
			condenser_complete.Play();
	}

	public void PlayQuestFinish(bool success)
	{
		if (success)
		{
			if (!quest_complete.isPlaying)
				quest_complete.Play();
		}
		else
		{
			if (!quest_fail.isPlaying)
				quest_fail.Play();
		}
	}
}
