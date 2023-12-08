using UnityEngine;
using TMPro;

public class GuildDefaultPage : MonoBehaviour
{
    [SerializeField]
    private BackendGuildSystem backendGuildSystem;
    [SerializeField]
    private TMP_InputField inputFieldGuildName;
    [SerializeField]
    private GuildPage guildPage;
    [SerializeField]
    private FadeEffect_TMP textLog;

    public void OnClickSearchGuild()
    {
        string guildName = inputFieldGuildName.text;

        if (guildName.Trim().Equals(""))
        {
            return;
        }

        inputFieldGuildName.text = "";

        string inDate = backendGuildSystem.GetGuildInfoBy(guildName);

        if (inDate.Equals(string.Empty))
        {
            textLog.FadeOut("존재하지 않는 길드입니다.");
        }
        else
        {
            backendGuildSystem.GetGuildInfo(inDate);
        }
    }

    public void OnClickMyGuildInfo()
    {
        backendGuildSystem.GetMyChildInfo();
    }

    public void SuccessMyGuildInfo()
    {
        bool isMaster = UserInfo.Data.nickname.Equals(backendGuildSystem.myGuildData.master.nickname);
        guildPage.Setup(backendGuildSystem.myGuildData.guildName, isMaster);
    }

    public void SuccessGuildInfo()
    {
        bool isMaster = UserInfo.Data.nickname.Equals(backendGuildSystem.otherGuildData.master.nickname);
        guildPage.Setup(backendGuildSystem.otherGuildData.guildName, isMaster, true);
    }
}