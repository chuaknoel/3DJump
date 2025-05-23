using UnityEngine;

/// <summary>
/// (14, 1, 0) 위치에 첫 점프대를 생성하고,
/// 이후 적당한 간격으로 위 방향에 점프대를 누적 배치합니다.
/// </summary>
public class JumpPadSpawner : MonoBehaviour
{
    [Header("점프대 프리팹")]
    public GameObject jumpPadPrefab;

    [Header("생성 설정")]
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
            // 생성 먼저
            Instantiate(jumpPadPrefab, currentPosition, Quaternion.identity);

            // 다음 위치 계산 (단 마지막 회차는 건너뜀)
            if (i < padCount - 1)
            {
                float xOffset = Random.Range(-horizontalRange, horizontalRange);
                float zOffset = Random.Range(-horizontalRange, horizontalRange);
                float yOffset = Random.Range(2f, verticalSpacing);

                currentPosition += new Vector3(xOffset, yOffset, zOffset);
            }
        }

        Debug.Log($"[JumpPadSpawner] 점프대 {padCount}개 생성 완료 (시작 위치 포함)");
    }
}


