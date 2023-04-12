using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueConditions
{
    static int foodType = 1;
    static bool confirmation = false;

    public List<Condition> conditions = new List<Condition>();

    public List<Condition> TurnToConditions(bool firstTime) 
    {
        conditions.Clear();
        Dictionary<string, bool> conditionBool = new Dictionary<string, bool>() {
            {nameof(firstTime), firstTime},
            {"hasSummer", ToBoolean(PlayerPrefs.GetInt("hasSummer"))},
            {"hasAutumn", ToBoolean(PlayerPrefs.GetInt("hasAutumn"))},
            {"hasWinter", ToBoolean(PlayerPrefs.GetInt("hasWinter"))},
            {nameof(confirmation), confirmation}
        };
        Dictionary<string, int> conditionInt = new Dictionary<string, int>() {
            {"summerCondition", PlayerPrefs.GetInt("summerCondition")},
            {"autumnCondition", PlayerPrefs.GetInt("autumnCondition")},
            {"winterCondition", PlayerPrefs.GetInt("winterCondition")},
            {nameof(foodType), foodType}
        };

        foreach (var cond in conditionBool) 
        {
            Condition condition = new Condition();
            condition.type = cond.Key;
            condition.boolValue = cond.Value;
            conditions.Add(condition);
        }

        foreach (var cond in conditionInt) 
        {
            Condition condition = new Condition();
            condition.type = cond.Key;
            condition.number = cond.Value;
            conditions.Add(condition);
        }

        return conditions;
    }

    bool ToBoolean(int converted)
    {
        return (converted != 0);
    }
}
