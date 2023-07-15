
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardEntry : MonoBehaviour
{
    [SerializeField] private Image background;
    [SerializeField] private TMP_Text Position;
    [SerializeField] private TMP_Text Name;
    [SerializeField] private TMP_Text Time;

    [SerializeField] private Color lightBGColor;
    [SerializeField] private Color darkBGColor;
    [SerializeField] private Color userBGColor;

    public void SetData(float time, int position, string name, bool isUser = false)
    {
        background.color = isUser ? userBGColor
                         : position % 2 == 0 ? lightBGColor
                         : darkBGColor;
        Position.text = $"{position}.";
        Name.text = name;
        Time.text = time.ToString("0.00");

    }
}
