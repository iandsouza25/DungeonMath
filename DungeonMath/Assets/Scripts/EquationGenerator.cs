using System.Collections.Generic;
using UnityEngine;

public class EquationGenerator : MonoBehaviour
{
    
    public int currentLevel = 1;
    private string fullEquation;

    private string partialEquation;
    private List<string> missingKeys = new List<string>();

    private int missingCount;
    private List<int> removableIndices = new List<int>();

    private List<string> parts = new List<string>();

   
    
    private string[] operators;

    public void GenerateEquation()
    {
        missingKeys.Clear();
        parts.Clear();
        removableIndices.Clear();

        int maxNumber = 10 + currentLevel * 10;
        

        if (currentLevel == 1){
            missingCount = 1; 
            operators = new string[] { "+", "-" };

            int A = Random.Range(1, maxNumber);
            int B = Random.Range(1, maxNumber);
            string op = operators[Random.Range(0, operators.Length)];
            
            int result = CalculateResult(A, B, op);
            // Equation format: "A op B = C"
            fullEquation = A + " " + op + " " + B + " = " + result;
            // Potential missing pieces: "A", "op", "B", "result"
            parts = new List<string>() { A.ToString(), op, B.ToString(), "=", result.ToString() };
            removableIndices = new List<int>() {0,1,2,4}; //dont remove  = sign
        }
        else if (currentLevel == 2){
            missingCount = 3; 
            operators = new string[] { "+", "-" };
            int A = Random.Range(1, maxNumber);
            int B = Random.Range(1, maxNumber);
            int C = Random.Range(1, maxNumber);
            int D = Random.Range(1, maxNumber);
            string op1 = operators[Random.Range(0, operators.Length)];
            string op2 = operators[Random.Range(0, operators.Length)];
            string op3 = operators[Random.Range(0, operators.Length)];
            
            int a_op_b = CalculateResult(A, B, op1);
            int b_op_c = CalculateResult(a_op_b, C, op2);
            int result = CalculateResult(b_op_c, D, op3);

            // Equation format: "A op B op C op D = E"
            fullEquation = A + " " + op1 + " " + B+ " " + op2 + " " + C + " " + op3 + " " + D + " "+ " = " + result;
            //potential missing parts "A", "op1", "B", "op2", "C", "op3", "D"
            parts = new List<string>() { A.ToString(), op1, B.ToString(), op2, C.ToString(), op3, D.ToString(), "=", result.ToString()};
            removableIndices = new List<int>() {0,1,2,3,4,5,6};
        }
        else if (currentLevel == 3){
            missingCount = 1;
            operators = new string[] { "*", "/" };
            string op = operators[Random.Range(0, operators.Length)];
            int A = Random.Range(1, maxNumber);
            int B = Random.Range(1, maxNumber);
            if (op == "/"){
                while (A % B != 0  || B == 0){
                    A = Random.Range(1, maxNumber);
                    B = Random.Range(1, maxNumber);
                }
            }
            int result = CalculateResult(A, B, op);
            fullEquation = A + " " + op + " " + B + " = " + result;
            parts = new List<string>() { A.ToString(), op, B.ToString(), "=", result.ToString() };
            removableIndices = new List<int>() {0,1,2,4}; //dont remove  = sign
            }

        else if (currentLevel == 4){

            //generate equation in format a op b op c op d = e
            //will have 3 missing parts

            missingCount = 3;

            operators = new string[] { "*", "/" };

            string op1 = operators[Random.Range(0, operators.Length)];
            string op2 = operators[Random.Range(0, operators.Length)];
            string op3 = operators[Random.Range(0, operators.Length)];
            int A = Random.Range(1, maxNumber);
            int B = Random.Range(1, maxNumber);
            int C = Random.Range(1, maxNumber);
            int D = Random.Range(1, maxNumber);

            //make sure we have integer division
            if (op1 == "/"){
                while (A % B != 0  || B == 0){
                    A = Random.Range(1, maxNumber);
                    B = Random.Range(1, maxNumber);
                }
            }
            int a_op_b = CalculateResult(A, B, op1);

            //make sure we have integer division
            if (op2 == "/"){
                int num_attempts = 0;
                while(a_op_b % C != 0 || C == 0){
                    num_attempts +=1;
                    C = Random.Range(1, maxNumber);
                    //if integer division not possible, switch to multiplication
                    if (num_attempts > 10000 && C != 0){
                        op2 = "*";
                        break;
                    }
                }
                
                
            }
            int b_op_c = CalculateResult(a_op_b, C, op2);

            if (op3 == "/"){
                int num_attempts = 0;
                while(b_op_c % D != 0 || D == 0){
                    num_attempts +=1;
                    D = Random.Range(1, maxNumber);
                    //if integer division not possible, switch to multiplication
                    if (num_attempts > 10000 && D != 0){
                        op3 = "*";
                        break;
                    }
                }
               
                
            }
            int result = CalculateResult(b_op_c, D, op3);
            // Equation format: "A op B op C op D = E"
            fullEquation = A + " " + op1 + " " + B+ " " + op2 + " " + C + " " + op3 + " " + D + " "+ " = " + result;
            //potential missing parts "A", "op1", "B", "op2", "C", "op3", "D"
            parts = new List<string>() { A.ToString(), op1, B.ToString(), op2, C.ToString(), op3, D.ToString(), "=", result.ToString()};
            removableIndices = new List<int>() {0,1,2,3,4,5,6};
        }

        else if (currentLevel == 5){

            //generate equation in format a op b op c op d op e = f
            //will have 4 missing parts

            missingCount = 4;

            string[] operators1 = new string[] { "+", "-" };
            string[] operators2 = new string[] { "*", "/" };

            string op1 = operators2[Random.Range(0, operators2.Length)];
            string op2 = operators2[Random.Range(0, operators2.Length)];
            string op3 = operators1[Random.Range(0, operators1.Length)];
            string op4 = operators1[Random.Range(0, operators1.Length)];
            int A = Random.Range(1, maxNumber);
            int B = Random.Range(1, maxNumber);
            int C = Random.Range(1, maxNumber);
            int D = Random.Range(1, maxNumber);
            int E = Random.Range(1, maxNumber);

            //make sure we have integer division
            if (op1 == "/"){
                while (A % B != 0  || B == 0){
                    A = Random.Range(1, maxNumber);
                    B = Random.Range(1, maxNumber);
                }
            }
            int a_op_b = CalculateResult(A, B, op1);

            if (op2 == "/"){
                int num_attempts = 0;
                while(a_op_b % C != 0 || C == 0){
                    num_attempts +=1;
                    C = Random.Range(1, maxNumber);
                    //if integer division not possible, switch to multiplication
                    if (num_attempts > 10000 && C != 0){
                        op2 = "*";
                        break;
                    }
                }  
            }
            int b_op_c = CalculateResult(a_op_b, C, op2);
            int c_op_d = CalculateResult(b_op_c, D, op3);
            int result = CalculateResult(c_op_d, E, op4);

             // Equation format: "A op B op C op D op E = F"
            fullEquation = A + " " + op1 + " " + B+ " " + op2 + " " + C + " " + op3 + " " + D + " "+ op4 + " " + E + " = " + result;
            //potential missing parts "A", "op1", "B", "op2", "C", "op3", "D"
            parts = new List<string>() { A.ToString(), op1, B.ToString(), op2, C.ToString(), op3, D.ToString(), op4, E.ToString(), "=", result.ToString()};
            removableIndices = new List<int>() {0,2,4,6,8,10};
        }
    
        
        for (int i = 0; i < missingCount; i++)
        {
            int rIndex = Random.Range(0, removableIndices.Count);
            int partIndex = removableIndices[rIndex];
            removableIndices.RemoveAt(rIndex);
            
            // Mark that part as missing and store which key is needed
            missingKeys.Add(parts[partIndex]);
            parts[partIndex] = "___"; // Placeholder
        }

        partialEquation = string.Join(" ", parts);
    }

    public string GetEquation()
    {
        return partialEquation;
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
            case "*": return A * B;
            case "/": return A / B;
        }
        return 0;
    }
}