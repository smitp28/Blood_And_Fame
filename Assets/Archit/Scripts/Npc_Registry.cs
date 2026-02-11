using UnityEngine;
using System.Collections.Generic;

public static class NPC_Registry
{
    private static Dictionary<string, Transform> npcs = new Dictionary<string, Transform>();

    public static void Register(string npcID, Transform transform)
    {
        if (!npcs.ContainsKey(npcID))
        {
            npcs.Add(npcID, transform);
        }
        else
        {
            npcs[npcID] = transform;
        }
    }

    public static void Unregister(string npcID)
    {
        if (npcs.ContainsKey(npcID))
        {
            npcs.Remove(npcID);
        }
    }

    public static Transform GetNPC(string npcID)
    {
        if (npcs.TryGetValue(npcID, out Transform t))
        {
            return t;
        }
        Debug.LogWarning($"NPC with ID '{npcID}' not found in Registry!");
        return null;
    }
}