using UnityEngine;
using TMPro;

public class GuildPage : MonoBehaviour
{
    [SerializeField]
    private BackendGuildSystem backendGuildSystem;
    [SerializeField]
    private TextMeshProUGUI textGuildName; // popup 상단에 출력되는 길드 이름 Text UI
    [SerializeField]
    private Notice notice;
    [SerializeField]
    private GameObject excutivesOption;
    [SerializeField]
    private TextMeshProUGUI textMemberCount;

    [SerializeField]
    private GameObject memberPrefab;
    [SerializeField]
    private Transform parentContent;

    private string guildName = string.Empty; // 길드 이름
    private MemoryPool memoryPool;

    private void Awake()
    {
        memoryPool = new MemoryPool(memberPrefab, parentContent);
    }

    public void Setup(string guildName, bool isMaster = false)
    {
        notice.Setup(isMaster);
        excutivesOption.SetActive(isMaster);

        gameObject.SetActive(true);

        textGuildName.text = guildName;
        this.guildName = guildName;
        textMemberCount.text = $"길드 인원 {backendGuildSystem.myGuildData.memberCount}/100";

        backendGuildSystem.GetGuildMemberList(backendGuildSystem.myGuildData.guildInDate);
    }

    public void Activate(GuildMemberData member)
    {
        GameObject item = memoryPool.ActivatePoolItem();
        item.GetComponent<GuildMember>().Setup(member);
    }

    public void Deactivate(GameObject member)
    {
        memoryPool.DeactivatePoolItem(member);
    }

    public void DeactivateAll()
    {
        memoryPool.DeactivateAllPoolItems();
    }

    public void OnClickApplyGuild()
    {
        backendGuildSystem.ApplyGuild(guildName);
    }
}