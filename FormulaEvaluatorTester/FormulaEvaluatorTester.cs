using FormulaEvaluator;

class FormulaEvaluatorTester
{
    static void Main(string[] args)
    {
        isVariableTest("X1", true);
    }

    public static void isVariableTest(string s, bool expected)
    {
        Console.WriteLine("Testing isVariable. Input: " + s + "Expected: " + expected + "Result: " + Evaluator.isVariable(s));

    }
}