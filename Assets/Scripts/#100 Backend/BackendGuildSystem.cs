using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using System;

public class BackendGuildSystem : MonoBehaviour
{
    [SerializeField]
    private FadeEffect_TMP textLog;
    [SerializeField]
    private GuildCreatePage guildCreatePage;

    public void CreateGuild(string guildName, int goodsCount = 1)
    {
        Backend.Guild.CreateGuildV3(guildName, goodsCount, callback =>
        {
            if (!callback.IsSuccess())
            {
                ErrorLogCreateGuild(callback);

                return;
            }

            Debug.Log($"��尡 �����Ǿ����ϴ�. : {callback}");

            // ��� ������ �������� �� ȣ��
            guildCreatePage.SuccessCreateGuild();
        });
    }

    public void ApplyGuild(string guildName)
    {
        // GetGuildIndateByGuildNameV3() �޼ҵ带 ȣ���� ���ϴ� ���(guildName)�� guildInDate ���� ��ȯ
        string guildInDate = GetGuildInfoBy(guildName);

        // guildInDate ������ ���� ��忡 ���� ��û�� ������.
        Backend.Guild.ApplyGuildV3(guildInDate, callback =>
        {
            if (!callback.IsSuccess())
            {
                ErrorLogApplyGuild(callback);

                return;
            }

            Debug.Log($"��� ���� ��û�� �����߽��ϴ�. : {callback}");
        });
    }

    public void GetApplicants()
    {
        Backend.Guild.GetApplicantsV3(callback =>
        {
            if (!callback.IsSuccess())
            {
                // ���� ������ 403 �ϳ� �ۿ� ���� ������ ������ �޼ҵ� ���� X
                ErrorLog(callback.GetMessage(), "Guild_Failed_Log", "GetApplicants");

                return;
            }

            // JSON ������ �Ľ� ����
            try
            {
                LitJson.JsonData jsonData = callback.GetFlattenJSON()["rows"];

                if (jsonData.Count <= 0)
                {
                    Debug.LogWarning("��� ���� ��û ����� ����ֽ��ϴ�.");
                    return;
                }

                List<TransactionValue> transcationList = new List<TransactionValue>();
                List<GuildMemberData> guildMemberDataList = new List<GuildMemberData>();

                foreach (LitJson.JsonData item in jsonData)
                {
                    GuildMemberData guildMember = new GuildMemberData();

                    guildMember.nickname = item["nickname"].ToString().Equals("True") ? "NONAME" : item["nickname"].ToString();
                    guildMember.inDate = item["inDate"].ToString();

                    guildMemberDataList.Add(guildMember);

                    // guildMember.inDate�� ������ ģ���� UserGameData ���� �ҷ�����
                    Where where = new Where();
                    where.Equal("owner_inDate", guildMember.inDate);
                    transcationList.Add(TransactionValue.SetGet(Constants.USER_DATA_TABLE, where));
                }

                Backend.GameData.TransactionReadV2(transcationList, callback =>
                {
                    if (!callback.IsSuccess())
                    {
                        ErrorLog(callback.GetMessage(), "Guild_Failed_Log", "GetApplicants - TransactionReadV2");
                        return;
                    }

                    LitJson.JsonData userData = callback.GetFlattenJSON()["Responses"];

                    if (userData.Count <= 0)
                    {
                        Debug.LogWarning("�����Ͱ� �������� �ʽ��ϴ�.");
                        return;
                    }

                    for (int i = 0; i < userData.Count; ++i)
                    {
                        guildMemberDataList[i].level = userData[i]["level"].ToString();
                        Debug.Log(guildMemberDataList[i].ToString());
                    }
                });
            }
            // JSON ������ �Ľ� ����
            catch (Exception e)
            {
                // try-catch ���� ���
                Debug.LogError(e);
            }
        });
    }

    public string GetGuildInfoBy(string guildName)
    {
        // �ش� ����(guildName)�� ��尡 �����ϴ��� ���δ� ����� ����
        var bro = Backend.Guild.GetGuildIndateByGuildNameV3(guildName);
        string inDate = string.Empty;

        if (!bro.IsSuccess())
        {
            Debug.LogError($"��� �˻� ���� ������ �߻��߽��ϴ�. : {bro}");
            return inDate;
        }

        try
        {
            inDate = bro.GetFlattenJSON()["guildInDate"].ToString();

            Debug.LogError($"{guildName}�� inDate ���� {inDate} �Դϴ�.");
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }

        return inDate;
    }

    private void ErrorLogCreateGuild(BackendReturnObject callback)
    {
        string message = string.Empty;

        switch (int.Parse(callback.GetStatusCode()))
        {
            case 403: // Backend Console�� ������ ������ �������� ������ ��
                message = "��� ������ ���� ������ �����մϴ�.";
                break;
            case 409: // ������ �������� ���� �õ��� ���
                message = "�̹� ������ �̸��� ��尡 �����մϴ�.";
                break;
            default:
                message = callback.GetMessage();
                break;
        }

        ErrorLog(message, "Guild_Failed_Log", "ApplyGuild");
    }

    private void ErrorLogApplyGuild(BackendReturnObject callback)
    {
        string message = string.Empty;

        switch (int.Parse(callback.GetStatusCode()))
        {
            case 403: // Backend Console�� ������ ������ �������� ������ ��
                message = "��� ������ ���� ������ �����մϴ�.";
                break;
            case 409:
                message = "�̹� ���� ��û�� ����Դϴ�.";
                break;
            case 412:
                message = "�̹� �ٸ� ��忡 �ҼӵǾ� �ֽ��ϴ�.";
                break;
            case 429:
                message = "��忡 �� �̻� �ڸ��� �����ϴ�.";
                break;
        }

        ErrorLog(message, "Guild_Failed_Log", "ApplyGuild");
    }

    private void ErrorLog(string message, string behaviorType = "", string paramKey = "")
    {
        // ���� ������ Console View�� ���
        Debug.LogError($"{paramKey} : {message}");

        // ���� ������ UI�� ���
        textLog.FadeOut(message);

        // ���� ������ Backend Console�� ����
        Param param = new Param() { { paramKey, message } };
        // InsertLogV2(�ൿ ����, Key&Value)
        Backend.GameLog.InsertLog(behaviorType, param);
    }
}