using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantShelf : MonoBehaviour
{
    [SerializeField]
    private List<Pot> pots;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
		{
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider == null)
                return;

            foreach (Pot p in pots)
			{
                if (hit.collider.gameObject == p.potCollider)
				{
                    GameObject go = new GameObject();
                    go.AddComponent<SpriteRenderer>().sprite = p.plant.sprite;
                    Draggable d = go.AddComponent<Draggable>();
                    d.isPlant = true;
                    go.AddComponent<Plant>().plantData = p.plant;
                    d.StartDragging();
				}
			}
		}
    }

    [Serializable]
    private struct Pot
	{
        public GameObject potCollider;
        public PlantData plant;
	}
}
