using UnityEngine;

using RimWorld;
using Verse;

namespace SyrScarRemoval
{
	public class ScarRemoval : Mod
	{
		public static ScarRemovalSettings settings;
		public ScarRemoval(ModContentPack content) : base(content)
		{
			settings = GetSettings<ScarRemovalSettings>();
		}

		public override string SettingsCategory() => "ScarRemovalSettings".Translate();

		public override void DoSettingsWindowContents(Rect inRect)
		{
			checked
			{
				Listing_Standard listing_Standard = new Listing_Standard();
				listing_Standard.Begin(inRect);

				float diff = listing_Standard.SliderLabeled("ScarRemovalSettingsCost".Translate(ScarRemovalSettings.costAdjust.ToString("0.##")), ScarRemovalSettings.costAdjust, 1, 3, 0.6f, "ScarRemovalSettingsCostTooltip".Translate());
				ScarRemovalSettings.costAdjust = GenMath.RoundTo(diff, 0.25f);

				listing_Standard.Gap(12f);
				listing_Standard.CheckboxLabeled("ScarRemovalSettingsapplyToAnimals".Translate(), ref ScarRemovalSettings.applyToAnimals, "ScarRemovalSettingsapplyToAnimalsTooltip".Translate());
				listing_Standard.End();
				settings.Write();
			}
		}
		public override void WriteSettings()
		{
			base.WriteSettings();
			ScarRemoval_Constructor.ApplySettings();
		}
	}
}
