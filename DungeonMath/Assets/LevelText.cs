using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelText : MonoBehaviour
{
    // Start is called before the first frame update
    public TMP_Text level;
    void Start()
    {
        level.text = "Level " + GameManager.currentLevel;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
