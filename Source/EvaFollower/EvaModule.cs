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
using System.Text;

namespace EvaFollower
{
    /// <summary>
    /// Keep track of the Context Menu.
    /// </summary>
    class EvaModule : PartModule
    {
        private EvaContainer currentContainer;

        public void Update()
        {
            if (!FlightGlobals.ready || PauseMenu.isOpen)
                return;

            if (currentContainer == null)
                return;

                ResetEvents();
                SetEvents();

        }

        public void Load(EvaContainer current)
        {
            this.currentContainer = current;
        }

        /// <summary>
        /// The default events based on the kerbal status.
        /// </summary>
        public void ResetEvents()
        {
            Events["Follow"].active = false;
            Events["Stay"].active = false;
            Events["SetPoint"].active = false;
            Events["Wait"].active = false;
            Events["Patrol"].active = false;
            Events["EndPatrol"].active = false;
            Events["PatrolRun"].active = false;
            Events["PatrolWalk"].active = false;
			Events["ToggleHelmet"].active = false;
			Events["StartWanderer"].active = false;
        }

        /// <summary>
        /// Set events based on the kerbal status.
        /// </summary>
        public void SetEvents()
        {
            if (!currentContainer.Loaded)
                return;

			if (!currentContainer.EVA.vessel.Landed) {
				return; 
			}

            if (currentContainer.mode == Mode.None)
            {
                Events["Follow"].active = true;
                Events["Stay"].active = false;
				//Events["StartWanderer"].active = true;
            }
            else if (currentContainer.mode == Mode.Follow)
            {
                Events["Follow"].active = false;
                Events["Stay"].active = true;
            }
			else if (currentContainer.mode == Mode.Patrol)
            {
                if (currentContainer.AllowRunning)
                {
                    Events["PatrolWalk"].active = true;
                }
                else
                {
                    Events["PatrolRun"].active = true;
                }

                Events["Patrol"].active = false;
                Events["EndPatrol"].active = true;
            }
            else if (currentContainer.mode == Mode.Order)
            {
                Events["Stay"].active = true;
                Events["Follow"].active = true;
            }

            if (currentContainer.CanTakeHelmetOff)
            {
                Events["ToggleHelmet"].active = true;
            }

            if (currentContainer.IsActive)
            {
                Events["Follow"].active = false;
                Events["Stay"].active = false;
                Events["SetPoint"].active = true;
                Events["Wait"].active = true;

                if (currentContainer.mode != Mode.Patrol)
                {
                    if (currentContainer.AllowPatrol)
                    {
                        Events["Patrol"].active = true;
                    }
                }
                else
                {
                    Events["SetPoint"].active = false;
                    Events["Wait"].active = false;
                }
            }
        }


        [KSPEvent(guiActive = true, guiName = "Follow Me", active = true, guiActiveUnfocused = true, unfocusedRange = 8)]
        public void Follow()
        {
            currentContainer.Follow();
        }

        [KSPEvent(guiActive = true, guiName = "Stay Put", active = true, guiActiveUnfocused = true, unfocusedRange = 8)]
        public void Stay()
        {
            currentContainer.Stay();
        }

        [KSPEvent(guiActive = true, guiName = "Add Waypoint", active = true, guiActiveUnfocused = true, unfocusedRange = 8)]
        public void SetPoint()
        {
                currentContainer.SetWaypoint();
        }

        [KSPEvent(guiActive = true, guiName = "Wait", active = true, guiActiveUnfocused = true, unfocusedRange = 8)]
        public void Wait()
        {
            currentContainer.Wait();
        }

        [KSPEvent(guiActive = true, guiName = "Patrol", active = true, guiActiveUnfocused = true, unfocusedRange = 8)]
        public void Patrol()
        {
            currentContainer.StartPatrol();
        }

        [KSPEvent(guiActive = true, guiName = "End Patrol", active = true, guiActiveUnfocused = true, unfocusedRange = 8)]
        public void EndPatrol()
        {
            currentContainer.EndPatrol();
        }

        [KSPEvent(guiActive = true, guiName = "Walk", active = true, guiActiveUnfocused = true, unfocusedRange = 8)]
        public void PatrolWalk()
        {
            currentContainer.SetWalkPatrolMode();
        }

        [KSPEvent(guiActive = true, guiName = "Run", active = true, guiActiveUnfocused = true, unfocusedRange = 8)]
        public void PatrolRun()
        {
            currentContainer.SetRunPatrolMode();
        }

        [KSPEvent(guiActive = true, guiName = "Toggle Helmet", active = true, guiActiveUnfocused = true, unfocusedRange = 8)]
        public void ToggleHelmet()
        {
            currentContainer.ToggleHelmet();
        }

		[KSPEvent(guiActive = true, guiName = "Wander", active = true, guiActiveUnfocused = true, unfocusedRange = 8)]
		public void StartWanderer()
		{
			currentContainer.StartWanderer();
		}

#if DEBUG
        [KSPEvent(guiActive = true, guiName = "Debug", active = true, guiActiveUnfocused = true, unfocusedRange = 8)]
        public void Debug()
        {
            foreach (var item in EvaController.instance.collection)
            {
                Log.detail("Item: {0}", item.flightID);
                Log.detail("leader: {0}", item.formation.GetLeader());
                Log.detail("patrol: {0}", item.patrol.ToString());
                Log.detail("order: {0}", item.order.ToString());
                Log.detail("patrol: {0}", item.patrol);
            }

            currentContainer.EVA.headLamp.SetActive(true); // It was light.intensity += 100;
        }


        [KSPEvent(guiActive = true, guiName = "Save", active = true, guiActiveUnfocused = true, unfocusedRange = 8)]
        public void ClearSave()
        {
            EvaSettings.Instance.SaveFunction();
        }


        [KSPEvent(guiActive = true, guiName = "Load", active = true, guiActiveUnfocused = true, unfocusedRange = 8)]
        public void Load()
        {
            EvaSettings.Instance.LoadFunction();
        }
#endif

    }
}
