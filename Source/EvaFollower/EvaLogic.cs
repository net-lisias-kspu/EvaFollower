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
using KSPe.Annotations;

namespace EvaFollower
{
	[KSPAddon(KSPAddon.Startup.Flight, false)]
	public class EvaLogic:MonoBehaviour
	{
		List<IDetection> detectionSystems = new List<IDetection>();

		public EvaLogic() {
			detectionSystems.Add (new DeadSpaceDetection ());
		}

		[UsedImplicitly]
		private void Awake()
		{
			Log.trace("EvaLogic.Awake()");
		}

		[UsedImplicitly]
		private void Start()
		{
			Log.trace("EvaLogic.Start()");
			GameEvents.onShowUI.Add(this.OnShowUI);
			GameEvents.onHideUI.Add(this.OnHideUI);
			this.OnShowUI();
		}

		[UsedImplicitly]
		private void OnDestroy()
		{
			Log.trace("EvaLogic.OnDestroy()");
			GameEvents.onHideUI.Remove(this.OnHideUI);
			GameEvents.onShowUI.Remove(this.OnShowUI);
		}

		internal void OnShowUI()
		{
			foreach (EvaContainer container in EvaController.instance.collection)
				container.SetPatrolLinesVisible(true);
		}

		internal void OnHideUI()
		{
			foreach (EvaContainer container in EvaController.instance.collection)
				container.SetPatrolLinesVisible(false);
		}

		[UsedImplicitly]
		private void FixedUpdate()
		{
			// Update detection systems.
			//foreach (IDetection detection in this.detectionSystems)
			//	detection.UpdateMap(EvaController.instance.collection);
		}

		[UsedImplicitly]
		private void Update()
		{
			if (!FlightGlobals.ready || PauseMenu.isOpen) return;

			if (Input.GetKeyDown (KeyCode.B)) {
				foreach (EvaContainer container in EvaController.instance.collection) {
					container.EVA.PackToggle ();
				}
			}

			foreach (EvaContainer eva in EvaController.instance.collection.ToArray()) if (null != eva && Mode.None != eva.mode && eva.Loaded) try
			{
				//Recover from ragdoll, if possible.
				if (eva.IsRagDoll)
				{
					eva.RecoverFromRagdoll();
					continue;
				}

				Vector3d move = -eva.Position;

				//Get next Action, Formation or Patrol
				Vector3d target = eva.GetNextTarget();

				// Path Finding
				//todo: check if the target is occopied.
				move += target;

				double sqrDist = move.sqrMagnitude;
				float speed = TimeWarp.deltaTime;

				if (eva.OnALadder)
				{
					eva.ReleaseLadder();
				}

				#region Break Free Code

				if (eva.IsActive)
				{
					Mode mode = eva.mode;

					if (Input.GetKeyDown(KeyCode.W))
						mode = EvaFollower.Mode.None;
					if (Input.GetKeyDown(KeyCode.S))
						mode = EvaFollower.Mode.None;
					if (Input.GetKeyDown(KeyCode.A))
						mode = EvaFollower.Mode.None;
					if (Input.GetKeyDown(KeyCode.D))
						mode = EvaFollower.Mode.None;
					if (Input.GetKeyDown(KeyCode.Q))
						mode = EvaFollower.Mode.None;
					if (Input.GetKeyDown(KeyCode.E))
						mode = EvaFollower.Mode.None;

					if (mode == Mode.None)
					{
						//break free!
						eva.mode = mode;
						continue;
					}
				}
				#endregion

				//Animation Logic
				eva.UpdateAnimations(sqrDist, ref speed);

				move.Normalize();

				//Distance Logic
				eva.CheckDistance(move, speed, sqrDist);

				//Reset Animation Mode Events
				eva.CheckModeIsNone();

			}
			catch (Exception exp)
			{
				Log.err("EvaLogic: {0} : {1}", exp.Message, exp);
			}
		}
	}
}
