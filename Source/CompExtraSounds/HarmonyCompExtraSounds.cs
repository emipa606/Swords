using System;
using HarmonyLib;
using RimWorld;
using Verse;

namespace CompExtraSounds
{
	[StaticConstructorOnStartup]
	internal static class HarmonyCompExtraSounds
	{
		static HarmonyCompExtraSounds()
		{
			var harmonyInstance = new Harmony("rimworld.jecrell.comps.sounds");
			harmonyInstance.Patch(AccessTools.Method(typeof(Verb_MeleeAttack), "SoundMiss", null, null), null, new HarmonyMethod(typeof(HarmonyCompExtraSounds), "SoundMissPrefix", null), null);
			harmonyInstance.Patch(AccessTools.Method(typeof(Verb_MeleeAttack), "SoundHitPawn", null, null), null, new HarmonyMethod(typeof(HarmonyCompExtraSounds), "SoundHitPawnPrefix", null), null);
			harmonyInstance.Patch(AccessTools.Method(typeof(Verb_MeleeAttack), "SoundHitBuilding", null, null), null, new HarmonyMethod(typeof(HarmonyCompExtraSounds), "SoundHitBuildingPrefix", null), null);
		}

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
