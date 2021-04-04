﻿using Verse;

namespace BigChoppa
{
    // Token: 0x02000002 RID: 2
    public class CompBigChoppa : ThingComp
    {
        // Token: 0x04000001 RID: 1
        private bool isEquipped;

        // Token: 0x06000002 RID: 2 RVA: 0x0000205D File Offset: 0x0000025D
        public CompBigChoppa()
        {
            if (!(props is CompProperties_BigChoppa))
            {
                props = new CompProperties_BigChoppa();
            }
        }

        // Token: 0x17000001 RID: 1
        // (get) Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
        public CompProperties_BigChoppa Props => props as CompProperties_BigChoppa;

        // Token: 0x17000002 RID: 2
        // (get) Token: 0x06000003 RID: 3 RVA: 0x00002084 File Offset: 0x00000284
        private CompEquippable GetEquippable
        {
            get
            {
                var equippableParent = parent;
                return equippableParent?.GetComp<CompEquippable>();
            }
        }

        // Token: 0x17000003 RID: 3
        // (get) Token: 0x06000004 RID: 4 RVA: 0x000020A4 File Offset: 0x000002A4
        private Pawn GetPawn
        {
            get
            {
                var getEquippable = GetEquippable;
                Pawn result;
                if (getEquippable == null)
                {
                    result = null;
                }
                else
                {
                    var verbTracker = getEquippable.verbTracker;
                    if (verbTracker == null)
                    {
                        result = null;
                    }
                    else
                    {
                        var primaryVerb = verbTracker.PrimaryVerb;
                        result = primaryVerb?.CasterPawn;
                    }
                }

                return result;
            }
        }

        // Token: 0x17000004 RID: 4
        // (get) Token: 0x06000005 RID: 5 RVA: 0x000020E4 File Offset: 0x000002E4
        public bool IsEquipped
        {
            get
            {
                bool result;
                if (Find.TickManager.TicksGame % 60 != 0)
                {
                    result = isEquipped;
                }
                else
                {
                    isEquipped = GetPawn != null;
                    result = isEquipped;
                }

                return result;
            }
        }

        // Token: 0x17000005 RID: 5
        // (get) Token: 0x06000006 RID: 6 RVA: 0x00002123 File Offset: 0x00000323
        // (set) Token: 0x06000007 RID: 7 RVA: 0x0000212B File Offset: 0x0000032B
        public bool FirstAttack { get; set; }

        // Token: 0x04000002 RID: 2
    }
}