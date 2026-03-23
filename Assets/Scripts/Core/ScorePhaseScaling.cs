using UnityEngine;

/// <summary>
/// スコアフェーズに応じた落下速度倍率・敵必要弾数（下限寄り＋山場補正）。
/// </summary>
public static class ScorePhaseScaling
{
    /// <summary>[min,max] で min(u1,u2) により下限寄りの一様より低い分布。</summary>
    public static int WeightedLowBiasInclusive(int min, int max)
    {
        if (min >= max)
            return min;
        var t = Mathf.Min(Random.value, Random.value);
        var span = max - min + 1;
        return min + Mathf.FloorToInt(t * span);
    }

    public static float GetFallSpeedMultiplier(int score)
    {
        if (score < 300)
            return 1f;
        if (score < 800)
            return 1.05f;
        if (score < 1500)
            return 1.11f;
        if (score < 2500)
            return 1.18f;
        if (score < 4000)
            return 1.28f;
        return 1.40f;
    }

    public static void GetEnemyHitsRange(int score, out int min, out int max)
    {
        if (score < 300)
        {
            min = 3;
            max = 5;
        }
        else if (score < 800)
        {
            min = 4;
            max = 7;
        }
        else if (score < 1500)
        {
            min = 6;
            max = 9;
        }
        else if (score < 2500)
        {
            min = 8;
            max = 12;
        }
        else if (score < 4000)
        {
            min = 11;
            max = 16;
        }
        else
        {
            min = 14;
            max = 20;
        }
    }

    /// <summary>通常枠（下限寄り）＋山場補正。上限連発を少し抑える。</summary>
    public static int RollEnemyRequiredHits(int score)
    {
        GetEnemyHitsRange(score, out var min, out var max);
        var baseHits = WeightedLowBiasInclusive(min, max);
        var bonus = RollMountainBonus();
        if (bonus >= 3 && Random.value < 0.35f)
            bonus = Random.Range(1, 3);

        var hp = baseHits + bonus;
        var hardCap = max + 5;
        return Mathf.Clamp(hp, min, hardCap);
    }

    static int RollMountainBonus()
    {
        var r = Random.value;
        if (r < 0.70f)
            return 0;
        if (r < 0.90f)
            return Random.Range(1, 3);
        return Random.Range(3, 6);
    }
}
