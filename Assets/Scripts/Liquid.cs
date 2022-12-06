using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Liquid
{

	public Color color;
	private List<Effect> effects;


	public List<Effect> GetEffects()
	{
		return effects;
	}

	public void SetEffects(List<Effect> efs)
	{
		effects = efs;
		
		if (effects.Count > 0)
			color = effects[0].effectColor;

		foreach (Effect e in effects.GetRange(1, effects.Count-1))
		{
			color = Color.Lerp(color, e.effectColor, 0.9f);
		}
		color.a = 1f;
	}

	/// <summary>
	/// Adds a new effect to the potion.
	/// </summary>
	/// <returns>The state of the effect after addition.</returns>
	public EffectResult AddEffect(Effect e)
	{
		if (effects == null)
			effects = new List<Effect>();

		if (e.reverses && e.reverseIndex <= effects.Count-1)
		{
			ReverseEffect(e.reverseIndex);
			return EffectResult.CauseReverse;
		}

		if (effects.Count > 0 && effects[effects.Count-1].reverses)
		{
			// Remove the reverser
			effects.RemoveAt(effects.Count - 1);
			AddEffect(e);
			return EffectResult.Reversed;
		}

		for (int i = 0; i < effects.Count; i++)
		{
			if (effects[i] == e.negateEffect)
			{
				NegateEffect(i);
				return EffectResult.Negated;
			}
		}

		color = Color.Lerp(color, e.effectColor, 0.9f);
		color.a = 1f;
		effects.Add(e);
		return EffectResult.Added;
	}

	public List<EffectResult> AddEffects(List<Effect> efs)
	{
		List<EffectResult> ret = new List<EffectResult>();
		foreach (Effect e in efs)
		{
			ret.Add(AddEffect(e));
		}

		return ret;
	}

	private void NegateEffect(int index)
	{
		effects.RemoveAt(index);
	}

	private void ReverseEffect(int index)
	{
		effects[index] = effects[index].reverseEffect;
	}

	public override bool Equals(object other)
	{
		if (!(other is Liquid))
			return false;

		Liquid otherLiq = other as Liquid;

		if (otherLiq.effects.Count != effects.Count)
			return false;
		

		for (int i = 0; i < effects.Count; i++)
		{
			if (effects[i] != otherLiq.effects[i])
				return false;
		}

		return true;
	}
	public override int GetHashCode()
	{
		int hashCode = 607782651;
		hashCode = hashCode * -1521134295 + base.GetHashCode();
		hashCode = hashCode * -1521134295 + EqualityComparer<List<Effect>>.Default.GetHashCode(effects);
		return hashCode;
	}
}
