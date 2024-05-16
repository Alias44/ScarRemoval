using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using RimWorld;
using Verse;
using Verse.AI;
using System.Reflection;

namespace SyrScarRemoval
{
	[DefOf]
	public static class ScarRemovalDefOf
	{
		static ScarRemovalDefOf()
		{

		}
		public static RecipeDef SSR_RemoveScar;
		public static RecipeDef SSR_RemoveScarBrain;
		public static RecipeDef SSR_RegrowSmallBodyPart;
		public static RecipeDef SSR_HealDementia;
		public static RecipeDef SSR_HealAlzheimers;
		public static RecipeDef SSR_HealFrailty;

		public static BodyPartDef Finger;
		public static BodyPartDef Toe;
		public static BodyPartDef Ear;
		public static BodyPartDef Nose;
		public static BodyPartDef Tail;

		public static BodyPartDef Brain;
	}
}
