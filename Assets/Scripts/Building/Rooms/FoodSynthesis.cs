using System.Collections.Generic;
using UnityEngine;

public class FoodSynthesis : BaseRoom
{
    public FoodSynthesis(RoomData data) : base(data)
    {
    }

    protected override void _CalculateTimeBasedOnColonist()
    {
        float modifier = 1.0f;                      
        foreach (var colonist in _Colonist)
        {
            int nut = colonist.SupernovaStats.NutrientSynthesis;
            float modReduction = (nut / 13.0f) / 10.0f;
            modifier -= modReduction;
        }
        
        CalculateTime(modifier);
    }
}