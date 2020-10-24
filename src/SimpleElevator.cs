using System;
using Vintagestory.API.Common;

[assembly: ModInfo("SimpleElevator")]

namespace SimpleElevator
{
    public class SimpleElevator : ModSystem
    {
        public static string MOD_ID = "simpleelevator";
        public static string MOD_SPACE = "SimpleElevator";
        public override void Start(ICoreAPI api)
        {
            base.Start(api);
            api.RegisterBlockClass("BlockElevator", Type.GetType(MOD_SPACE + ".BlockElevator"));
            api.RegisterBlockEntityClass("BlockEntityElevator", Type.GetType(MOD_SPACE + ".BlockEntityElevator"));
        }
    }
}