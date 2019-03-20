using Smod2;
using Smod2.Events;
using Smod2.EventHandlers;
using Smod2.API;
using System;
using System.Threading.Tasks;

namespace Overwatch
{
	public class OverwatchEventLogic : IEventHandlerWaitingForPlayers, IEventHandlerUpdate, IEventHandlerRoundStart, IEventHandlerRoundEnd
	{
		DateTime timeOnEvent = DateTime.Now;
		readonly Plugin plugin;
		static bool ow_restore;

		public OverwatchEventLogic(Plugin plugin)
		{
			this.plugin = plugin;
		}

		public static async Task ReturnOverWatchAfterRoundStarted()
		{
			await Task.Delay(500);

			if(ow_restore)
			{
				foreach (Player playa in Smod2.PluginManager.Manager.Server.GetPlayers())
				{
					if (OverwatchMain.CheckIfSteamIdIsInOverwatch.ContainsKey(playa.SteamId))
					{
						playa.OverwatchMode = true;
					}
				}
			}

			OverwatchMain.plugin.eventManager.RemoveEventHandlers(OverwatchMain.plugin);
			OverwatchMain.plugin.AddEventHandler(typeof(IEventHandlerRoundEnd), new OverwatchEventLogic(OverwatchMain.plugin));
		}


		public void OnRoundStart(RoundStartEvent ev)
		{
			Task t = ReturnOverWatchAfterRoundStarted();
		}

		public void OnUpdate(UpdateEvent ev)
		{
			if (DateTime.Now >= timeOnEvent)
			{
				timeOnEvent = DateTime.Now.AddSeconds(0.25);
				foreach (Player player in PluginManager.Manager.Server.GetPlayers())
				{
					if (player.OverwatchMode)
					{
						player.OverwatchMode = false;
						OverwatchMain.CheckIfSteamIdIsInOverwatch[player.SteamId] = true;
					}
				}
			}
		}

		public void OnWaitingForPlayers(WaitingForPlayersEvent ev)
		{
			if (plugin.GetConfigBool("overwatch_disable"))
			{
				Smod2.PluginManager.Manager.DisablePlugin(plugin.Details.id);
				return;
			}
			OverwatchMain.CheckIfSteamIdIsInOverwatch.Clear();
			ow_restore = plugin.GetConfigBool("ow_restore");
		}

		public void OnRoundEnd(RoundEndEvent ev)
		{
			plugin.eventManager.RemoveEventHandlers(plugin);
			OverwatchEventLogic events = new OverwatchEventLogic(plugin);
			plugin.AddEventHandler(typeof(IEventHandlerRoundEnd), events);
			plugin.AddEventHandler(typeof(IEventHandlerRoundStart), events);
			plugin.AddEventHandler(typeof(IEventHandlerUpdate), events);
			plugin.AddEventHandler(typeof(IEventHandlerWaitingForPlayers), events);
		}
	}
}