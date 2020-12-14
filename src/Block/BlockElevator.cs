using System.Text;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;

namespace SimpleElevator
{
    public class BlockElevator : Block
    {
        public int Range
        {
            get
            {
                return Attributes["range"][LastCodePart(1)].AsInt() * (LastCodePart(0) == "temporal" ? 2 : 1);
            }
        }
        public override void OnEntityCollide(IWorldAccessor world, Entity entity, BlockPos pos, BlockFacing facing, Vec3d collideSpeed, bool isImpact)
        {
            base.OnEntityCollide(world, entity, pos, facing, collideSpeed, isImpact);

            if (facing == BlockFacing.UP)
            {
                BlockEntityElevator be = world.BlockAccessor.GetBlockEntity(pos) as BlockEntityElevator;
                if (be == null) return;
                be.OnEntityCollide(entity);
            }
        }

        public override void GetHeldItemInfo(ItemSlot inSlot, StringBuilder dsc, IWorldAccessor world, bool withDebugInfo)
        {
            base.GetHeldItemInfo(inSlot, dsc, world, withDebugInfo);

            dsc.AppendLine(Lang.Get(
                SimpleElevator.MOD_ID + ":heldhelp-material",
                Lang.Get("material-" + LastCodePart(1))
            ));
            dsc.AppendLine(Lang.Get(
                SimpleElevator.MOD_ID + ":heldhelp-range",
                Attributes["range"][LastCodePart(1)].AsInt() * (LastCodePart(0) == "temporal" ? 2 : 1)
            ));
        }
    }
}