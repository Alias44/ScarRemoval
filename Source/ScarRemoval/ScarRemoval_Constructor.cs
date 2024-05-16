using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using RimWorld;
using Verse;

using HarmonyLib;

namespace SyrScarRemoval
{
	[StaticConstructorOnStartup]
	public static class ScarRemoval_Constructor
	{
		public readonly struct ScarRecipe
		{
			public float DefaultCount { get; } = 1;
			public IngredientCount Ingredient { get; } = null;

			public ScarRecipe(IngredientCount ingredientCount)
			{
				DefaultCount = ingredientCount.GetBaseCount();
				Ingredient = ingredientCount;
			}
		}

		static Dictionary<RecipeDef, ScarRecipe> ScarCostBase = [];

		private static void AddToDict(RecipeDef def)
		{
			var baseCost = def.ingredients.Find(i => i.FixedIngredient == ThingDefOf.MedicineUltratech);
			ScarCostBase.Add(def, new ScarRecipe(baseCost));
		}

		static ScarRemoval_Constructor()
		{
			AddToDict(ScarRemovalDefOf.SSR_RemoveScar);
			AddToDict(ScarRemovalDefOf.SSR_RemoveScarBrain);
			AddToDict(ScarRemovalDefOf.SSR_RegrowSmallBodyPart);
			AddToDict(ScarRemovalDefOf.SSR_HealAlzheimers);
			AddToDict(ScarRemovalDefOf.SSR_HealDementia);
			AddToDict(ScarRemovalDefOf.SSR_HealFrailty);

			ApplySettings();

		}

		public static IEnumerable<ThingDef> allAnimals;
		public static void ApplySettings()
		{
            foreach (var item in ScarCostBase)
            {
				float newCost = GenMath.RoundTo(item.Value.DefaultCount * ScarRemovalSettings.costAdjust, 1f);

				item.Value.Ingredient.SetBaseCount(newCost);
            }

			FieldInfo recipeCachedInfo = typeof(ThingDef).GetField("allRecipesCached", AccessTools.all);
			allAnimals ??= DefDatabase<ThingDef>.AllDefs.Where((ThingDef x) => x?.race?.FleshType != null && x.race.Animal);

			if (ScarRemovalSettings.applyToAnimals && allAnimals != null && recipeCachedInfo != null)
			{
				foreach (ThingDef thingDef in allAnimals)
				{
					foreach(var k in ScarCostBase.Keys)
					{
						thingDef.recipes.Add(k);
					}

					recipeCachedInfo.SetValue(thingDef, null);
				}
			}
			else if (!ScarRemovalSettings.applyToAnimals && allAnimals != null && recipeCachedInfo != null)
			{
				foreach (ThingDef thingDef in allAnimals)
				{
					foreach (var k in ScarCostBase.Keys)
					{
						thingDef.recipes.Remove(k);
					}

					recipeCachedInfo.SetValue(thingDef, null);
				}
			}
		}
	}
}
