using UnityEngine;
using TMPro;

public class GuildPage : MonoBehaviour
{
    [SerializeField]
    private BackendGuildSystem backendGuildStstem;
    [SerializeField]
    private TextMeshProUGUI textGuildName; // popup 상단에 출력되는 길드 이름 Text UI

    private string guildName = string.Empty; // 길드 이름

    public void Activate(string guildName)
    {
        gameObject.SetActive(true);

        textGuildName.text = guildName;
        this.guildName = guildName;
    }

    public void OnClickApplyGuild()
    {
        backendGuildStstem.ApplyGuild(guildName);
    }
}