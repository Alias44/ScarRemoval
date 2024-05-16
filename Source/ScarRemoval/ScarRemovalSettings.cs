using Verse;

namespace SyrScarRemoval
{
	public class ScarRemovalSettings : ModSettings
	{
		public static bool hardMode;
		public static float costAdjust = 1;
		public static bool applyToAnimals;
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look(ref hardMode, "ScarRemoval_hardMode", false);
			Scribe_Values.Look(ref costAdjust, "ScarRemoval_Cost", 1);
			Scribe_Values.Look(ref applyToAnimals, "ScarRemoval_applyToAnimals", false, true);

			if(Scribe.mode == LoadSaveMode.LoadingVars && hardMode)
			{
				costAdjust = 2;
				hardMode = false;
			}
		}
	}
}
