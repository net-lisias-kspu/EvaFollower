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
    interface IEvaControlType
    {
        /// <summary>
        /// Check if the criteria is met, 
        /// </summary>
        /// <param name="sqrDistance">The distance between position and target</param>
        /// <returns>Returns true if the criteria is met.</returns>
        bool CheckDistance(double sqrDistance);

        /// <summary>
        /// Get the next target to move on. 
        /// </summary>
        /// <param name="move"></param>
        Vector3d GetNextTarget();

		string ToSave ();
		void FromSave (string action);
    }
}
