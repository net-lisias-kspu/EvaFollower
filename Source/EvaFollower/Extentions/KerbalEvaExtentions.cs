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
    static class KerbalEvaExtentions
    {
        public static Mesh helmetMesh = null;
        public static Mesh visorMesh = null;
        
        /// <summary>
        /// Enable you to remove the helmet.
        /// </summary>
        /// <param name="eva"></param>
        /// <param name="showHelmet"></param>
        public static void ShowHelmet(this KerbalEVA eva,bool showHelmet)
        {
            foreach (Renderer renderer in eva.GetComponentsInChildren<Renderer>())
            {
                SkinnedMeshRenderer smr = renderer as SkinnedMeshRenderer;

                if (smr != null)
                {
                    switch (smr.name)
                    {
                        case "helmet":
                            {
                                if (helmetMesh == null)
                                    helmetMesh = smr.sharedMesh;

                                smr.sharedMesh = showHelmet ? helmetMesh : null; 
                            }break;
                        case "visor":
                            {
                                if (visorMesh == null)
                                    visorMesh = smr.sharedMesh;

                                smr.sharedMesh = showHelmet ? visorMesh : null;
                            } break;
                    }
                }
            }

            if (!showHelmet)
            {
                eva.TurnLamp(false);
            }
        }

        /// <summary>
        /// Jump the current kerbal.
        /// </summary>
        /// <param name="eva"></param>
        public static void Jump(this KerbalEVA eva)
        {
            try
            {
                foreach (KFSMEvent item in eva.fsm.CurrentState.StateEvents)
                {
                    if (item.name == "Jump Start")
                    {
                        eva.fsm.RunEvent(item);
                    }
                }
            }
            catch { }
        }

        /// <summary>
        /// Let the current kerbal grap a nearby ladder.
        /// </summary>
        /// <param name="eva"></param>
        public static void GrapLadder(this KerbalEVA eva)
        {
            try
            {
                foreach (KFSMEvent item in eva.fsm.CurrentState.StateEvents)
                {
                    if (item.name == "Ladder Grab Start")
                    {
                        eva.fsm.RunEvent(item);
                    }
                }
            }
            catch { }
        }

        /// <summary>
        /// Let the current kerbal release the ladder, if on it.
        /// </summary>
        /// <param name="eva"></param>
        public static void ReleaseLadder(this KerbalEVA eva)
        {
            try
            {
                foreach (KFSMEvent item in eva.fsm.CurrentState.StateEvents)
                {
                    if (item.name == "Ladder Let Go")
                    {
                        eva.fsm.RunEvent(item);
                    }
                }
            }
            catch { }
        }

        /// <summary>
        /// Toggle the jetpack. 
        /// </summary>
        /// <param name="eva"></param>
        public static void PackToggle(this KerbalEVA eva)
        {
            try
            {
                foreach (KFSMEvent item in eva.fsm.CurrentState.StateEvents)
                {
                    if (item.name == "Pack Toggle")
                    {
                        eva.fsm.RunEvent(item);
                    }
                }
            }
            catch { }
        }

		public static void RecoverFromRagdoll(this KerbalEVA eva)
		{
			Rigidbody rigidbody = null;
			eva.GetComponentCached<Rigidbody> (ref rigidbody);
			if (null != rigidbody && rigidbody.isKinematic) return;
			if (!eva.isEnabled) return;

			// Much Kudos to Razchek for finally slaying the Ragdoll Monster!
			//
			// However, this is not working everytime on newer KSPs (detected on KSP 1.12.2 at least)
			// **Aparently**, when the Kerbal "stumbles", the state is not ragdoll and, so, this doesn't
			// recovers from it - besides eva.IsRagDoll being true!
			//
			// A little tap on a movement key solves the issue, however.
			//
			// Additionally, and interestingly, this unrecoverable ragdoll situation only happens when
			// the Kerbal that stumbles has de focus!! By switching vessels, it recovers, so perhaps this
			// is something I can tackle down on code!
			if (eva.canRecover && eva.fsm.TimeAtCurrentState > 1.21f && !eva.part.GroundContact)
				foreach (KFSMEvent stateEvent in eva.fsm.CurrentState.StateEvents) if ("Recover Start" == stateEvent.name)
				{
					eva.fsm.RunEvent(stateEvent);
					break;
				}
		}

		public static void Animate(this KerbalEVA eva, AnimationState state)
        {
            string anim = "Idle";

            switch (state)
            {
                case AnimationState.None: { } break;
                case AnimationState.Swim: { anim = "swim_forward"; } break;
                case AnimationState.Run: { anim = "wkC_run"; } break;
                case AnimationState.Walk: { anim = "wkC_forward"; } break;
                case AnimationState.BoundSpeed: { anim = "wkC_loG_forward"; } break;
                case AnimationState.Idle:
                    {
                        if (eva.part.WaterContact)
                            anim = "swim_idle";
                        else if (eva.JetpackDeployed)
                            anim = "jp_suspended";
                        else
                            anim = "idle";

                    } break;
            }

			Animation _animation = null;
			eva.GetComponentCached<Animation> (ref _animation);
			if (_animation != null) 
			{
				_animation.CrossFade (anim);
			}
		}

        /// <summary>
        /// Toggle the light of the current kerbal.
        /// </summary>
        /// <param name="eva"></param>
        public static void ToggleLight(this KerbalEVA eva)
        {
            eva.lampOn = !eva.lampOn;
            eva.TurnLamp(eva.lampOn);
        }

        /// <summary>
        /// Turn the lamp on of a kerbal.
        /// </summary>
        /// <param name="eva"></param>
        /// <param name="lampOn"></param>
        public static void TurnLamp(this KerbalEVA eva, bool lampOn)
        {
            eva.lampOn = lampOn;
            eva.headLamp.SetActive(lampOn);
        }

        /// <summary>
        /// Doesn't work ... yet!
        /// </summary>
        /// <param name="eva"></param>
        /// <param name="fear"></param>
        public static void FearFactor(this KerbalEVA eva, float fear)
        {
            kerbalExpressionSystem[] expS = UnityEngine.Object.FindObjectsOfType<kerbalExpressionSystem>();

            foreach (kerbalExpressionSystem item in expS)
            {
                if (item.kerbalEVA == eva)
                {
                    item.flight_gee = 10000;
                }
            }
        }



    }
}
