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
            missingCount = 2; 
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

            missingCount = 2;

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

            missingCount = 2;

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

    
    // returns string in format ___ + 7 = 9
    public string GetEquation()
    {
       
        return partialEquation;
    }

    //returns List in format ["___", "+", "7", "=", "9"]
    public List<string> GetEquationParts()
    {
        return parts;
    }

    //returns the keys that are necessary for level to be solved
    public List<string> GetMissingKeys()
    {
        return missingKeys;
    }

    //takes in list of strings of full equation ex. ["1", "+", "2", "=", "3"]
    public bool isEquationCorrect(List<string> fullEquation){
        // a op b = c
        if(currentLevel == 1 || currentLevel == 3){
            if (fullEquation.Count != 5){
                return false;
            }
            int a = int.Parse(fullEquation[0]);
            int b = int.Parse(fullEquation[2]);
            int c = int.Parse(fullEquation[4]);
            string op = fullEquation[1];
            int res = CalculateResult(a, b, op);
            return res == c;
        }
        else if (currentLevel == 2 || currentLevel == 4){
            // a op b op c op d = e
            if (fullEquation.Count != 9){
                return false;
            }
            int a = int.Parse(fullEquation[0]);
            int b = int.Parse(fullEquation[2]);
            int c = int.Parse(fullEquation[4]);
            int d = int.Parse(fullEquation[6]);
            int e = int.Parse(fullEquation[8]);

            string op1 = fullEquation[1];
            string op2 = fullEquation[3];
            string op3 = fullEquation[5];

            //prevent division by zero
            if ((op1 == "/" && b == 0) || (op2 == "/" && c == 0) || (op3 == "/" && d == 0)){
                return false;
            }

            int res1 = CalculateResult(a, b, op1);
            int res2 = CalculateResult(res1, c, op2);
            int res3 = CalculateResult(res2, d, op3);
            return res3 == e;
        }
        else if (currentLevel == 5){
            // a op b op c op d op e = f
            if (fullEquation.Count != 11){
                return false;
            }
            int a = int.Parse(fullEquation[0]);
            int b = int.Parse(fullEquation[2]);
            int c = int.Parse(fullEquation[4]);
            int d = int.Parse(fullEquation[6]);
            int e = int.Parse(fullEquation[8]);
            int f = int.Parse(fullEquation[10]);

            string op1 = fullEquation[1];
            string op2 = fullEquation[3];
            string op3 = fullEquation[5];
            string op4 = fullEquation[7];

            //prevent division by zero
            if ((op1 == "/" && b == 0) || (op2 == "/" && c == 0) || (op3 == "/" && d == 0) || (op4 == "/" && e == 0)){
                return false;
            }

            int res1 = CalculateResult(a, b, op1);
            int res2 = CalculateResult(res1, c, op2);
            int res3 = CalculateResult(res2, d, op3);
            int res4 = CalculateResult(res3, e, op4);
            return res4 == f;
        }
        return false;
    }

    

    private int CalculateResult(int A, int B, string op)
    {
        switch (op)
        {
            case "+": return A + B;
            case "-": return A - B;
            case "*": return A * B;
            case "/": 
                if (B == 0){return -1000000000;}
                else if (A % B == 0){return A / B;}
                else          { return 0;}
        }
        return 0;
    }
}