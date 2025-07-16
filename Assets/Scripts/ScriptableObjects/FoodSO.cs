using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "NewFood", menuName = "Foods/Food")]
public class FoodSO : ScriptableObject
{
    public string foodName;
    public int foodID, foodStar, energyConsumption = 10, timeConsumption = 10;
    public List<IngredientsList> ingredientsList = new List<IngredientsList>();
    public Sprite foodImage;

    [System.Serializable]
    public class IngredientsList
    {
        public IngredientSO ingredient;
        public int amounts;
    }

}
