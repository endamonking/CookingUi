using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FoodPrefab : MonoBehaviour
{

    public Image foodImage, whiteBG;
    public TextMeshProUGUI foodName;
    public Transform starContainer;
    public FoodSO food;

    public void SetUp(FoodSO food)
    {
        this.food = food;
        foodName.text = food.foodName;
        foodImage.sprite = food.foodImage;
        SetUpStar(food.foodStar);
        whiteBG.enabled = false;
            
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    private void SetUpStar(int totalStar)
    {
        int indexCount = 0;
        foreach (Transform child in starContainer)
        {
            child.gameObject.SetActive(false);
        }
        foreach (Transform child in starContainer)
        {
            if (indexCount >= totalStar)
                break;
            child.gameObject.SetActive(true);
            indexCount++;
        }

    }
    //clikcFuntion
    public void OnClicked()
    {
        if (CookingPanel.Instance == null) return;
        CookingPanel.Instance.SelectFood(this);
        whiteBG.enabled = true;
    }
    public void DeSelectedFood()
    {
        whiteBG.enabled = false;
    }
}
