using FormulaEvaluator;

class FormulaEvaluatorTester
{
    static void Main(string[] args)
    {

        // test vaild variables
        isVariableTest("X1", true);
        isVariableTest("XxX2", true);
        isVariableTest("CD2", true);
        isVariableTest("cd2345", true);
        isVariableTest("alsdkflaskjdflasj2398475093287459032874957", true);


        // test invaild variables
        isVariableTest("X", false);
        isVariableTest("X2X2", false);
        isVariableTest("CD2a", false);
        isVariableTest("cd", false);
        isVariableTest("cakldjflqjw'ofjlsakdj3lksjdlfkja", false);


        // test valid integers
        isValueTest("1", true);
        isValueTest("0", true);
        isValueTest("345", true);
        isValueTest("230948", true);

        // test invalid integers
        isValueTest("-1", false);
        isValueTest(" ", false);
        isValueTest("398a", false);
        isValueTest("d394857", false);


    }

    public static void isValueTest(string s, bool expected)
    {
        bool result = Evaluator.isValue(s);
        Console.WriteLine("Testing isValue. Input: " + s + ". Expected: " + expected + ". Result: " + result);
        if (expected == result)
        {
            Console.WriteLine("TEST PASSED");
        }
        else
        {
            Console.WriteLine("*TEST FAILED*");

        }
        Console.WriteLine("\n");
    }


    public static void isVariableTest(string s, bool expected)
    {
        bool result = Evaluator.isVariable(s);
        Console.WriteLine("Testing isVariable. Input: " + s + ". Expected: " + expected + ". Result: " + result);
        if (expected == result)
        {
            Console.WriteLine("TEST PASSED");
        } else
        {
            Console.WriteLine("*TEST FAILED*");

        }
        Console.WriteLine("\n");

    }
}