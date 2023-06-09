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

using KSPe;

using Asset = KSPe.IO.Asset<EvaFollower.Startup>;
using Savegame = KSPe.IO.Save<EvaFollower.Startup>;
using File = KSPe.IO.File<EvaFollower.Startup>.Save;
using SIO = System.IO;

namespace EvaFollower
{
    class EvaSettings
    {
        private static EvaSettings instance = null;
        internal static EvaSettings Instance => instance ?? (instance = new EvaSettings());

        internal bool targetVesselBySelection = false;
        internal bool displayDebugLines = false;
        internal bool displayDebugLinesSetting = false;
        internal bool displayToggleHelmet = true;
        internal bool displayLoadingKerbals = true;

        internal int selectMouseButton = 0;
        internal int dispatchMouseButton = 2;

        internal string selectKeyButton = "o";
        internal string dispatchKeyButton = "p";


        private Dictionary<Guid, string> collection = new Dictionary<Guid, string>();

        private bool isLoaded = false;

		internal void Destroy()
		{
			instance = null;
		}

		private readonly Asset.ConfigNode DEFAULTS = Asset.ConfigNode.For("EvaFollower", "Config.cfg");
        private readonly Savegame.ConfigNode SAVE = Savegame.ConfigNode.For("EvaFollower", "Config.cfg");
        public void LoadConfiguration()
        {
            if (!SAVE.IsLoadable)
            {
                DEFAULTS.Load();
                SAVE.Save(DEFAULTS.Node);
            }
            else
            {
                ConfigNodeWithSteroids cn = ConfigNodeWithSteroids.from(SAVE.Load().Node);
                try
                {
                    displayDebugLines = displayDebugLinesSetting
                        = cn.GetValue<bool>("ShowDebugLines");
                    displayLoadingKerbals = cn.GetValue<bool>("ShowLoadingKerbals");
                    displayToggleHelmet = cn.GetValue<bool>("EnableHelmetToggle");
                    selectMouseButton = cn.GetValue<int>("SelectMouseButton");
                    dispatchMouseButton = cn.GetValue<int>("DispatchMouseButton");
                    selectKeyButton = cn.GetValue<string>("SelectKey");
                    dispatchKeyButton = cn.GetValue<string>("DispatchKey");
                    targetVesselBySelection = cn.GetValue<bool>("TargetVesselBySelection");
                }
                catch (Exception e)
                {
                    Log.err("Config loading error {0}", e.Message);
                }
            }
        }

        public void SaveConfiguration()
        {
            SAVE.Save();
        }

        public void Load()
        {
            Log.trace("OnLoad()");
			if (displayLoadingKerbals) {
				ScreenMessages.PostScreenMessage ("Loading Kerbals...", 3, ScreenMessageStyle.LOWER_CENTER);
			}

            LoadFunction();
        }

        public void LoadFunction()
        {
            EvaDebug.ProfileStart();
            LoadFile();
            EvaDebug.ProfileEnd("EvaSettings.Load()");
            isLoaded = true;
        }

		public void Save()
		{
			if (isLoaded)
			{
				Log.trace("OnSave()");

				if (displayLoadingKerbals)
				{
					ScreenMessages.PostScreenMessage("Saving Kerbals...", 3, ScreenMessageStyle.LOWER_CENTER);
				}

				SaveFunction();
			}
		}

		public void SaveFunction()
        {
            EvaDebug.ProfileStart();
            SaveFile();
            EvaDebug.ProfileEnd("EvaSettings.Save()");
        }

        public void LoadEva(EvaContainer container)
        {
            Log.trace("EvaSettings.LoadEva({0})", container.Name);

            //The eva was already has a old save.
            //Load it.
            if (collection.ContainsKey(container.flightID))
            {
#if DEBUG
                string evaString = collection[container.flightID];
                Log.dbg(evaString);
#endif
                container.FromSave(collection[container.flightID]);
            }
            else
            {
                //No save yet.
            }
        }
        public void SaveEva(EvaContainer container){

            Log.trace("EvaSettings.SaveEva({0})", container.Name);

            if (container.status == Status.Removed)
            {
                if (collection.ContainsKey(container.flightID))
                {
                    collection.Remove(container.flightID);
                }
            }
            else
            {
                //The eva was already has a old save.
                if (collection.ContainsKey(container.flightID))
                {
                    //Replace the old save.
                    collection[container.flightID] = container.ToSave();
                }
                else
                {
                    //No save yet. Add it now.
                    collection.Add(container.flightID, container.ToSave());
                }
            }
        }

        private const string EVA_FILENAME = "Evas.txt";
        private void LoadFile()
        {
            if (File.Exists(EVA_FILENAME))
            {
				SIO.TextReader tr = Savegame.StreamReader.CreateFor(EVA_FILENAME);
                string file = tr.ReadToEnd();
                tr.Close();

                EvaTokenReader reader = new EvaTokenReader(file);
                Log.detail("Size KeySize: {0}", collection.Count);

                //read every eva.
                while (!reader.EOF)
                {
                    //Load all the eva's in the list.
                    LoadEva(reader.NextToken('[', ']'));
                }
            }
        }

        private void LoadEva(string eva)
        {
            Guid flightID = GetFlightIDFromEvaString(eva);
            collection.Add(flightID, eva);
        }


        private static Guid GetFlightIDFromEvaString(string evaString)
        {
            EvaTokenReader reader = new EvaTokenReader(evaString);

            string sflightID = reader.NextTokenEnd(',');

            //Load the eva
            Guid flightID = new Guid(sflightID);
            return flightID;
        }


        private void SaveFile()
        {
			SIO.TextWriter tw = Savegame.StreamWriter.CreateFor(EVA_FILENAME);

            foreach (KeyValuePair<Guid, string> item in collection)
                tw.Write("[" + item.Value + "]");

            tw.Close();

            collection.Clear();
			this.isLoaded = false;
        }
    }
}
