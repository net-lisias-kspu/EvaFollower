/*
	This file is part of EVA Follower /L Unleashed
		© 2021-2022 Lisias T : http://lisias.net <support@lisias.net>
		© 2014-2016 Marijn Stevens (MSD)
		© 2013 Fel

	EVA Follower /L Unleashed is licensed as follows:
		* CC-BY-NC-SA 3.0 : https://creativecommons.org/licenses/by-nc-sa/3.0/

	EVA Follower /L Unleashed is distributed in the hope that
	it will be useful, but WITHOUT ANY WARRANTY; without even the implied
	warranty of	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.

*/
using UnityEngine;

namespace EvaFollower
{
	/// <summary>
	/// Add the module to all kerbals available. 
	/// </summary>
	[KSPAddon(KSPAddon.Startup.SpaceCentre, true)]
	class EvaAddonAddModule:MonoBehaviour
	{
		private static string[] EVAS =
		{
			"kerbalEVA", "kerbalEVAfemale"
			,"kerbalEVAVintage", "kerbalEVAfemaleVintage"
			,"kerbalEVAFuture", "kerbalEVAFemaleFuture"
			,"kerbalEVASlimSuit", "kerbalEVASlimSuitFemale"
		};

		public void Awake()
		{
			Log.trace("Loaded AddonAddModule.");

			foreach (string partName in EVAS) try // There should be a better way, but for now let it go as it is.
				{
					ConfigNode EVA = new ConfigNode("MODULE");
					EVA.AddValue("name", "EvaModule");
					PartLoader.getPartInfoByName(partName).partPrefab.AddModule(EVA);
				}
				catch { } // Yuck. Empty try catches... :(
		}

	}
}
