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
using SD = System.Diagnostics;

namespace EvaFollower
{

#if DEBUG
     [KSPAddon(KSPAddon.Startup.MainMenu, true)]
    public class EvaDebug : MonoBehaviour
    {
         private Rect pos;
         private string content = "None";
         private GUIStyle style = null;
 
         public void Start()
         {
             DontDestroyOnLoad(this);
         }

         public void OnGUI()
         {
             if (HighLogic.LoadedScene == GameScenes.FLIGHT)
             {
                 if (style == null)
                 {
                    int w = 600;
                    int h = 250;

					pos = new Rect(Screen.width - (20+w), 60, w, h);

	                style = new GUIStyle(GUI.skin.label);
	                style.alignment = TextAnchor.UpperRight;
	                style.normal.textColor = new Color(0.8f, 0.8f, 0.8f, 0.6f);
                 }

                 GUI.Label(pos, content, style);
             }
         }

         public void Update()
         {
             if (HighLogic.LoadedScene == GameScenes.FLIGHT)
             {
				content = "Active Kerbals: " + EvaController.instance.collection.Count;
				content += Environment.NewLine + EvaController.instance.debug;
             }
             else
             {
                 content = "None";
             }
         }
#else 
          public class EvaDebug : MonoBehaviour
        {
#endif

        public static void ProfileStart()
        {
            StartTimer();
        }

        public static void ProfileEnd(string name)
        {
            EndTimer();
            Log.trace("Profile: {0}: {1}ms", name, Elapsed);
        }

        public static float Elapsed = 0;
        private static SD.Stopwatch watch;
        /// <summary>
        /// Start the timer
        /// </summary>
        private static void StartTimer()
        {
            watch = SD.Stopwatch.StartNew();
        }

        /// <summary>
        /// End the timer, and get the elapsed time.
        /// </summary>
        private static void EndTimer()
        {
            watch.Stop();
            Elapsed = watch.ElapsedMilliseconds;
        }
    }
}
