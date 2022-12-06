using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectViewer : MonoBehaviour
{
    private bool hovering = false;
    private EffectPanel panel;

	private void Start()
	{
        panel = Locator.GetEffectPanel();
	}

	// Update is called once per frame
	void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit.collider != null && hit.collider.GetComponent<IEffectHolder>() != null)
        {
            List<Effect> effects = hit.collider.GetComponent<IEffectHolder>().GetEffects();

            if (effects == null)
                return;

            if (!hovering)
			{
                hovering = true;

                panel.gameObject.SetActive(true);
                panel.SetEffects(effects);
                panel.SetPositionToMouse();
            }

        }
        else
        {
            panel.gameObject.SetActive(false);
            hovering = false;
		}
    }

}
