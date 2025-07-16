using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static Database;
using static FoodSO;

public class CookingPanel : MonoBehaviour
{

    #region Instance

    public static CookingPanel Instance { get; private set; }
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
    [Header("Tab gameObject")]
    public GameObject blackScreen;
    [Header("Container")]
    public GameObject foodsContainer;
    public GameObject ingredientsContainer;
    [Header("PRefabs")]
    public GameObject foodPrefab;
    public GameObject ingredientPrefab;
    [Header("Variables")]
    public List<FoodSO> foodsList = new List<FoodSO>();
    public FoodPrefab selectedFood;
    private int currentStarFilter = 0;
    private bool isCooking, isDone = false;
    [SerializeField]
    private float scrollStep = 0.1f;
    [Header("GameObject")]
    public GameObject cookingButton;
    public GameObject potGOJ;
    public GameObject filterTab;
    public List<GameObject> filterStar;
    public Slider energySlider;
    public ScrollRect foodScrollRect;
    [Header("Text")]
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI energyText;
    

    private PotAnimationController potAnimation;
    // Start is called before the first frame update
    void Start()
    {
        CloseCookingPanel();
        foodsList.AddRange(Database.Instance.foodsList);
        potAnimation = potGOJ.GetComponent<PotAnimationController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenCookingPanel()
    {
        blackScreen.SetActive(true);
        this.gameObject.SetActive(true);
        //UpdateScrollStep(foodsList.Count());
        SpawnFoods(foodsList);
        UpdateEnergySlider();
        //reset
        ClearSelectedFood();
        //Get Cooking time
        StartCookingTimer();
    }

    public void CloseCookingPanel()
    {
        blackScreen.SetActive(false);
        this.gameObject.SetActive(false);
        CloseFilterTab();
        currentStarFilter = 0;
    }

    private void ClearSelectedFood()
    {
        if (selectedFood != null)
        {
            selectedFood.DeSelectedFood();
            selectedFood = null;
        }
        foreach (Transform child in ingredientsContainer.transform)
        {
            Destroy(child.gameObject);
        }
        cookingButton.GetComponent<Button>().interactable = false;
    }

    private void SpawnFoods(List<FoodSO> foodsList)
    {
        foreach (Transform child in foodsContainer.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (FoodSO food in foodsList)
        {
            GameObject item = Instantiate(foodPrefab, foodsContainer.transform);
            item.GetComponent<FoodPrefab>().SetUp(food);
        }
    }
    private void SpawnIngredient(FoodSO food)
    {
        foreach (Transform child in ingredientsContainer.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (IngredientsList ingredient in food.ingredientsList)
        {
            GameObject item = Instantiate(ingredientPrefab, ingredientsContainer.transform);
            item.GetComponent<IngredientPrefab>().SetUp(ingredient);
        }
    }

    public void SelectFood(FoodPrefab newFood)
    {
        if (selectedFood == newFood) return;

        if (selectedFood != null)
            selectedFood.DeSelectedFood();

        selectedFood = newFood;
        SpawnIngredient(selectedFood.food);
        if (CheckIngredient(selectedFood.food) && isCooking == false)
        {
            cookingButton.GetComponent<Button>().interactable = true;
        }
        else
            cookingButton.GetComponent<Button>().interactable = false;


    }

    private bool CheckIngredient(FoodSO food)
    {
        bool result = true;
        if (GameMaster.Instance.energy < food.energyConsumption)
        {
            result = false;
            return result;
        }

        foreach (IngredientsList ingredient in food.ingredientsList)
        {
            int playerIngredient = GameMaster.Instance.GetPlayerIngredient(ingredient.ingredient);
            if (playerIngredient < ingredient.amounts)
            {
                result = false;
                break;
            }
        }

        return result;
    }

    public void ClickStartCooking()
    {
        
        DateTime endCooking = DateTime.Now.AddSeconds(selectedFood.food.timeConsumption);
        long endUnixTime = ((DateTimeOffset)endCooking).ToUnixTimeSeconds();
        if (GameMaster.Instance.RemovePlayerIngredientByFood(selectedFood.food))
        {
            PlayerPrefs.SetInt("CookingEndTime", (int)endUnixTime);
            PlayerPrefs.SetString("CookingFoodName", selectedFood.foodName.text);
            PlayerPrefs.Save();
            ClearSelectedFood();
            UpdateEnergySlider();
            StartCookingTimer();
        }

        
    }

    public void StartCookingTimer()
    {
        if (!PlayerPrefs.HasKey("CookingEndTime"))
        {
            timerText.text = "00:00";
            potAnimation.PlayAnimation(CookingState.Idle);
            return;
        }

        int endUnix = PlayerPrefs.GetInt("CookingEndTime");
        long currentUnixTime = ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds();
        long timeRemaining = (endUnix - currentUnixTime);

        if (timeRemaining <= 0)
        {
            timerText.text = "00:00";
            potAnimation.PlayAnimation(CookingState.Done);
            isDone = true;
            return;
        }
        potAnimation.PlayAnimation(CookingState.Cooking);

        StartCoroutine(CountdownTimer(timeRemaining));

    }

    IEnumerator CountdownTimer(float totalSeconds)
    {
        cookingButton.GetComponent<Button>().interactable = false;
        isCooking = true;
        isDone = false;
        while (totalSeconds > 0f)
        {
            int minutes = (int)(totalSeconds / 60);
            int seconds = (int)(totalSeconds % 60);

            string timeFormatted = string.Format("{0:D2}:{1:D2}", minutes, seconds);
            timerText.text = timeFormatted;
            yield return null; // wait for next frame
            totalSeconds -= Time.deltaTime;
        }

        Debug.Log("Timer finished!");
        potAnimation.PlayAnimation(CookingState.Done);
        isDone = true;
    }

    public void GetFoodFromPot()
    {
        if (!isDone) return;
        potAnimation.PlayAnimation(CookingState.Succes);
        string foodName = PlayerPrefs.GetString("CookingFoodName");
        string message = "ยินดีด้วยคุณได้รับ " + foodName;
        MSGBox.Instance.CreateMSGBox(message);

        cookingButton.GetComponent<Button>().interactable = true;
        PlayerPrefs.DeleteKey("CookingEndTime");
        PlayerPrefs.DeleteKey("CookingFoodName");
        PlayerPrefs.Save();
        ClearSelectedFood();
        isCooking = false;
        isDone = false;

    }

    public void SearchFood(string searchText)
    {
        ClearSelectedFood();
        currentStarFilter = 0;
        ChangeFilterStarColour(currentStarFilter, "#EAD3D3");
        string lower = searchText.ToLower();
        List<FoodSO> filtered = foodsList
            .Where(p => p.foodName.ToLower().Contains(lower))
            .ToList();

        SpawnFoods(filtered);
    }

    public void SearchFoodByStar(int star)
    {
        ClearSelectedFood();
        if (currentStarFilter == star)
        {
            currentStarFilter = 0;
            SpawnFoods(foodsList);
            //change colour
            ChangeFilterStarColour(star, "#EAD3D3"); //Default
        } else
        {
            currentStarFilter = star;
            List<FoodSO> filtered = foodsList
            .Where(p => p.foodStar == star)
            .ToList();

            SpawnFoods(filtered);
            ChangeFilterStarColour(star, "#6D6D6D");//toggle
        }
    }

    private void ChangeFilterStarColour(int star, string hexCode, string defaultHexCode = "#EAD3D3")
    {
        UnityEngine.Color color;
        UnityEngine.Color defaultColor;
        UnityEngine.ColorUtility.TryParseHtmlString(hexCode, out color);
        UnityEngine.ColorUtility.TryParseHtmlString(defaultHexCode, out defaultColor);
        
        foreach(GameObject filStar in filterStar)
        {
            filStar.GetComponent<Image>().color = defaultColor;
        }
        switch (star)
        {
            case 3:
                filterStar[2].GetComponent<Image>().color = color;
                break;
            case 2:
                filterStar[1].GetComponent<Image>().color = color;
                break;
            case 1:
                filterStar[0].GetComponent<Image>().color = color;
                break;
        }
    }

    public void ShowFilterTab()
    {
        if (filterTab.activeSelf)
        {
            CloseFilterTab();
            return;
        }

        filterTab.SetActive(true);
    }
    public void CloseFilterTab()
    {
        filterTab.SetActive(false);
    }

    public void UpdateEnergySlider()
    {
        int newEnergy = GameMaster.Instance.energy;
        int maxEnergy = GameMaster.Instance.maxEnergy;
        string energyString = string.Format("{0:D2}/{1:D2}", newEnergy, maxEnergy);
        energyText.text = energyString;
        energySlider.value = (float)newEnergy/maxEnergy;
    }

    private void UpdateScrollStep(int foodListCount)
    {
        int pages = Mathf.CeilToInt((float)foodListCount / 4);
        //scrollStep = pages <= 1 ? 0f : 430 / (foodsContainer.GetComponent<RectTransform>().rect.width - 430);
        StartCoroutine(UpdateScrollStepValue(pages));
    }

    IEnumerator UpdateScrollStepValue(int pages)
    {
        yield return null;
        scrollStep = pages <= 1 ? 0f : 430 / (foodsContainer.GetComponent<RectTransform>().rect.width);
    }

    public void ScrollLeft()
    {
        
        foodScrollRect.horizontalNormalizedPosition = Mathf.Clamp01(foodScrollRect.horizontalNormalizedPosition - scrollStep);
    }

    public void ScrollRight()
    {
        /*
        int pages = Mathf.CeilToInt((float)Database.Instance.foodsList.Count() / 4);
        scrollStep = pages <= 1 ? 0f : 1f / (pages - 1);*/
        foodScrollRect.horizontalNormalizedPosition = Mathf.Clamp01(foodScrollRect.horizontalNormalizedPosition + scrollStep);
    }

}
