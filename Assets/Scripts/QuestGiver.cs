using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
    using UnityEditor;
#endif

[ExecuteInEditMode]
public class QuestGiver : MonoBehaviour
{
    public int minEffects;
    public int maxEffects;

    [Header("Read Only")]
    [SerializeField]
    private List<Effect> effects;

    [SerializeField]
    private List<Effect> currentQuestRequirements;

    // Start is called before the first frame update
    void Start()
    {
        GenerateNewQuest();
    }

    // Update is called once per frame
    void Update()
    {
		#region Auto Assign values
        #if UNITY_EDITOR
		    string[] assetNames = AssetDatabase.FindAssets("", new[] { "Assets/GameData/Effects" });
            effects = new List<Effect>();

            foreach (string s in assetNames)
            {
                var SOpath = AssetDatabase.GUIDToAssetPath(s);
                Effect e = AssetDatabase.LoadAssetAtPath<Effect>(SOpath);
                if (e.questable)
                    effects.Add(e);
            }
        #endif
        #endregion
    
    }

    public void GenerateNewQuest()
	{
        currentQuestRequirements = new List<Effect>();

        List<Effect> validEffects = new List<Effect>(effects);
        int numEffects = Random.Range(minEffects, maxEffects+1);

        for (int i=0; i<numEffects; i++)
		{
            Effect newEffect = validEffects[Random.Range(0, validEffects.Count)];
            
            foreach (Effect e in effects)
			{
                // Remove now invalid effects
                if (validEffects.Contains(e) && (e.negateEffect == newEffect || e.reverseEffect == newEffect))
                    validEffects.Remove(e);
			}

            currentQuestRequirements.Add(newEffect);

        }
	}

    public bool TryTurnIn(List<Effect> effects)
	{
        if (currentQuestRequirements.Count != effects.Count)
            return false;

        for (int i=0; i<effects.Count; i++)
		{
            if (currentQuestRequirements[i] != effects[i])
                return false;
		}

        GenerateNewQuest();

        return true;
	}

    public List<Effect> GetQuestRequirements()
	{
        return currentQuestRequirements;
	}
}
