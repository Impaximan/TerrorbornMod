using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using System.Collections.Generic;

namespace TerrorbornMod
{
    public abstract class BaseWorm : ModNPC
    {
        public override bool CheckActive()
        {
            return !(body || tail);
        }

        public bool head;
		public bool body;
		public bool tail;

		public bool customBodyList = false;
		public List<int> customBodyTypeList = new List<int>();

		public int headType;
		public int bodyType;
		public int tailType;

		public int bodySegmentCount;

		List<int> bodySegments = new List<int>();

		public bool touchingTiles;

        public override void AI()
		{
			touchingTiles = Collision.SolidCollision(NPC.position, NPC.width, NPC.height);
			actualAI();

			if (head)
			{
				NPC.rotation = NPC.velocity.ToRotation() + MathHelper.ToRadians(90f);
				bool hasSegments = false;
				for (int i = 0; i < Main.npc.Length; i++)
				{
					bool isPart = false;
					NPC thingy = Main.npc[i];
					if ((thingy.type == bodyType || customBodyTypeList.Contains(thingy.type)) && bodySegments.Contains(thingy.whoAmI) && thingy.ai[0] == NPC.whoAmI && thingy.active)
					{
						isPart = true;
						thingy.velocity = Vector2.Zero;
						int index = bodySegments.IndexOf(thingy.whoAmI);
						NPC latchedNPC = Main.npc[0];
						if (index == 0)
                        {
							latchedNPC = NPC;
                        }
                        else
                        {
							latchedNPC = Main.npc[bodySegments[index - 1]];
                        }
                        if (latchedNPC == NPC)
						{
							thingy.position += (thingy.Distance(latchedNPC.Center) - (thingy.height / 2 + latchedNPC.height / 2) + NPC.velocity.Length()) * thingy.DirectionTo(latchedNPC.Center);
						}
                        else
                        {
							thingy.position += (thingy.Distance(latchedNPC.Center) - (thingy.height / 2 + latchedNPC.height / 2)) * thingy.DirectionTo(latchedNPC.Center);
						}
                        thingy.rotation = thingy.DirectionTo(latchedNPC.Center).ToRotation() + MathHelper.ToRadians(90f);
						thingy.life = NPC.life;
					}

					if (thingy.type == tailType && thingy.ai[0] == NPC.whoAmI && thingy.active)
					{
						isPart = true;
						hasSegments = true;
						thingy.velocity = Vector2.Zero;
						int index = bodySegments.IndexOf(thingy.whoAmI);
						NPC latchedNPC = Main.npc[bodySegments[bodySegments.Count - 1]];
						thingy.position += (thingy.Distance(latchedNPC.Center) - (thingy.height / 2 + latchedNPC.height / 2)) * thingy.DirectionTo(latchedNPC.Center);
						thingy.rotation = thingy.DirectionTo(latchedNPC.Center).ToRotation() + MathHelper.ToRadians(90f);
						thingy.life = NPC.life;
					}
				}

				if (!hasSegments)
                {
					if (customBodyList)
					{
						for (int i = 0; i < customBodyTypeList.Count; i++)
						{
							int thingy = NPC.NewNPC(Entity.GetSource_NaturalSpawn(), (int)NPC.Center.X, (int)NPC.Center.Y, customBodyTypeList[i]);
							Main.npc[thingy].ai[0] = NPC.whoAmI;
							Main.npc[thingy].ai[1] = i;
							Main.npc[thingy].realLife = NPC.whoAmI;
							Main.npc[thingy].dontCountMe = true;
							bodySegments.Add(thingy);
						}
					}
                    else
					{
						for (int i = 0; i < bodySegmentCount; i++)
						{
							int thingy = NPC.NewNPC(Entity.GetSource_NaturalSpawn(), (int)NPC.Center.X, (int)NPC.Center.Y, bodyType);
							Main.npc[thingy].ai[0] = NPC.whoAmI;
							Main.npc[thingy].ai[1] = i;
							Main.npc[thingy].realLife = NPC.whoAmI;
							Main.npc[thingy].dontCountMe = true;
							bodySegments.Add(thingy);
						}
					}
					int tail = NPC.NewNPC(Entity.GetSource_NaturalSpawn(), (int)NPC.Center.X, (int)NPC.Center.Y, tailType);
					Main.npc[tail].ai[0] = NPC.whoAmI;
					Main.npc[tail].ai[1] = bodySegmentCount;
					Main.npc[tail].realLife = NPC.whoAmI;
					Main.npc[tail].dontCountMe = true;
				}
			}
            else
            {
				NPC headNPC = Main.npc[(int)NPC.ai[0]];
				if (headNPC.type != headType || !headNPC.active)
                {
					NPC.active = false;
				}
				TerrorbornNPC globalNPC = TerrorbornNPC.modNPC(NPC);
				globalNPC.extraWormSegment = true;
			}
        }

        public virtual void actualAI()
        {

		}

		public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
		{
			return head ? (bool?)null : false;
		}
	}
}
