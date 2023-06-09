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
using System;
using System.Collections.Generic;
using UnityEngine;

namespace EvaFollower
{
	class DeadSpaceDetection : IDetection
	{
		public DeadSpaceDetection ()
		{
		}

		public IEnumerable<Collider> GetComponents(Vector3 center, float radius){
			
			Collider[] hitColliders = Physics.OverlapSphere(center, radius);
			int i = 0;
			while (i < hitColliders.Length) {
				yield return hitColliders [i];
			}
		}

		public void UpdateMap(List<EvaContainer> containers){

			string str = "";

			foreach (EvaContainer container in containers) {
				Rigidbody body = null;
				container.EVA.GetComponentCached<Rigidbody> (ref body);

				foreach (Collider collision in GetComponents(body.position, 1)) {
					str += collision.gameObject.name + Environment.NewLine;
				}
			}


			EvaController.instance.debug = str;
		}

		public void Debug(){
			
		}
	}
}

