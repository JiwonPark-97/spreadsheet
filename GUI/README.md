
```
Author:     Jiwon Park
Partner:    None
Course:     CS 3500, University of Utah, School of Computing
GitHub ID:  JiwonPark-97
Repo:       https://github.com/uofu-cs3500-spring23/spreadsheet-JiwonPark-97.git
Date:       6-Mar-2023 11.30 pm (when submission was completed) 
Project:    Spreadsheet
Copyright:  CS 3500 and Jiwon Park - This work may not be copied for use in Academic Coursework.
```

# Partnership:

    I worked solo.

# Branching:

    All of my work is done under main branch.

# Additional Features and Design Decisions:
    
    I added a "sum" feature which adds up values on the entire row or column.
    User can enter either a label of row (i.e. 1-99) or a label of column (i.e. A-Z)
    and click "calculate" button to get the result.

    Under help menu, there is "what's this error?" feature which displays a pop up that explains possible errors.

# Time Tracking (Personal Software Practice):

    I estimated that this assignment would take about 20 hours like the previous assignments. 
    However, understanding how MAUI works and handling edge cases took longer than I thought, and I ended up spending about 30 hours for this assignment.

# Best (Team) Practices:

    Write a paragraph about how assigning tasks help you keep focused (pun intended) on "task".  

    Assigning tasks on Github project helped me keep focused on tasks for this assignment.
    
    I tried to avoid repeated code (DRY) by adding helper methods and make the code self explanatory by using descriptive names.
    When the code cannot be self explanatory, I added comments to make the code easier to understand. Also, I made multiple commits
    with descriptive commit messages.

# Comments to Evaluators:
    
    When asking to save the data, I tried to call FileMenuSave method to avoid repeated code, but the next line was always executed before
    FileMenuSave finishes. Therefore, I decided to simply copy and paste FileMenuSave's code to FileMenuNew and FileMenuOpen.

    The horizontal scroll bar does not scroll row headers (only cells). Since it is not safe to nest ScrollViews - informed in lecture - I did not 
    fix this issue.

# Consulted Peers:

    I have not directly consulted with others but referred to Piazza posts and the Discord server listed below in reference.

# References:

    1. CS 3500 Spring: Software Practice - https://discord.gg/psqhddzf
    2. How can we get the numeric value of a character in C# - https://www.educative.io/answers/how-can-we-get-the-numeric-value-of-a-character-in-c-sharp
    3. Display pop-ups - https://learn.microsoft.com/en-us/dotnet/maui/user-interface/pop-ups?view=net-maui-7.0
    4. Focus Cell A1 on Spreadsheet Application Open - https://utah.instructure.com/courses/834041/external_tools/90790
    5. regular expressions 101 - https://regex101.com/
    6. Adding a newline into a string in C# - https://stackoverflow.com/questions/224236/adding-a-newline-into-a-string-in-c-sharp
    7. Fonts in .NET MAUI - https://learn.microsoft.com/en-us/dotnet/maui/user-interface/fonts?view=net-maui-7.0
    8. Align and position .NET MAUI controls - https://learn.microsoft.com/en-us/dotnet/maui/user-interface/align-position?view=net-maui-7.0
    9. Changing your MAUI application’s title bar color  - https://putridparrot.com/blog/changing-your-maui-applications-title-bar-colour/
    10. Regex 101 - https://regex101.com/
    11. Get started with XAML - https://learn.microsoft.com/en-us/dotnet/maui/xaml/fundamentals/get-started?view=net-maui-7.0
    

