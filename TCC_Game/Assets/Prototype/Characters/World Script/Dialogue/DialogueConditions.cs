using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueConditions
{
    public static int foodType = 1, summerCondition = 1, autumnCondition = 1, winterCondition = 1;
    public static bool confirmation = false, hasSummer = false, hasAutumn = false, hasWinter = false;

    public List<Condition> conditions = new List<Condition>();

    public List<Condition> TurnToConditions(bool firstTime) 
    {
        conditions.Clear();
        Dictionary<string, bool> conditionBool = new Dictionary<string, bool>() {
            {nameof(firstTime), firstTime},
            {nameof(hasSummer), hasSummer},
            {nameof(hasAutumn), hasAutumn},
            {nameof(hasWinter), hasWinter},
            {nameof(confirmation), confirmation}
        };
        Dictionary<string, int> conditionInt = new Dictionary<string, int>() {
            {nameof(summerCondition), summerCondition},
            {nameof(autumnCondition), autumnCondition},
            {nameof(winterCondition), winterCondition},
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

    public static void RemoveAllArtifacts()
    {
        hasSummer = false;
        hasAutumn = false;
        hasWinter = false;
    }
}
