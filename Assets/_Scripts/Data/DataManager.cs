using System;
using System.Collections.Generic;
using FrozenPhoenix.Utilities;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Windows;

public class DataManager : MonoSingleton<DataManager>
{
    private const string CONFIG_FILE_NAME = "config.json";
    private const string GAME_SAVE_DATA_FILE_NAME = "gameDaySaveData.json";
    private const string PERSISTANT_SAVE_DATA_FILE_NAME = "persistantSaveData.json";
    
    private AllGameDayDatas _allGameDayDatas;

    private ConfigData _currentConfigData;

    private PersistantSaveData _persistantSaveData;

    public GameDaySaveData CurrentGameDaySaveData { get; private set; }

    public readonly Dictionary<EventType, float> EventValuesMap = new Dictionary<EventType, float>();
    public readonly Dictionary<EventType, int> EventTriggersMap = new Dictionary<EventType, int>();

    private void Start()
    {
        InitConfig();

        InitSaveData();

        InitPersistantSaveData();
    }

    private void InitPersistantSaveData()
    {
        if (File.Exists(Application.persistentDataPath + "/" + PERSISTANT_SAVE_DATA_FILE_NAME))
        {
            var json = System.IO.File.ReadAllText(Application.persistentDataPath + "/" +
                                                  PERSISTANT_SAVE_DATA_FILE_NAME);
            _persistantSaveData = JsonUtility.FromJson<PersistantSaveData>(json);
        }
        else
        {
            _persistantSaveData = new PersistantSaveData();
        }
    }

    private void InitSaveData()
    {
        if (File.Exists(GetSaveDataPath()))
        {
            var json = System.IO.File.ReadAllText(GetSaveDataPath());
            CurrentGameDaySaveData = JsonUtility.FromJson<GameDaySaveData>(json);
        }
        else
        {
            CurrentGameDaySaveData = new GameDaySaveData();
        }
    }

    private void InitConfig()
    {
        if (File.Exists(GetConfigSavePath()))
        {
            var json = System.IO.File.ReadAllText(GetConfigSavePath());
            _currentConfigData = JsonUtility.FromJson<ConfigData>(json);
        }
        else
        {
            CreateDefaultData();
        }

        PopulateEventValuesDictionary();
    }

    private void PopulateEventValuesDictionary()
    {
        EventValuesMap.TryAdd(EventType.TryScored, _currentConfigData.TryScoredValue);
        EventValuesMap.TryAdd(EventType.TryAssist, _currentConfigData.TryAssistValue);
        EventValuesMap.TryAdd(EventType.Passes, _currentConfigData.PassesValue);
        EventValuesMap.TryAdd(EventType.Tackles, _currentConfigData.TacklesValue);
        EventValuesMap.TryAdd(EventType.Organization, _currentConfigData.DefenceOrganisationValue);
    }

    private void CreateDefaultData()
    {
        _currentConfigData = new ConfigData
        {
            CreatedDate = System.DateTime.Now,
            TryScoredValue = DefaultConfigData.DefaultConfig.TryScoredValue,
            TryAssistValue = DefaultConfigData.DefaultConfig.TryAssistValue,
            PassesValue = DefaultConfigData.DefaultConfig.PassesValue,
            TacklesValue = DefaultConfigData.DefaultConfig.TacklesValue,
            DefenceOrganisationValue = DefaultConfigData.DefaultConfig.DefenceOrganisationValue
        };
    }
    
    public float GetValueOfEvent(EventType eventType)
    {
        if (EventValuesMap.TryGetValue(eventType, out var value))
        {
            return value;
        }
        Start();
        return GetValueOfEvent(eventType);
    }

    public void SaveConfigData()
    {
        var json = JsonUtility.ToJson(_currentConfigData);
        System.IO.File.WriteAllText(GetConfigSavePath(), json);
    }

    private void SaveData()
    {
        var json = JsonUtility.ToJson(CurrentGameDaySaveData);
        System.IO.File.WriteAllText(GetSaveDataPath(), json);

        var persistantJson = JsonUtility.ToJson(_persistantSaveData);
        System.IO.File.WriteAllText(Application.persistentDataPath + "/" + PERSISTANT_SAVE_DATA_FILE_NAME,
            persistantJson);
    }

    public void UpdateQuarterValue(Quarter quarter, bool played)
    {
        if (played)
            CurrentGameDaySaveData.QuartersPlayed |= quarter;
        else
            CurrentGameDaySaveData.QuartersPlayed &= ~quarter;

        SaveData();
    }

    public void UpdateEventValue(EventType eventType, int eventsTriggered)
    {
        EventTriggersMap[eventType] = eventsTriggered;

        switch (eventType)
        {
            case EventType.TryScored:
                CurrentGameDaySaveData.TryScored = eventsTriggered;
                break;
            case EventType.TryAssist:
                CurrentGameDaySaveData.TryAssist = eventsTriggered;
                break;
            case EventType.Passes:
                CurrentGameDaySaveData.Passes = eventsTriggered;
                break;
            case EventType.Tackles:
                CurrentGameDaySaveData.Tackles = eventsTriggered;
                break;
            case EventType.Organization:
                CurrentGameDaySaveData.DefenceOrganisation = eventsTriggered;
                break;
        }

        RecalculateEarnings();

        SaveData();
    }

    private static string GetConfigSavePath() => Application.persistentDataPath + "/" + CONFIG_FILE_NAME;

    private static string GetSaveDataPath() => Application.persistentDataPath + "/" + GAME_SAVE_DATA_FILE_NAME;

    [Button]
    private void ClearData()
    {
        CurrentGameDaySaveData = new GameDaySaveData();
        _persistantSaveData = new PersistantSaveData();
        SaveData();
    }

    [Button]
    private void ConfirmDaysEarnings()
    {
        _persistantSaveData.CurrentBalance += CurrentGameDaySaveData.DaysEarnings;
        CurrentGameDaySaveData = new GameDaySaveData();
        SaveData();
    }

    private void RecalculateEarnings()
    {
        CurrentGameDaySaveData.DaysEarnings = 0;
        foreach (var eventTrigger in EventTriggersMap)
        {
            CurrentGameDaySaveData.DaysEarnings += EventValuesMap[eventTrigger.Key] * eventTrigger.Value;
        }
    }

    public int GetEventTriggers(DateTime today, EventType eventType)
    {
        throw new NotImplementedException();
    }
}