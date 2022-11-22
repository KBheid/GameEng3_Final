using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CondenserTower : MonoBehaviour, ILiquidEmitter, ILiquidReceiver
{
	public float cookTime = 5;
	public int animationCyclesPerCook = 3;
	public int maxLiquid = 5;
	public bool hasFuel = true;

	private float currentCookDuration;
	private int liquidAmount;

	private int stage = 0;
	private int numStages = 5; // Empty, Calm, Cook 1, Cook 2, Cook 3
	private bool[] emitted = { false, false, false };
	private bool soundPlayed = false;

	ILiquidReceiver[] receivers = new ILiquidReceiver[3];
	private int numInitialized = 0;
	ILiquidEmitter emitter;

	[Header("Liquid")]
	public Liquid currentLiquid;
	[SerializeField] Animator anim;
	[SerializeField] SpriteRenderer sprite;

	public bool BeginReceiving(Liquid l)
	{
		if (liquidAmount >= maxLiquid)
			return false;

		if (currentLiquid == null)
			currentLiquid = l;

		if (!l.Equals(currentLiquid))
			return false;

		liquidAmount++;
		SetState();

		return true;
	}

	public void SetEmitter(ILiquidEmitter emitter)
	{
		this.emitter = emitter;
	}

	public void SetReceiver(ILiquidReceiver receiver)
	{
		receivers[numInitialized] = receiver;
		numInitialized++;
	}

	public bool StartEmitting(Liquid l)
	{
		List<Effect>[] effects = {
			new List<Effect>(),
			new List<Effect>(),
			new List<Effect>(),
		};

		int i = 0;
		foreach (Effect eff in l.GetEffects())
		{
			effects[i].Add(eff);

			i = i+1 % 3;
		}

		bool complete = true;
		for (int i2=0; i<3; i2++)
		{
			if (!emitted[i2])
			{
				complete = false;

				Liquid newLiq = new Liquid();
				newLiq.AddEffects(effects[i2]);

				if (receivers[i2].BeginReceiving(newLiq))
				{

				}
			}
		}

		if (complete)
		{
			liquidAmount--;
			if (liquidAmount <= 0)
				currentLiquid = null;
			soundPlayed = false;
		}
		return complete;
	}


	void SetState()
	{
		anim.SetInteger("State", stage);

		if (currentLiquid != null)
			sprite.color = currentLiquid.color;
	}

    // Update is called once per frame
    void Update()
    {
		if (hasFuel && liquidAmount > 0 && currentCookDuration < cookTime)
		{
			currentCookDuration += Time.deltaTime;

			float stageTime = cookTime / animationCyclesPerCook / (numStages - 2);
			stage = Mathf.FloorToInt(currentCookDuration / stageTime) % (numStages - 2) + 2;
		}
		if (currentCookDuration >= cookTime)
		{
			stage = (liquidAmount > 0) ? 1 : 0;
			
			if (!soundPlayed)
			{
				Locator.GetAudioPlayer().PlayCondenser();
				soundPlayed = true;
			}
			
			StartEmitting(currentLiquid);
		} 
		else { 
			stage = (liquidAmount > 0) ? 1 : 0;
		}
		SetState();
	}
}
