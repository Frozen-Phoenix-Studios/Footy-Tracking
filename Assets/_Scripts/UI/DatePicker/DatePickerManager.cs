using System;
using System.Collections.Generic;
using FrozenPhoenix.Utilities;
using UnityEngine;
using UnityEngine.UI;

public class DatePickerManager : MonoSingleton<DatePickerManager>
{
    public event Action<DateTime> OnDateConfirmed;
    public event Action OnModalClosed;
    
    private Dictionary<int, string> _monthValues = new Dictionary<int, string>();
    private Dictionary<int, int> _yearValues = new Dictionary<int, int>();
    
    [SerializeField] private GameObject panel;
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button closeButton;
    
    private DateTime _selectedDate;
    
    private List<SelectorButton> _selectors = new List<SelectorButton>();

    protected override void Init()
    {
        base.Init();
        _selectedDate = DateTime.Today;

        SetUpDictionaries();
        
        _selectors = new List<SelectorButton>(GetComponentsInChildren<SelectorButton>());
        
        foreach (var selectorButton in _selectors)
            selectorButton.Init();
        
        confirmButton.onClick.AddListener(ConfirmDate);
        closeButton.onClick.AddListener(CloseModal);
        
        panel.SetActive(false);
    }

    private void SetUpDictionaries()
    {
        _monthValues = new Dictionary<int, string>()
        {
            {0, "January"},
            {1, "February"},
            {2, "March"},
            {3, "April"},
            {4, "May"},
            {5, "June"},
            {6, "July"},
            {7, "August"},
            {8, "September"},
            {9, "October"},
            {10, "November"},
            {11, "December"}
        };

        _yearValues = new Dictionary<int, int>()
        {
            { 0, 2025 },
            { 1, 2026 },
            { 2, 2027 },
            // { 3, 2028 },
        };
    }

    public void HandleDataPicked(DateDataType type, int value)
    {
        switch (type)
        {
            case DateDataType.Day:
                _selectedDate = new DateTime(_selectedDate.Year, _selectedDate.Month, value + 1);
                break;
            case DateDataType.Month:
                _selectedDate = new DateTime(_selectedDate.Year, value + 1, _selectedDate.Day);
                break;
            case DateDataType.Year:
                _selectedDate = new DateTime(_yearValues[value], _selectedDate.Month, _selectedDate.Day);
                break;
        }
    }

    public string Convert(DateDataType type, DateTime date)
    {
        switch (type)
        {
            case DateDataType.Day:
                return date.Day.ToString();
            case DateDataType.Month:
                return _monthValues[date.Month - 1];
            case DateDataType.Year:
                return date.Year.ToString();
            default:
                return string.Empty;
        }
    }
    
    public string Convert(DateDataType type, int value)
    {
        switch (type)
        {
            case DateDataType.Day:
                return (value + 1).ToString();
            case DateDataType.Month:
                return _monthValues[value];
            case DateDataType.Year:
                return _yearValues[value].ToString();
            default:
                return string.Empty;
        }
    }
    
    private void ConfirmDate()
    {
        OnDateConfirmed?.Invoke(_selectedDate);
        Debug.Log($"Selected Date: {_selectedDate}");
        panel.SetActive(false);
    }
    
    private void CloseModal()
    {
        OnModalClosed?.Invoke();
        panel.SetActive(false);
    }

    public void ActivateDatePicker() => panel.SetActive(true);
}