using Apps.Runtime.Common;
using UnityEngine;

namespace Apps.Runtime.Domains
{
    public static class PointCalculator
    {
        public static (PointRank, uint) CalculatePoint(
            float spawnTime, float dropDuration, float clickTime)
        {
            // rhythm accuracy bonus.
            var tileReachTime = spawnTime + dropDuration / 2f; // time when tile is in the middle
            var accuracyOffset = Mathf.Abs(clickTime - tileReachTime);
            var accuracyFactor = Mathf.Clamp(1f - (accuracyOffset / Constants.MaxAccuracyOffset), 0.5f, 1f);

            // 	awards extra bonuses for quick taps.
            float reactionTime = clickTime - spawnTime;
            float reactionBonus = Mathf.Clamp01(1f - reactionTime / dropDuration) * Constants.ReactionBonusMultiplier;

            var totalFactor = accuracyFactor * (1f + reactionBonus);

            // rank.
            var rank = totalFactor switch
            {
                // note: sample-based, unverified data. 
                >= 0.8f => PointRank.Perfect,
                < 0.6f => PointRank.Good,
                _ => PointRank.Great
            };

            // final point.
            var pointMultiplier = rank switch
            {
                PointRank.Perfect => 3f,
                PointRank.Great => 2f,
                _ => 1f,
            };
            var point = (uint)Mathf.RoundToInt(Constants.BasePoints * pointMultiplier);

            return (rank, Constants.BasePoints * point);
        }
    }
}