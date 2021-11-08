/*
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
using System;
using System.Collections.Generic;
using UnityEngine;

namespace EvaFollower
{

    [KSPAddon(KSPAddon.Startup.Flight, false)]
    class EvaController : MonoBehaviour
    {
        public static EvaController instance;
		public string debug = "";
        public List<EvaContainer> collection = new List<EvaContainer>();
        
        public void Start()
        {

            Log.trace("EvaController.Start()");
            //initialize the singleton.
            instance = this;
                     
            GameEvents.onPartPack.Add(OnPartPack);
            GameEvents.onPartUnpack.Add(OnPartUnpack);
            
            GameEvents.onCrewOnEva.Add(OnCrewOnEva);
            GameEvents.onCrewBoardVessel.Add(OnCrewBoardVessel);
//            GameEvents.onCrewKilled.Add(OnCrewKilled);
            GameEvents.onVesselWillDestroy.Add(VesselDestroyed);

            GameEvents.onGameStateSave.Add(OnSave);
            GameEvents.onFlightReady.Add(onFlightReadyCallback);
        }

        public void OnDestroy()
        {
            Log.trace("EvaController.OnDestroy()");
        
            
            GameEvents.onPartPack.Remove(OnPartPack);
            GameEvents.onPartUnpack.Remove(OnPartUnpack);
        
            GameEvents.onCrewOnEva.Remove(OnCrewOnEva);
            GameEvents.onCrewBoardVessel.Remove(OnCrewBoardVessel);
//            GameEvents.onCrewKilled.Remove(OnCrewKilled);
            GameEvents.onVesselWillDestroy.Add(VesselDestroyed);

            GameEvents.onGameStateSave.Remove(OnSave);
            GameEvents.onFlightReady.Remove(onFlightReadyCallback);
        }
           
        /// <summary>
        /// Load the list 
        /// </summary>
        private void onFlightReadyCallback()
        {
            //Load the eva list.
            Log.trace("onFlightReadyCallback()");
            EvaSettings.Load();
        }

        public void OnSave(ConfigNode node)
        {
            //Save the eva list.
            // Might be double.
            foreach (var item in collection)
            {
                EvaSettings.SaveEva(item);
            }

            EvaSettings.Save();
        }

        public void OnPartPack(Part part)
        {
            if (part.vessel.isEVA)
            {
               //save before pack
                Log.detail("Pack: {0}", part.vessel.name);
                                
                Unload(part.vessel, false);
            }
        }

        public void OnPartUnpack(Part part)
        {
            if (part.vessel.isEVA)
            {               
                //save before pack
                Log.detail("Unpack: {0}", part.vessel.name);

                Load(part.vessel);
            }
        }

        /// <summary>
        /// Runs when the kerbal goes on EVA.
        /// </summary>
        /// <param name="e"></param>
        public void OnCrewOnEva(GameEvents.FromToAction<Part, Part> e)
        {
            //add new kerbal
            Log.trace("OnCrewOnEva()");
            Load(e.to.vessel);
        }

        /// <summary>
        /// Runs when the EVA goes onboard a vessel.
        /// </summary>
        /// <param name="e"></param>
        public void OnCrewBoardVessel(GameEvents.FromToAction<Part, Part> e)
        {
            //remove kerbal
            Log.trace("OnCrewBoardVessel()");
            Unload(e.from.vessel, true);
        }

        /// <summary>
        /// Runs when the EVA is killed.
        /// </summary>
        /// <param name="report"></param>
/*
        public void OnCrewKilled(EventReport report)
        {
            Log.trace("OnCrewKilled()");
		KerbalRoster boboo = new KerbalRoster(Game.Modes.SANDBOX);	
		print(boboo[report.sender].name);
		//MonoBehaviour.print(report.origin);
		//MonoBehaviour.print(report.origin.vessel);
            //Unload(report.origin.vessel, true);
        }
*/
        public void VesselDestroyed(Vessel report) {
            Log.trace("VesselDestroyed()");
		if (report.isEVA) Unload(report, true);
        }

        public void Load(Vessel vessel)
        {
            if (!vessel.isEVA)
            {
                Log.warn("Tried loading a non eva.");
                return;
            }

            KerbalEVA currentEVA = vessel.GetComponent<KerbalEVA>();

            if (!Contains(vessel.id))
            {
                EvaContainer container = new EvaContainer(vessel.id);

                //load the vessel here.
                container.Load(currentEVA);
                EvaSettings.LoadEva(container);

                collection.Add(container);
            }
            else
            {
                //Reload
                EvaContainer container = GetEva(vessel.id);

                container.Load(currentEVA);
                EvaSettings.LoadEva(container);
            }
        }

        public void Unload(Vessel vessel, bool delete)
        {
            if (!vessel.isEVA)
            {
                Log.warn("Tried unloading a non eva.");
                return;
            }

            Log.trace("Unload({0}", vessel.name);

            foreach (var item in collection)
            {
                if(item.flightID == vessel.id)
                {
                    if (delete)
                    {
                       item.status = Status.Removed;
                    }

                    //unload the vessel here. 
                    item.Unload();
                    EvaSettings.SaveEva(item);


                    Log.trace("Remove EVA: ({0})", vessel.name);
                    collection.Remove(item);
                    break;
                }
            }     
        }

        internal bool Contains(Guid id)
        {
            Log.trace("Contains()");

            for (int i = 0; i < collection.Count; i++)
            {
                if (collection[i].flightID == id)
                    return true;
            }

            return false;
        }

        
        internal EvaContainer GetEva(Guid flightID)
        {
            for (int i = 0; i < collection.Count; i++)
            {
                if (collection[i].flightID == flightID)
                    return collection[i];
            }

            return null;
        }
    }
}
