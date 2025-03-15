using System;
using FrozenPhoenix.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DateDisplayButton : MonoBehaviour
{
    [SerializeField] private TMP_Text dateText;
    private Button _button;

    private void Start()
    {
        DatePickerManager.Instance.OnDateConfirmed += HandleDateConfirmed;
        HelperMethods.CheckAndAssignComponent(ref _button, gameObject);
        _button.onClick.AddListener(ActivateDatePicker);
    }

    private void ActivateDatePicker()
    {
        DatePickerManager.Instance.ActivateDatePicker();
    }

    private void HandleDateConfirmed(DateTime date)
    {
        dateText.SetText($"{date:dd/MM/yyyy}");
    }
}
