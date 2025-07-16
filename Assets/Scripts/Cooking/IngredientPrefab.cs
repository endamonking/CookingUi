using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static FoodSO;

public class IngredientPrefab : MonoBehaviour
{

    public Image ingredientImage;
    public TextMeshProUGUI ingredientAmount;

    public void SetUp(IngredientsList ingredient)
    {
        int amount = GameMaster.Instance.GetPlayerIngredient(ingredient.ingredient);
        ingredientImage.sprite = ingredient.ingredient.ingredientImage;
        if (amount <= 0)
        {
            ingredientAmount.text = $"<color=red>{amount.ToString()}</color>/" + ingredient.amounts.ToString();
        } else
            ingredientAmount.text = $"{amount.ToString()}/" + ingredient.amounts.ToString();

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
