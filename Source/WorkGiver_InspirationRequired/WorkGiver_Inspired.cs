using RimWorld;
using Verse;

namespace InspirationBuild;

public class WorkGiver_Inspired : WorkGiver_DoBill
{
    public override bool ShouldSkip(Pawn pawn, bool forced = false)
    {
        var inspired = pawn.Inspired;
        if (!inspired)
        {
            return true;
        }

        var inspiration = pawn.Inspiration;
        var inspirationDef = inspiration?.def;
        return inspirationDef != InspirationDefOf.Inspired_Creativity;
    }
}