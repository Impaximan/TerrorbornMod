using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace TerrorbornMod.TwilightMode
{
    abstract class TwilightNPCChange : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        public virtual bool ShouldChangeNPC(NPC npc)
        {
            return false;
        }

        public virtual bool HasNewAI(NPC npc)
        {
            return false;
        }

        public virtual bool RemoveOriginalAI(NPC npc)
        {
            return true;
        }

		public virtual void NewAI(NPC npc)
        {

        }

        public virtual void NewSetDefaults(NPC npc)
        {

        }

        public override void SetDefaults(NPC npc)
        {
            if (!TerrorbornSystem.TwilightMode)
            {
                return;
            }
            if (ShouldChangeNPC(npc))
            {
                NewSetDefaults(npc);
            }
        }

        public virtual void NewOnSpawn(NPC npc, IEntitySource source)
        {

        }

        public override void OnSpawn(NPC npc, IEntitySource source)
        {
            if (!TerrorbornSystem.TwilightMode)
            {
                return;
            }
            NewOnSpawn(npc, source);
        }

        public virtual void NewOnKill(NPC npc)
        {

        }

        public override void OnKill(NPC npc)
        {
            if (!TerrorbornSystem.TwilightMode)
            {
                return;
            }
            NewOnKill(npc);
        }

        public override bool PreAI(NPC npc)
		{
			if (!TerrorbornSystem.TwilightMode)
			{
				return base.PreAI(npc);
			}

            if (ShouldChangeNPC(npc) && HasNewAI(npc))
            {
                NewAI(npc);
                if (RemoveOriginalAI(npc))
                {
                    return false;
                }
            }

			return base.PreAI(npc);
		}
    }
}
