using System;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;

namespace SimpleElevator
{
    class BlockEntityElevator : BlockEntity
    {

        long lastCollideMs;
        Entity lastEntity;
        int range;
        public override void Initialize(ICoreAPI api)
        {
            base.Initialize(api);

            if (api.Side == EnumAppSide.Server)
            {
                if (Block?.Attributes != null)
                {
                    range = (Block as BlockElevator).Range;
                    RegisterGameTickListener(OnServerGameTick, 50);
                }
            }
        }

        internal void OnEntityCollide(Entity entity)
        {
            if (entity is EntityAgent && lastEntity != entity) lastEntity = entity;
            lastCollideMs = Api.World.ElapsedMilliseconds;
        }
        public void OnServerGameTick(float dt)
        {
            // ! need remove entity is EntityAgent in new version
            if (lastEntity != null && lastEntity is EntityAgent)
            {
                if (Api.World.ElapsedMilliseconds - lastCollideMs > 300)
                {
                    lastEntity = null;
                    return;
                }
                if (Api.Side == EnumAppSide.Server)
                {
                    if ((lastEntity as EntityAgent).Controls.Sneak)
                    {
                        for (int i = 1; i <= range; i++)
                        {
                            if (TryTP(-i)) break;
                        }
                    }
                    else if ((lastEntity as EntityAgent).Controls.Jump)
                    {
                        for (int i = 1; i <= range; i++)
                        {
                            if (TryTP(i)) break;
                        }
                    }
                }
            }
        }

        internal bool TryTP(int shiftY)
        {
            BlockPos newPos = new BlockPos(Pos.X, Pos.Y + shiftY, Pos.Z);
            Block sec = Api.World.BlockAccessor.GetBlock(newPos);
            if (sec.GetType() == Type.GetType(SimpleElevator.MOD_SPACE + ".BlockElevator"))
            {
                lastEntity.TeleportTo(new Vec3d(newPos.X, newPos.Y, newPos.Z).Add(.5, 1, .5));
                return true;
            }
            return false;
        }
    }
}