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
namespace EvaFollower
{
	internal static class Const
	{
		internal static string SHADER_PARTICLE_ADDITIVE = 2019 == KSPe.Util.UnityTools.UnityVersion
					? "Legacy Shaders/Particles/Additive"
					: "Particles/Additive"
				;

	}
}
