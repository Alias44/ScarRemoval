using System.Collections.Generic;
using System.Linq;

using RimWorld;
using Verse;

namespace SyrScarRemoval;

internal class Recipe_BodyPartRegrowth : RecipeWorker
{
	public static HashSet<BodyPartDef> regroables = [ScarRemovalDefOf.Ear, ScarRemovalDefOf.Finger, ScarRemovalDefOf.Nose, ScarRemovalDefOf.Toe, ScarRemovalDefOf.Tail];

	public override IEnumerable<BodyPartRecord> GetPartsToApplyOn(Pawn pawn, RecipeDef recipe)
	{
		return pawn.health.hediffSet.hediffs
			.Where(hediff => hediff.def == HediffDefOf.MissingBodyPart)
			.Select(hediff => hediff.Part)
			.Where(part => part != null)
			.Where(part => regroables.Contains(part.def))
			.Where(part => !pawn.health.hediffSet.AncestorHasDirectlyAddedParts(part) && !ParentIsMissing(pawn, part));
	}

	private bool ParentIsMissing(Pawn pawn, BodyPartRecord part)
	{
		return pawn.health.hediffSet.hediffs.Exists(hediff => hediff?.Part == part.parent);
	}

	public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
	{
		Hediff hediff = pawn.health.hediffSet.hediffs.Find((Hediff x) => x.def == HediffDefOf.MissingBodyPart && x.Part == part);
		if (hediff != null)
		{
			if (PawnUtility.ShouldSendNotificationAbout(pawn) || PawnUtility.ShouldSendNotificationAbout(billDoer))
			{
				string text;
				text = "SyrScarRemovalSuccessfullyRegrown".Translate(
				billDoer.LabelShort,
				pawn.LabelShort,
				part.Label);
				Messages.Message(text, pawn, MessageTypeDefOf.PositiveEvent, true);
			}
			pawn.health.RemoveHediff(hediff);
		}
	}

	public override string GetLabelWhenUsedOn(Pawn pawn, BodyPartRecord part)
	{
		Hediff hediff = pawn.health.hediffSet.hediffs.Find((Hediff x) => x.def == HediffDefOf.MissingBodyPart && x.Part == part);
		return "SyrScarRemovalRegrown".Translate() + " " + part.Label;
	}
}
