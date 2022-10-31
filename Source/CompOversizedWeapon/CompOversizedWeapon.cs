using System;
using Verse;

namespace CompOversizedWeapon
{
	public class CompOversizedWeapon : ThingComp
	{
		public CompProperties_OversizedWeapon Props
		{
			get
			{
				return this.props as CompProperties_OversizedWeapon;
			}
		}

		public CompOversizedWeapon()
		{
			bool flag = !(this.props is CompProperties_OversizedWeapon);
			if (flag)
			{
				this.props = new CompProperties_OversizedWeapon();
			}
		}

		public CompEquippable GetEquippable
		{
			get
			{
				ThingWithComps parent = this.parent;
				return (parent != null) ? parent.GetComp<CompEquippable>() : null;
			}
		}

		public Pawn GetPawn
		{
			get
			{
				CompEquippable getEquippable = this.GetEquippable;
				Pawn result;
				if (getEquippable == null)
				{
					result = null;
				}
				else
				{
					VerbTracker verbTracker = getEquippable.verbTracker;
					if (verbTracker == null)
					{
						result = null;
					}
					else
					{
						Verb primaryVerb = verbTracker.PrimaryVerb;
						result = ((primaryVerb != null) ? primaryVerb.CasterPawn : null);
					}
				}
				return result;
			}
		}

		public bool IsEquipped
		{
			get
			{
				bool flag = Find.TickManager.TicksGame % 60 != 0;
				bool result;
				if (flag)
				{
					result = this.isEquipped;
				}
				else
				{
					this.isEquipped = (this.GetPawn != null);
					result = this.isEquipped;
				}
				return result;
			}
		}

		public bool FirstAttack
		{
			get
			{
				return this.firstAttack;
			}
			set
			{
				this.firstAttack = value;
			}
		}

		private bool isEquipped = false;

		private bool firstAttack = false;
	}
}
