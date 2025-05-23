using UnityEngine;

/// <summary>
/// (14, 1, 0) ��ġ�� ù �����븦 �����ϰ�,
/// ���� ������ �������� �� ���⿡ �����븦 ���� ��ġ�մϴ�.
/// </summary>
public class JumpPadSpawner : MonoBehaviour
{
    [Header("������ ������")]
    public GameObject jumpPadPrefab;

    [Header("���� ����")]
    public int padCount = 5;
    public float verticalSpacing = 3f;
    public float horizontalRange = 5f;

    private Vector3 currentPosition = new Vector3(14f, 1f, 0f);

    private void Start()
    {
        SpawnJumpPads();
    }

    private void SpawnJumpPads()
    {
        for (int i = 0; i < padCount; i++)
        {
            // ���� ����
            Instantiate(jumpPadPrefab, currentPosition, Quaternion.identity);

            // ���� ��ġ ��� (�� ������ ȸ���� �ǳʶ�)
            if (i < padCount - 1)
            {
                float xOffset = Random.Range(-horizontalRange, horizontalRange);
                float zOffset = Random.Range(-horizontalRange, horizontalRange);
                float yOffset = Random.Range(2f, verticalSpacing);

                currentPosition += new Vector3(xOffset, yOffset, zOffset);
            }
        }

        Debug.Log($"[JumpPadSpawner] ������ {padCount}�� ���� �Ϸ� (���� ��ġ ����)");
    }
}


