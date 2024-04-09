using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace BigChoppa;

[StaticConstructorOnStartup]
internal static class HarmonyCompBigChoppa
{
    static HarmonyCompBigChoppa()
    {
        var harmonyInstance = new Harmony("rimworld.jecrellpelador.comps.oversizedbigchoppa");
        harmonyInstance.Patch(typeof(PawnRenderUtility).GetMethod("DrawEquipmentAiming"),
            new HarmonyMethod(typeof(HarmonyCompBigChoppa).GetMethod("DrawEquipmentAimingPreFix")));
        harmonyInstance.Patch(AccessTools.Method(typeof(Thing), "get_DefaultGraphic"), null,
            new HarmonyMethod(typeof(HarmonyCompBigChoppa), "get_Graphic_PostFix"));
    }

    public static bool DrawEquipmentAimingPreFix(Thing eq, Vector3 drawLoc, float aimAngle)
    {
        if (eq is not ThingWithComps thingWithComps)
        {
            return true;
        }

        var thingComp = thingWithComps.AllComps.FirstOrDefault(y =>
            y.GetType().ToString() == "CompDeflector.CompDeflector" ||
            y.GetType().BaseType?.ToString() == "CompDeflector.CompDeflector");
        if (thingComp != null && Traverse.Create(thingComp).Property("IsAnimatingNow").GetValue<bool>())
        {
            return false;
        }

        var compBigChoppa = thingWithComps.TryGetComp<CompBigChoppa>();
        if (compBigChoppa == null)
        {
            return true;
        }

        var flip = false;
        var num = aimAngle - 90f;
        var compEquippable = eq.TryGetComp<CompEquippable>();

        if (compEquippable?.parent is not Pawn value)
        {
            return true;
        }

        switch (aimAngle)
        {
            case > 20f and < 160f:
            {
                _ = MeshPool.plane10;
                num += thingWithComps.def.equippedAngleOffset;
                break;
            }
            case > 200f and < 340f:
            {
                _ = MeshPool.plane10Flip;
                flip = true;
                num -= 180f;
                num -= thingWithComps.def.equippedAngleOffset;
                break;
            }
            default:
                num = AdjustOffsetAtPeace(thingWithComps, value, compBigChoppa, num);
                break;
        }

        if (compBigChoppa.Props != null && !IsChopping(value) && compBigChoppa.Props.verticalFlipNorth &&
            value.Rotation == Rot4.North)
        {
            num += 180f;
        }

        if (!IsChopping(value))
        {
            num = AdjustNonCombatRotation(value, num, compBigChoppa);
        }

        num %= 360f;
        Material matSingle;
        if (thingWithComps.Graphic is Graphic_StackCount graphic_StackCount)
        {
            matSingle = graphic_StackCount.SubGraphicForStackCount(1, thingWithComps.def).MatSingle;
        }
        else
        {
            matSingle = thingWithComps.Graphic.MatSingle;
        }

        var vector = new Vector3(thingWithComps.def.graphicData.drawSize.x, 1f,
            thingWithComps.def.graphicData.drawSize.y);
        var matrix4x = default(Matrix4x4);
        var chopping = false;
        if (IsChopping(value) && !thingWithComps.def.IsRangedWeapon)
        {
            chopping = true;
            num += 180f;
            num %= 360f;
        }

        var vector2 = AdjustRenderOffsetFromDir(value, compBigChoppa, chopping, aimAngle, out var num2);
        num += num2;
        matrix4x.SetTRS(drawLoc + vector2, Quaternion.AngleAxis(num, Vector3.up), vector);
        Graphics.DrawMesh(!flip ? MeshPool.plane10 : MeshPool.plane10Flip, matrix4x, matSingle, 0);
        if (compBigChoppa.Props is not { isDualWeapon: true })
        {
            return false;
        }

        vector2 = new Vector3(-1f * vector2.x, vector2.y, vector2.z);
        Mesh mesh;
        if (value.Rotation == Rot4.North || value.Rotation == Rot4.South)
        {
            num += 135f;
            num %= 360f;
            mesh = !flip ? MeshPool.plane10Flip : MeshPool.plane10;
        }
        else
        {
            vector2 = new Vector3(vector2.x, vector2.y - 0.1f, vector2.z + 0.15f);
            mesh = !flip ? MeshPool.plane10 : MeshPool.plane10Flip;
        }

        matrix4x.SetTRS(drawLoc + vector2, Quaternion.AngleAxis(num, Vector3.up), vector);
        Graphics.DrawMesh(mesh, matrix4x, matSingle, 0);
        return false;
    }

    private static bool IsLolly(Pawn pawn)
    {
        object obj;
        if (pawn == null)
        {
            obj = null;
        }
        else
        {
            var stances = pawn.stances;
            obj = stances?.curStance;
        }

        return obj is not Stance_Busy { focusTarg.IsValid: true };
    }

    public static LocalTargetInfo GetStanceTarget(Pawn pawn)
    {
        LocalTargetInfo result = null;
        object obj;
        if (pawn == null)
        {
            obj = null;
        }
        else
        {
            var stances = pawn.stances;
            obj = stances?.curStance;
        }

        if (obj is Stance_Busy { focusTarg.IsValid: true } stance_Busy)
        {
            result = stance_Busy.focusTarg;
        }

        return result;
    }

    private static bool IsChopping(Pawn pawn)
    {
        if (pawn.CurJob == null || pawn.CurJob.def == JobDefOf.Wait_Combat ||
            pawn.CurJob.def == JobDefOf.Wait_MaintainPosture || pawn.CurJob.def == JobDefOf.Wait ||
            pawn.CurJob.def == JobDefOf.Goto || pawn.CurJob.def == JobDefOf.ExtinguishSelf ||
            pawn.CurJob.def == JobDefOf.BeatFire || pawn.CurJob.def == JobDefOf.PredatorHunt ||
            pawn.CurJob.def == JobDefOf.SocialFight)
        {
            return false;
        }

        if (pawn.CurJob.def == JobDefOf.AttackMelee)
        {
            var curJob = pawn.CurJob;
            bool hasJob;
            if (curJob == null)
            {
                hasJob = false;
            }
            else
            {
                _ = curJob.targetA;
                hasJob = true;
            }

            return hasJob && pawn.Position.AdjacentTo8Way(pawn.CurJob.targetA.HasThing
                ? pawn.CurJob.targetA.Thing.Position
                : pawn.CurJob.targetA.Cell);
        }

        if (pawn.CurJob.def != JobDefOf.AttackStatic)
        {
            return true;
        }

        var curJob2 = pawn.CurJob;
        bool alsoHasJob;
        if (curJob2 == null)
        {
            alsoHasJob = false;
        }
        else
        {
            _ = curJob2.targetA;
            alsoHasJob = true;
        }

        return alsoHasJob && pawn.Position.AdjacentTo8Way(pawn.CurJob.targetA.HasThing
            ? pawn.CurJob.targetA.Thing.Position
            : pawn.CurJob.targetA.Cell);
    }

    private static float AdjustOffsetAtPeace(Thing eq, Pawn pawn, CompBigChoppa compBigChoppa, float num)
    {
        _ = MeshPool.plane10;
        var num2 = eq.def.equippedAngleOffset;
        if (compBigChoppa.Props != null && !IsChopping(pawn) && compBigChoppa.Props.verticalFlipOutsideCombat)
        {
            num2 += 180f;
        }

        num += num2;
        return num;
    }

    private static float AdjustNonCombatRotation(Pawn pawn, float num, CompBigChoppa compBigChoppa)
    {
        if (compBigChoppa.Props == null)
        {
            return num;
        }

        if (pawn.Rotation == Rot4.North)
        {
            num += compBigChoppa.Props.angleAdjustmentNorth;
        }
        else if (pawn.Rotation == Rot4.East)
        {
            num += compBigChoppa.Props.angleAdjustmentEast;
        }
        else if (pawn.Rotation == Rot4.West)
        {
            num += compBigChoppa.Props.angleAdjustmentWest;
        }
        else if (pawn.Rotation == Rot4.South)
        {
            num += compBigChoppa.Props.angleAdjustmentSouth;
        }

        return num;
    }

    private static Vector3 AdjustRenderOffsetFromDir(Pawn pawn, CompBigChoppa compBigChoppa, bool chopping,
        float aimAngle, out float adjAngle)
    {
        var rotation = pawn.Rotation;
        var result = Vector3.zero;
        adjAngle = 0f;
        if (compBigChoppa.Props == null)
        {
            return result;
        }

        if (!chopping)
        {
            if (IsLolly(pawn))
            {
                if (rotation == Rot4.North)
                {
                    result = compBigChoppa.Props.northOffset;
                }
                else if (rotation == Rot4.East)
                {
                    result = compBigChoppa.Props.eastOffset;
                }
                else if (rotation == Rot4.South)
                {
                    result = compBigChoppa.Props.southOffset;
                }
                else if (rotation == Rot4.West)
                {
                    result = compBigChoppa.Props.westOffset;
                }
            }
            else
            {
                if (true)
                {
                    if (rotation == Rot4.North)
                    {
                        result = compBigChoppa.Props.newtargnorthOffset;
                    }
                    else if (rotation == Rot4.East)
                    {
                        result = compBigChoppa.Props.newtargeastOffset;
                    }
                    else if (rotation == Rot4.South)
                    {
                        result = compBigChoppa.Props.newtargsouthOffset;
                    }
                    else if (rotation == Rot4.West)
                    {
                        result = compBigChoppa.Props.newtargwestOffset;
                    }
                }

                var num = 45f;
                if (rotation == Rot4.West)
                {
                    switch (aimAngle)
                    {
                        case < 260f:
                            adjAngle = num;
                            break;
                        case > 280f:
                            adjAngle = -num;
                            break;
                    }
                }
                else if (rotation == Rot4.East)
                {
                    switch (aimAngle)
                    {
                        case > 100f:
                            adjAngle = -num;
                            break;
                        case < 80f:
                            adjAngle = num;
                            break;
                    }
                }
            }
        }
        else
        {
            var num2 = 22.5f;
            if (rotation == Rot4.North)
            {
                result = compBigChoppa.Props.chopnorthOffset;
                if (aimAngle > Rot4.North.AsAngle + num2 && aimAngle < Rot4.East.AsAngle - num2)
                {
                    result = GetAvgVectorOffset(compBigChoppa.Props.chopnorthOffset,
                        compBigChoppa.Props.chopeastOffset);
                }
                else if (aimAngle > Rot4.North.AsAngle + num2 && aimAngle > Rot4.West.AsAngle + num2)
                {
                    result = GetAvgVectorOffset(compBigChoppa.Props.chopnorthOffset,
                        compBigChoppa.Props.chopwestOffset);
                }
            }
            else if (rotation == Rot4.East)
            {
                result = compBigChoppa.Props.chopeastOffset;
                if (aimAngle > Rot4.East.AsAngle + num2)
                {
                    result = GetAvgVectorOffset(compBigChoppa.Props.chopeastOffset,
                        compBigChoppa.Props.chopsouthOffset);
                }
                else if (aimAngle < Rot4.East.AsAngle - num2)
                {
                    result = GetAvgVectorOffset(compBigChoppa.Props.chopeastOffset,
                        compBigChoppa.Props.chopnorthOffset);
                }
            }
            else if (rotation == Rot4.South)
            {
                result = compBigChoppa.Props.chopsouthOffset;
                if (aimAngle < Rot4.South.AsAngle - num2 && aimAngle > Rot4.East.AsAngle + num2)
                {
                    result = GetAvgVectorOffset(compBigChoppa.Props.chopsouthOffset,
                        compBigChoppa.Props.chopeastOffset);
                }
                else if (aimAngle > Rot4.South.AsAngle + num2 && aimAngle < Rot4.West.AsAngle - num2)
                {
                    result = GetAvgVectorOffset(compBigChoppa.Props.chopsouthOffset,
                        compBigChoppa.Props.chopwestOffset);
                }
            }
            else if (rotation == Rot4.West)
            {
                result = compBigChoppa.Props.chopwestOffset;
                if (aimAngle < Rot4.West.AsAngle - num2 && aimAngle > Rot4.South.AsAngle + num2)
                {
                    result = GetAvgVectorOffset(compBigChoppa.Props.chopwestOffset,
                        compBigChoppa.Props.chopsouthOffset);
                }
                else if (aimAngle > Rot4.West.AsAngle + num2 && aimAngle > Rot4.North.AsAngle + num2)
                {
                    result = GetAvgVectorOffset(compBigChoppa.Props.chopwestOffset,
                        compBigChoppa.Props.chopnorthOffset);
                }
            }
        }

        return result;
    }

    private static Vector3 GetAvgVectorOffset(Vector3 vec1, Vector3 vec2)
    {
        var num = (vec1.x + vec2.x) / 2f;
        var num2 = (vec1.y + vec2.y) / 2f;
        var num3 = (vec1.z + vec2.z) / 2f;
        return new Vector3(num, num2, num3);
    }

    public static void get_Graphic_PostFix(Thing __instance, ref Graphic __result)
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
        if (props?.groundGraphic == null)
        {
            value.drawSize = __instance.def.graphicData.drawSize;
            __result = value;
            return;
        }

        if (compBigChoppa.IsEquipped)
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