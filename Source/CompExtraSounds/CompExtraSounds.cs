using System;
using Verse;

namespace CompExtraSounds
{
	internal class CompExtraSounds : ThingComp
	{
		public CompProperties_ExtraSounds Props
		{
			get
			{
				return (CompProperties_ExtraSounds)this.props;
			}
		}
	}
}
