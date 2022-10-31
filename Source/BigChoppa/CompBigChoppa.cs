using Verse;

namespace BigChoppa;

public class CompBigChoppa : ThingComp
{
    private bool isEquipped;

    public CompBigChoppa()
    {
        if (props is not CompProperties_BigChoppa)
        {
            props = new CompProperties_BigChoppa();
        }
    }

    public CompProperties_BigChoppa Props => props as CompProperties_BigChoppa;

    private CompEquippable GetEquippable
    {
        get
        {
            var equippableParent = parent;
            return equippableParent?.GetComp<CompEquippable>();
        }
    }

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

    public bool FirstAttack { get; set; }
}