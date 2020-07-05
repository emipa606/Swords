using System;
using HarmonyLib;
using RimWorld;
using Verse;

namespace CompExtraSounds
{
	// Token: 0x02000004 RID: 4
	[StaticConstructorOnStartup]
	internal static class HarmonyCompExtraSounds
	{
		// Token: 0x06000004 RID: 4 RVA: 0x00002080 File Offset: 0x00000280
		static HarmonyCompExtraSounds()
		{
			var harmonyInstance = new Harmony("rimworld.jecrell.comps.sounds");
			harmonyInstance.Patch(AccessTools.Method(typeof(Verb_MeleeAttack), "SoundMiss", null, null), null, new HarmonyMethod(typeof(HarmonyCompExtraSounds), "SoundMissPrefix", null), null);
			harmonyInstance.Patch(AccessTools.Method(typeof(Verb_MeleeAttack), "SoundHitPawn", null, null), null, new HarmonyMethod(typeof(HarmonyCompExtraSounds), "SoundHitPawnPrefix", null), null);
			harmonyInstance.Patch(AccessTools.Method(typeof(Verb_MeleeAttack), "SoundHitBuilding", null, null), null, new HarmonyMethod(typeof(HarmonyCompExtraSounds), "SoundHitBuildingPrefix", null), null);
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002138 File Offset: 0x00000338
		public static void SoundHitPawnPrefix(ref SoundDef __result, Verb_MeleeAttack __instance)
		{
			Pawn pawn;
			bool flag = (pawn = (__instance.caster as Pawn)) != null;
			if (flag)
			{
				Pawn_EquipmentTracker equipment = pawn.equipment;
				bool flag2 = equipment != null;
				if (flag2)
				{
					ThingWithComps primary = equipment.Primary;
					bool flag3 = primary != null;
					if (flag3)
					{
						CompExtraSounds comp = primary.GetComp<CompExtraSounds>();
						bool flag4 = comp != null;
						if (flag4)
						{
							bool flag5 = comp.Props.soundHitPawn != null;
							if (flag5)
							{
								__result = comp.Props.soundHitPawn;
							}
						}
					}
				}
			}
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000021BC File Offset: 0x000003BC
		public static void SoundMissPrefix(ref SoundDef __result, Verb_MeleeAttack __instance)
		{
			Pawn pawn;
			bool flag = (pawn = (__instance.caster as Pawn)) != null;
			if (flag)
			{
				Pawn_EquipmentTracker equipment = pawn.equipment;
				bool flag2 = equipment != null;
				if (flag2)
				{
					ThingWithComps primary = equipment.Primary;
					bool flag3 = primary != null;
					if (flag3)
					{
						CompExtraSounds comp = primary.GetComp<CompExtraSounds>();
						bool flag4 = comp != null;
						if (flag4)
						{
							bool flag5 = comp.Props.soundMiss != null;
							if (flag5)
							{
								__result = comp.Props.soundMiss;
							}
						}
					}
				}
			}
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002240 File Offset: 0x00000440
		public static void SoundHitBuildingPrefix(ref SoundDef __result, Verb_MeleeAttack __instance)
		{
			Pawn pawn;
			bool flag = (pawn = (__instance.caster as Pawn)) != null;
			if (flag)
			{
				Pawn_EquipmentTracker equipment = pawn.equipment;
				bool flag2 = equipment != null;
				if (flag2)
				{
					ThingWithComps primary = equipment.Primary;
					bool flag3 = primary != null;
					if (flag3)
					{
						CompExtraSounds comp = primary.GetComp<CompExtraSounds>();
						bool flag4 = comp != null;
						if (flag4)
						{
							bool flag5 = comp.Props.soundHitBuilding != null;
							if (flag5)
							{
								__result = comp.Props.soundHitBuilding;
							}
						}
					}
				}
			}
		}
	}
}
