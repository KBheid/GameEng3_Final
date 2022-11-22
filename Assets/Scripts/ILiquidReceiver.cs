using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILiquidReceiver
{
	/// <summary>
	/// Begin receiving a liquid on this apparatus.
	/// </summary>
	/// <param name="l"></param>
	/// <returns>Whether or not this receiver is ready to take a liquid.</returns>
	bool BeginReceiving(Liquid l);
	void SetEmitter(ILiquidEmitter emitter);
}
