using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Netcode;
using StardewValley.BellsAndWhistles;
using StardewValley.Buildings;
using StardewValley.Characters;
using StardewValley.GameData.Movies;
using StardewValley.Locations;
using StardewValley.Menus;
using StardewValley.Minigames;
using StardewValley.Monsters;
using StardewValley.Network;
using StardewValley.Objects;
using StardewValley.Projectiles;
using StardewValley.TerrainFeatures;
using StardewValley.Tools;
using StardewValley.Util;
using StardewValley;
using System.Globalization;
using System.IO;
using System.Xml.Serialization;
using xTile;
using xTile.Dimensions;
using xTile.Layers;
using xTile.ObjectModel;
using xTile.Tiles;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;


namespace RestStopCode

{
    public class RestStopTileActions
    {
        static IModHelper Helper;

        static HashSet<Vector2> SpiritAltarTriggeredToday = new HashSet<Vector2>();
        internal static void Setup(IModHelper Helper)
        {
            RestStopTileActions.Helper = Helper;
            Helper.Events.GameLoop.DayStarted += OnDayStarted;
            TileActionHandler.RegisterTileAction("RestStop_SpiritAltar", Trigger);
        }


        public virtual bool performAction(string action, Farmer who, Location tileLocation)
        {
            if (action != null && who.IsLocalPlayer)
            {
                string[] actionParams = action.Split(' ');
                switch (actionParams[0])
                {
                    case "RestStop_SpiritAltar":
                        if (who.ActiveObject != null && (double)Game1.player.team.sharedDailyLuck != -0.12 && (double)Game1.player.team.sharedDailyLuck != 0.12)
                        {
                            if (who.ActiveObject.Price >= 60)
                            {
                                // temporarySprites.Add(new TemporaryAnimatedSprite(352, 70f, 2, 2, new Vector2(tileLocation.X * 64, tileLocation.Y * 64), flicker: false, flipped: false));
                                Game1.player.team.sharedDailyLuck.Value = 0.12;
                                Game1.playSound("money");
                            }
                            else
                            {
                                // temporarySprites.Add(new TemporaryAnimatedSprite(362, 50f, 6, 1, new Vector2(tileLocation.X * 64, tileLocation.Y * 64), flicker: false, flipped: false));
                                Game1.player.team.sharedDailyLuck.Value = -0.12;
                                Game1.playSound("thunder");
                            }
                            who.ActiveObject = null;
                            who.showNotCarrying();
                        }
                        break;

                    default:
                        return false;

                }
                return true;
            }
            if (action != null && !who.IsLocalPlayer)
            {
                switch (action.ToString().Split(' ')[0])
                {

                    /*    case "Minecart":
                            openChest(tileLocation, 4, Game1.random.Next(3, 7));
                            break;
                        case "RemoveChest":
                            map.GetLayer("Buildings").Tiles[tileLocation.X, tileLocation.Y] = null;
                            break;
                        case "Door":
                            openDoor(tileLocation, playSound: true);
                            break;
                        case "TV":
                            Game1.tvStation = Game1.random.Next(2);
                            break; */
                }
            }
            return false;
        }
    



   

    
       // public List<TemporaryAnimatedSprite> temporarySprites = new List<TemporaryAnimatedSprite>();

       

     
       // internal static void Setup(IModHelper Helper)
        //{
         //   SundryActions.Helper = Helper;
           // Helper.Events.GameLoop.DayStarted += OnDayStarted;
            //TileActionHandler.RegisterTileAction("RestStop_SpiritAltar", Trigger);
        //}
        private static void OnDayStarted(object sender, DayStartedEventArgs e)
        {
            SpiritAltarTriggeredToday.Clear();

        }


        public static void Trigger(string tileAction, Vector2 position)
        {
            if (SpiritAltarTriggeredToday.Contains(position))
            {
                return;
            }
            SpiritAltarTriggeredToday.Add(position);
            GameLocation location = Game1.currentLocation;
            //Vector2 itemSpawnPosition = position * 64f;
           // itemSpawnPosition.Y -= 64f;
            //itemSpawnPosition.X += 32f;
            Game1.stats.incrementStat("SpiritAltarChecked", 1);

            //string[] actionParams = action.Split(' ');
            //switch (actionParams[0])
              if (Game1.player.ActiveObject != null && (double)Game1.player.team.sharedDailyLuck != -0.12 && (double)Game1.player.team.sharedDailyLuck != 0.12)
                            {
                                if (Game1.player.ActiveObject.Price >= 60)
                                {
                                   // temporarySprites.Add(new TemporaryAnimatedSprite(352, 70f, 2, 2, new Vector2(tileLocation.X * 64, tileLocation.Y * 64), flicker: false, flipped: false));
                                    Game1.player.team.sharedDailyLuck.Value = 0.12;
                                    Game1.playSound("discoverMineral");
                                }
                                else
                                {
                                   // temporarySprites.Add(new TemporaryAnimatedSprite(362, 50f, 6, 1, new Vector2(tileLocation.X * 64, tileLocation.Y * 64), flicker: false, flipped: false));
                                    Game1.player.team.sharedDailyLuck.Value = -0.12;
                                    Game1.playSound("thunder");
                                }
                Game1.player.ActiveObject = null;
                Game1.player.showNotCarrying();
                            }





          
                    }
                } 
        }
    



