using Assets.Scripts;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public enum RoomType
{
    Enemy,
    Treasure,
    Empty,
    Puzzle,
    RestStop
}
public class RoomNode
{
    public RoomType Room;
    public RoomNode Left;
    public RoomNode Right;
    public int Depth;

    public bool IsLeaf => Left == null && Right == null;
}

public static class RoomTreeGenerator
{
    public const int MaxTotalLevels = 100;
    public const int MaxDepthFor100 = MaxTotalLevels;

    private static readonly Dictionary<RoomType, int> _weights = new Dictionary<RoomType, int>
    {
        { RoomType.Treasure,  2 },
        { RoomType.RestStop,  5 },
        { RoomType.Empty,     9 },
        { RoomType.Puzzle,   13 },
        { RoomType.Enemy,    20 }
    };
    public static RoomNode GenerateTreeForPlayer(float stopChancePerLevel = 0.0f)
    {
        var reached = Mathf.Clamp(GameManager.Instance.ReachedLevel, 1, MaxTotalLevels);
        var desiredDepth = Mathf.Clamp(reached, 1, MaxDepthFor100);
        return GenerateNode(0, desiredDepth, stopChancePerLevel);
    }

    private static RoomNode GenerateNode(int depth, int maxDepth, float stopChancePerLevel)
    {
        RoomNode node = new RoomNode { Depth = depth };
        node.Room = GetWeightedRandomRoomType();
        if (depth >= maxDepth - 1)
            return node;

        if (depth > 0 && UnityEngine.Random.value < stopChancePerLevel)
            return node;

        node.Left = GenerateNode(depth + 1, maxDepth, stopChancePerLevel);
        node.Right = GenerateNode(depth + 1, maxDepth, stopChancePerLevel);
        return node;
    }

    private static RoomType GetWeightedRandomRoomType()
    {
        var totalWeight = 0;
        foreach (var p in _weights)
            totalWeight += p.Value;

        var roll = UnityEngine.Random.Range(0, totalWeight);
        foreach (var p in _weights)
        {
            if (roll < p.Value)
                return p.Key;
            roll -= p.Value;
        }

        return RoomType.Enemy;
    }
}

public static class RoomHintGenerator
{
    private const float NoHintChance = 0.35f;
    private const float SingleFocusChance = 0.6f;
    private const int DefaultLookaheadDepth = 3;
    private const float DamageChancePerChar = 0.12f;

    public struct PathHint
    {
        public List<RoomType> FocusRooms;
        public string Text;
    }

    public static string GetLeftBranchHint(RoomNode fromNode, int lookaheadDepth = DefaultLookaheadDepth)
    {
        return GetBranchHint(fromNode, goLeft: true, lookaheadDepth: lookaheadDepth);
    }

    public static string GetRightBranchHint(RoomNode fromNode, int lookaheadDepth = DefaultLookaheadDepth)
    {
        return GetBranchHint(fromNode, goLeft: false, lookaheadDepth: lookaheadDepth);
    }

    public static string GetBranchHint(RoomNode fromNode, bool goLeft, int lookaheadDepth = DefaultLookaheadDepth)
    {
        if (fromNode == null)
            return null;

        RoomNode child = goLeft ? fromNode.Left : fromNode.Right;
        if (child == null)
            return null;

        var roomsAhead = CollectBranchRooms(child, lookaheadDepth);
        var hint = BuildHintForPath(roomsAhead);
        return hint.Text;
    }

    private static List<RoomType> CollectBranchRooms(RoomNode start, int lookaheadDepth)
    {
        var result = new List<RoomType>();
        RoomNode cur = start;
        int depth = 0;

        while (cur != null && depth < lookaheadDepth)
        {
            result.Add(cur.Room);
            cur = cur.Left ?? cur.Right;
            depth++;
        }

        return result;
    }

    public static PathHint BuildHintForPath(List<RoomType> path)
    {
        if (path == null || path.Count == 0 || UnityEngine.Random.value < NoHintChance)
        {
            return new PathHint
            {
                FocusRooms = new List<RoomType>(),
                Text = null
            };
        }

        var stats = BuildStats(path);
        var focusRooms = PickFocusRooms(stats, UnityEngine.Random.value < SingleFocusChance ? 1 : 2);
        var text = DamageHint(BuildHintText(focusRooms));

        return new PathHint
        {
            FocusRooms = focusRooms,
            Text = text
        };
    }

    private class RoomStats
    {
        public int total;
        public Dictionary<RoomType, int> counts = new();
        public int GetCount(RoomType t) => counts.TryGetValue(t, out var c) ? c : 0;
    }

    private static RoomStats BuildStats(List<RoomType> path)
    {
        var stats = new RoomStats();
        foreach (var rt in path)
        {
            if (!stats.counts.ContainsKey(rt))
                stats.counts[rt] = 0;
            stats.counts[rt]++;
            stats.total++;
        }
        return stats;
    }

    private static List<RoomType> PickFocusRooms(RoomStats stats, int focusCount)
    {
        focusCount = Mathf.Clamp(focusCount, 1, 2);

        var interestWeights = new Dictionary<RoomType, float>
        {
            { RoomType.Treasure,  1.4f },
            { RoomType.RestStop,  1.2f },
            { RoomType.Empty,     0.6f },
            { RoomType.Puzzle,    1.3f },
            { RoomType.Enemy,     1.7f }
        };

        var available = stats.counts.Keys.ToList();
        if (available.Count == 0)
            available.Add(RoomType.Empty);

        var chosen = new List<RoomType>();
        for (int i = 0; i < focusCount; i++)
        {
            if (available.Count == 0) break;

            float totalWeight = 0f;
            var weights = new List<float>();

            foreach (var rt in available)
            {
                float baseCount = stats.GetCount(rt);
                float iw = interestWeights.TryGetValue(rt, out var iwVal) ? iwVal : 1f;
                float w = baseCount * iw;
                if (w <= 0f) w = 0.1f;
                weights.Add(w);
                totalWeight += w;
            }

            float roll = UnityEngine.Random.value * totalWeight;
            int idx = 0;
            for (int j = 0; j < available.Count; j++)
            {
                if (roll < weights[j])
                {
                    idx = j;
                    break;
                }
                roll -= weights[j];
            }

            var selected = available[idx];
            chosen.Add(selected);
            available.RemoveAt(idx);
        }

        if (chosen.Count == 0)
            chosen.Add(RoomType.Empty);

        return chosen;
    }

    private static string BuildHintText(List<RoomType> focusRooms)
    {
        if (focusRooms == null || focusRooms.Count == 0)
            return null;

        if (focusRooms.Count == 1)
        {
            string phrase = SoftDescribe(focusRooms[0]);
            return $"You feel that this path hides {phrase}.";
        }
        return $"You sense {SoftDescribe(focusRooms[0])}, but also {SoftDescribe(focusRooms[1])} somewhere along this path.";
    }

    private static string SoftDescribe(RoomType type)
    {
        switch (type)
        {
            case RoomType.Enemy:
                return "lingering danger and hostile presence";
            case RoomType.Treasure:
                return "something valuable, glittering in the dark";
            case RoomType.Empty:
                return "long stretches of quiet emptiness";
            case RoomType.Puzzle:
                return "a test of wit or a strange mechanism";
            case RoomType.RestStop:
                return "a brief moment of safety and rest";
            default:
                return "something unclear";
        }
    }

    private static string DamageHint(string hint)
    {
        if (string.IsNullOrEmpty(hint))
            return hint;

        char[] chars = hint.ToCharArray();
        for (int i = 0; i < chars.Length; i++)
        {
            if (!char.IsWhiteSpace(chars[i]) &&
                UnityEngine.Random.value < DamageChancePerChar)
            {
                chars[i] = ' ';
            }
        }
        return new string(chars);
    }
}