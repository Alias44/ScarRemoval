using System.Collections.Generic;
using System.Linq;


using RimWorld;
using Verse;

namespace SyrScarRemoval;

internal class Recipe_ScarRemoval : RecipeWorker
{
	protected IEnumerable<BodyPartRecord> GetScars(Pawn pawn)
	{
		return pawn.health.hediffSet.hediffs
			.Where(hediff => hediff.IsPermanent())
			.Select(hediff => hediff.Part);
	}

	public override IEnumerable<BodyPartRecord> GetPartsToApplyOn(Pawn pawn, RecipeDef recipe)
	{
		return GetScars(pawn)
			.Where(part => part.def != ScarRemovalDefOf.Brain)
			.ToList();
	}

	public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
	{
		Hediff hediff = pawn.health.hediffSet.hediffs.Find((Hediff x) => x.Part == part && x.IsPermanent());

		if (hediff != null)
		{
			if (PawnUtility.ShouldSendNotificationAbout(pawn) || PawnUtility.ShouldSendNotificationAbout(billDoer))
			{
				string text = "SyrScarRemovalSuccessfullyHealed".Translate(
					billDoer.LabelShort,
					pawn.LabelShort,
					part.def.delicate ?
						hediff.TryGetComp<HediffComp_GetsPermanent>().Props.instantlyPermanentLabel :
						hediff.TryGetComp<HediffComp_GetsPermanent>().Props.permanentLabel,
					part.Label
				);

				Messages.Message(text, pawn, MessageTypeDefOf.PositiveEvent, true);
			}

			pawn.health.RemoveHediff(hediff);
		}
	}

	public override string GetLabelWhenUsedOn(Pawn pawn, BodyPartRecord part)
	{
		Hediff hediff = pawn.health.hediffSet.hediffs.Find((Hediff x) => x.TryGetComp<HediffComp_GetsPermanent>() != null && x.Part == part && x.IsPermanent());
		return "RemoveOrgan".Translate() + " " + hediff.Label;
	}
}
