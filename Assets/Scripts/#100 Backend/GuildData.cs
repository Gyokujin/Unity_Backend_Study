using System.Collections.Generic;

public class GuildData
{
    public string guildName; // 길드 이름
    public string guildInDate; // 길드 InDate
    public int memberCount; // 길드 인원수
    public GuildMemberData master; // 길드 마스터
    public List<GuildMemberData> viceMasterList; // 부 길드 마스터 목록
}