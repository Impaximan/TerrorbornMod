using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using Terraria.DataStructures;
using TerrorbornMod.NPCs;

namespace TerrorbornMod.TwilightMode.NPCs
{
    class Slimes : TwilightNPCChange
	{
        public override bool ShouldChangeNPC(NPC npc)
		{
			return npc.aiStyle == 1;
        }

		bool slimeAI_Large = false;
		float slimeAI_LargeMult = 1f;

		public override void NewOnSpawn(NPC npc, IEntitySource source)
        {
			if (npc != null)
			{
				if (npc.aiStyle == 1) //Slime AI
				{
					List<int> largeBlacklist = new()
					{
						{ NPCID.LavaSlime },
						{ NPCID.MotherSlime },
						{ NPCID.Slimer },
						{ ModContent.NPCType<TarSludge>() }
					};

					if (source != null && source.Context != null)
					{
						if (Main.rand.NextBool(10) && source.Context != "slimeAI_fromLarge" && !largeBlacklist.Contains(npc.type))
						{
							slimeAI_LargeMult = Main.rand.NextFloat(1.35f, 2f);
							npc.scale *= slimeAI_LargeMult;
							npc.knockBackResist /= slimeAI_LargeMult;
							npc.lifeMax = (int)(npc.lifeMax * slimeAI_LargeMult);
							slimeAI_Large = true;
							npc.life = npc.lifeMax;
						}
					}
					else
					{
						if (Main.rand.NextBool(10) && !largeBlacklist.Contains(npc.type))
						{
							slimeAI_LargeMult = Main.rand.NextFloat(1.35f, 2f);
							npc.scale *= slimeAI_LargeMult;
							npc.knockBackResist /= slimeAI_LargeMult;
							npc.lifeMax = (int)(npc.lifeMax * slimeAI_LargeMult);
							slimeAI_Large = true;
							npc.life = npc.lifeMax;
						}
					}
				}
			}
		}

        public override void NewOnKill(NPC npc)
		{
			if (slimeAI_Large)
			{
				int smaller = NPC.NewNPC(npc.GetSource_Death("slimeAI_fromLarge"), (int)npc.Center.X, (int)npc.Center.Y, npc.type);
				Main.npc[smaller].velocity = npc.velocity - new Vector2(4, 0);
				Main.npc[smaller].scale *= slimeAI_LargeMult / 2f;
				Main.npc[smaller].knockBackResist *= slimeAI_LargeMult * 1.25f;
				Main.npc[smaller].lifeMax = (int)(Main.npc[smaller].lifeMax * slimeAI_LargeMult / 2f);
				Main.npc[smaller].life = Main.npc[smaller].lifeMax;

				smaller = NPC.NewNPC(npc.GetSource_Death("slimeAI_fromLarge"), (int)npc.Center.X, (int)npc.Center.Y, npc.type);
				Main.npc[smaller].velocity = npc.velocity + new Vector2(4, 0);
				Main.npc[smaller].scale *= slimeAI_LargeMult / 2f;
				Main.npc[smaller].knockBackResist *= slimeAI_LargeMult * 1.25f;
				Main.npc[smaller].lifeMax = (int)(Main.npc[smaller].lifeMax * slimeAI_LargeMult / 2f);
				Main.npc[smaller].life = Main.npc[smaller].lifeMax;
			}
		}
	}
}
