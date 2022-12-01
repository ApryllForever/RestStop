using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Locations;
using StardewModdingAPI.Utilities;
using SpaceCore.Events;

using StardewModdingAPI.Enums;



namespace RestStopCode


{
    public class ModEntry : Mod
    {
        public static Mod modInstance;

        internal static IMonitor ModMonitor { get; set; }
        internal new static IModHelper Helper { get; set; }

        public override void Entry(IModHelper helper)
        {
            ModEntry.modInstance = this;
            ModMonitor = Monitor;
            Helper = helper;
            // helper.Events.GameLoop.SaveLoaded += this.OnSaveLoaded;
            // helper.Events.GameLoop.DayStarted += this.OnDayStarted;
            Helper.Events.Specialized.LoadStageChanged += OnLoadStageChanged;
            helper.Events.GameLoop.GameLaunched += OnGameLaunched;
            helper.Events.Input.ButtonPressed += this.OnButtonPressed;

            
            TouchActionProperties.Enable(helper, Monitor);
            //Assets.Load(helper.ModContent);
            //Helper.Events.Content.AssetRequested += OnAssetRequested;
        }

        //private void OnAssetRequested(object? sender, AssetRequestedEventArgs e)
          //  => Assets.ApplyEdits(e);
        private void OnGameLaunched(object sender, EventArgs e)
        {

           TileActionHandler.Initialize(Helper);
           RestStopTileActions.Setup(Helper);
           ExternalAPIs.Initialize(Helper);
           WarpTotem.Initialize(this);
        }

        private void OnLoadStageChanged(object sender, LoadStageChangedEventArgs e)
        {
            if (e.NewStage == LoadStage.CreatedInitialLocations || e.NewStage == LoadStage.SaveAddedLocations)
            {

               // Game1.locations.Add(new VolcanoDungeonHellEntrance(Helper.ModContent));

            }
        }

        private void OnDayStarted(object sender, DayStartedEventArgs e)
        {
            VolcanoDungeonHell.ClearAllLevels();
        }

        private void OnButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            // ignore if player hasn't loaded a save yet
            if (!Context.IsWorldReady)
                return;

        }
    }
}