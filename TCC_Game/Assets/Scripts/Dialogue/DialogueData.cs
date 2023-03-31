using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueData
{
    public List<Dialogue> dialogue;
}

[System.Serializable]
public class Dialogue
{
    public int id;
    public string character;
    public string text;
    public List<Condition> condition;
    public List<Choice> choices;
    public int nextId;
    public int nextFirstId;
}

[System.Serializable]
public class Condition
{
    public string type;
    public bool boolValue;
    public int number;
}

[System.Serializable]
public class Choice
{
    public int choiceId;
    public string text;
    public int nextId;
}
