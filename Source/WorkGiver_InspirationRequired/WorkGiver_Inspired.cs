using System;
using RimWorld;
using Verse;

namespace InspirationBuild
{
	// Token: 0x02000002 RID: 2
	public class WorkGiver_Inspired : WorkGiver_DoBill
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			bool inspired = pawn.Inspired;
			if (inspired)
			{
				InspirationDef inspirationDef;
				if (pawn == null)
				{
					inspirationDef = null;
				}
				else
				{
					Inspiration inspiration = pawn.Inspiration;
					inspirationDef = ((inspiration != null) ? inspiration.def : null);
				}
				bool flag = inspirationDef == InspirationDefOf.Inspired_Creativity;
				if (flag)
				{
					return false;
				}
			}
			return true;
		}
	}
}
