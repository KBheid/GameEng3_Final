using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Effect", menuName = "Effect", order = 1)]
public class Effect : ScriptableObject
{
	public string effectName;
	[Tooltip("What effect negates this one.")]
	public Effect negateEffect;
	[Tooltip("What effect this one becomes if reversed.")]
	public Effect reverseEffect;

	[Tooltip("Whether or not this effect will reverse another.")]
	public bool reverses;
	
	[Tooltip("Effect at what index on the receiving potion is reversed.")]
	public int reverseIndex;

	public Color effectColor;
}

public enum EffectResult
{
	Added,
	Negated,
	CauseReverse,
	Reversed
}