using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventsButton : MonoBehaviour
{
    [SerializeField] private EventType eventType;
    [SerializeField] private TMP_Text eventTypeText;

    [SerializeField] private int eventsTriggered;
    [SerializeField] private TMP_Text eventsTriggeredText;

    private float _earnedValue;
    [SerializeField] private TMP_Text earnedValueText;

    [SerializeField] private Button confirmButton; //adds one
    [SerializeField] private Button cancelButton; //removes one

    private void Start()
    {
        confirmButton.onClick.AddListener(AddValue);
        cancelButton.onClick.AddListener(RemoveValue);
        eventTypeText.SetText(eventType.ToString());
        
        eventsTriggered = DataManager.Instance.GetEventTriggers(DateTime.Today, eventType);
        
        _earnedValue = DataManager.Instance.GetValueOfEvent(eventType) * eventsTriggered;
        earnedValueText.SetText($"${_earnedValue}");
    }

    private void AddValue() 
    {
        eventsTriggered++;
        eventsTriggeredText.SetText(eventsTriggered.ToString());

        _earnedValue = DataManager.Instance.EventValuesMap[eventType] * eventsTriggered;
        earnedValueText.SetText($"${_earnedValue}");
        
        DataManager.Instance.UpdateEventValue(eventType, eventsTriggered);
    }

    private void RemoveValue()
    {
        if (eventsTriggered <= 0) return;
        
        eventsTriggered--;
        eventsTriggeredText.SetText(eventsTriggered.ToString());

        _earnedValue = DataManager.Instance.EventValuesMap[eventType] * eventsTriggered;
        earnedValueText.SetText($"${_earnedValue}");
        
        DataManager.Instance.UpdateEventValue(eventType, eventsTriggered);
    }
}