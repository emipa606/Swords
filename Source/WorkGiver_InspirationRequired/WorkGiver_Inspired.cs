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
            var inspired = pawn.Inspired;
            if (!inspired)
            {
                return true;
            }

            var inspiration = pawn.Inspiration;
            var inspirationDef = inspiration?.def;
            if (inspirationDef == InspirationDefOf.Inspired_Creativity)
            {
                return false;
            }

            return true;
        }
    }
}