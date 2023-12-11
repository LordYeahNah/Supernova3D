using System.Collections.Generic;
using UnityEngine;

public static class ColonistGenerator
{

    public static BaseColonist GenerateColonist(bool isChild, string lastName = "")
    {
        ESex sex = (ESex)Random.Range(0, 2);                    // Determine the gender of the character

        string fname = "";                      // pre-define the first name
        if(sex == ESex.MALE)
        {
            fname = NameDatabase.Instance.GetMaleName();
        } else 
        {
            fname = NameDatabase.Instance.GetFemaleName();
        }

        if(string.IsNullOrEmpty(lastName))
            lastName = NameDatabase.Instance.GetLastName();

        EAgeGroup age = isChild ? EAgeGroup.CHILD : EAgeGroup.ADULT;
        return new BaseColonist(fname, lastName, sex, GenerateSupernova(), age);
    }

    private static Supernova GenerateSupernova()
    {
       
        Supernova supernova = new Supernova();
        supernova.StellarNavigation = Random.Range(1, 10);
        supernova.UniversalExploration = Random.Range(1, 10);
        supernova.PowerSystemManagement = Random.Range(1, 10);
        supernova.EnvironmentControl = Random.Range(1, 10);
        supernova.ResourceExtraction = Random.Range(1, 10);
        supernova.NutrientSynthesis = Random.Range(1, 10);
        supernova.OribitalDefense = Random.Range(1, 10);
        supernova.VoyagerCommunications = Random.Range(1, 10);
        supernova.AdvancedAnayltics = Random.Range(1, 10);
        return supernova;
    }
}