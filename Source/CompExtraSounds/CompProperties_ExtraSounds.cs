using System;
using Verse;

namespace CompExtraSounds
{
	// Token: 0x02000003 RID: 3
	internal class CompProperties_ExtraSounds : CompProperties
	{
		// Token: 0x06000003 RID: 3 RVA: 0x00002066 File Offset: 0x00000266
		public CompProperties_ExtraSounds()
		{
			this.compClass = typeof(CompExtraSounds);
		}

		// Token: 0x04000001 RID: 1
		public SoundDef soundExtra;

		// Token: 0x04000002 RID: 2
		public SoundDef soundExtraTwo;

		// Token: 0x04000003 RID: 3
		public SoundDef soundHitBuilding;

		// Token: 0x04000004 RID: 4
		public SoundDef soundHitPawn;

		// Token: 0x04000005 RID: 5
		public SoundDef soundMiss;
	}
}
