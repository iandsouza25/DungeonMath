using System.Collections.Generic;
using UnityEngine;

public class EquationGenerator : MonoBehaviour
{
    //level 1, a + b = c or a - b = c
    
    public int currentLevel = 1;
    private string equation;
    private List<string> missingKeys = new List<string>();

   
    
    private string[] operators = { "+", "-"};

    public void GenerateEquation()
    {
        // Increase difficulty by increasing range of numbers and number of missing pieces
        int maxNumber = 10 + currentLevel * 5;
        
        int A = Random.Range(-1 * maxNumber, maxNumber);
        int B = Random.Range(-1 * maxNumber, maxNumber);
        string op = operators[Random.Range(0, operators.Length)];
        
        int result = CalculateResult(A, B, op);

        // Equation format: "A op B = C"
        equation = A + " " + op + " " + B + " = " + result;
        
        //remove current level number of pieces
        int missingCount = currentLevel; 

        // Potential missing pieces: "A", "op", "B", "result"
        // remove pieces from A, op, B, and result randomly
        List<string> parts = new List<string>() { A.ToString(), op, B.ToString(), "=", result.ToString() };
        List<int> removableIndices = new List<int>() {0,1,2,4}; //dont remove  = sign
        
        for (int i = 0; i < missingCount; i++)
        {
            int rIndex = Random.Range(0, removableIndices.Count);
            int partIndex = removableIndices[rIndex];
            removableIndices.RemoveAt(rIndex);
            
            // Mark that part as missing and store which key is needed
            missingKeys.Add(parts[partIndex]);
            parts[partIndex] = "___"; // Placeholder
        }

        equation = string.Join(" ", parts);
    }

    public string GetEquation()
    {
        return equation;
    }

    public List<string> GetMissingKeys()
    {
        return missingKeys;
    }

    private int CalculateResult(int A, int B, string op)
    {
        switch (op)
        {
            case "+": return A + B;
            case "-": return A - B;
            // case "*": return A * B;
        }
        return 0;
    }
}