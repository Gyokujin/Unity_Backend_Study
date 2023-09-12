using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BackEnd;

public class RegisterAccount : LoginBase
{
    [SerializeField]
    private Image imageID; // ID �ʵ� ���� ����
    [SerializeField]
    private TMP_InputField inputFieldID; // ID �ʵ� �ؽ�Ʈ ���� ����
    [SerializeField]
    private Image imagePW; // PW �ʵ� ���� ����
    [SerializeField]
    private TMP_InputField inputFieldPW; // PW �ʵ� �ؽ�Ʈ ���� ����
    [SerializeField]
    private Image imageConfirmPW; // Confirm PW �ʵ� ���� ����
    [SerializeField]
    private TMP_InputField inputFieldConfirmPW; // Confirm PW �ʵ� �ؽ�Ʈ ���� ����
    [SerializeField]
    private Image imageEmail; // E-mail �ʵ� ���� ����
    [SerializeField]
    private TMP_InputField inputFieldEmail; // E-mail �ʵ� �ؽ�Ʈ ���� ����

    [SerializeField]
    private Button btnRegisterAccount; // "���� ����" ��ư (��ȣ�ۿ� ����/�Ұ���)

    /// <summary>
    /// "���� ����" ��ư�� ������ �� ȣ��
    /// </summary>
    public void OnClickRegisterAccount()
    {
        // �Ű������� �Է��� InputField ID�� ����� Message ���� �ʱ�ȭ
        ResetUI(imageID, imagePW, imageConfirmPW, imageEmail);

        // �ʵ� ���� ����ִ��� üũ
        if (IsFieldDataEmpty(imageID, inputFieldID.text, "���̵�")) return;
        if (IsFieldDataEmpty(imagePW, inputFieldPW.text, "��й�ȣ")) return;
        if (IsFieldDataEmpty(imageConfirmPW, inputFieldConfirmPW.text, "��й�ȣ Ȯ��")) return;
        if (IsFieldDataEmpty(imageEmail, inputFieldEmail.text, "���� �ּ�")) return;
        
        // ��й�ȣ�� ��й�ȣ Ȯ���� ������ �ٸ� ��
        if (!inputFieldPW.text.Equals(inputFieldConfirmPW.text))
        {
            GuideForIncorrectlyEnteredData(imageConfirmPW, "��й�ȣ�� ��ġ���� �ʽ��ϴ�.");
            return;
        }

        // ���� ���� �˻�
        if (!inputFieldEmail.text.Contains("@"))
        {
            GuideForIncorrectlyEnteredData(imageEmail, "���� ������ �߸��Ǿ����ϴ�. (ex. address@xx.xx)");
            return;
        }

        // ���� ���� ��ư�� ��ȣ�ۿ� ��Ȱ��ȭ
        btnRegisterAccount.interactable = false;
        SetMessage("���� �������Դϴ�..");

        // �ڳ� ���� ���� ���� �õ�
        CustomSignUp();
    }

    /// <summary>
    /// ���� ���� �õ� �� �����κ��� ���޹��� message�� ������� ���� ó��
    /// </summary>
    private void CustomSignUp()
    {
        Backend.BMember.CustomSignUp(inputFieldID.text, inputFieldPW.text, callback =>
        {
            // "���� ����" ��ư ��ȣ�ۿ� Ȱ��ȭ
            btnRegisterAccount.interactable = true;

            // ���� ���� ����
            if (callback.IsSuccess())
            {
                // E-mail ���� ������Ʈ
                Backend.BMember.UpdateCustomEmail(inputFieldEmail.text, callback =>
                {
                    if (callback.IsSuccess())
                    {
                        SetMessage($"���� ���� ����. {inputFieldID.text}�� ȯ���մϴ�.");

                        // ���� ������ �������� �� �ش� ������ ���� ���� ����
                        BackendGameData.Instance.GameDataInsert();

                        // ��� ��Ʈ ������ �ҷ�����
                        BackendChartData.LoadAllChart();

                        // Lobby ������ �̵�
                        Utils.LoadScene(SceneNames.Lobby);
                    }
                });
            }
            // ���� ���� ����
            else
            {
                string message = string.Empty;

                switch (int.Parse(callback.GetStatusCode()))
                {
                    case 409: // �ߺ��� customID�� �����ϴ� ��� 
                        message = "�̹� �����ϴ� ���̵��Դϴ�.";
                        break;
                    case 403: // ���ܴ��� ����̽��� ���
                    case 401: // ������Ʈ ���°� '����'�� ���
                    case 400: // ����̽� ������ null�� ���
                    default: 
                        message = callback.GetMessage();
                        break;
                }

                if (message.Contains("���̵�"))
                {
                    GuideForIncorrectlyEnteredData(imageID, message);
                }
                else
                {
                    SetMessage(message);
                }
            }
        });
    }
}