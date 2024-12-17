using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class CowboyText : MonoBehaviour
{
    public TMP_Text cowBoyText;
    public Button infoButton;
    // Start is called before the first frame update
    void Start()
    {
        infoButton.onClick.AddListener(OnInfoButtonClicked);
        switch (GameManager.currentLevel)
        {
            case 2:
                StartCoroutine(DisplaySpech("This place seems familiar"));
                break;
            case 3:
                StartCoroutine(DisplaySpech("Here AGAIN!!??"));
                break;
            case 4:
                StartCoroutine(DisplaySpech("Am I stuck here forever?"));
                break;
            case 5:
                StartCoroutine(DisplaySpech("......."));
                break;
        }
    }

     private IEnumerator DisplaySpech(String speech)
    {
        cowBoyText.gameObject.SetActive(true);
        cowBoyText.text = speech;
        yield return new WaitForSeconds(3);
        cowBoyText.gameObject.SetActive(false);
    }

    void OnInfoButtonClicked()
    {
        Application.OpenURL("https://docs.google.com/document/d/1Wha1rziv6SlfMnradr14qybvsZy2kzAx2ouqb8rDGxY/edit?usp=sharing");
    }
}
