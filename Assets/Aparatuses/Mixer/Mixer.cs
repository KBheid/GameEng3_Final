using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mixer : MonoBehaviour, ILiquidEmitter, IDroppable, IEffectHolder
{
	[SerializeField] Animator anim;
	[SerializeField] SpriteRenderer sprite;

	public float mixTime;
	private float timeMixed;
	private Liquid currentLiquid;
	private bool hasLiquid = false;
	private bool playedMixNoise = false;
	private int stage;
	private readonly int stageCount = 4;
	private IngredientData data;

	private ILiquidReceiver receiver;

	public bool CanReceiveDrop(Draggable dragged)
	{
		return dragged.isPlant && !hasLiquid;
	}

	public void ReceiveDrop(Draggable dragged)
	{
		Destroy(dragged.gameObject);
		data = dragged.GetComponent<Ingredient>().data;
		hasLiquid = true;
		playedMixNoise = false;
		timeMixed = 0f;
		stage = 1;

		Locator.GetAudioPlayer().PlayDrop();

		currentLiquid = new Liquid();
		currentLiquid.SetEffects(data.effects);
		sprite.color = currentLiquid.color;

		SetState();
	}

	public void SetReceiver(ILiquidReceiver receiver)
	{
		this.receiver = receiver;
	}

	public bool StartEmitting(Liquid l)
	{
		if (receiver == null)
			return false;

		if (receiver.BeginReceiving(l))
		{
			data = null;
			hasLiquid = false;
			timeMixed = 0f;
			stage = 0;

			sprite.color = Color.white;

			SetState();

			return true;
		}
		return false;
	}

    // Update is called once per frame
    void Update()
    {
		if (hasLiquid)
		{
			timeMixed += Time.deltaTime;
			stage = Mathf.Min((int)(timeMixed / mixTime * (stageCount - 1)), stageCount - 1) + 1; // Stage 1-4

			if (stage >= 4)
			{
				if (!playedMixNoise)
				{
					playedMixNoise = true;
					Locator.GetAudioPlayer().PlayMix();
				}
				StartEmitting(currentLiquid);
			}
		}

		SetState();
    }

	void SetState()
	{
		anim.SetBool("HasLiquid", hasLiquid);
		anim.SetInteger("State", stage);
	}

	public List<Effect> GetEffects()
	{
		if (!hasLiquid)
			return null;

		return currentLiquid.GetEffects();
	}

	public List<ILiquidReceiver> GetReceivers()
	{
		return (receiver == null) ? null : new List<ILiquidReceiver>
		{
			receiver
		};
	}
}
