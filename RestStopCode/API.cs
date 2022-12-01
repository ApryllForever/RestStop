using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewModdingAPI;

namespace RestStopCode
{
    internal class ExternalAPIs
    {
        public static IContentPatcherApi CP;
        public static IJsonAssetsApi JA;
        //public static IWearMoreRingsApi MR;
        public static ISpaceCoreApi SC;

        private static IMonitor Monitor { get; set; }
        private static IModHelper Helper { get; set; }

        internal static void Initialize(IModHelper helper)
        {
            Helper = helper;

            CP = Helper.ModRegistry.GetApi<IContentPatcherApi>("Pathoschild.ContentPatcher");
            if (CP is null)
            {
                Monitor.Log("Content Patcher is not installed; Rest Stop requires CP to run. Please install CP and restart your game.");
                return;
            }

            JA = Helper.ModRegistry.GetApi<IJsonAssetsApi>("spacechase0.JsonAssets");
            if (JA == null)
            {

                Monitor.Log("Json Assets API not found. If you end the day and save the game, You are doomed. The shuffle which will result is beyond comprehension.");
            }

           // MR = Helper.ModRegistry.GetApi<IWearMoreRingsApi>("bcmpinc.WearMoreRings");
           // if (MR == null)
           // {
             //   Log.Trace("Wear More Rings API not found. Using base game ring slots only.");
           // }

            SC = Helper.ModRegistry.GetApi<ISpaceCoreApi>("spacechase0.SpaceCore");
            if (SC == null)
            {
                Monitor.Log("SpaceCore API not found. This could lead to issues.");
            }
        }
    }
}