using System.Collections.Generic;

using HarmonyLib;

using Verse;

namespace SyrScarRemoval
{
	[StaticConstructorOnStartup]
	public class HarmonyPatches
	{
		static HarmonyPatches()
		{
			// Add a custom back compatibility to the conversion chain
			List<BackCompatibilityConverter> compatibilityConverters =
				AccessTools.StaticFieldRefAccess<List<BackCompatibilityConverter>>(typeof(BackCompatibility),
					"conversionChain");

			compatibilityConverters.Add(new BackCompatibilityConverter_SSR());
		}
	}
}
