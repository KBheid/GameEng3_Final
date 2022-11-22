using System.Collections.Generic;
using UnityEngine;

public class LiquidIOSetter : MonoBehaviour
{
	#region Interface override
	[SerializeField]
	[RequireInterface(typeof(ILiquidEmitter))]
	Object Emitter;
	[SerializeField]
	[RequireInterface(typeof(ILiquidReceiver))]
	Object Receiver;
	#endregion

	ILiquidEmitter output => Emitter as ILiquidEmitter;
	ILiquidReceiver input => Receiver as ILiquidReceiver;

	public Switch switchPrefab;
	public List<Transform> inputLocations;
	public List<Transform> outputLocations;

	private bool createdInput = false;
	private bool createdOuput = false;


	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.I) && !createdInput)
		{
			RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, distance: 100f, layerMask: 1 << 6);
			if (hit.collider != null && hit.collider.gameObject == gameObject)
			{
				foreach (Transform inputLocation in inputLocations)
				{
					Switch sw = Instantiate(switchPrefab);
					input.SetEmitter(sw);
					sw.Receiver = gameObject.GetComponent<ILiquidReceiver>() as Object;
					sw.transform.position = inputLocation.position;
					sw.transform.rotation = inputLocation.rotation;

					createdInput = true;
				}
			}
		}

		if (Input.GetKeyDown(KeyCode.O) && !createdOuput)
		{

			RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, distance: 100f, layerMask:  1 << 6);
			if (hit.collider != null && hit.collider.gameObject == gameObject)
			{

				foreach (Transform outputLocation in outputLocations)
				{
					Switch sw = Instantiate(switchPrefab);
					output.SetReceiver(sw);
					sw.Emitter = gameObject.GetComponent<ILiquidEmitter>() as Object;
					sw.transform.position = outputLocation.position;
					sw.transform.rotation = outputLocation.rotation;

					createdOuput = true;
				}
			}
		}
	}
}
