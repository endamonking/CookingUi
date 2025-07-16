using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewIngredient", menuName = "Foods/Ingredient")]
public class IngredientSO : ScriptableObject
{
    public string ingredientName;
    public int id;
    public Sprite ingredientImage;
}
