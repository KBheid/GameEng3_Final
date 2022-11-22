using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reservoir : MonoBehaviour, ILiquidReceiver, ILiquidEmitter
{ 
	#region Interface override
	[SerializeField]
	[RequireInterface(typeof(ILiquidEmitter))]
	Object Emitter;
	[SerializeField]
	[RequireInterface(typeof(ILiquidReceiver))]
	Object Receiver;
	#endregion

	ILiquidEmitter last => Emitter as ILiquidEmitter;
	ILiquidReceiver next => Receiver as ILiquidReceiver;

	[SerializeField] Animator anim;
	[SerializeField] SpriteRenderer liquidRenderer;

	public Liquid heldLiquid = null;
	public int numberOfLiquid = 0;

	private int maxLiquid = 10;

	private void Start()
	{
		InvokeRepeating(nameof(Emit), 0f, 1f);
		heldLiquid = null;
	}

	public bool BeginReceiving(Liquid l)
	{
		if (heldLiquid == null)
			heldLiquid = l;

		if (!l.Equals(heldLiquid))
			return false;

		if (numberOfLiquid >= maxLiquid)
			return false;

		numberOfLiquid++;
		UpdateState();

		return true;
	}

	private void Emit()
	{
		StartEmitting(heldLiquid);
	}
	public bool StartEmitting(Liquid l)
	{
		if (Receiver != null && heldLiquid != null)
		{
			if (next.BeginReceiving(l)) {
				numberOfLiquid--;
				UpdateState();

				if (numberOfLiquid <= 0)
					heldLiquid = null;

				return true;
			}
		}

		return false;
	}

	void UpdateState()
	{
		anim.SetInteger("State", numberOfLiquid);
		if (heldLiquid != null)
			liquidRenderer.color = heldLiquid.color;
	}

	public void SetEmitter(ILiquidEmitter emitter)
	{
		Emitter = emitter as Object;
	}

	public void SetReceiver(ILiquidReceiver receiver)
	{
		Receiver = receiver as Object;
	}
}
