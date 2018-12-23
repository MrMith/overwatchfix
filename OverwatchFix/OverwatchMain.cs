using Smod2;
using Smod2.API;
using Smod2.Attributes;
using System;
using System.Collections.Generic;

namespace Overwatch
{

	[PluginDetails(
	author = "Mith",
	name = "overwatchfix",
	description = "Tries to fix the glitch where MTF spawns at the start of the round if someone is in overwatch.",
	id = "mith.overwatchfix",
	version = "0.0.1",
	SmodMajor = 3,
	SmodMinor = 2,
	SmodRevision = 0
	)]

	class OverwatchMain : Plugin
	{
		public static Dictionary<string, Boolean> CheckIfSteamIdIsInOverwatch = new Dictionary<string, bool>();

		public override void OnDisable()
		{
			this.Info(this.Details.id +" "+ this.Details.version +" has been Disabled.");
		}
		
		public override void OnEnable()
		{
			this.Info(this.Details.id + " " + this.Details.version + " has been Enabled.");
		}

		public override void Register()
		{
			this.AddEventHandlers(new OverwatchEventLogic(this));

			this.AddConfig(new Smod2.Config.ConfigSetting("overwatch_disable", false, Smod2.Config.SettingType.BOOL, true, "Disables the entire of this plugin."));

			this.AddCommand("overwatch_version", new Overwatch_Version(this));
			this.AddCommand("overwatch_disable", new Overwatch_Version(this));
		}
	}
}