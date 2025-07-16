using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Database : MonoBehaviour
{
    //This class for contain data like foods and ingredients since we don't server to store database so we store in here instead
    #region Instance
    public static Database Instance { get; private set; }
    void Awake()
    {

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); 
    }
    #endregion
    public List<FoodSO> foodsList;
    public List<PlayerIngredient> playerIngredients = new List<PlayerIngredient>();

    [System.Serializable]
    public class PlayerIngredient
    {
        public IngredientSO ingredient;
        public int amount = 0;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetPlayerIngredientAmount(IngredientSO targetIngredient)
    {

        foreach (PlayerIngredient pi in playerIngredients)
        {
            if (pi.ingredient == targetIngredient)
                return pi.amount;
        }

        return 0;
    }

}
