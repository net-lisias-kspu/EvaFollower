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
using UnityEngine;

namespace EvaFollower
{
    internal class EvaContainer
    {
        public Guid flightID;
        public Mode mode = Mode.None;
        public Status status = Status.None;

        private bool selected = false;
        private bool loaded = false;
        private bool showHelmet = true;

        private KerbalEVA eva;

		internal readonly EvaFormation formation = new EvaFormation();
		internal readonly EvaPatrol patrol = new EvaPatrol();
		internal readonly EvaOrder order = new EvaOrder();
		internal readonly EvaWanderer wanderer = new EvaWanderer();

		const float RunMultiplier = 1.75f;
		const float BoundSpeedMultiplier = 1.25f;

		public void SetPatrolLinesVisible(bool v)
		{
			if (v && EvaSettings.Instance.displayDebugLines) {
				patrol.GenerateLine ();
			} else {
				patrol.Hide ();
			}
		}

        public bool IsActive
        {
            get { return (eva.vessel == FlightGlobals.ActiveVessel); }
        }

        public bool IsRagDoll
        {
            get { return eva.isRagdoll; }
        }

        public bool AllowPatrol
        {
            get { return (patrol.actions.Count >= 1);  }
        }

        public bool AllowRunning
        {
            get {

                if (mode == Mode.Patrol)
                {
                    return patrol.AllowRunning;
                }
                else if (mode == Mode.Order)
                {
                    return order.AllowRunning;
                }

                return false;
            }
        }

		public bool CanTakeHelmetOff
		{
			get
			{
				return
					EvaSettings.Instance.displayToggleHelmet
					&&
					FlightGlobals.ActiveVessel.mainBody.atmosphereContainsOxygen
				;
			}
		}

        public KerbalEVA EVA
        {
            get { return eva; }
        }

        public bool Selected
        {
            get { return selected; }
            set { selected = value; }
        }

        /// <summary>
        /// Get the world position of the kerbal.
        /// </summary>
        public Vector3d Position
        {
            get { return eva.vessel.GetWorldPos3D(); }
        }

        public bool Loaded
        {
            get {
                bool isLoaded = loaded;

				if (loaded) {
					isLoaded |= eva.isEnabled;
				}

                return isLoaded;
            }
        }

        public string Name { get; set; }

        public bool OnALadder { get { return eva.OnALadder; } }

        // NOTE: That's the history:
        //
        // Kerbals used to drift a lot since KSP 1.8.0. There's a nasty bug on KSP on Unity2019
        // where a miriad of tiny spurious forces are injected on the parts on every physics frame.
        //
        // I'm guessing they are not clamping the Random generators (perhaps they upgraded the code
        // from float to doubles, and now that spurius noise became a problem, as the conversion to
        // float would be doing the clamping by side effect).
        //
        // 
        System.Reflection.MethodInfo removeRBAnchor = null;

        public EvaContainer(Guid flightID)
        {
            this.flightID = flightID;
            this.removeRBAnchor = typeof(KerbalEVA).GetMethod("RemoveRBAnchor", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            this.loaded = false;
        }

        public void Load(KerbalEVA eva)
        {
            //Load KerbalEVA.
            this.eva = eva;
            loaded = true;

            //Set Name
            this.Name = eva.name;

            //module on last.
            EvaModule module = (EvaModule)eva.GetComponent(typeof(EvaModule));
            module.Load(this);

            Log.trace("EvaContainer.Load({0})", eva.name);
        }

        public void Unload()
        {
            Log.trace("EvaContainer.Unload({0})", eva.name);
            loaded = false;
        }

        internal string ToSave()
        {
            return (flightID + "," + Name + "," + mode + "," + status + "," + selected + "," + showHelmet + ","
                + this.formation.ToSave() + ","
                + this.patrol.ToSave() + ","
				+ this.order.ToSave()  + ","
				+ this.wanderer.ToSave()			
			);
        }

        internal void FromSave(string evaSettings)
        {
            EvaTokenReader reader = new EvaTokenReader(evaSettings);

            try
            {
                string sflightID = reader.NextTokenEnd(',');
                string sName = reader.NextTokenEnd(',');
                string mode = reader.NextTokenEnd(',');
                string status = reader.NextTokenEnd(',');
                string selected = reader.NextTokenEnd(',');
                string showHelmet = reader.NextTokenEnd(',');

                string formation = reader.NextToken('(', ')'); reader.Consume();
                string patrol = reader.NextToken('(', ')'); reader.Consume();
				string order = reader.NextToken('(', ')'); reader.Consume();
				string wanderer = reader.NextToken('(', ')');

                this.Name = sName;
                this.mode = (Mode)Enum.Parse(typeof(Mode), mode);
                this.status = (Status)Enum.Parse(typeof(Status), status);
                this.selected = bool.Parse(selected);
                this.showHelmet = bool.Parse(showHelmet);


                this.formation.FromSave(formation);
                this.patrol.FromSave(patrol);
                this.order.FromSave(order);
				this.wanderer.FromSave(wanderer);

                Log.detail("Loaded: {0}", mode);
                Log.detail("name: {0}", sName);
                Log.detail("status: {0}", status);
                Log.detail("selected: {0}", selected);

                if (this.showHelmet == false)
                {
                    eva.ShowHelmet(this.showHelmet);
                }
            }
            catch
            {
                throw new Exception("FromSave Failed.");
            }
        }


        internal void Follow()
        {
            Guid flightID = (FlightGlobals.fetch.activeVessel).id;
            EvaContainer leader = EvaController.instance.GetEva(flightID);

            selected = false;
            mode = Mode.Follow;
            formation.SetLeader(leader);
        }

        internal void Stay()
        {
            mode = Mode.None;
        }

        internal void SetWaypoint()
        {
            patrol.Move(eva.vessel);
        }

        internal void Wait()
        {
            patrol.Wait(eva.vessel);
        }

        internal void StartPatrol()
        {
            mode = Mode.Patrol;
        }

        internal void EndPatrol()
        {
            mode = Mode.None;
            patrol.Clear();
            eva.Animate(AnimationState.Idle);
        }

        internal void SetRunPatrolMode()
        {
            patrol.AllowRunning = true;
        }

        internal void SetWalkPatrolMode()
        {
            patrol.AllowRunning = false;
        }

		internal void StartWanderer()
		{
			mode = Mode.Wander;
			wanderer.SetEva (eva);
		}

        internal void ToggleHelmet()
        {
            this.showHelmet = !showHelmet;
            eva.ShowHelmet(this.showHelmet);
        }

        internal void UpdateLamps()
        {
            bool lampOn = Util.IsDark(eva.transform);

            if (showHelmet)
            {
                eva.TurnLamp(lampOn);
            }

			if (!showHelmet && eva.lampOn)
            {               
               eva.TurnLamp(false);
            }
        }

        internal void RecoverFromRagdoll()
        {
            Log.trace("Trying to recover {0} from Ragdoll", this.eva.part.GetInstanceID());
            this.UnanchorKerbal(eva);
            eva.RecoverFromRagdoll();
        }

        internal Vector3d GetNextTarget()
        {
            if (mode == Mode.Follow)
            {
                Vector3d target = formation.GetNextTarget();

				if (target == Vector2d.zero) {
					mode = Mode.None;
				}

				return target;
            }
            else if (mode == Mode.Patrol)
            {
                return patrol.GetNextTarget();
            }
            else if (mode == Mode.Order)
            {
                return order.GetNextTarget();
            }
			else if (mode == Mode.Wander)
			{
				return wanderer.GetNextTarget();
			}

            //Error
            throw new Exception("New Mode Introduced");
        }

        internal void ReleaseLadder()
        {
            eva.ReleaseLadder();
        }

        internal void UpdateAnimations(double sqrDist, ref float speed)
        {
            double geeForce = FlightGlobals.currentMainBody.GeeASL;

            if (eva.part.WaterContact)
            {
                speed *= eva.swimSpeed;
                eva.Animate(AnimationState.Swim);
            }
            else if (eva.JetpackDeployed)
            {
                speed *= 1f;
                eva.Animate(AnimationState.Idle);
            }
            else if (geeForce >= eva.minRunningGee)//sqrDist > 5f &&
            {
				if (AllowRunning) {
					speed *= eva.runSpeed;
					eva.Animate (AnimationState.Run);
				} 
				else if (sqrDist > 4 && mode == Mode.Follow) {
					speed *= eva.runSpeed * RunMultiplier;
					eva.Animate(AnimationState.Run);
				}
                else if (sqrDist > 8f && mode == Mode.Follow)
                {
					speed *= eva.runSpeed * RunMultiplier;
                    eva.Animate(AnimationState.Run);
                }
                else
                {
                    speed *= eva.walkSpeed;
                    eva.Animate(AnimationState.Walk);
                }
            }
            else if (geeForce >= eva.minWalkingGee)
            {
                speed *= eva.walkSpeed;
                eva.Animate(AnimationState.Walk);
            }
            else
            {
				speed *= eva.boundSpeed * BoundSpeedMultiplier; //speedup
                eva.Animate(AnimationState.BoundSpeed);
            }
        }

        internal void CheckDistance(Vector3d move, float speed, double sqrDist)
        {
            IEvaControlType controlType = null;

			if (mode == Mode.Follow)		{ controlType = formation; }
			else if (mode == Mode.Patrol)	{ controlType = patrol; }
			else if (mode == Mode.Order)	{ controlType = order; }
			else if (mode == Mode.Wander)	{ controlType = wanderer; }

			if (controlType.CheckDistance(sqrDist))
            {
                eva.Animate(AnimationState.Idle);

                if (controlType is EvaOrder)
                {
                    mode = Mode.None;
                }
            }
            else
            {
                if (AbleToMove())
                {
                    Move(move, speed);
                }
            }
        }

        private bool AbleToMove()
        {
			Rigidbody rigidbody = null;
			eva.GetComponentCached<Rigidbody>(ref rigidbody);
			return (!eva.isEnabled) | (!eva.isRagdoll) | (!rigidbody.isKinematic);
        }
        /// <summary>
        /// Move the current kerbal to target.
        /// </summary>
        /// <param name="move"></param>
        /// <param name="speed"></param>
        internal void Move(Vector3d move, float speed)
        {
            #region Move & Rotate Kerbal

            //speed values
            move *= speed;

            //rotate
            if (move != Vector3d.zero)
            {
                if (eva.JetpackDeployed)
                {
                    eva.PackToggle();
                }
                else
                {
                    //rotation
                    Quaternion from = eva.part.vessel.transform.rotation;
                    Quaternion to = Quaternion.LookRotation(move, eva.fUp);
                    Quaternion result = Quaternion.RotateTowards(from, to, eva.turnRate);

                    eva.part.vessel.SetRotation(result);

					Rigidbody rigidbody = null;
					eva.GetComponentCached<Rigidbody>(ref rigidbody);

                    //move
					if(rigidbody != null){
                        this.UnanchorKerbal(eva);
						rigidbody.MovePosition(rigidbody.position + move);
					}
                }
            }

            #endregion
        }

        internal void CheckModeIsNone()
        {
            if(mode == Mode.None)
                eva.Animate(AnimationState.Idle);
        }

        internal void Order(Vector3d position, Vector3d vector3d)
        {
            order.Move(position, vector3d);
        }


        void UnanchorKerbal(KerbalEVA eva)
        {
            if (null != this.removeRBAnchor)
                this.removeRBAnchor.Invoke(eva,  null);
        }


    }
}
