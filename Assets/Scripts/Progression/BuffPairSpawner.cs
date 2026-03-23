using UnityEngine;

/// <summary>
/// 左右固定2レーンの上端から、バフ2つを同時に落とす（プレイヤーは自由横移動で取りに行く）。
/// </summary>
public sealed class BuffPairSpawner : MonoBehaviour
{
    [SerializeField] private BuffChoicePickup pickupPrefab;
    [SerializeField] private float initialSpawnDelay = 4f;
    [SerializeField] private float spawnInterval = 10f;
    [SerializeField] private float buffSpawnTopY = 5.5f;
    [SerializeField] private float laneLeftX = -2f;
    [SerializeField] private float laneRightX = 2f;

    private float _cooldown;

    private void Awake()
    {
        _cooldown = initialSpawnDelay;
    }

    private void Update()
    {
        if (pickupPrefab == null)
            return;

        _cooldown -= Time.deltaTime;
        if (_cooldown > 0f)
            return;

        _cooldown = spawnInterval;
        SpawnPair();
    }

    private void SpawnPair()
    {
        var existing = FindObjectsByType<BuffChoicePickup>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        for (var i = 0; i < existing.Length; i++)
        {
            if (existing[i] != null)
                Destroy(existing[i].gameObject);
        }

        var leftGo = Instantiate(
            pickupPrefab,
            new Vector3(laneLeftX, buffSpawnTopY, 0f),
            Quaternion.identity);
        var rightGo = Instantiate(
            pickupPrefab,
            new Vector3(laneRightX, buffSpawnTopY, 0f),
            Quaternion.identity);

        var left = leftGo.GetComponent<BuffChoicePickup>();
        var right = rightGo.GetComponent<BuffChoicePickup>();
        if (left == null || right == null)
            return;

        var attackOnLeft = Random.value >= 0.5f;
        left.Initialize(
            attackOnLeft ? BuffChoicePickup.BuffKind.AttackSpeedUp : BuffChoicePickup.BuffKind.MaxHpUp,
            right);
        right.Initialize(
            attackOnLeft ? BuffChoicePickup.BuffKind.MaxHpUp : BuffChoicePickup.BuffKind.AttackSpeedUp,
            left);
    }
}
