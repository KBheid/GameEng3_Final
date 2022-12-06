using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestReader : MonoBehaviour
{
    [SerializeField]
    GameObject textPrefab;

    List<GameObject> effects;
    List<Effect> lastRequirements;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        List<Effect> requirements = Locator.GetQuestGiver().GetQuestRequirements();
        if (requirements != lastRequirements)
		{
            lastRequirements = requirements;

            if (effects != null)
                foreach (GameObject go in effects)
                    Destroy(go);

            effects = new List<GameObject>();

            foreach (Effect e in requirements)
			{
                GameObject go = Instantiate(textPrefab, transform);
                go.GetComponentInChildren<Text>().text = e.effectName;
                effects.Add(go);
			}
		}
    }
}
