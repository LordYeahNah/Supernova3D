using UnityEngine;
using System.Collections.Generic;

public enum ESex
{
    MALE,
    FEMALE,
}

public enum EAgeGroup
{
    CHILD,
    ADULT
}

[System.Serializable]
public class BaseColonist : BaseCharacter
{

    // === AGE SETTINGS === //
    private readonly float AGE_INCREMENT = 0.03f;                               // Rate that the character will age at
    private readonly float CHILD_MAX_AGE = 1.0f;
    private readonly float ADULT_MIN_AGE = 3.0f;
    private readonly float ADULT_MAX_AGE = 4.5f;
    private EAgeGroup _AgeGroup;
    private float _CurrentAge;
    private float _MaxAge;
    private Timer _AgeTimer;

    // === Colonist Stats === //
    private Supernova _Supernova;
    private readonly float MAX_HEALTH = 100f;
    public Supernova SupernovaStats => _Supernova;

    // === Controller Details === //
    public ColonistController Controller;                                   // Reference to the controller node
    
    // === Resource Usage === //
    public readonly float WATER_USAGE_PER_SECOND = 0.02f;                  // Water that the colonist will use each second
    public readonly float FOOD_USAGE_PER_SECOND = 0.02f;                // Food that the colonist will use each second

    public float WaterUsageModifier = 1.0f;                     // Modifiers how much water is used
    public float FoodUsageModifier = 1.0f;                      // Modifies how much food is used

    public float WaterUsagePerSecond => WATER_USAGE_PER_SECOND + WaterUsageModifier;
    public float FoodUsagePerSecond => FOOD_USAGE_PER_SECOND + FoodUsageModifier;

    public BaseColonist(string fname, string lname, ESex sex, Supernova nova, EAgeGroup ageGroup = EAgeGroup.ADULT)
    {
        _FirstName = fname;
        _LastName = lname;
        _Sex = sex;
        _AgeGroup = ageGroup;
        _MaxAge = DetermineAgeLength();
        _Supernova = nova;
        _CurrentHealth = MAX_HEALTH;
        _AgeTimer = new Timer(1.0f, UpdateAge, true, true);
    }

    public override void OnInitialize()
    {
        base.OnInitialize();
        _BlackboardRef.SetValue(GeneralKeys.MOVE_TO_LOCATION, Controller.DebugPosition);
    }

    public override void OnUpdate()
    {
        // Update the characters age
        if(_AgeTimer != null)
            _AgeTimer.OnUpdate();
    }

    protected override void InitializeAI()
    {
        base.InitializeAI();
        _Tree = new ColonistBT(_BlackboardRef);
    }

    private void UpdateAge()
    {
        _CurrentAge += 1 * AGE_INCREMENT;                       // Increment the current age
        // Check which age group this colonist is in
        if(_AgeGroup == EAgeGroup.CHILD)
        {
            if(_CurrentAge > CHILD_MAX_AGE)
            {
                // TODO: Update character sprite
                _AgeGroup = EAgeGroup.ADULT;                   
                _CurrentAge = 0f;
            }
        } else if(_AgeGroup == EAgeGroup.ADULT)
        {
            if(_CurrentAge > _MaxAge)
            {
                KillColonist();
            }
        }
    }

    private void KillColonist()
    {
        // TODO: Handle death
    }

    private float DetermineAgeLength()
    {
        return Random.Range(ADULT_MIN_AGE, ADULT_MAX_AGE);
    }
}