using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class KeyCollector : MonoBehaviour
{
    // Start is called before the first frame update
    public const int NUM_VALID_KEYS = 2;
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
        // Mock, Level 1, 
        Queue<String> equationKeys = new Queue<String>();
        equationKeys.Enqueue("+");
        equationKeys.Enqueue("5");

        // The above lines should be taken out
        // We should Hhve a list of the valid keys passed in, it should be created when the equation is made
        // The list would be iterated through and added to the equationKeys list

        // Populates which indexes have been taken, first x indexes are valid keys, rest third of remaining keys are operators (ex. 2 valid keys, 13 non valid, 1/3 of the non valid keys will be operators)
        const int NUM_OPERATORS = (TOTAL_KEYS - NUM_VALID_KEYS) / 3;
        while (keySlotsTaken.Count < NUM_VALID_KEYS + NUM_OPERATORS) 
        {
            int keyIndex = GetRandomInt(0, TOTAL_KEYS - 1);
            if (!keySlotsTaken.Contains(keyIndex)) {
                keySlotsTaken.Add(keyIndex);
            }
        }
 

        // will have 5 operator keys, 10 numbers, can change the amounts
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
                keyCollectingOrder.Add(GetRandomInt(-12, 12).ToString());
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
        List<String> operators = new List<String> {"+", "-", "/", "*"};
        return operators[GetRandomInt(0, 3)];
    }

    // Can be used by UI to for inventory
    public List<String> GetKeysCollected()
    {
        return keysCollected;
    }

}
