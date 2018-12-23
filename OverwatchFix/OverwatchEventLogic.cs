using Smod2;
using Smod2.Events;
using Smod2.EventHandlers;
using Smod2.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Overwatch
{
	public class OverwatchEventLogic : IEventHandlerWaitingForPlayers, IEventHandlerUpdate ,IEventHandlerRoundStart
	{

		readonly Plugin plugin;
		public OverwatchEventLogic(Plugin plugin)
		{
			this.plugin = plugin;
		}

		public static async Task ReturnOverWatchAfterRoundStarted()
		{
			await Task.Delay(1000);

			foreach (Player playa in Smod2.PluginManager.Manager.Server.GetPlayers())
			{
				if (OverwatchMain.CheckIfSteamIdIsInOverwatch.ContainsKey(playa.SteamId))
				{
					playa.OverwatchMode = true;
				}
			}
		}

		public void OnRoundStart(RoundStartEvent ev)
		{
			Task t = ReturnOverWatchAfterRoundStarted();
		}

		public void OnUpdate(UpdateEvent ev)
		{
			DateTime timeOnEvent = DateTime.Now;
			if (DateTime.Now >= timeOnEvent)
			{
				if (Smod2.PluginManager.Manager.Server.Round.Duration != 0)
				{
					return;
				}
				timeOnEvent = DateTime.Now.AddSeconds(0.5);
				foreach (Player player in PluginManager.Manager.Server.GetPlayers())
				{
					if (player.OverwatchMode)
					{
						plugin.Info(player.Name);
						player.OverwatchMode = false;
						OverwatchMain.CheckIfSteamIdIsInOverwatch[player.SteamId] = true;
					}
				}
			}
		}

		public void OnWaitingForPlayers(WaitingForPlayersEvent ev)
		{
			OverwatchMain.CheckIfSteamIdIsInOverwatch.Clear();

			if (plugin.GetConfigBool("overwatch_disable"))
			{
				Smod2.PluginManager.Manager.DisablePlugin(plugin.Details.id);
				return;
			}
		}
	}
}