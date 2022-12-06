using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectPanel : MonoBehaviour
{
	public GameObject effectUIPrefab;
	private List<GameObject> currentEffects;

	private void Start()
	{
		currentEffects = new List<GameObject>();
	}

	public void SetEffects(List<Effect> newEffects)
	{
		foreach (GameObject go in currentEffects)
		{
			Destroy(go);
		}

		currentEffects.Clear();

		foreach (Effect e in newEffects)
		{
			GameObject go = Instantiate(effectUIPrefab, transform);
			go.GetComponentInChildren<Text>().text = e.effectName;
			currentEffects.Add(go);
		}

	}

	public void SetPositionToMouse()
	{
		StartCoroutine(nameof(SetPosToMouseAfterDelay));
	}

	private IEnumerator SetPosToMouseAfterDelay()
	{
		yield return new WaitForEndOfFrame();

		transform.position = Input.mousePosition;
		transform.position += Vector3.up * GetComponent<RectTransform>().rect.height/2;
	}
}
