using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILiquidEmitter
{
	bool StartEmitting(Liquid l);
	void SetReceiver(ILiquidReceiver receiver);

	List<ILiquidReceiver> GetReceivers();
}
