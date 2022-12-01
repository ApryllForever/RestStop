using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using Microsoft.Xna.Framework.Graphics;
using Netcode;
using StardewValley.BellsAndWhistles;
using StardewValley.Menus;
using StardewValley.Monsters;
using StardewValley.Network;
using StardewValley.Objects;
using StardewValley.TerrainFeatures;
using StardewValley.Tools;
using System.Xml.Serialization;
using xTile;
using xTile.Dimensions;
using xTile.Layers;
using xTile.ObjectModel;
using xTile.Tiles;
using StardewValley.Locations;
using StardewModdingAPI.Utilities;
using SpaceCore.Events;
using Object = StardewValley.Object;


namespace RestStopCode
{
    [HarmonyPatch(typeof(Game1), nameof(Game1.getLocationFromNameInLocationsList))]
   

    /*
     [HarmonyPatch(typeof(Game1), nameof(Game1.loadForNewGame))]
        public class Game1_loadForNewGame_Patch
        {
            public static void Postfix()
            {
                if (!Config.EnableMod || !Game1.IsMasterGame)
                    return;
                mapAssetKey = SHelper.ModContent.GetInternalAssetName("assets/Dreamscape.tmx").BaseName;
                GameLocation location = new GameLocation(mapAssetKey, "Dreamscape") { IsOutdoors = false, IsFarm = true, IsGreenhouse = true };
                Game1.locations.Add(location);
                SHelper.GameContent.InvalidateCache("Data/Locations");
            }
        }
    */
    
    
    
    
    
    
    
    
    
    public static class Game1FetchDungeonInstancePatch
    {
        public static bool Prefix(string name, bool isStructure, ref GameLocation __result)
        {
            if (name.StartsWith(VolcanoDungeonHell.BaseLocationName))
            {
                __result = VolcanoDungeonHell.GetLevelInstance(name);
                return false;
            }

            return true;
        }
    }

    [HarmonyPatch(typeof(Game1), "UpdateLocations")]
    public static class Game1UpdateDungeonLocationsPatch
    {
        public static void Postfix(GameTime time)
        {
            if (Game1.menuUp && !Game1.IsMultiplayer)
            {
                return;
            }
            if (Game1.IsClient)
            {
                return;
            }

            VolcanoDungeonHell.UpdateLevels(time);
        }
    }

    [HarmonyPatch(typeof(Multiplayer), nameof(Multiplayer.updateRoots))]
    public static class MultiplayerUpdateDungeonRootsPatch
    {
        public static void Postfix(Multiplayer __instance)
        {
            foreach (var level in VolcanoDungeonHell.activeLevels)
            {
                if (level.Root.Value is not null)
                {
                    level.Root.Clock.InterpolationTicks = __instance.interpolationTicks();
                    __instance.updateRoot(level.Root);
                }
            }
        }
    }
}