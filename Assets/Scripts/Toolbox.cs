using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toolbox : MonoBehaviour
{
	public GameObject currentTool;

	public GameObject MixerPrefab;
	public GameObject TowerPrefab;
	public GameObject ReservoirPrefab;
	public GameObject CombinerPrefab;

	public void SetTool_CondenserTower()
	{
		if (currentTool != null)
			Destroy(currentTool);

		currentTool = Instantiate(TowerPrefab);
	}
	public void SetTool_Reservoir()
	{
		if (currentTool != null)
			Destroy(currentTool);

		currentTool = Instantiate(ReservoirPrefab);
	}
	public void SetTool_Mixer()
	{
		if (currentTool != null)
			Destroy(currentTool);

		currentTool = Instantiate(MixerPrefab);
	}

	public void SetTool_Combiner()
	{
		if (currentTool != null)
			Destroy(currentTool);

		currentTool = Instantiate(CombinerPrefab);
	}

	private void Update()
	{
		if (currentTool != null)
		{
			Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			pos.x = Mathf.Round(pos.x);
			pos.y = Mathf.Round(pos.y);
			pos.z = 0;
			currentTool.transform.position = pos;

			if (Input.GetMouseButtonDown(0))
				currentTool = null;
		}
	}
}
