using System;
using System.Collections.Generic;

namespace EvaFollower
{
	interface IDetection
	{
		void UpdateMap (List<EvaContainer> collection);
		void Debug();
	}
}

