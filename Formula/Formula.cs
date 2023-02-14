// Skeleton written by Joe Zachary for CS 3500, September 2013
// Read the entire skeleton carefully and completely before you
// do anything else!

// Version 1.1 (9/22/13 11:45 a.m.)

// Change log:
//  (Version 1.1) Repaired mistake in GetTokens
//  (Version 1.1) Changed specification of second constructor to
//                clarify description of how validation works

// (Daniel Kopta) 
// Version 1.2 (9/10/17) 

// Change log:
//  (Version 1.2) Changed the definition of equality with regards
//                to numeric tokens

/// <summary>
/// Author:    Jiwon Park
/// Partner:   None
/// Date:      3-Feb-2023
/// Course:    CS 3500, University of Utah, School of Computing
/// Copyright: CS 3500 and Jiwon Park - This work may not 
///            be copied for use in Academic Coursework.
///
/// I, Jiwon Park, certify that I wrote this code from scratch and
/// did not copy it in part or whole from another source.  All 
/// references used in the completion of the assignments are cited 
/// in my README file.
///
/// This file contains two classes Formula and FormulaFormatException, and a struct FormulaError.
/// Formula stores and check for a valid expression with normalizer and validator (if given), 
/// and FormulaFormatException is used to report syntactic errors in a formula.
/// FormulaError returns value from the Formula's Evaluator method when the input formula is bad.
/// </summary>


using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;

namespace SpreadsheetUtilities
{
    /// <summary>
    /// Represents formulas written in standard infix notation using standard precedence
    /// rules.  The allowed symbols are non-negative numbers written using double-precision 
    /// floating-point syntax (without unary preceeding '-' or '+'); 
    /// variables that consist of a letter or underscore followed by 
    /// zero or more letters, underscores, or digits; parentheses; and the four operator 
    /// symbols +, -, *, and /.  
    /// 
    /// Spaces are significant only insofar that they delimit tokens.  For example, "xy" is
    /// a single variable, "x y" consists of two variables "x" and y; "x23" is a single variable; 
    /// and "x 23" consists of a variable "x" and a number "23".
    /// 
    /// Associated with every formula are two delegates:  a normalizer and a validator.  The
    /// normalizer is used to convert variables into a canonical form, and the validator is used
    /// to add extra restrictions on the validity of a variable (beyond the standard requirement 
    /// that it consist of a letter or underscore followed by zero or more letters, underscores,
    /// or digits.)  Their use is described in detail in the constructor and method comments.
    /// </summary>
    public class Formula
    {

        // helper methods for checking type of token //

        /// <summary>
        /// Returns if an input token is a numeric value
        /// </summary>
        /// <param name="token" a token </param>
        private bool IsValue(string token)
        {
            double value;
            return (double.TryParse(token, out value));
        }

        /// <summary>
        /// Returns if an input token is an operator
        /// </summary>
        /// <param name="token"> a token </param>
        private bool IsOperator(string token)
        {
            string pattern = string.Format(@"[\+\-*/]$");
            return (Regex.IsMatch(token, pattern));
        }

        /// <summary>
        /// Returns if an input token is a left parenthesis
        /// </summary>
        /// <param name="token"> a token </param>
        private bool IsLeftParen(string token)
        {
            //string pattern = string.Format(@"\(");
            //return (Regex.IsMatch(token, pattern));
            return token == "(";
        }

        /// <summary>
        /// Returns if an input token is a right parenthesis
        /// </summary>
        /// <param name="token"> a token </param>
        private bool IsRightParen(string token)
        {
            //string pattern = string.Format(@"\)");
            //return (Regex.IsMatch(token, pattern));
            return token == ")";
        }

        /// <summary>
        /// Returns if an input token is a variable.
        /// Any letter or underscore followed by any number of letters and/or digits and/or underscores
        /// would be considered as valid variable names.
        /// </summary>
        /// <param name="token"> a token </param>
        private bool IsVariable(string token)
        {
            string pattern = string.Format(@"[a-zA-Z_](?: [a-zA-Z_]|\d)*");
            return (Regex.IsMatch(token, pattern));
        }

        // helper methods for parsing rules //

        /// <summary>
        /// Returns if the balanced parentheses rule is held:
        /// The total number of opening parentheses must equal the total number of closing parentheses.
        /// </summary>
        /// <param name="tokens"> a list of tokens </param>
        private bool BalancedParenRule(List<string> list)
        {
            int lp = 0;
            int rp = 0;
            foreach (string t in list)
            {
                if (IsLeftParen(t))
                {
                    lp++;
                }
                if (IsRightParen(t))
                {
                    rp++;
                }
            }
            return (lp == rp);
        }

        /// <summary>
        /// Returns if the right parentheses rule is held:
        /// When reading tokens from left to right, at no point should the number of closing parentheses seen so far be greater than the number of opening parentheses seen so far.
        /// </summary>
        /// <param name="tokens"> a list of tokens </param>
        private bool RtParenRule(List<string> list)
        {
            int lp = 0;
            int rp = 0;
            foreach (string t in list)
            {
                if (IsLeftParen(t))
                {
                    lp++;
                }
                if (IsRightParen(t))
                {
                    rp++;
                }
                if (lp < rp)
                {
                    return false;
                }
            }
            return true;
        }


        private List<string> tokens;

        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically invalid,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer is the identity function, and the associated validator
        /// maps every string to true.  
        /// </summary>
        public Formula(String formula) :
            this(formula, s => s, s => true)
        {
        }

        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically incorrect,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer and validator are the second and third parameters,
        /// respectively.  
        /// 
        /// If the formula contains a variable v such that normalize(v) is not a legal variable, 
        /// throws a FormulaFormatException with an explanatory message. 
        /// 
        /// If the formula contains a variable v such that isValid(normalize(v)) is false,
        /// throws a FormulaFormatException with an explanatory message.
        /// 
        /// Suppose that N is a method that converts all the letters in a string to upper case, and
        /// that V is a method that returns true only if a string consists of one letter followed
        /// by one digit.  Then:
        /// 
        /// new Formula("x2+y3", N, V) should succeed
        /// new Formula("x+y3", N, V) should throw an exception, since V(N("x")) is false
        /// new Formula("2x+y3", N, V) should throw an exception, since "2x+y3" is syntactically incorrect.
        /// </summary>
        public Formula(String formula, Func<string, string> normalize, Func<string, bool> isValid)
        {
            // check for null input before getting tokens
            if (formula == null)
            {
                throw new FormulaFormatException("Input can't be null");
            }

            // get tokens from formula
            tokens = GetTokens(formula).ToList();

            // After tokenizing, verify the following rules are all held

            // One Token Rule
            if (tokens.Count() == 0)
            {
                throw new FormulaFormatException("There must be at least one token.");
            }

            // Starting Token Rule
            if (!(IsValue(tokens[0]) || IsVariable(tokens[0]) || IsLeftParen(tokens[0])))
            {
                throw new FormulaFormatException("The first token of an expression must be a number, a variable, or an opening parenthesis.");
            }

            // Ending Token Rule
            if (!(IsValue(tokens.Last()) || IsVariable(tokens.Last()) || IsRightParen(tokens.Last())))
            {
                throw new FormulaFormatException("The last token of an expression must be a number, a variable, or a closing parenthesis.");
            }

            // Right Parentheses Rule
            if (!RtParenRule(tokens))
            {
                throw new FormulaFormatException("When reading tokens from left to right, at no point should the number of closing parentheses seen so far be greater than the number of opening parentheses seen so far.");
            }

            // Balanced Parentheses Rule
            if (!BalancedParenRule(tokens))
            {
                throw new FormulaFormatException("The total number of opening parentheses must equal the total number of closing parentheses.");
            }

            for (int i = 0; i < tokens.Count() - 1; i++)
            {

                // Parenthesis/Operator Following Rule
                if (IsLeftParen(tokens[i]) || IsOperator(tokens[i]))
                {
                    if (!(IsValue(tokens[i + 1]) || IsVariable(tokens[i + 1]) || IsLeftParen(tokens[i + 1])))
                    {
                        throw new FormulaFormatException("Any token that immediately follows an opening parenthesis or an operator must be either a number, a variable, or an opening parenthesis.");
                    }

                // Extra Following Rule
                }
                else if (IsValue(tokens[i]) || IsVariable(tokens[i]) || IsRightParen(tokens[i]))
                {
                    if (!(IsOperator(tokens[i + 1]) || IsRightParen(tokens[i + 1])))
                    {
                        throw new FormulaFormatException("Any token that immediately follows a number, a variable, or a closing parenthesis must be either an operator or a closing parenthesis.");
                    }
                }

                // Specific Token Rule
                else
                {
                    throw new FormulaFormatException("The only valid tokens are (, ), +, -, *, /, variables, and decimal real numbers (including scientific notation).");
                }
            }


            // normalize tokens
            for (int i = 0; i < tokens.Count(); i++)
            {
                if (IsVariable(tokens[i]))
                {
                    tokens[i] = normalize(tokens[i]);
                }
            }

            // check for invalid variables
            List<string> varList = GetVariables().ToList();
            foreach (string t in varList)
            {
                if (!isValid(t))
                {
                    throw new FormulaFormatException("Given variable name is invalide: " + t);
                }
            }
        }

        /// <summary>
        /// Evaluates this Formula, using the lookup delegate to determine the values of
        /// variables.  When a variable symbol v needs to be determined, it should be looked up
        /// via lookup(normalize(v)). (Here, normalize is the normalizer that was passed to 
        /// the constructor.)
        /// 
        /// For example, if L("x") is 2, L("X") is 4, and N is a method that converts all the letters 
        /// in a string to upper case:
        /// 
        /// new Formula("x+7", N, s => true).Evaluate(L) is 11
        /// new Formula("x+7").Evaluate(L) is 9
        /// 
        /// Given a variable symbol as its parameter, lookup returns the variable's value 
        /// (if it has one) or throws an ArgumentException (otherwise).
        /// 
        /// If no undefined variables or divisions by zero are encountered when evaluating 
        /// this Formula, the value is returned.  Otherwise, a FormulaError is returned.  
        /// The Reason property of the FormulaError should have a meaningful explanation.
        ///
        /// This method should never throw an exception.
        /// </summary>
        public object Evaluate(Func<string, double> lookup)
        {
            Stack<double> values = new Stack<double>();
            Stack<string> operators = new Stack<string>();

            // proceed tokens from left to right
            foreach (string token in tokens)
            {
                if (IsValue(token) || IsVariable(token))
                {

                    // get decimal values from the token
                    double tokenVal = 0;

                    if (IsValue(token))
                    {
                        tokenVal = Double.Parse(token);
                    }

                    // if token is variable, check if defined
                    else
                    {
                        try
                        {
                            tokenVal = lookup(token);
                        }
                        catch (ArgumentException)
                        {
                            return new FormulaError("Undefined variable " + token);
                        }
                    }

                    // check the operator stack for "*" or "/" at the top
                    if (operators.Count != 0)
                    {
                        if ((operators.Peek()) == "*" || (operators.Peek() == "/"))
                        {
                            // pop the value and operator stack and apply with the token
                            double tempVal = values.Pop();
                            string tempOpr = operators.Pop();
                            double tempResult;

                            if (tempOpr == "*")
                            {
                                tempResult = tempVal * tokenVal;
                            }

                            // if operator is "/"
                            else
                            {
                                if (tokenVal != 0)
                                {
                                    tempResult = tempVal / tokenVal;

                                }
                                // prevent division by 0
                                else
                                {
                                    return new FormulaError("Divison by 0 occured.");
                                }
                            }

                            // push the result onto the value stack
                            values.Push(tempResult);
                        }

                        // if there isn't "*" or "/" at the top of the operator stack
                        else
                        {
                            values.Push(tokenVal);
                        }
                    }

                    // if operator stack is empty
                    else
                    {
                        values.Push(tokenVal);
                    }
                }

                else if (token == "+" || token == "-")
                {

                    // if "+" or "-" at the top of the operator stack
                    if (operators.Count() != 0)
                    {
                        if (operators.Peek() == "+" || operators.Peek() == "-")
                        {
                            // pop the value stack twice and the operator stack once
                            double val1 = values.Pop();
                            double val2 = values.Pop();
                            string op = operators.Pop();
                            double result;

                            // apply the popped operator to the popped numbers and push the result onto the value stack
                            if (op == "+")
                            {
                                result = val2 + val1;
                            }
                            else
                            {
                                result = val2 - val1;
                            }
                            values.Push(result);
                        }
                    }

                    // push token onto the operator stack
                    operators.Push(token);
                }

                else if (token == "*" || token == "/" || token == "(")
                {
                    // push token onto the operator stack
                    operators.Push(token);
                }

                else if (token == ")")
                {

                    // if "+" or "-" at the top of the operator stack
                    if (operators.Peek() == "+" || operators.Peek() == "-")
                    {
                        // pop the value stack twice and the operator stack once
                        double val1 = values.Pop();
                        double val2 = values.Pop();
                        string op = operators.Pop();
                        double result;

                        // apply the popped operator to the popped numbers and push the result onto the value stack
                        if (op == "+")
                        {
                            result = val2 + val1;
                        }

                        // op == "-"
                        else
                        {
                            result = val2 - val1;
                        }
                        values.Push(result);
                    }


                    // the top of the operator stack must be "(". pop it
                    
                    string tempOp = operators.Pop();

                    // if "*" or "/" at the top of the operator stack
                    if (operators.Count() != 0)
                    {
                        if (operators.Peek() == "*" || operators.Peek() == "/")
                        {
                            // pop the value stack twice and the operator stack once
                            double val1 = values.Pop();
                            double val2 = values.Pop();
                            string op = operators.Pop();
                            double result;

                            // apply the popped operator to the popped numbers and push the result onto the value stack
                            if (op == "*")
                            {
                                result = val2 * val1;
                            }

                            // op == "/"
                            else
                            {
                                if (val1 != 0)
                                {
                                    result = val2 / val1;
                                }

                                // prevent division by 0
                                else
                                {
                                    return new FormulaError("Divison by 0 occured.");
                                }
                            }
                            values.Push(result);
                        }
                    }

                }
            }
            // last token proceeded

            // if operator stack is empty, there should be a single value
            if (operators.Count() == 0)
            {
                return values.Pop();
            }

            // if operator stack is not empty, there should be exactly one operator (+ or -) and two values.
            else
            {
                    string op = operators.Pop();
                    double val1 = values.Pop();
                    double val2 = values.Pop();

                    if (op == "+")
                    {
                        return val2 + val1;

                    }
                    // should be "-"
                    else
                    {
                        return val2 - val1;
                    }
                }
        }

        /// <summary>
        /// Enumerates the normalized versions of all of the variables that occur in this 
        /// formula.  No normalization may appear more than once in the enumeration, even 
        /// if it appears more than once in this Formula.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        /// 
        /// new Formula("x+y*z", N, s => true).GetVariables() should enumerate "X", "Y", and "Z"
        /// new Formula("x+X*z", N, s => true).GetVariables() should enumerate "X" and "Z".
        /// new Formula("x+X*z").GetVariables() should enumerate "x", "X", and "z".
        /// </summary>
        public IEnumerable<String> GetVariables()
        {
            HashSet<string> returnSet = new HashSet<string>();
            foreach (string t in tokens)
            {
                if (IsVariable(t))
                {
                    returnSet.Add(t);
                }
            }
            return returnSet;
        }

        /// <summary>
        /// Returns a string containing no spaces which, if passed to the Formula
        /// constructor, will produce a Formula f such that this.Equals(f).  All of the
        /// variables in the string should be normalized.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        /// 
        /// new Formula("x + y", N, s => true).ToString() should return "X+Y"
        /// new Formula("x + Y").ToString() should return "x+Y"
        /// </summary>
        public override string ToString()
        {

            // Two equal formulas that (as defined by equals) should return the exact same string in their ToString method. (according to @607 on Piazza)
            for (int i = 0; i < tokens.Count(); i++)
            {
                if (IsValue(tokens[i]))
                {
                    double value = double.Parse(tokens[i]);
                    tokens[i] = value.ToString();
                }
            }
            string returnstr = string.Join("", tokens);
            return returnstr;
        }

        /// <summary>
        ///  <change> make object nullable </change>
        ///
        /// If obj is null or obj is not a Formula, returns false.  Otherwise, reports
        /// whether or not this Formula and obj are equal.
        /// 
        /// Two Formulae are considered equal if they consist of the same tokens in the
        /// same order.  To determine token equality, all tokens are compared as strings 
        /// except for numeric tokens and variable tokens.
        /// Numeric tokens are considered equal if they are equal after being "normalized" 
        /// by C#'s standard conversion from string to double, then back to string. This 
        /// eliminates any inconsistencies due to limited floating point precision.
        /// Variable tokens are considered equal if their normalized forms are equal, as 
        /// defined by the provided normalizer.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        ///  
        /// new Formula("x1+y2", N, s => true).Equals(new Formula("X1  +  Y2")) is true
        /// new Formula("x1+y2").Equals(new Formula("X1+Y2")) is false
        /// new Formula("x1+y2").Equals(new Formula("y2+x1")) is false
        /// new Formula("2.0 + x7").Equals(new Formula("2.000 + x7")) is true
        /// </summary>
        public override bool Equals(object? obj)
        {
            if (obj is null || obj is not Formula)
            {
                return false;
            }

            return this.GetHashCode() == obj.GetHashCode();
        }

        /// <summary>
        ///   <change> We are now using Non-Nullable objects.  Thus neither f1 nor f2 can be null!</change>
        /// Reports whether f1 == f2, using the notion of equality from the Equals method.
        /// </summary>
        /// <Nullable>enable</Nullable>
        public static bool operator ==(Formula f1, Formula f2)
        {
            return f1.Equals(f2);
        }

        /// <summary>
        ///   <change> We are now using Non-Nullable objects.  Thus neither f1 nor f2 can be null!</change>
        ///   <change> Note: != should almost always be not ==, if you get my meaning </change>
        ///   Reports whether f1 != f2, using the notion of equality from the Equals method.
        /// </summary>
        /// <Nullable>enable</Nullable>
        public static bool operator !=(Formula f1, Formula f2)
        {
            return !(f1.Equals(f2));
        }

        /// <summary>
        /// Returns a hash code for this Formula.  If f1.Equals(f2), then it must be the
        /// case that f1.GetHashCode() == f2.GetHashCode().  Ideally, the probability that two 
        /// randomly-generated unequal Formulae have the same hash code should be extremely small.
        /// </summary>
        public override int GetHashCode()
        {
            // get hash code from converted-to-string formula
            string str = this.ToString();
            return str.GetHashCode();
        }

        /// <summary>
        /// Given an expression, enumerates the tokens that compose it.  Tokens are left paren;
        /// right paren; one of the four operator symbols; a string consisting of a letter or underscore
        /// followed by zero or more letters, digits, or underscores; a double literal; and anything that doesn't
        /// match one of those patterns.  There are no empty tokens, and no token contains white space.
        /// </summary>
        private static IEnumerable<string> GetTokens(String formula)
        {
            // Patterns for individual tokens
            String lpPattern = @"\(";
            String rpPattern = @"\)";
            String opPattern = @"[\+\-*/]";
            String varPattern = @"[a-zA-Z_](?: [a-zA-Z_]|\d)*";
            String doublePattern = @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: [eE][\+-]?\d+)?";
            String spacePattern = @"\s+";

            // Overall pattern
            String pattern = String.Format("({0}) | ({1}) | ({2}) | ({3}) | ({4}) | ({5})",
                                            lpPattern, rpPattern, opPattern, varPattern, doublePattern, spacePattern);

            // Enumerate matching tokens that don't consist solely of white space.
            foreach (String s in Regex.Split(formula, pattern, RegexOptions.IgnorePatternWhitespace))
            {
                if (!Regex.IsMatch(s, @"^\s*$", RegexOptions.Singleline))
                {
                    yield return s;
                }
            }

        }
    }

    /// <summary>
    /// Used to report syntactic errors in the argument to the Formula constructor.
    /// </summary>
    public class FormulaFormatException : Exception
    {
        /// <summary>
        /// Constructs a FormulaFormatException containing the explanatory message.
        /// </summary>
        public FormulaFormatException(String message)
            : base(message)
        {
        }
    }

    /// <summary>
    /// Used as a possible return value of the Formula.Evaluate method.
    /// </summary>
    public struct FormulaError
    {
        /// <summary>
        /// Constructs a FormulaError containing the explanatory reason.
        /// </summary>
        /// <param name="reason"></param>
        public FormulaError(String reason)
            : this()
        {
            Reason = reason;
        }

        /// <summary>
        ///  The reason why this FormulaError was created.
        /// </summary>
        public string Reason { get; private set; }
    }
}


// <change>
//   If you are using Extension methods to deal with common stack operations (e.g., checking for
//   an empty stack before peeking) you will find that the Non-Nullable checking is "biting" you.
//
//   To fix this, you have to use a little special syntax like the following:
//
//       public static bool OnTop<T>(this Stack<T> stack, T element1, T element2) where T : notnull
//
//   Notice that the "where T : notnull" tells the compiler that the Stack can contain any object
//   as long as it doesn't allow nulls!
// </change>
