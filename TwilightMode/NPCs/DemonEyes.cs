using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using System.Collections.Generic;

namespace TerrorbornMod.TwilightMode.NPCs
{
    class DemonEyes : TwilightNPCChange
	{
		public override bool InstancePerEntity => true;

        public override bool ShouldChangeNPC(NPC npc)
		{
			List<int> DemonEyeIDs = new List<int>()
					{
						{ 2 },
						{ -43 },
						{ 190 },
						{ -38 },
						{ 191 },
						{ -39 },
						{ 192 },
						{ -40 },
						{ 193 },
						{ -41 },
						{ 194 },
						{ -42 },
						{ 317 },
						{ 318 },
					};

			return DemonEyeIDs.Contains(npc.type);
        }

        public override bool HasNewAI(NPC npc)
        {
            return true;
        }

        public override void NewAI(NPC NPC)
        {
			if (NPC.aiStyle == 2) //Demon Eye AI
			{
				List<int> DemonEyeIDs = new List<int>()
					{
						{ 2 },
						{ -43 },
						{ 190 },
						{ -38 },
						{ 191 },
						{ -39 },
						{ 192 },
						{ -40 },
						{ 193 },
						{ -41 },
						{ 194 },
						{ -42 },
						{ 317 },
						{ 318 },
					};

				if (DemonEyeIDs.Contains(NPC.type))
				{
					NPC.noGravity = true;

					//Bounce off of tiles
					if (!NPC.noTileCollide)
					{
						if (NPC.collideX)
						{
							NPC.velocity.X = NPC.oldVelocity.X * -0.5f;
							if (NPC.direction == -1 && NPC.velocity.X > 0f && NPC.velocity.X < 2f)
							{
								NPC.velocity.X = 2f;
							}
							if (NPC.direction == 1 && NPC.velocity.X < 0f && NPC.velocity.X > -2f)
							{
								NPC.velocity.X = -2f;
							}
						}

						if (NPC.collideY)
						{
							NPC.velocity.Y = NPC.oldVelocity.Y * -0.5f;
							if (NPC.velocity.Y > 0f && NPC.velocity.Y < 1f)
							{
								NPC.velocity.Y = 1f;
							}
							if (NPC.velocity.Y < 0f && NPC.velocity.Y > -1f)
							{
								NPC.velocity.Y = -1f;
							}
						}
					}

					//Self explanatory
					if (NPC.DespawnEncouragement_AIStyle2_FloatingEye_IsDiscouraged(NPC.type, NPC.position, NPC.target))
					{
						NPC.EncourageDespawn(10);
						NPC.directionY = -1;
						if (NPC.velocity.Y > 0f)
						{
							NPC.direction = 1;
						}
						NPC.direction = -1;
						if (NPC.velocity.X > 0f)
						{
							NPC.direction = 1;
						}
					}
					else
					{
						NPC.TargetClosest();
					}

					float num2 = 6f; //Max speed X, original is 4f
					float num3 = 2f; //Max speed Y, original is 1.5f
					num2 *= 1f + (1f - NPC.scale);
					num3 *= 1f + (1f - NPC.scale);
					if (NPC.direction == -1 && NPC.velocity.X > 0f - num2)
					{
						//Accellerate left
						NPC.velocity.X -= 0.15f; //0.1f originally
						if (NPC.velocity.X > num2)
						{
							NPC.velocity.X -= 0.15f; //0.1f originally
						}
						else if (NPC.velocity.X > 0f)
						{
							NPC.velocity.X += 0.05f; //0.05f originally
						}
						if (NPC.velocity.X < 0f - num2)
						{
							NPC.velocity.X = 0f - num2;
						}
					}
					else if (NPC.direction == 1 && NPC.velocity.X < num2)
					{
						//Accellerate right
						NPC.velocity.X += 0.15f; //0.1f originally
						if (NPC.velocity.X < 0f - num2)
						{
							NPC.velocity.X += 0.15f; //0.1f originally
						}
						else if (NPC.velocity.X < 0f)
						{
							NPC.velocity.X -= 0.05f; //0.05f originally
						}
						if (NPC.velocity.X > num2)
						{
							NPC.velocity.X = num2;
						}
					}
					if (NPC.directionY == -1 && NPC.velocity.Y > 0f - num3)
					{
						//Accellerate up
						NPC.velocity.Y -= 0.1f; //0.04f originally
						if (NPC.velocity.Y > num3)
						{
							NPC.velocity.Y -= 0.05f; //0.05f originally
						}
						else if (NPC.velocity.Y > 0f)
						{
							NPC.velocity.Y += 0.03f; //0.03f originally
						}
						if (NPC.velocity.Y < 0f - num3)
						{
							NPC.velocity.Y = 0f - num3;
						}
					}
					else if (NPC.directionY == 1 && NPC.velocity.Y < num3)
					{
						//Accellerate down
						NPC.velocity.Y += 0.04f; //0.04f originally
						if (NPC.velocity.Y < 0f - num3)
						{
							NPC.velocity.Y += 0.05f; //0.05f originally
						}
						else if (NPC.velocity.Y < 0f)
						{
							NPC.velocity.Y -= 0.03f; //0.03f originally
						}
						if (NPC.velocity.Y > num3)
						{
							NPC.velocity.Y = num3;
						}
					}

					if ((NPC.type == NPCID.DemonEye || NPC.type == NPCID.WanderingEye || NPC.type == NPCID.CataractEye || NPC.type == NPCID.SleepyEye || NPC.type == NPCID.DialatedEye || NPC.type == NPCID.GreenEye || NPC.type == NPCID.PurpleEye) && Main.rand.NextBool(40))
					{
						NPC.position += NPC.netOffset;
						int num4 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y + (float)NPC.height * 0.25f), NPC.width, (int)((float)NPC.height * 0.5f), DustID.Blood, NPC.velocity.X, 2f);
						Main.dust[num4].velocity.X *= 0.5f;
						Main.dust[num4].velocity.Y *= 0.1f;
						NPC.position -= NPC.netOffset;
					}
				}
			}
		}
    }
}
