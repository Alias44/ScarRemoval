using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using RimWorld;
using Verse;

using HarmonyLib;

namespace SyrScarRemoval;

[StaticConstructorOnStartup]
public static class ScarRemoval_Constructor
{
	public readonly struct ScarRecipe(IngredientCount ingredientCount)
	{
		public float DefaultCount { get; } = ingredientCount.GetBaseCount();
		public IngredientCount Ingredient { get; } = ingredientCount;
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
	}
}
