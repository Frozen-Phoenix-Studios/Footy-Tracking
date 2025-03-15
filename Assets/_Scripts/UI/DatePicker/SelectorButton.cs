using System;
using System.Collections.Generic;
using FrozenPhoenix.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectorButton : MonoBehaviour
{
    [SerializeField] private DateDataType type;
    [SerializeField] private TMP_Text displayText;
    private Button _button;
    
    [SerializeField] private GameObject scrollRect;
    

    private List<DatePickerButton> _buttons = new List<DatePickerButton>();

    private void Awake()
    {
        HelperMethods.CheckAndAssignComponent(ref _button, gameObject);
        _button.onClick.AddListener(ActivateScrollRect);
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveListener(ActivateScrollRect);
        DatePickerManager.Instance.OnDateConfirmed -= HandleDateConfirmed;
        DatePickerManager.Instance.OnModalClosed -= DeactivateScrollRect;
    }

    public void Init()
    {
        displayText.SetText(DatePickerManager.Instance.Convert(type, DateTime.Now));
        
        _buttons = new List<DatePickerButton>(GetComponentsInChildren<DatePickerButton>());

        // HelperMethods.CheckAndAssignComponentInChild(ref _buttons, gameObject);
        foreach (var button in _buttons)
        {
            button.OnDateSelected += HandleDateSelected;
        }
        
        scrollRect.SetActive(false);
        
        DatePickerManager.Instance.OnDateConfirmed += HandleDateConfirmed;
        DatePickerManager.Instance.OnModalClosed += DeactivateScrollRect;
    }

    private void HandleDateConfirmed(DateTime obj)
    {
        displayText.SetText($"{DatePickerManager.Instance.Convert(type, obj)}");
        DeactivateScrollRect();
    }

    private void HandleDateSelected(DateDataType dateType, int value)
    {
        displayText.SetText($"{DatePickerManager.Instance.Convert(type, value)}");
        DatePickerManager.Instance.HandleDataPicked(dateType, value);
        
        DeactivateScrollRect();
    }

    private void ActivateScrollRect()
    {
        scrollRect.SetActive(true);
        displayText.gameObject.SetActive(false);
    }

    private void DeactivateScrollRect()
    {
        scrollRect.SetActive(false);
        displayText.gameObject.SetActive(true);
    }
}