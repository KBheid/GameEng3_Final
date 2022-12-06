using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewIngredient", menuName = "IngredientData", order = 1)]
public class IngredientData : ScriptableObject
{
    public List<Effect> effects;
    public Sprite sprite;
}
