using System;
using Verse;

namespace CompExtraSounds
{
	internal class CompProperties_ExtraSounds : CompProperties
	{
		public CompProperties_ExtraSounds()
		{
			this.compClass = typeof(CompExtraSounds);
		}

		public SoundDef soundExtra;

		public SoundDef soundExtraTwo;

		public SoundDef soundHitBuilding;

		public SoundDef soundHitPawn;

		public SoundDef soundMiss;
	}
}
