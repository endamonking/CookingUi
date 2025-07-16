using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MSGTextBox : MonoBehaviour
{

    public TextMeshProUGUI messageTMP;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void SetupMSGBox(string text)
    {
        messageTMP.text = text;
    }

    public void CloseBox()
    {
        MSGBox.Instance.blackBG.SetActive(false);
        this.gameObject.SetActive(false);
        Destroy(this.gameObject,1f);
    }

}
