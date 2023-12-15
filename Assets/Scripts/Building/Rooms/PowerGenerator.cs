using System.Collections.Generic;
using UnityEngine;

public class PowerGenerator : BaseRoom
{
    public PowerGenerator(RoomData data) : base(data)
    {
    }

    public override void OnCollectResources()
    {
        base.OnCollectResources();
        ShipController.Instance?.Resources?.AddPower(Mathf.FloorToInt(ResourcesPerCycle));
    
    }

    protected override void _CalculateTimeBasedOnColonist()
    {
        float modifier = 1.0f;
        foreach (var colonist in _Colonist)
        {
            int powerManagement = colonist.SupernovaStats.PowerSystemManagement;
            float modifierPerColonist = (powerManagement / 13.0f);
            modifier -= modifierPerColonist;
        }

        CalculateTime(modifier);
    }
}