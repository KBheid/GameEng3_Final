using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPlant", menuName = "PlantData", order = 1)]
public class PlantData : ScriptableObject
{
    public List<Effect> effects;
    public Sprite sprite;
}
