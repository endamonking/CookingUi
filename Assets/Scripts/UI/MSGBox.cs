using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Spine.Unity.Examples.SpineboyFootplanter;

public class MSGBox : MonoBehaviour
{
    #region Instance

    public static MSGBox Instance { get; private set; }
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
    [Header("GmaeObject")]
    public GameObject blackBG;
    [Header("Prefab")]
    public GameObject msgBoxPrefab;
    // Start is called before the first frame update
    void Start()
    {
        blackBG.SetActive(false);
    }

    public void CreateMSGBox(string message)
    {
        blackBG.SetActive(true);
        GameObject box = Instantiate(msgBoxPrefab, this.gameObject.transform);
        box.GetComponent<MSGTextBox>().SetupMSGBox(message);
    }


}
