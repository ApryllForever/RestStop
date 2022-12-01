using StardewModdingAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewValley;
using StardewValley.Network;
using StardewModdingAPI.Events;
using Microsoft.Xna.Framework;
using StardewValley.Menus;
using StardewModdingAPI.Utilities;

namespace RestStopCode
{
    //This section is heavily inspired by tlitookilakin's Warp Network code, which can be found here:
    //https://github.com/tlitookilakin/WarpNetwork/tree/master/WarpNetwork

    internal static class WarpTotem
    {
        static readonly string Destination = "VolcanoDungeonHell.BaseLocationName";
        static readonly int Dest_X = 0;
        static readonly int Dest_Y = 0;

        public static int Totem = -1;
        static Color color = Color.Indigo;

        static IModHelper Helper;
        static IMonitor Monitor;

        internal static void Initialize(IMod ModInstance)
        {
            Helper = ModInstance.Helper;
            Monitor = ModInstance.Monitor;
            Helper.Events.GameLoop.SaveLoaded += OnSaveLoaded;
            Helper.Events.Input.ButtonPressed += OnButtonPressed;

        }

        private static void OnSaveLoaded(object sender, SaveLoadedEventArgs e)
        {
            Totem = ExternalAPIs.JA.GetObjectId("Warp Totem: Morana");
        }

        private static void OnButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            if (!Context.IsWorldReady)
            {
                return;
            }
            if (e.Button.IsActionButton())
            {
                try
                {
                    if (Game1.player.CurrentItem?.ParentSheetIndex == Totem)
                    {
                        Monitor.Log($"RestStop: Using Warp Totem: Morana");
                        Game1.player.reduceActiveItemByOne();
                        DoTotemWarpEffects(Game1.player, (f) => DirectWarp());
                    }
                }
                catch (Exception ex)
                {
                    Monitor.Log($"Could not find warp totem ID. Error: {ex}");
                }
            }
        }

        public static bool DirectWarp()
        {
            if (!(Game1.getLocationFromName(Destination) is null) || !Game1.isFestival())
            {
                // Don't go if player is at a festival
                if (!(Game1.timeOfDay > 2550))
                {
                    Game1.warpFarmer(VolcanoDungeonHell.BaseLocationName, Dest_X, Dest_Y, flip: false);
                    return true;
                }
                else
                {
                    Monitor.Log("Failed to warp to '" + Destination + "': Festival not ready.");
                    Game1.drawObjectDialogue(Game1.parseText(Helper.Translation.Get("RestStop.WarpFestival")));
                    return false;
                }
            }
            else
            {
                Monitor.Log("Failed to warp to '" + Destination + "': Location not found or player is at festival.");
                Game1.drawObjectDialogue(Game1.parseText(Helper.Translation.Get("RestStop.WarpFail")));
                return false;
            }
        }

        private static void DoTotemWarpEffects(Farmer who, Func<Farmer, bool> action)
        {
            who.jitterStrength = 1f;
            who.currentLocation.playSound("warrior", NetAudio.SoundContext.Default);
            who.faceDirection(2);
            who.canMove = false;
            who.temporarilyInvincible = true;
            who.temporaryInvincibilityTimer = -4000;
            Game1.changeMusicTrack("none", false, Game1.MusicContext.Default);
            who.FarmerSprite.animateOnce(new FarmerSprite.AnimationFrame[2]
            {
                new FarmerSprite.AnimationFrame(57, 2000, false, false,  null, false),
                new FarmerSprite.AnimationFrame( (short) who.FarmerSprite.CurrentFrame, 0, false, false, new AnimatedSprite.endOfAnimationBehavior((f) => {
                    if (action(f))
                    {
                    } else
                    {
                        who.temporarilyInvincible = false;
                        who.temporaryInvincibilityTimer = 0;
                    }
                }), true)
            }, null);
            // reflection
            Multiplayer mp = ModEntry.Helper.Reflection.GetField<Multiplayer>(typeof(Game1), "multiplayer").GetValue();
            // --
            mp.broadcastSprites(who.currentLocation,
            new TemporaryAnimatedSprite(Totem, 9999f, 1, 999, who.Position + new Vector2(0.0f, -96f), false, false, false, 0.0f)
            {
                motion = new Vector2(0.0f, -1f),
                scaleChange = 0.01f,
                alpha = 1f,
                alphaFade = 0.0075f,
                shakeIntensity = 1f,
                initialPosition = who.Position + new Vector2(0.0f, -96f),
                xPeriodic = true,
                xPeriodicLoopTime = 1000f,
                xPeriodicRange = 4f,
                layerDepth = 1f
            },
            new TemporaryAnimatedSprite(Totem, 9999f, 1, 999, who.Position + new Vector2(-64f, -96f), false, false, false, 0.0f)
            {
                motion = new Vector2(0.0f, -0.5f),
                scaleChange = 0.005f,
                scale = 0.5f,
                alpha = 1f,
                alphaFade = 0.0075f,
                shakeIntensity = 1f,
                delayBeforeAnimationStart = 10,
                initialPosition = who.Position + new Vector2(-64f, -96f),
                xPeriodic = true,
                xPeriodicLoopTime = 1000f,
                xPeriodicRange = 4f,
                layerDepth = 0.9999f
            },
            new TemporaryAnimatedSprite(Totem, 9999f, 1, 999, who.Position + new Vector2(64f, -96f), false, false, false, 0.0f)
            {
                motion = new Vector2(0.0f, -0.5f),
                scaleChange = 0.005f,
                scale = 0.5f,
                alpha = 1f,
                alphaFade = 0.0075f,
                delayBeforeAnimationStart = 20,
                shakeIntensity = 1f,
                initialPosition = who.Position + new Vector2(64f, -96f),
                xPeriodic = true,
                xPeriodicLoopTime = 1000f,
                xPeriodicRange = 4f,
                layerDepth = 0.9988f
            });
            Game1.screenGlowOnce(color, false, 0.005f, 0.3f);
            Utility.addSprinklesToLocation(who.currentLocation, who.getTileX(), who.getTileY(), 16, 16, 1300, 20, color, null, true);
        }

    }


}