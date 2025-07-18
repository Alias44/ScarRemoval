using System.Collections.Generic;
using System.Linq;


using RimWorld;
using Verse;

namespace SyrScarRemoval;

internal class Recipe_ScarRemoval : Recipe_Surgery
{
	protected virtual IEnumerable<Hediff> GetScars(Pawn pawn)
	{
		return pawn.health.hediffSet.hediffs
			.Where(hediff => hediff.IsPermanent());
	}

	protected IEnumerable<Hediff> GetScarsFor(Pawn pawn, BodyPartRecord part)
	{
		return GetScars(pawn)
			.Where(hediff => hediff.Part == part);
	}

	public override IEnumerable<BodyPartRecord> GetPartsToApplyOn(Pawn pawn, RecipeDef recipe)
	{
		return GetScars(pawn)
			.Select(hediff => hediff.Part)
			.Where(part => part.def != ScarRemovalDefOf.Brain)
			.Distinct();
	}

	public override bool AvailableOnNow(Thing thing, BodyPartRecord part = null)
	{
#if RELEASE_1_5
		return base.AvailableOnNow(thing, part) && thing is Pawn pawn && (!pawn.IsNonMutantAnimal || ScarRemovalSettings.applyToAnimals);
#else
		return base.AvailableOnNow(thing, part) && thing is Pawn pawn && (!pawn.IsAnimal || ScarRemovalSettings.applyToAnimals);
#endif
	}

	public override bool CompletableEver(Pawn surgeryTarget)
	{
#if RELEASE_1_5
		return base.CompletableEver(surgeryTarget) && (!surgeryTarget.IsNonMutantAnimal || ScarRemovalSettings.applyToAnimals);
#else
		return base.CompletableEver(surgeryTarget) && (!surgeryTarget.IsAnimal || ScarRemovalSettings.applyToAnimals);
#endif
	}

	public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
	{
		var hediffs = GetScarsFor(pawn, part).ToList();

		if (hediffs != null)
		{
			if (PawnUtility.ShouldSendNotificationAbout(pawn) || PawnUtility.ShouldSendNotificationAbout(billDoer))
			{
				string text = "SyrScarRemovalSuccessfullyHealed".Translate(
					billDoer.LabelShort,
					pawn.LabelShort,
					GetPartLabels(hediffs),
					part.Label
				);

				Messages.Message(text, pawn, MessageTypeDefOf.PositiveEvent, true);
			}

			for (int i = 0; i < hediffs.Count(); i++)
			{
				pawn.health.RemoveHediff(hediffs[i]);

			}
		}
	}

	public virtual string GetPartLabels(IEnumerable<Hediff> hediffs)
	{
		var s = hediffs.Select(hediff => hediff.TryGetComp<HediffComp_GetsPermanent>().Props)
			.Select(prop => prop.permanentLabel ?? prop.instantlyPermanentLabel);

		return string.Join(", ", s);
	}

	public override string GetLabelWhenUsedOn(Pawn pawn, BodyPartRecord part)
	{
		return "SyrScarRemovalRemove".Translate(GetPartLabels(GetScarsFor(pawn, part)));
	}
}
