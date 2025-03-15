using FrozenPhoenix.Utilities;
using UnityEngine;
using UnityEngine.UI;

public class QuartersButton : MonoBehaviour
{
    [SerializeField] private Quarter quarter;
    [SerializeField] private Image activeImage;
    
    private bool _toggled;
    private Button _button;
    
    private void Start()
    {
        HelperMethods.CheckAndAssignComponent(ref _button, gameObject);
        _button.onClick.AddListener(ToggleQuarter);
        
        _toggled = DataManager.Instance.CurrentGameDaySaveData.QuartersPlayed.HasFlag(quarter);
        activeImage.enabled = _toggled;
    }

    private void ToggleQuarter()
    {
        _toggled = !_toggled;
        activeImage.enabled = _toggled;
        
        DataManager.Instance.UpdateQuarterValue(quarter, _toggled);
    }
}