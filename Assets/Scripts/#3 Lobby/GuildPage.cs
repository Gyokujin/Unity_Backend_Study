using UnityEngine;
using TMPro;

public class GuildPage : MonoBehaviour
{
    [SerializeField]
    private BackendGuildSystem backendGuildStstem;
    [SerializeField]
    private TextMeshProUGUI textGuildName; // popup ��ܿ� ��µǴ� ��� �̸� Text UI
    [SerializeField]
    private GameObject excutivesOption;

    private string guildName = string.Empty; // ��� �̸�

    public void Activate(string guildName, bool isMaster = false)
    {
        excutivesOption.SetActive(isMaster);

        gameObject.SetActive(true);

        textGuildName.text = guildName;
        this.guildName = guildName;
    }

    public void OnClickApplyGuild()
    {
        backendGuildStstem.ApplyGuild(guildName);
    }
}