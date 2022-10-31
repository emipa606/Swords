using System;
using UnityEngine;
using Verse;

namespace CompOversizedWeapon
{
	public class CompProperties_OversizedWeapon : CompProperties
	{
		public CompProperties_OversizedWeapon()
		{
			this.compClass = typeof(CompOversizedWeapon);
		}

		public Vector3 offset = new Vector3(0f, 0f, 0f);

		public Vector3 northOffset = new Vector3(0f, 0f, 0f);

		public Vector3 eastOffset = new Vector3(0f, 0f, 0f);

		public Vector3 southOffset = new Vector3(0f, 0f, 0f);

		public Vector3 westOffset = new Vector3(0f, 0f, 0f);

		public bool verticalFlipOutsideCombat = false;

		public bool verticalFlipNorth = false;

		public bool isDualWeapon = false;

		public float angleAdjustmentEast = 0f;

		public float angleAdjustmentWest = 0f;

		public float angleAdjustmentNorth = 0f;

		public float angleAdjustmentSouth = 0f;

		public GraphicData groundGraphic = null;
	}
}
