﻿using System;
using System.Xml;

using Verse;

namespace SyrScarRemoval
{
	internal class BackCompatibilityConverter_SSR : BackCompatibilityConverter
	{
		public override bool AppliesToVersion(int majorVer, int minorVer) => majorVer == 0 || (majorVer == 1 && minorVer <= 5); // applies to <= 1.5

		public override string BackCompatibleDefName(Type defType, string defName, bool forDefInjections = false, XmlNode node = null)
		{
			if (GenDefDatabase.GetDefSilentFail(defType, defName, false) == null)
			{
				if (defType == typeof(RecipeDef))
				{
					var def = GenDefDatabase.GetDefSilentFail(defType, "SSR_" + defName, false);

					if (def != null)
					{
						return def.defName;
					}
				}
			}
			return null;
		}

		public override Type GetBackCompatibleType(Type baseType, string providedClassName, XmlNode node)
		{
			return null;
		}

		public override void PostExposeData(object obj)
		{
			return;
		}
	}
}
