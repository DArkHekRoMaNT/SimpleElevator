using SharedUtils.Extensions;
using System.Collections.Generic;
using Vintagestory.API.Common;

namespace SimpleElevator
{
    public class Core : ModSystem
    {
        public static Config Config { get; private set; }

        public override void Start(ICoreAPI api)
        {
            Config = api.LoadOrCreateConfig(Mod.Info.ModID + ".json", new Config());

            foreach (var prop in typeof(Config.ElevatorRangesConfig).GetProperties())
            {
                int value = (int)prop.GetValue(Config.ElevatorRanges);
                api.World.Config.SetInt(Mod.Info.ModID + ":ElevatorRange" + prop.Name, value);
            }

            api.RegisterBlockClass("BlockElevator", typeof(BlockElevator));
            api.RegisterBlockEntityClass("BlockEntityElevator", typeof(BlockEntityElevator));
        }
    }
}