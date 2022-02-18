using SharedUtils;
using SharedUtils.Extensions;
using System;
using System.Collections.Generic;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;

namespace SimpleElevator
{
    class BlockEntityElevator : BlockEntity
    {
        public int Range => (Block as BlockElevator)?.Range ?? 0;

        private readonly List<CollidedEntity> _collidedEntities = new List<CollidedEntity>();

        public override void Initialize(ICoreAPI api)
        {
            base.Initialize(api);

            if (api.Side == EnumAppSide.Server)
            {
                if (Range > 0)
                {
                    RegisterGameTickListener(OnServerGameTick, 100);
                }
            }
        }

        public void OnEntityCollide(EntityAgent entity)
        {
            if (entity == null) return;

            bool exists = false;

            foreach (var ce in _collidedEntities)
            {
                if (ce.Entity.EntityId == entity.EntityId)
                {
                    exists = true;
                    ce.LastTime = Api.World.ElapsedMilliseconds;
                }
            }

            if (!exists)
            {
                _collidedEntities.Add(new CollidedEntity()
                {
                    Entity = entity,
                    LastTime = Api.World.ElapsedMilliseconds
                });
            }
        }

        public void OnServerGameTick(float dt)
        {
            var toRemove = new List<CollidedEntity>();

            foreach (var ce in _collidedEntities)
            {
                if (Api.World.ElapsedMilliseconds - ce.LastTime > 300)
                {
                    toRemove.Add(ce);
                    continue;
                }

                if (ce.Entity.Controls.Sneak)
                {
                    TriggerTeleport(ce.Entity, Range, BlockFacing.DOWN);
                }
                else if (ce.Entity.Controls.Jump)
                {
                    TriggerTeleport(ce.Entity, Range, BlockFacing.UP);
                }
            }

            foreach (var ce in toRemove)
            {
                _collidedEntities.Remove(ce);
            }
        }

        private void TriggerTeleport(EntityAgent entity, int range, BlockFacing dir)
        {
            BlockPos nextPos = Pos.Copy();
            for (int i = 1; i <= range; i++)
            {
                nextPos.Offset(dir);
                Block nextBlock = Api.World.BlockAccessor.GetBlock(nextPos);
                if (nextBlock is BlockElevator)
                {
                    if (CheckArea(nextPos.Copy()))
                    {
                        entity.TeleportToDouble(nextPos.X + 0.5, nextPos.Y + 1, nextPos.Z + 0.5);
                    }
                    else
                    {
                        entity.SendMessage(Lang.Get(ConstantsCore.ModId + ":elevator-notenoughspace"));
                    }
                    break;
                }
            }
        }

        private bool CheckArea(BlockPos nextPos)
        {
            int id1 = Api.World.BlockAccessor.GetBlockId(nextPos.Add(0, 1, 0));
            int id2 = Api.World.BlockAccessor.GetBlockId(nextPos.Add(0, 1, 0));

            return id1 == 0 && id2 == 0; // is air
        }
    }

    public class CollidedEntity
    {
        public EntityAgent Entity { get; set; }
        public long LastTime { get; set; }
    }
}