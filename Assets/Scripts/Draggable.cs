using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable : MonoBehaviour
{
    public bool isPlant;
    public bool isPowder;
    public bool isPotion;
    public bool isEmptyBottle;

    private bool beingDragged;

    // Update is called once per frame
    void Update()
    {
        if (beingDragged)
		{
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0;
            transform.position = pos;

            if (Input.GetMouseButtonUp(0))
			{
                beingDragged = false;

                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (hit.collider != null && hit.collider.gameObject.TryGetComponent(out IDroppable droppable))
				{
                    if (droppable.CanReceiveDrop(this))
					{
                        droppable.ReceiveDrop(this);
					}
				}
            }
		}
        else
		{
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (hit.collider != null && hit.collider.gameObject == gameObject)
				{
                    beingDragged = true;
				}
            }
		}
    }

    public void StartDragging()
	{
        beingDragged = true;
	}
    public void StopDragging()
	{
        beingDragged = false;
	}
}
