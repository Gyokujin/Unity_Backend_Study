using System.Collections.Generic;

public class GuildData
{
    public string guildName; // ��� �̸�
    public string guildInDate; // ��� InDate
    public int memberCount; // ��� �ο���
    public GuildMemberData master; // ��� ������
    public List<GuildMemberData> viceMasterList; // �� ��� ������ ���
}