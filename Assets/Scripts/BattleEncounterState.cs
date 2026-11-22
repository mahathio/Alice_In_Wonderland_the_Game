using System.Collections.Generic;
using UnityEngine;

public static class BattleEncounterState
{
    public static string CurrentEncounterId = "";
    private static readonly HashSet<string> defeatedEncounters = new HashSet<string>();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetOnPlay()
    {
        CurrentEncounterId = "";
        defeatedEncounters.Clear();
    }

    public static void MarkDefeated(string encounterId)
    {
        if (!string.IsNullOrEmpty(encounterId))
        {
            defeatedEncounters.Add(encounterId);
        }
    }

    public static bool IsDefeated(string encounterId)
    {
        return !string.IsNullOrEmpty(encounterId) && defeatedEncounters.Contains(encounterId);
    }
}
