using SharedUtils;
using System.Text;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;

namespace SimpleElevator
{
    public class BlockElevator : Block
    {
        public string Material => LastCodePart(1);
        public string Type => LastCodePart(0);
        public int Range
        {
            get
            {
                float mult = Type == "temporal" ? Core.Config.TemporalElevatorMultiplier : 1;
                return (int)(Attributes["range"]?[Material]?.AsInt() * mult);
            }
        }

        public override void OnEntityCollide(IWorldAccessor world, Entity entity, BlockPos pos, BlockFacing facing, Vec3d collideSpeed, bool isImpact)
        {
            base.OnEntityCollide(world, entity, pos, facing, collideSpeed, isImpact);

            if (facing == BlockFacing.UP && entity is EntityAgent agent)
            {
                if (world.BlockAccessor.GetBlockEntity(pos) is BlockEntityElevator be)
                {
                    be.OnEntityCollide(agent);
                }
            }
        }

        public override void GetHeldItemInfo(ItemSlot inSlot, StringBuilder dsc, IWorldAccessor world, bool withDebugInfo)
        {
            base.GetHeldItemInfo(inSlot, dsc, world, withDebugInfo);

            dsc.AppendLine(Lang.Get(ConstantsCore.ModId + ":heldhelp-material", Lang.Get("material-" + Material)));
            dsc.AppendLine(Lang.Get(ConstantsCore.ModId + ":heldhelp-range", Range));
        }
    }
}