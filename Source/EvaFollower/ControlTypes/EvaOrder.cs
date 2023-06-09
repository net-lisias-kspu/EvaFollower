/*
	This file is part of EVA Follower /L Unleashed
		© 2021-2023 Lisias T : http://lisias.net <support@lisias.net>
		© 2014-2016 Marijn Stevens (MSD)
		© 2013 Fel

	EVA Follower /L Unleashed is licensed as follows:
		* CC-BY-NC-SA 3.0 : https://creativecommons.org/licenses/by-nc-sa/3.0/

	EVA Follower /L Unleashed is distributed in the hope that
	it will be useful, but WITHOUT ANY WARRANTY; without even the implied
	warranty of	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.

*/

namespace EvaFollower
{
    /// <summary>
    /// The object responsible for ordering the kerbal around. 
    /// </summary>
    class EvaOrder : IEvaControlType
    {
        internal Vector3d Offset = -Vector3d.one;
        internal Vector3d Position;

        public bool AllowRunning { get; set; }

        public EvaOrder()
        {
            AllowRunning = true;
        }

        public bool CheckDistance(double sqrDistance)
        {
            bool complete = (sqrDistance < 0.8);            
            return complete;
        }

        public Vector3d GetNextTarget()
        {
            return Position;
        }

        public void Move(Vector3d pos, Vector3d off)
        {
            this.Offset = off;
            this.Position = pos;
        }

        public override string ToString()
        {
            return Position + ": offset(" + Offset + ")";
        }

		public string ToSave()
        {
            return "(" + AllowRunning.ToString() + "," + Position + "," + Offset + ")";
        }

		public void FromSave(string order)
        {
            Log.trace("Order.FromSave()");
            EvaTokenReader reader = new EvaTokenReader(order);

            string sAllowRunning = reader.NextTokenEnd(',');
            string sPosition = reader.NextToken('[', ']'); reader.Consume(); // , 
            string sOffset = reader.NextToken('[', ']');

            AllowRunning = bool.Parse(sAllowRunning);
            Position = Util.ParseVector3d(sPosition, false);
            Offset = Util.ParseVector3d(sOffset, false);                        
        }
    }
}
