// See https://aka.ms/new-console-template for more information

using Calculator.Helper;
using Calculator.Model;
using System.Collections.Generic;

Console.WriteLine("Please, Enter numbers followed by operation eg. a+b*c-d/e");
while (true)
{
    try
    {
        //Compute priority
        //วงเล็บ	parenthesis
        //เลขยกำลัง	Exponents
        //คูน หาร	Multiplication/Division
        //บวก ลบ	Addition/Subtraction
        //Step1: Validate & Push input text to Stack then revert Stack, list
        //Step2: Pading left, Right then take group of operation
        //Step3: Compute case by Operation priority *, / and then +, -
        #region User Input
        string userInput = Console.ReadLine();
        #endregion

        #region Compute
        Console.WriteLine($"/***** The Result is {Compute(userInput)} *****/");
        #endregion

        Console.WriteLine("Please, Enter numbers followed by operation eg. a+b*c-d/e");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Exception: {ex.Message}");
    }
}

static double Compute(string sInput)
{
    var sText = sInput.ToClentText();
    if (string.IsNullOrEmpty(sInput.ToClentText()))
    {
        throw new Exception("Invalid input!.");
    }
    Stack<string> stack = PrepareTextToStack(sText);

    double result = 0;
    #region Stack => List then revert list
    Stack<string> revertStack = new Stack<string>();
    List<string> list = stack.Select(s => s).ToList();
    list.Reverse();
    #endregion

    result = Calculate(list);
    return result;
}

static List<OperationModel> OperationList()
{
    return new List<OperationModel>
    {
        new OperationModel{ Seq = 1, Operator = "*" },
        new OperationModel{ Seq = 2, Operator = "/" },
        new OperationModel{ Seq = 3, Operator = "+" },
        new OperationModel{ Seq = 4, Operator = "-" },
    }.OrderByDescending(o => o.Seq).ToList();
}

static Stack<string> PrepareTextToStack(string sText)
{
    Stack<string> stack = new Stack<string>();
    string value = string.Empty;
    for (int i = 0; i < sText.Length; i++)
    {
        string s = sText.Substring(i, 1);
        //Filter operators <= >= and ==
        if (i < sText.Length - 1)
        {
            string oper = sText.Substring(i, 2);
            if (oper.Equals("<=", StringComparison.OrdinalIgnoreCase) 
                || oper.Equals(">=", StringComparison.OrdinalIgnoreCase) 
                || oper.Equals("==", StringComparison.OrdinalIgnoreCase))
            {
                stack.Push(value);
                value = string.Empty;
                stack.Push(oper);
                i++;
                continue;
            }
        }

        char chr = s.ToCharArray()[0];
        #region Check char is oprator or not?
        if (!char.IsDigit(chr) && chr != '.' && !string.IsNullOrEmpty(value))
        {
            stack.Push(value);
            value = string.Empty;
        }
        #endregion
        if (s.Equals("("))
        {
            string innerExp = string.Empty;
            #region Check char is parenthesis "(" or not -> if yes then Fetch Next Character
            //If action this then compute data in () first
            i++;
            int bracketCount = 0;
            for (; i < sText.Length; i++)
            {
                s = sText.Substring(i, 1);
                if (s.Equals("("))
                {
                    bracketCount++;
                }

                if (s.Equals(")"))
                {
                    if (bracketCount == 0)
                    {
                        break;
                    }
                    bracketCount--;
                }
                innerExp += s;
            }
            //Compute data in () first!
            stack.Push(Compute(innerExp).ToString());
            #endregion
        }
        else if (s.Equals("+") ||
                 s.Equals("-") ||
                 s.Equals("*") ||
                 s.Equals("/") ||
                 s.Equals("<") ||
                 s.Equals(">"))
        {
            #region Char is operator then push to stack
            stack.Push(s);
            #endregion
        }
        else if (char.IsDigit(chr) || chr == '.')
        {
            #region Char is digi then push to stack
            value += s;
            if (value.Split('.').Length > 2)
            {
                throw new Exception("Invalid decimal.");
            }

            if (i == (sText.Length - 1))
            {
                stack.Push(value);
            }
            #endregion
        }
        else
        {
            throw new Exception("Invalid character.");
        }
    }
    return stack;
}

static double Calculate(List<string> list)
{
    Stack<string> stackOperation = new Stack<string>();
    OperationList().ForEach(s => { stackOperation.Push(s.Operator); });
    while (stackOperation.Count != 0)
    {
        var oper = stackOperation.Pop();
        for (int dataIdx = list.Count - 2; dataIdx >= 0; dataIdx--)
        {
            if (list[dataIdx].Equals(oper, StringComparison.OrdinalIgnoreCase))
            {
                list[dataIdx] = CalculatorHelper.PerformCalculation(leftValue: list[dataIdx - 1].ToDouble(), rightValue: list[dataIdx + 1].ToDouble(), sOperation: oper).ToString();
                list.RemoveAt(dataIdx + 1);
                list.RemoveAt(dataIdx - 1);
                dataIdx -= 2;
            }
        }
    }

    double result = 0;
    Stack<string> stack = new Stack<string>();
    #region Prepare final stack
    for (int i = 0; i < list.Count; i++)
    {
        stack.Push(list[i]);
    }
    #endregion

    #region Compute final result
    while (stack.Count >= 3)
    {
        //Pop by Sequence top is number, 
        double rNumber = Convert.ToDouble(stack.Pop());
        string sOper = stack.Pop();
        double lNumber = Convert.ToDouble(stack.Pop());
        result = CalculatorHelper.PerformCalculation(leftValue: lNumber, rightValue: rNumber, sOperation: sOper);
        stack.Push(result.ToString());
    }
    #endregion
    //Return top of stack for final result
    return Convert.ToDouble(stack.Pop());
}
