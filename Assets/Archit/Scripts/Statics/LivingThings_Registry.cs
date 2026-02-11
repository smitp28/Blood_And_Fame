using UnityEngine;
using System.Collections.Generic;

public static class LivingRegistry
{
    private static Dictionary<string, Rabbit> livingThings = new Dictionary<string, Rabbit>();

    public static void Register(string id, Rabbit rabbit)
    {
        livingThings[id] = rabbit;
    }

    public static void Unregister(string id)
    {
        if (livingThings.ContainsKey(id))
            livingThings.Remove(id);
    }

    public static Rabbit GetLiving(string id)
    {
        if (livingThings.TryGetValue(id, out Rabbit rabbit))
            return rabbit;

        Debug.LogWarning($"Living thing with ID '{id}' not found!");
        return null;
    }
}
