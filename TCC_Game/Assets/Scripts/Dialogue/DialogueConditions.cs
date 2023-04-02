using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueConditions
{
    static bool hasSummer = true, hasAutumn = false, hasWinter = true;
    static int summerCondition = 2, autumnCondition = 1, winterCondition = 1;
    static int foodType = 1;
    static bool confirmation = false;

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
}
