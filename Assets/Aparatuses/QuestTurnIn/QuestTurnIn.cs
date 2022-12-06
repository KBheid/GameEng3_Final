using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestTurnIn : MonoBehaviour, ILiquidReceiver
{

	ILiquidEmitter emitter;

	public bool BeginReceiving(Liquid l)
	{
		bool success = Locator.GetQuestGiver().TryTurnIn(l.GetEffects());

		Locator.GetAudioPlayer().PlayQuestFinish(success);

		return true;
	}

	public List<ILiquidEmitter> GetEmitters()
	{
		return (emitter == null) ? null : new List<ILiquidEmitter>
		{
			emitter
		};
	}

	public void SetEmitter(ILiquidEmitter emitter)
	{
		this.emitter = emitter;
	}

}
