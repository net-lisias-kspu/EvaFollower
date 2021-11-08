﻿/*
	This file is part of EVA Follower /L Unleashed
		© 2021 Lisias T : http://lisias.net <support@lisias.net>
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
    class EvaAddonAddModule : MonoBehaviour
    {
        public void Awake()
        {
            Log.trace("Loaded AddonAddModule.");

            ConfigNode EVA = new ConfigNode("MODULE");
            EVA.AddValue("name", "EvaModule");

            try
            {
              PartLoader.getPartInfoByName("kerbalEVA").partPrefab.AddModule(EVA);
            }
            catch { 
			
			}

			EVA = new ConfigNode("MODULE");
            EVA.AddValue("name", "EvaModule");

			try {
				PartLoader.getPartInfoByName("kerbalEVAfemale").partPrefab.AddModule(EVA);
			} catch {

			}
       	}

    }
}
