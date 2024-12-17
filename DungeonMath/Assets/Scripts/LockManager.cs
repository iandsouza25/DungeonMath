using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LockManager : MonoBehaviour
{
    public CharacterController controller;
    public TMP_Text text;
    public GameObject[] slots; 
    private EquationGenerator equationgenerator;
    private List<string> solution = new List<string>();

    void Start() {
        equationgenerator = FindObjectOfType<EquationGenerator>();
    }

    public void UpdateInventory(){   
        for (int i = 0; i < slots.Length; i++) {
            if (slots[i].transform.childCount > 0) {
                TextMeshProUGUI keyText = slots[i].GetComponentInChildren<TextMeshProUGUI>();
                Debug.Log($"Slot {i} contains {keyText.text}");
            }
            else {
                Debug.Log($"Slot {i} is empty");
            }
        }
    }

    public void CheckSolution() {
        Debug.Log(GameManager.currentLevel);
        solution.Clear();
        List<string> partialEQ = equationgenerator.GetEquationParts();
        
        for (int i = 0; i < slots.Length; i++) {
            if (slots[i].transform.childCount > 0) {
                TextMeshProUGUI keyText = slots[i].GetComponentInChildren<TextMeshProUGUI>();
                solution.Add(keyText.text);
            }
        // foreach (string item in solution) {
        // Debug.Log(item);
        // }
        }
        if (solution.Contains("")) 
        {
            text.color = Color.white;
            text.text = "You're missing a Key!";
        }
        else {
            int j = 0;
            
            List<string> newList = new List<string>(partialEQ);
            for (int i = 0; i < newList.Count; i++) {
                if (newList[i] == "___"){
                    newList[i] = solution[j];
                    j++;
                }
            }
                foreach (string item in newList) {
            Debug.Log(item);
            }
            bool equationCorrect = equationgenerator.isEquationCorrect(newList);
            if (equationCorrect) {
                controller.enabled = false;

                Debug.Log("You won!");
                StartCoroutine(DisplayMessageAndLoadNextLevel());
            }
            else {
                text.color = Color.red;
                text.text = "INCORRECT! Try Again!";
            }
        }
        
    }

    private IEnumerator DisplayMessageAndLoadNextLevel()
    {
        text.gameObject.SetActive(true);
        text.color = Color.green;
        text.text = "CORRECT! Loading next level...";

        yield return new WaitForSeconds(3);

        text.gameObject.SetActive(false);

        if (GameManager.currentLevel < 5)
        {
            GameManager.currentLevel++;
            SceneManager.LoadScene(1);
        }
        else if (GameManager.currentLevel == 5)
        {
            SceneManager.LoadScene(2);
        }
    }

}

