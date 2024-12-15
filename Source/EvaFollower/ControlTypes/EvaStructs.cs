/*
	This file is part of EVA Follower /L Unleashed
		© 2021-2024 LisiasT : http://lisias.net <support@lisias.net>
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
    /// The mode the EVA is in.
    /// </summary>
    /// 
    [Flags]
    enum Mode
    {
        None = 1,
        Follow = 2,
        Patrol = 3,
        Leader = 4,
        Order = 5,
		Wander = 6
    }

    /// <summary>
    /// The status the EVA is in.
    /// </summary>
    /// 
    [Flags]
    enum Status
    {
        None,
        Removed
    }

    /// <summary>
    /// The animation states for the EvaControllerContainer
    /// </summary>
    enum AnimationState
    {
        None,
        Swim,
        Run,
        Walk,
        BoundSpeed,
        Idle
    }
}
