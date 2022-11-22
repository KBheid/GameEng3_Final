using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDroppable
{
	public bool CanReceiveDrop(Draggable dragged);
	public void ReceiveDrop(Draggable dragged);
}
