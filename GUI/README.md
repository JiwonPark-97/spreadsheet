
```
Author:     Jiwon Park
Partner:    None
Course:     CS 3500, University of Utah, School of Computing
GitHub ID:  JiwonPark-97
Repo:       https://github.com/uofu-cs3500-spring23/spreadsheet-JiwonPark-97.git
Date:       6-Mar-2023 11.30 pm (when submission was completed) 
Project:    GUI
Copyright:  CS 3500 and Jiwon Park - This work may not be copied for use in Academic Coursework.
```

# Partnership:

    I worked solo for this assignment.

# Branching:

    All of my work is done under main branch.

# Additional Features and Design Decisions:
    
    I added a "sum" feature which adds up values on the entire row or column.
    User can enter either a label of row (i.e. 1-99) or a label of column (i.e. A-Z) and click "calculate" button to get the result.

    Under help menu, there is "what's this error?" feature which displays a pop up that contains summary of possible errors.

    A selected cell gets different background color so that the user can recognize it easily.

# Time Tracking (Personal Software Practice):

    estimated hours: 20
    actual hours: 30
    hours learning tools and techniques: 5
    hours implementing required features: 10
    hours spent debugging: 15

    Overall, my time estimates are getting closer to the actual time spent and I think this means that
    I'm getting better or used to working on programming assignments.
    However, as this was the first time for me to building a GUI, it took me much longer than I thought. 

# Best (Team) Practices:

    Assigning tasks on Github project helped me keep track of multiple tasks for the assignment. 
    The tasks on each column helped me organized and quickly go back to what I was doing (an hour or a day) before.
    Especially, the code review section reminded me of reviewing specific parts of code, which helped me to find multiple errors.
    
    I tried to avoid repeated code (DRY) by adding helper methods and make the code self explanatory by using descriptive names.
    When the code cannot be self explanatory, I added comments to make the code easier to understand. Also, I made multiple commits
    with descriptive commit messages.

# Comments to Evaluators:
    
    For safety feature, when open/new clicked with unsaved changes, I tried to call FileMenuSave method to avoid repeated code.
    However, the line where I called the FileMenuSave method always finished after the next line gets executed - resulted in loss of data/
    So, I decided to simply copy and paste the code from FileMenuSave to of FileMenuNew and FileMenuOpen methods.

    The horizontal scroll bar does not scroll row headers (scroll only cells). Since it is not safe to nest ScrollViews 
    - informed in lecture - I did not fix this issue.

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
    

