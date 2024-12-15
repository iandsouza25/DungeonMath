using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class menuFunc : MonoBehaviour
{
    public Button startButton;
    public string sceneName;

    void beginGame()
    {
        SceneManager.LoadScene(sceneName);
    }

    // Start is called before the first frame update
    void Start()
    {
        startButton.onClick.AddListener(beginGame);
    }
}
