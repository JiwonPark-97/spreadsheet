// See https://aka.ms/new-console-template for more information
using System.Linq.Expressions;
using System.Text.RegularExpressions;

string s1 = "1/2/3";
string[] substrings1 =
            Regex.Split(s1, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");

foreach (var token in substrings1)
{
    Console.WriteLine(token);
}

string s2 = "(5)  2 0";
string[] substrings2 =
            Regex.Split(s2, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");

foreach (var token in substrings2)
{
    Console.WriteLine(token);
}
