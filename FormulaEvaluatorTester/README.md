
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

I first tested how basic syntaxes (Regex.Split, Trim, TryParse) work through SplitTest, RemoveWhitespaceTest, and StrToIntTest.
Then, I checked if the helper methods in Evaluator (IsValue, IsVariable) work properly.
Finally, I made groups of tests for Evaluate method; valid/invalid, simple/complex, with/without variables, etc.

I could not find a way to verify that an exception has been thrown (such as assert). Therefore, for invaild expression tests,
I simply commented in and out each line to make sure each invalid expression throws an exception. Currently, tests for
invaild expression are commented out.

I could not find how to enable VS spell checker on Mac. I checked thoroughly but there might be some typos.

# Assignment Specific Topics

This section was not required.

# Consulted Peers:

I have not consulted with others since the instruction and the given algorithm were clear. Though I referred to the piazza posts.

# References:

    1. Main() and command-line arguments - https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/program-structure/main-command-line
    1. AsAssignment One - Formula Evaluator (instruction) - https://docs.google.com/document/d/1eB3YWaXpMuaRg4c28fJFNwlyZwzib10twioAJxu0z0A/edit#
    2. CS 3500 Piazza posts 