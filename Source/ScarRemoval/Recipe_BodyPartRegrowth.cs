using System.Collections.Generic;
using System.Linq;

using RimWorld;
using Verse;

namespace SyrScarRemoval;

internal class Recipe_BodyPartRegrowth : Recipe_ScarRemoval
{
	public static HashSet<BodyPartDef> regroables = [ScarRemovalDefOf.Ear, ScarRemovalDefOf.Finger, ScarRemovalDefOf.Nose, ScarRemovalDefOf.Toe, ScarRemovalDefOf.Tail];

	protected override IEnumerable<Hediff> GetScars(Pawn pawn)
	{
		return pawn.health.hediffSet.hediffs
			.Where(hediff => hediff.def == HediffDefOf.MissingBodyPart)
			.Where(hediff =>
			{
				var part = hediff.Part;

				return part != null &&
					regroables.Contains(part.def) &&
					!pawn.health.hediffSet.AncestorHasDirectlyAddedParts(part) &&
					!pawn.health.hediffSet.hediffs.Exists(hediff => hediff?.Part == part.parent);
			});
	}

	public override IEnumerable<BodyPartRecord> GetPartsToApplyOn(Pawn pawn, RecipeDef recipe)
	{
		return GetScars(pawn)
			.Select(hediff => hediff.Part)
			.Distinct();
	}

	public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
	{
		Hediff hediff = GetScars(pawn).First();

		if (hediff != null)
		{
			if (PawnUtility.ShouldSendNotificationAbout(pawn) || PawnUtility.ShouldSendNotificationAbout(billDoer))
			{
				string text = "SyrScarRemovalSuccessfullyRegrown".Translate(
					billDoer.LabelShort,
					pawn.LabelShort,
					part.Label
				);
				
				Messages.Message(text, pawn, MessageTypeDefOf.PositiveEvent, true);
			}
			pawn.health.RemoveHediff(hediff);
		}
	}

	public override string GetLabelWhenUsedOn(Pawn pawn, BodyPartRecord part)
	{
		Hediff hediff = GetScars(pawn).First();
		return "SyrScarRemovalRegrown".Translate(part.Label);
	}
}
