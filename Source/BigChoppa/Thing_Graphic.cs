using HarmonyLib;
using Verse;

namespace BigChoppa;

[HarmonyPatch(typeof(Thing), nameof(Thing.Graphic), MethodType.Getter)]
public static class Thing_Graphic
{
    public static void Postfix(Thing __instance, ref Graphic __result)
    {
        var value = Traverse.Create(__instance).Field("graphicInt").GetValue<Graphic>();
        if (value == null)
        {
            return;
        }

        if (__instance is not ThingWithComps thingWithComps || thingWithComps.ParentHolder is Pawn)
        {
            return;
        }

        var thingComp =
            thingWithComps.AllComps.FirstOrDefault(y => y.GetType().ToString().Contains("ActivatableEffect"));
        if (thingComp != null && Traverse.Create(thingComp).Property("GetPawn").GetValue<Pawn>() != null)
        {
            return;
        }

        var compBigChoppa = thingWithComps.TryGetComp<CompBigChoppa>();
        if (compBigChoppa == null)
        {
            return;
        }

        var props = compBigChoppa.Props;
        if (props?.groundGraphic == null || compBigChoppa.IsEquipped)
        {
            value.drawSize = __instance.def.graphicData.drawSize;
            __result = value;
            return;
        }

        var props2 = compBigChoppa.Props;
        Graphic graphic;
        if (props2 == null)
        {
            graphic = null;
        }
        else
        {
            var groundGraphic = props2.groundGraphic;
            graphic = groundGraphic?.GraphicColoredFor(__instance);
        }

        var graphic2 = graphic;
        if (graphic2 != null)
        {
            graphic2.drawSize = compBigChoppa.Props.groundGraphic.drawSize;
            __result = graphic2;
            return;
        }

        value.drawSize = __instance.def.graphicData.drawSize;
        __result = value;
    }
}