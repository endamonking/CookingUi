using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using static Database;
using static FoodSO;

public class GameMaster : MonoBehaviour
{
    #region Instance

    public static GameMaster Instance { get; private set; }

    void Awake()
    {

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    #endregion

    public int energy = 0, maxEnergy = 30;
    public List<PlayerIngredient> playerIngredients = new List<PlayerIngredient>();


    // Start is called before the first frame update
    void Start()
    {
        energy = PlayerPrefs.GetInt("playerEnergy", maxEnergy);
        playerIngredients.AddRange(Database.Instance.playerIngredients);
        StartCoroutine(EnergyTimer());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetPlayerIngredient(IngredientSO targetIngredient)
    {
        int ingredientAmount = 0;
        PlayerIngredient ingredient = playerIngredients.Find(item => item.ingredient == targetIngredient);

        if (ingredient == null)
            Debug.LogError("Can't find ingredient"); 
        else
            ingredientAmount = ingredient.amount;

        return ingredientAmount;
    }

    public bool RemovePlayerIngredientByFood(FoodSO food)
    {
        foreach (IngredientsList fi in food.ingredientsList)
        {
            foreach (PlayerIngredient p in playerIngredients)
            {
                if (p.ingredient == fi.ingredient)
                {
                    //error case
                    if (fi.amounts > p.amount)
                    {
                        Debug.LogError("player has ingredient not met requirment");
                        return false;
                    }
                    p.amount = p.amount - fi.amounts;
                    if (p.amount < 0)
                        Debug.LogError("player item be negative");
                    break;
                }
            }
        }

        energy = energy - food.energyConsumption;
        return true;
    }

    IEnumerator EnergyTimer()
    {
        while (true)
        {
            if (energy < maxEnergy)
            {
                yield return new WaitForSeconds(5f);
                energy++;
                if (CookingPanel.Instance != null)
                    CookingPanel.Instance.UpdateEnergySlider();
            }
            else
            {
                yield return null; 
            }
        }

    }

    void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("playerEnergy", energy);
        PlayerPrefs.Save();
        Debug.Log("Saved energy at " + energy);
    }
}
