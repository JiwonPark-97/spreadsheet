```
Author:     Jiwon Park
Partner:    None
Course:     CS 3500, University of Utah, School of Computing
GitHub ID:  JiwonPark-97
Repo:       https://github.com/uofu-cs3500-spring23/spreadsheet-JiwonPark-97.git
Date:       23-Jan-2023 4.00 am (when submission was completed) 
Project:    Formula Evaluator
Copyright:  CS 3500 and Jiwon Park - This work may not be copied for use in Academic Coursework.
```

# Comments to Evaluators:

I followed the instruction given; declared two empty stacks, splitted the input expression into tokens, and proceeded the tokens from left to right following the given algorithm.
After proceeding the last token, I checked the operator stack to return the final value.

I also added helper methods (RemoveWhiteSpace, IsValue, and IsVariable) to avoid repeated code and make the code more readable. 

I could not find how to enable VS spell checker on Mac. I checked thoroughly but there might be some typos.

# Assignment Specific Topics

This section was not required.

# Consulted Peers:

I have not consulted with others since the instruction and the given algorithm were clear. Though I referred to the piazza posts.

# References:

    1. How to check if a string is a number in C# - https://www.arungudelli.com/tutorial/c-sharp/check-if-string-is-number/
    2. How to convert a string to a number (C# Programming Guide) - https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/types/how-to-convert-a-string-to-a-number
    3. Check if a string contains a letter - https://social.msdn.microsoft.com/Forums/vstudio/en-US/8daa5f84-8535-47b0-b4bf-1669a9682911/check-if-a-string-contains-a-letter?forum=csharpgeneral
    4. RegEx pattern any two letters followed by six numbers - https://stackoverflow.com/questions/10439666/regex-pattern-any-two-letters-followed-by-six-numbers
    5. REGULAR EXPRESSIONS - https://users.cs.cf.ac.uk/Dave.Marshall/Internet/NEWS/regexp.html
    6. a Ruby regular expression editor - https://rubular.com/r/gleAi4duNh
    7. C# - How to convert string to char? - https://stackoverflow.com/questions/33946594/c-sharp-how-to-convert-string-to-char
    8. Stack.Count Property in C# - https://www.geeksforgeeks.org/stack-count-property-in-c-sharp/
    9. How to remove all white space from the beginning or end of a string? - https://stackoverflow.com/questions/3381952/how-to-remove-all-white-space-from-the-beginning-or-end-of-a-string