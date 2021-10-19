using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ResourceAmountView : MonoBehaviour
{
    [SerializeField] Image _resourceIcon;
    [SerializeField] TextMeshProUGUI _resourceAmountText;

    public Image ResourceIcon => _resourceIcon;
    public TextMeshProUGUI ResouurceAmountText => _resourceAmountText;
}
