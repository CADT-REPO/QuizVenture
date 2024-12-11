using System.Collections.Generic;

[System.Serializable]
public class Question
{
    public string question; // The question text
    public Answers answers; // An object containing correct and wrong answers
}

[System.Serializable]
public class Answers
{
    public List<string> correct; // List of correct answers
    public List<string> wrong;   // List of wrong answers
}
