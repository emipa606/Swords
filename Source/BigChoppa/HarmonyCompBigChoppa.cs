using System.Linq;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace BigChoppa
{
    // Token: 0x02000004 RID: 4
    [StaticConstructorOnStartup]
    internal static class HarmonyCompBigChoppa
    {
        // Token: 0x06000009 RID: 9 RVA: 0x000022AC File Offset: 0x000004AC
        static HarmonyCompBigChoppa()
        {
            var harmonyInstance = new Harmony("rimworld.jecrellpelador.comps.oversizedbigchoppa");
            harmonyInstance.Patch(typeof(PawnRenderer).GetMethod("DrawEquipmentAiming"),
                new HarmonyMethod(typeof(HarmonyCompBigChoppa).GetMethod("DrawEquipmentAimingPreFix")));
            harmonyInstance.Patch(AccessTools.Method(typeof(Thing), "get_DefaultGraphic"), null,
                new HarmonyMethod(typeof(HarmonyCompBigChoppa), "get_Graphic_PostFix"));
        }

        // Token: 0x0600000A RID: 10 RVA: 0x0000232C File Offset: 0x0000052C
        public static bool DrawEquipmentAimingPreFix(PawnRenderer __instance, Thing eq, Vector3 drawLoc, float aimAngle)
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

            var flag = false;
            var num = aimAngle - 90f;
            var value = Traverse.Create(__instance).Field("pawn").GetValue<Pawn>();
            if (value == null)
            {
                return true;
            }

            if (aimAngle > 20f && aimAngle < 160f)
            {
                var unused = MeshPool.plane10;
                num += eq.def.equippedAngleOffset;
            }
            else if (aimAngle > 200f && aimAngle < 340f)
            {
                var unused = MeshPool.plane10Flip;
                flag = true;
                num -= 180f;
                num -= eq.def.equippedAngleOffset;
            }
            else
            {
                num = AdjustOffsetAtPeace(eq, value, compBigChoppa, num);
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
            if (eq.Graphic is Graphic_StackCount graphic_StackCount)
            {
                matSingle = graphic_StackCount.SubGraphicForStackCount(1, eq.def).MatSingle;
            }
            else
            {
                matSingle = eq.Graphic.MatSingle;
            }

            var vector = new Vector3(eq.def.graphicData.drawSize.x, 1f, eq.def.graphicData.drawSize.y);
            var matrix4x = default(Matrix4x4);
            var chopping = false;
            if (IsChopping(value) && !((ThingWithComps) eq).def.IsRangedWeapon)
            {
                chopping = true;
                num += 180f;
                num %= 360f;
            }

            var vector2 = AdjustRenderOffsetFromDir(value, compBigChoppa, chopping, aimAngle, out var num2);
            num += num2;
            matrix4x.SetTRS(drawLoc + vector2, Quaternion.AngleAxis(num, Vector3.up), vector);
            Graphics.DrawMesh(!flag ? MeshPool.plane10 : MeshPool.plane10Flip, matrix4x, matSingle, 0);
            if (compBigChoppa.Props == null || !compBigChoppa.Props.isDualWeapon)
            {
                return false;
            }

            vector2 = new Vector3(-1f * vector2.x, vector2.y, vector2.z);
            Mesh mesh;
            if (value.Rotation == Rot4.North || value.Rotation == Rot4.South)
            {
                num += 135f;
                num %= 360f;
                mesh = !flag ? MeshPool.plane10Flip : MeshPool.plane10;
            }
            else
            {
                vector2 = new Vector3(vector2.x, vector2.y - 0.1f, vector2.z + 0.15f);
                mesh = !flag ? MeshPool.plane10 : MeshPool.plane10Flip;
            }

            matrix4x.SetTRS(drawLoc + vector2, Quaternion.AngleAxis(num, Vector3.up), vector);
            Graphics.DrawMesh(mesh, matrix4x, matSingle, 0);
            return false;
        }

        // Token: 0x0600000B RID: 11 RVA: 0x0000268C File Offset: 0x0000088C
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

            return !(obj is Stance_Busy stance_Busy) || !stance_Busy.focusTarg.IsValid;
        }

        // Token: 0x0600000C RID: 12 RVA: 0x000026CC File Offset: 0x000008CC
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

            if (obj is Stance_Busy stance_Busy && stance_Busy.focusTarg.IsValid)
            {
                result = stance_Busy.focusTarg;
            }

            return result;
        }

        // Token: 0x0600000D RID: 13 RVA: 0x00002718 File Offset: 0x00000918
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
                bool flag;
                if (curJob == null)
                {
                    flag = false;
                }
                else
                {
                    var unused = curJob.targetA;
                    flag = true;
                }

                if (!flag)
                {
                    return false;
                }

                if (pawn.CurJob.targetA.HasThing)
                {
                    return pawn.Position.AdjacentTo8Way(pawn.CurJob.targetA.Thing.Position);
                }

                return pawn.Position.AdjacentTo8Way(pawn.CurJob.targetA.Cell);
            }

            if (pawn.CurJob.def != JobDefOf.AttackStatic)
            {
                return true;
            }

            var curJob2 = pawn.CurJob;
            bool flag2;
            if (curJob2 == null)
            {
                flag2 = false;
            }
            else
            {
                var unused = curJob2.targetA;
                flag2 = true;
            }

            if (!flag2)
            {
                return false;
            }

            if (pawn.CurJob.targetA.HasThing)
            {
                return pawn.Position.AdjacentTo8Way(pawn.CurJob.targetA.Thing.Position);
            }

            return pawn.Position.AdjacentTo8Way(pawn.CurJob.targetA.Cell);
        }

        // Token: 0x0600000E RID: 14 RVA: 0x000028E4 File Offset: 0x00000AE4
        private static float AdjustOffsetAtPeace(Thing eq, Pawn pawn, CompBigChoppa compBigChoppa, float num)
        {
            var unused = MeshPool.plane10;
            var num2 = eq.def.equippedAngleOffset;
            if (compBigChoppa.Props != null && !IsChopping(pawn) && compBigChoppa.Props.verticalFlipOutsideCombat)
            {
                num2 += 180f;
            }

            num += num2;
            return num;
        }

        // Token: 0x0600000F RID: 15 RVA: 0x00002934 File Offset: 0x00000B34
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

        // Token: 0x06000010 RID: 16 RVA: 0x000029DC File Offset: 0x00000BDC
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
                        if (aimAngle < 260f)
                        {
                            adjAngle = num;
                        }
                        else if (aimAngle > 280f)
                        {
                            adjAngle = -num;
                        }
                    }
                    else if (rotation == Rot4.East)
                    {
                        if (aimAngle > 100f)
                        {
                            adjAngle = -num;
                        }
                        else if (aimAngle < 80f)
                        {
                            adjAngle = num;
                        }
                    }
                }
            }
            else
            {
                var num2 = 22.5f;
                var flag = rotation == Rot4.North;
                var flag2 = rotation == Rot4.East;
                var flag3 = rotation == Rot4.South;
                var flag4 = rotation == Rot4.West;
                if (flag)
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
                else if (flag2)
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
                else if (flag3)
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
                else if (flag4)
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

        // Token: 0x06000011 RID: 17 RVA: 0x00002E10 File Offset: 0x00001010
        private static Vector3 GetAvgVectorOffset(Vector3 vec1, Vector3 vec2)
        {
            var num = (vec1.x + vec2.x) / 2f;
            var num2 = (vec1.y + vec2.y) / 2f;
            var num3 = (vec1.z + vec2.z) / 2f;
            return new Vector3(num, num2, num3);
        }

        // Token: 0x06000012 RID: 18 RVA: 0x00002E60 File Offset: 0x00001060
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
}