using System;
using System.Collections.Generic;

[Serializable]
public class GameDaySaveData
{
    public DateTime EventDate;

    public float DaysEarnings;

    public string EventLocation;
    public string EventOpposition;
    
    public string Notes;

    public int Score;
    public int OppositionScore;
    
    public int TryScored;
    public int TryAssist;
    public int Passes;
    public int Tackles;
    public int DefenceOrganisation;
    
    public Quarter QuartersPlayed;
}

[Serializable]
public class AllGameDayDatas
{
    public Dictionary<DateTime, GameDaySaveData> GameDaySaveDatas = new Dictionary<DateTime, GameDaySaveData>();
}

public class PersistantSaveData
{
    public float CurrentBalance;
}


[Serializable]
public class ConfigData
{
    public DateTime CreatedDate;
    
    public float TryScoredValue;
    public float TryAssistValue;
    public float PassesValue;
    public float TacklesValue;
    public float DefenceOrganisationValue;
    
}

public static class DefaultConfigData
{
    public static readonly ConfigData DefaultConfig = new ConfigData
    {
        CreatedDate = DateTime.Now,
        TryScoredValue = 5,
        TryAssistValue = 1,
        PassesValue = 0.2f,
        TacklesValue = 0.5f,
        DefenceOrganisationValue = 0.5f 
    };
}