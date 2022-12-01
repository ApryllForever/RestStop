using StardewModdingAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewValley;
using StardewModdingAPI.Events;
using Microsoft.Xna.Framework;
using System.Reflection;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI.Utilities;
using StardewValley.Quests;


namespace RestStopCode
{
    //Corrects the location name in the "X has begun in Y" message
    internal static class EventPatches
    {
        private static IMonitor Monitor { get; set; }
        private static IModHelper Helper { get; set; }


       

        private static void OnGameLaunched(object sender, GameLaunchedEventArgs e)
        {
            //MethodInfo showImageCommands = typeof(EventPatches).GetMethod("command_RestStopShowImage");
            //ExternalAPIs.SC.AddEventCommand("RSVShowImage", showImageCommands);


            MethodInfo AddSOCommand = typeof(EventPatches).GetMethod("command_RestStopAddSO");
            ExternalAPIs.SC.AddEventCommand("RestStopAddSO", AddSOCommand);
        }


        public static void command_RestStopAddSO(Event @event, GameLocation location, GameTime time, string[] split)
        {
            string specialOrderKey = split[2];
            try
            {
                if (!Game1.player.team.SpecialOrderActive(specialOrderKey))
                {
                    Game1.player.team.specialOrders.Add(SpecialOrder.GetSpecialOrder(specialOrderKey, null));
                }
                @event.CurrentCommand++;
                @event.checkForNextCommand(location, time);
            }
            catch
            {
                Monitor.Log($"Special order {specialOrderKey} not found and thus not added");
                @event.CurrentCommand++;
                @event.checkForNextCommand(location, time);
            }
        }
      /*  public static void command_RestStopAddQuest(Event @event, GameLocation location, GameTime time, string[] split)
        {
            string specialOrderKey = split[2];
            int questID = -1;
            try
            {
               
                {
                    Game1.player.addQuest(questID);
                }
                @event.CurrentCommand++;
                @event.checkForNextCommand(location, time);
            }
            catch
            {
                Monitor.Log($"Quest {questID} not found and thus not added");
                @event.CurrentCommand++;
                @event.checkForNextCommand(location, time);
            }
        }
        /*
        public static void command_RSVShowImage(Event @event, GameLocation location, GameTime time, string[] split)
        {
            try
            {
                Texture2D image = Helper.GameContent.Load<Texture2D>(PathUtilities.NormalizeAssetName(split[1]));
                if (!float.TryParse(split[2], out float scale))
                {
                    scale = 1f;
                }
                ImageMenu.Open(image, scale);
                @event.CurrentCommand++;
                if (split.Length > 3)
                {
                    @event.checkForNextCommand(location, time);
                }
            }
            catch
            {
                Log.Error($"Image {split[1]} not found");
                @event.CurrentCommand++;
                @event.checkForNextCommand(location, time);
            }
        } */



        /*
        internal static bool checkEventPrecondition_Prefix(ref string precondition, ref int __result)
        {
            if (precondition.Contains("/rsvRidingHorse", StringComparison.OrdinalIgnoreCase))
            {
                if (Game1.player.mount is null)
                {
                    __result = -1;
                    return false;
                }
                precondition = precondition.Replace("/rsvRidingHorse", "", StringComparison.OrdinalIgnoreCase);
                return true;
            }
            else
            {
                return true;
            }
        }  */

    }
}