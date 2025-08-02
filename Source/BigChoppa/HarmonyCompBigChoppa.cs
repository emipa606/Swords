using System.Reflection;
using HarmonyLib;
using Verse;

namespace BigChoppa;

[StaticConstructorOnStartup]
internal static class HarmonyCompBigChoppa
{
    static HarmonyCompBigChoppa()
    {
        new Harmony("rimworld.jecrellpelador.comps.oversizedbigchoppa").PatchAll(Assembly.GetExecutingAssembly());
    }
}