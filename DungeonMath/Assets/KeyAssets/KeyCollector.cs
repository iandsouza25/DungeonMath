using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class KeyCollector : MonoBehaviour
{
    // Start is called before the first frame update
    public int NUM_VALID_KEYS;
    private const int TOTAL_KEYS = 15; 
    private List<String> keysCollected = new List<String>();
    // keyCollectingOrder list gets populated in Start(), it will contain the value of keys that will be collecting
    // index 0 will represent the first key collected, 1 will be second key collected, etc
    private List<String> keyCollectingOrder = new List<String>();
    private List<int> keySlotsTaken = new List<int>();
    private static readonly System.Random rnd = new System.Random();
    private int currentIndex = 0;


    // Everything in the Start function will be moved to a new function once I 
    void Start()
    {


        //check for instance of equation generator
        EquationGenerator eqGenerator = FindObjectOfType<EquationGenerator>();
        if (eqGenerator == null)
        {
            Debug.LogError("No EquationGenerator found in the scene.");
            return;
        }

        eqGenerator.GenerateEquation();
        List<string> missingKeys = eqGenerator.GetMissingKeys();
        Debug.Log(eqGenerator.GetEquation());
        int currentLevel = eqGenerator.currentLevel;


        Queue<String> equationKeys = new Queue<String>();
        foreach (var key in missingKeys)
        {
            equationKeys.Enqueue(key);
        }
        NUM_VALID_KEYS = missingKeys.Count;

        // Populates which indexes have been taken, first x indexes are valid keys, rest third of remaining keys are operators (ex. 2 valid keys, 13 non valid, 1/3 of the non valid keys will be operators)
        int NUM_OPERATORS = (TOTAL_KEYS - NUM_VALID_KEYS) / 3;
        while (keySlotsTaken.Count < NUM_VALID_KEYS + NUM_OPERATORS) 
        {
            int keyIndex = GetRandomInt(0, TOTAL_KEYS - 1);
            if (!keySlotsTaken.Contains(keyIndex)) {
                keySlotsTaken.Add(keyIndex);
            }
        }

        int maxNumber = 10 + currentLevel * 5;
        
 
        for (int i = 0; i < 15; i++) 
        {
            if (keySlotsTaken.Contains(i) && keySlotsTaken.IndexOf(i) < NUM_VALID_KEYS) 
            {
                keyCollectingOrder.Add(equationKeys.Dequeue());
            }
            else if (keySlotsTaken.Contains(i) && keySlotsTaken.IndexOf(i) >= NUM_VALID_KEYS)
            {
                keyCollectingOrder.Add(GetRandomOperator());
            }
            else
            {
                keyCollectingOrder.Add(GetRandomInt(-1 * maxNumber, maxNumber).ToString());
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Key"))
        {
            Destroy(other.gameObject);
            keysCollected.Add(keyCollectingOrder[currentIndex++]);
            Debug.Log(keysCollected.Last());
        }
    }

    private static int GetRandomInt(int min, int max)
    {
        lock(rnd)
        {
            return rnd.Next(min, max);
        }
    }

    private String GetRandomOperator()
    {
        List<String> operators = new List<String> {"+", "-"};
        return operators[GetRandomInt(0, 1)];
    }

    // Can be used by UI to for inventory
    public List<String> GetKeysCollected()
    {
        return keysCollected;
    }

}
