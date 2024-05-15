using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using RimWorld;
using Verse;
using Verse.AI;
using System.Reflection;
using System.Globalization;

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
			AddToDict(ScarRemovalDefOf.RemoveScar);
			AddToDict(ScarRemovalDefOf.RemoveScarBrain);
			AddToDict(ScarRemovalDefOf.RegrowSmallBodyPart);
			AddToDict(ScarRemovalDefOf.HealAlzheimers);
			AddToDict(ScarRemovalDefOf.HealDementia);
			AddToDict(ScarRemovalDefOf.HealFrailty);

			ApplySettings();

		}

		public static BindingFlags all = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.SetField | BindingFlags.GetProperty | BindingFlags.SetProperty;
		public static IEnumerable<ThingDef> allAnimals;
		public static void ApplySettings()
		{
            foreach (var item in ScarCostBase)
            {
				float newCost = GenMath.RoundTo(item.Value.DefaultCount * ScarRemovalSettings.costAdjust, 1f);

				item.Value.Ingredient.SetBaseCount(newCost);
            }

			FieldInfo recipeCachedInfo = typeof(ThingDef).GetField("allRecipesCached", all);
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
