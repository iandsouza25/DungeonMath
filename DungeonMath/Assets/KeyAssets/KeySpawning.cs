using System.Collections.Generic;
using UnityEngine;

public class KeySpawning : MonoBehaviour
{
    void Start()
    {
        GameObject[] allKeys = GameObject.FindGameObjectsWithTag("Key");
        List<GameObject> keyList = new List<GameObject>(allKeys);

        while (keyList.Count > 15)
        {
            int randomIndex = Random.Range(0, keyList.Count);
            GameObject keyToRemove = keyList[randomIndex];
            keyList.RemoveAt(randomIndex);
            Destroy(keyToRemove);
        }

        Debug.Log($"Successfully set up {keyList.Count} keys in the scene.");
    }
}
