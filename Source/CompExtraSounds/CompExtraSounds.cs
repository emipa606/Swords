using System;
using Verse;

namespace CompExtraSounds
{
	// Token: 0x02000002 RID: 2
	internal class CompExtraSounds : ThingComp
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public CompProperties_ExtraSounds Props
		{
			get
			{
				return (CompProperties_ExtraSounds)this.props;
			}
		}
	}
}
