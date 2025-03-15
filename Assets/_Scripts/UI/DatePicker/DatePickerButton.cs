using System;
using FrozenPhoenix.Utilities;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class DatePickerButton : MonoBehaviour
{
    public event Action<DateDataType, int> OnDateSelected;
    public DateDataType DateType;

    [SerializeField] private int value;
    private Button _button;
    
    private void Start()
    {
        HelperMethods.CheckAndAssignComponent(ref _button, gameObject);
        _button.onClick.AddListener(InformSelection);
    }

    private void OnDestroy() => _button.onClick.RemoveListener(InformSelection);

    private void InformSelection() => OnDateSelected?.Invoke(DateType, value);

    [Button]
    private void FuckThis()
    {
        value = transform.GetSiblingIndex();
    }
}