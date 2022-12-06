using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combiner : MonoBehaviour, ILiquidReceiver, ILiquidEmitter, IEffectHolder
{
    [SerializeField]
    Animator mainLiquidAnim;
    [SerializeField]
    Animator leftLiquidAnim;
    [SerializeField]
    Animator rightLiquidAnim;

	public float mixingTime = 5f;

	Liquid leftLiquid = null;
	Liquid rightLiquid = null;
	Liquid mainLiquid = null;

	bool hasMainLiquid = false;
	bool hasLeftLiquid = false;
	bool hasRightLiquid = false;
	bool mixing = false;
	bool mixed = false;

	float curMixingTime = 0;

	List<ILiquidEmitter> emitters;
	ILiquidReceiver receiver;

	public bool BeginReceiving(Liquid l)
	{
		if (hasLeftLiquid == false)
		{
			leftLiquid = l;
			leftLiquidAnim.gameObject.GetComponent<SpriteRenderer>().color = l.color;
			hasLeftLiquid = true;
			return true;
		}
		if (hasRightLiquid == false)
		{
			rightLiquid = l;
			rightLiquidAnim.gameObject.GetComponent<SpriteRenderer>().color = l.color;
			hasRightLiquid = true;
			mixing = true;
			curMixingTime = 0;
			hasMainLiquid = true;
			return true;
		}

		return !hasMainLiquid;
	}

	public List<Effect> GetEffects()
	{
		if (mainLiquid == null)
			return null;

		return mainLiquid.GetEffects();
	}

	public List<ILiquidEmitter> GetEmitters()
	{
		return emitters;
	}

	public List<ILiquidReceiver> GetReceivers()
	{
		return (receiver == null) ? null : new List<ILiquidReceiver>
		{
			receiver
		};
	}

	public void SetEmitter(ILiquidEmitter emitter)
	{
		if (emitters == null || emitters.Count >= 2)
			emitters = new List<ILiquidEmitter>();

		emitters.Add(emitter);
	}

	public void SetReceiver(ILiquidReceiver receiver)
	{
		this.receiver = receiver;
	}

	public bool StartEmitting(Liquid l)
	{
		if (receiver == null)
			return false;

		return receiver.BeginReceiving(l);
	}



    // Update is called once per frame
    void Update()
    {

		leftLiquidAnim.SetBool("HasLiquid", hasLeftLiquid);
		rightLiquidAnim.SetBool("HasLiquid", hasRightLiquid);
		mainLiquidAnim.SetBool("HasLiquid", hasMainLiquid);

		mainLiquidAnim.SetBool("Mixing", mixing);

		if (mixing)
			curMixingTime += Time.deltaTime;

		if (curMixingTime >= mixingTime)
		{
			mixing = false;

			if (!mixed)
			{
				mainLiquid = new Liquid();
				mainLiquid.AddEffects(rightLiquid.GetEffects());
				mainLiquid.AddEffects(leftLiquid.GetEffects());
				mixed = true;

				mainLiquidAnim.gameObject.GetComponent<SpriteRenderer>().color = mainLiquid.color;
			}

			if (StartEmitting(mainLiquid))
			{
				leftLiquid = null;
				rightLiquid = null;

				hasLeftLiquid = false;
				hasRightLiquid = false;
				hasMainLiquid = false;
				
				mixed = false;
				curMixingTime = 0;
			}
		}
    }

	
}
