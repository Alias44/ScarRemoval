﻿using System.Collections.Generic;
using System.Linq;

using Verse;

namespace SyrScarRemoval
{
	internal class Recipe_ScarRemovalBrain : Recipe_ScarRemoval
	{
		public override IEnumerable<BodyPartRecord> GetPartsToApplyOn(Pawn pawn, RecipeDef recipe)
		{
			return GetScars(pawn)
				.Where(part => part.def == ScarRemovalDefOf.Brain)
				.ToList();
		}
	}
}
