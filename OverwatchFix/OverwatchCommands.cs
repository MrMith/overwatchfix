using Smod2;
using Smod2.Commands;

namespace Overwatch
{
	class Overwatch_Version : ICommandHandler
	{

		private Plugin plugin;
		public Overwatch_Version(Plugin plugin)
		{
			this.plugin = plugin;
		}

		public string GetCommandDescription()
		{
			return "Version for this plugin.";
		}

		public string GetUsage()
		{
			return "overwatch_version";
		}

		public string[] OnCall(ICommandSender sender, string[] args)
		{
			return new string[] { "This is version " + plugin.Details.version };
		}
	}

	class Overwatch_Disable : ICommandHandler
	{
		private Plugin plugin;

		public Overwatch_Disable(Plugin plugin)
		{
			this.plugin = plugin;
		}

		public string GetCommandDescription()
		{
			return "Enables or disables OverwatchFix.";
		}

		public string GetUsage()
		{
			return "overwatch_disable";
		}

		public string[] OnCall(ICommandSender sender, string[] args)
		{
			Smod2.PluginManager.Manager.DisablePlugin(plugin.Details.id);
			return new string[] { "Disabled " + plugin.Details.id };
		}
	}
}
