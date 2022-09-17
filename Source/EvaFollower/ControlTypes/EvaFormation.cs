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
using System;

namespace EvaFollower
{
    /// <summary>
    /// The object responsible for Formations.
    /// </summary>
    class EvaFormation : IEvaControlType
    {
        private EvaContainer leader;

        /// <summary>
        /// Get the next position to walk to. 
        /// Formation should handle differents positions.
        /// </summary>
        /// <param name="move"></param>
        /// <returns></returns>
        public Vector3d GetNextTarget()
        {           
            if (leader == null)
            {
                return Vector3d.zero;
            }

            //get the leader. 
            Vector3d target = leader.EVA.vessel.GetWorldPos3D();        
            
            //update move vector.
            return target;
        }

        public void SetLeader(EvaContainer leader)
        {
            this.leader = leader;
        }

        public string GetLeader()
        {
            if (leader != null)
            {
                if (leader.Loaded)
                {
                    return leader.EVA.name;
                }
            }

            return "None";
        }

        /// <summary>
        /// Check if the distance to the target is reached.
        /// </summary>
        /// <param name="sqrDistance"></param>
        /// <returns></returns>
        public bool CheckDistance(double sqrDistance)
        {
            if (sqrDistance < 3.0)
            {
                return true;
            }
            return false;
        }

        
        public string ToSave()
        {
            string leaderID = "null";
            if(leader != null)
            {
                leaderID = leader.flightID.ToString();
            }
            return "(Leader:" + leaderID + ")";
        }

		public void FromSave(string formation)
        {
            try
            {
                Log.trace("Formation.FromSave()");
                formation = formation.Remove(0, 7); //Leader:
                
                if (formation != "null")
                {
                    Guid flightID = new Guid(formation);
                    EvaContainer container = EvaController.instance.GetEva(flightID);

                    if (container != null)
                    {
                        leader = container;
                    }
                }
            }
            catch
            {
                throw new Exception("Formation.FromSave Failed.");
            }  
        }
    }
}
