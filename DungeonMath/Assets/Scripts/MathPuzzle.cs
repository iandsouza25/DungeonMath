using UnityEngine;
using UnityEngine.UI;

public class MathPuzzle : MonoBehaviour
{
    public Text problemText;       // Display the equation
    public Text solutionText;      // Display the player's solution
    public Button submitButton;    // Submit the solution
    public InventoryManager inventoryManager;

    private string correctSolution; // Correct solution (e.g., "2+3")
    private string playerSolution = ""; // Player's current solution

    void Start()
    {
        GenerateEquation();
        submitButton.onClick.AddListener(CheckSolution);
    }

    void GenerateEquation()
    {
        correctSolution = "1+1"; // Example problem
        problemText.text = "Solve: ? + ? = 2";
    }

    public void AddToSolution(string item)
    {
        if (playerSolution.Length < correctSolution.Length)
        {
            playerSolution += item;
            solutionText.text = playerSolution;
        }
    }

    void CheckSolution()
    {
        if (playerSolution == correctSolution)
        {
            Debug.Log("Correct! Door unlocked.");
            // Unlock the door
        }
        else
        {
            Debug.Log("Incorrect. Try again!");
        }
    }
}