using System;
using UnityEngine;
using Verse;

namespace BigChoppa
{
	// Token: 0x02000003 RID: 3
	public class CompProperties_BigChoppa : CompProperties
	{
		// Token: 0x06000008 RID: 8 RVA: 0x00002134 File Offset: 0x00000334
		public CompProperties_BigChoppa()
		{
			this.compClass = typeof(CompBigChoppa);
		}

		// Token: 0x04000003 RID: 3
		public Vector3 offset = new Vector3(0f, 0f, 0f);

		// Token: 0x04000004 RID: 4
		public Vector3 northOffset = new Vector3(0f, 0f, 0f);

		// Token: 0x04000005 RID: 5
		public Vector3 eastOffset = new Vector3(0f, 0f, 0f);

		// Token: 0x04000006 RID: 6
		public Vector3 southOffset = new Vector3(0f, 0f, 0f);

		// Token: 0x04000007 RID: 7
		public Vector3 westOffset = new Vector3(0f, 0f, 0f);

		// Token: 0x04000008 RID: 8
		public Vector3 chopnorthOffset = new Vector3(0f, 0f, 0f);

		// Token: 0x04000009 RID: 9
		public Vector3 chopeastOffset = new Vector3(0f, 0f, 0f);

		// Token: 0x0400000A RID: 10
		public Vector3 chopsouthOffset = new Vector3(0f, 0f, 0f);

		// Token: 0x0400000B RID: 11
		public Vector3 chopwestOffset = new Vector3(0f, 0f, 0f);

		// Token: 0x0400000C RID: 12
		public Vector3 newtargnorthOffset = new Vector3(0f, 0f, 0f);

		// Token: 0x0400000D RID: 13
		public Vector3 newtargeastOffset = new Vector3(0f, 0f, 0f);

		// Token: 0x0400000E RID: 14
		public Vector3 newtargsouthOffset = new Vector3(0f, 0f, 0f);

		// Token: 0x0400000F RID: 15
		public Vector3 newtargwestOffset = new Vector3(0f, 0f, 0f);

		// Token: 0x04000010 RID: 16
		public bool verticalFlipOutsideCombat;

		// Token: 0x04000011 RID: 17
		public bool verticalFlipNorth;

		// Token: 0x04000012 RID: 18
		public bool isDualWeapon;

		// Token: 0x04000013 RID: 19
		public float angleAdjustmentEast;

		// Token: 0x04000014 RID: 20
		public float angleAdjustmentWest;

		// Token: 0x04000015 RID: 21
		public float angleAdjustmentNorth;

		// Token: 0x04000016 RID: 22
		public float angleAdjustmentSouth;

		// Token: 0x04000017 RID: 23
		public GraphicData groundGraphic;
	}
}
