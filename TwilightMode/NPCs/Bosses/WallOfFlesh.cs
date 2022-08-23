using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using System;

namespace TerrorbornMod.TwilightMode.NPCs.Bosses
{
    class WallOfFleshMouth : TwilightNPCChange
    {
        public override bool HasNewAI(NPC npc)
        {
            return true;
        }

        public override void NewAI(NPC npc)
        {
			WoFMouthAI(npc);
        }

        public override bool ShouldChangeNPC(NPC npc)
        {
            return npc.type == NPCID.WallofFlesh;
		}

		NPC topEye;
		NPC bottomEye;
		int timeAlive = 0;
		float eyeRotOffest = 0f;
		int WoFAttackCounter = 0;
		bool WoFDoingSpread = false;
		public void WoFMouthAI(NPC NPC)
		{
			timeAlive++;
			if (NPC.position.X < 160f || NPC.position.X > (float)((Main.maxTilesX - 10) * 16))
			{
				NPC.active = false;
			}
			if (NPC.localAI[0] == 0f)
			{
				NPC.localAI[0] = 1f;
				Main.wofDrawAreaBottom = -1;
				Main.wofDrawAreaTop = -1;
			}
			NPC.ai[1] += 1f;
			if (NPC.ai[2] == 0f)
			{
				if ((double)NPC.life < (double)NPC.lifeMax * 0.5)
				{
					NPC.ai[1] += 1f;
				}
				if ((double)NPC.life < (double)NPC.lifeMax * 0.2)
				{
					NPC.ai[1] += 1f;
				}
				if (NPC.ai[1] > 2700f)
				{
					NPC.ai[2] = 1f;
				}
			}
			if (NPC.ai[2] > 0f && NPC.ai[1] > 60f)
			{
				int num333 = 3;
				if ((double)NPC.life < (double)NPC.lifeMax * 0.3)
				{
					num333++;
				}
				NPC.ai[2] += 1f;
				NPC.ai[1] = 0f;
				if (NPC.ai[2] > (float)num333)
				{
					NPC.ai[2] = 0f;
				}
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					int num334 = NPC.NewNPC(NPC.GetSource_ReleaseEntity(), (int)(NPC.position.X + (float)(NPC.width / 2)), (int)(NPC.position.Y + (float)(NPC.height / 2) + 20f), 117, 1);
					Main.npc[num334].velocity.X = NPC.direction * 8;
				}
			}
			NPC.localAI[3] += 1f;
			if (NPC.localAI[3] >= (float)(600 + Main.rand.Next(1000)))
			{
				NPC.localAI[3] = -Main.rand.Next(200);
				SoundExtensions.PlaySoundOld(SoundID.NPCDeath10, (int)NPC.position.X, (int)NPC.position.Y, 10);
			}
			Main.wofNPCIndex = NPC.whoAmI;
			int num335 = (int)(NPC.position.X / 16f);
			int num336 = (int)((NPC.position.X + (float)NPC.width) / 16f);
			int num337 = (int)((NPC.position.Y + (float)(NPC.height / 2)) / 16f);
			int num338 = 0;
			int num339 = num337 + 7;
			while (num338 < 15 && num339 > Main.maxTilesY - 200)
			{
				num339++;
				for (int num340 = num335; num340 <= num336; num340++)
				{
					try
					{
						if (WorldGen.SolidTile(num340, num339) || Main.tile[num340, num339].LiquidType > 0)
						{
							num338++;
						}
					}
					catch
					{
						num338 += 15;
					}
				}
			}
			num339 += 4;
			if (Main.wofDrawAreaBottom == -1)
			{
				Main.wofDrawAreaBottom = num339 * 16;
			}
			else if (Main.wofDrawAreaBottom > num339 * 16)
			{
				Main.wofDrawAreaBottom--;
				if (Main.wofDrawAreaBottom < num339 * 16)
				{
					Main.wofDrawAreaBottom = num339 * 16;
				}
			}
			else if (Main.wofDrawAreaBottom < num339 * 16)
			{
				Main.wofDrawAreaBottom++;
				if (Main.wofDrawAreaBottom > num339 * 16)
				{
					Main.wofDrawAreaBottom = num339 * 16;
				}
			}
			num338 = 0;
			num339 = num337 - 7;
			while (num338 < 15 && num339 < Main.maxTilesY - 10)
			{
				num339--;
				for (int num341 = num335; num341 <= num336; num341++)
				{
					try
					{
						if (WorldGen.SolidTile(num341, num339) || Main.tile[num341, num339].LiquidType > 0)
						{
							num338++;
						}
					}
					catch
					{
						num338 += 15;
					}
				}
			}
			num339 -= 4;
			if (Main.wofDrawAreaTop == -1)
			{
				Main.wofDrawAreaTop = num339 * 16;
			}
			else if (Main.wofDrawAreaTop > num339 * 16)
			{
				Main.wofDrawAreaTop--;
				if (Main.wofDrawAreaTop < num339 * 16)
				{
					Main.wofDrawAreaTop = num339 * 16;
				}
			}
			else if (Main.wofDrawAreaTop < num339 * 16)
			{
				Main.wofDrawAreaTop++;
				if (Main.wofDrawAreaTop > num339 * 16)
				{
					Main.wofDrawAreaTop = num339 * 16;
				}
			}
			float num342 = (Main.wofDrawAreaBottom + Main.wofDrawAreaTop) / 2 - NPC.height / 2;
			if (NPC.position.Y > num342 + 1f)
			{
				NPC.velocity.Y = -1f;
			}
			else if (NPC.position.Y < num342 - 1f)
			{
				NPC.velocity.Y = 1f;
			}
			NPC.velocity.Y = 0f;
			int num343 = (Main.maxTilesY - 180) * 16;
			if (num342 < (float)num343)
			{
				num342 = num343;
			}
			NPC.position.Y = num342;

			float num344 = MathHelper.Lerp(6.5f, 4f, (float)NPC.life / (float)NPC.lifeMax);
			if (Main.player[NPC.target].dead || NPC.Distance(Main.player[NPC.target].Center) > 3000f)
			{
				num344 *= 25f;
			}

			if (NPC.velocity.X == 0f)
			{
				NPC.TargetClosest();
				NPC.velocity.X = NPC.direction;
			}
			if (NPC.velocity.X < 0f)
			{
				NPC.velocity.X = -num344;
				NPC.direction = -1;
			}
			else
			{
				NPC.velocity.X = num344;
				NPC.direction = 1;
			}
			NPC.spriteDirection = NPC.direction;
			Vector2 vector37 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
			float num345 = Main.player[NPC.target].position.X + (float)(Main.player[NPC.target].width / 2) - vector37.X;
			float num346 = Main.player[NPC.target].position.Y + (float)(Main.player[NPC.target].height / 2) - vector37.Y;
			float num347 = (float)Math.Sqrt(num345 * num345 + num346 * num346);
			float num348 = num347;
			num345 *= num347;
			num346 *= num347;
			if (NPC.direction > 0)
			{
				if (Main.player[NPC.target].position.X + (float)(Main.player[NPC.target].width / 2) > NPC.position.X + (float)(NPC.width / 2))
				{
					NPC.rotation = (float)Math.Atan2(0f - num346, 0f - num345) + 3.14f;
				}
				else
				{
					NPC.rotation = 0f;
				}
			}
			else if (Main.player[NPC.target].position.X + (float)(Main.player[NPC.target].width / 2) < NPC.position.X + (float)(NPC.width / 2))
			{
				NPC.rotation = (float)Math.Atan2(num346, num345) + 3.14f;
			}
			else
			{
				NPC.rotation = 0f;
			}
			if (Main.expertMode && Main.netMode != NetmodeID.MultiplayerClient)
			{
				int num349 = (int)(1f + (float)NPC.life / (float)NPC.lifeMax * 10f);
				num349 *= num349;
				if (num349 < 400)
				{
					num349 = (num349 * 19 + 400) / 20;
				}
				if (num349 < 60)
				{
					num349 = (num349 * 3 + 60) / 4;
				}
				if (num349 < 20)
				{
					num349 = (num349 + 20) / 2;
				}
				num349 = (int)((double)num349 * 0.7);
				if (Main.rand.NextBool(num349))
				{
					int num350 = 0;
					float[] array = new float[10];
					for (int num351 = 0; num351 < 200; num351++)
					{
						if (num350 < 10 && Main.npc[num351].active && Main.npc[num351].type == NPCID.TheHungry)
						{
							array[num350] = Main.npc[num351].ai[0];
							num350++;
						}
					}
					int maxValue = 1 + num350 * 2;
					if (num350 < 10 && Main.rand.Next(maxValue) <= 1)
					{
						int num352 = -1;
						for (int num353 = 0; num353 < 1000; num353++)
						{
							int num354 = Main.rand.Next(10);
							float num355 = (float)num354 * 0.1f - 0.05f;
							bool flag27 = true;
							for (int num356 = 0; num356 < num350; num356++)
							{
								if (num355 == array[num356])
								{
									flag27 = false;
									break;
								}
							}
							if (flag27)
							{
								num352 = num354;
								break;
							}
						}
						if (num352 >= 0)
						{
							int num357 = NPC.NewNPC(NPC.GetSource_ReleaseEntity(), (int)NPC.position.X, (int)num342, 115, NPC.whoAmI);
							Main.npc[num357].ai[0] = (float)num352 * 0.1f - 0.05f;
						}
					}
				}
			}
			if (NPC.localAI[0] == 1f && Main.netMode != NetmodeID.MultiplayerClient)
			{
				NPC.localAI[0] = 2f;
				num342 = (Main.wofDrawAreaBottom + Main.wofDrawAreaTop) / 2;
				num342 = (num342 + (float)Main.wofDrawAreaTop) / 2f;

				int num358 = NPC.NewNPC(NPC.GetSource_ReleaseEntity(), (int)NPC.position.X, (int)num342, 114, NPC.whoAmI);
				Main.npc[num358].ai[0] = 1f;
				num342 = (Main.wofDrawAreaBottom + Main.wofDrawAreaTop) / 2;
				num342 = (num342 + (float)Main.wofDrawAreaBottom) / 2f;

				topEye = Main.npc[num358];
				topEye.realLife = NPC.whoAmI;

				num358 = NPC.NewNPC(NPC.GetSource_ReleaseEntity(), (int)NPC.position.X, (int)num342, 114, NPC.whoAmI);
				Main.npc[num358].ai[0] = -1f;
				num342 = (Main.wofDrawAreaBottom + Main.wofDrawAreaTop) / 2;
				num342 = (num342 + (float)Main.wofDrawAreaBottom) / 2f;

				bottomEye = Main.npc[num358];
				bottomEye.realLife = NPC.whoAmI;

				for (int num359 = 0; num359 < 11; num359++)
				{
					num358 = NPC.NewNPC(NPC.GetSource_ReleaseEntity(), (int)NPC.position.X, (int)num342, 115, NPC.whoAmI);
					Main.npc[num358].ai[0] = (float)num359 * 0.1f - 0.05f;
				}
			}
			Player player = Main.player[NPC.target];
			topEye.rotation = topEye.DirectionTo(player.Center).ToRotation();
			bottomEye.rotation = bottomEye.DirectionTo(player.Center).ToRotation();
			if (topEye.spriteDirection == -1)
			{
				topEye.rotation += MathHelper.ToRadians(180f);
				bottomEye.rotation += MathHelper.ToRadians(180f);
				topEye.rotation += MathHelper.ToRadians(eyeRotOffest);
				bottomEye.rotation -= MathHelper.ToRadians(eyeRotOffest);
			}
			else
			{
				topEye.rotation -= MathHelper.ToRadians(eyeRotOffest);
				bottomEye.rotation += MathHelper.ToRadians(eyeRotOffest);
			}
			float topEyeRot = topEye.rotation;
			float bottomEyeRot = bottomEye.rotation;
			if (topEye.spriteDirection == -1)
			{
				topEyeRot -= MathHelper.ToRadians(180f);
				bottomEyeRot -= MathHelper.ToRadians(180f);
			}

			if (WoFDoingSpread)
			{
				eyeRotOffest = MathHelper.Lerp(eyeRotOffest, (float)Math.Sin((float)timeAlive / 25f) * 60f, 0.5f);
				WoFAttackCounter++;
				if (WoFAttackCounter > 180)
				{
					WoFAttackCounter = 0;
					WoFDoingSpread = !WoFDoingSpread;
				}
				if (timeAlive % 7 == 6)
				{
					int proj = Projectile.NewProjectile(NPC.GetSource_ReleaseEntity(), topEye.Center, topEyeRot.ToRotationVector2() * 7.5f, ProjectileID.GoldenShowerHostile, 60 / 4, 0f, player.whoAmI);
					Main.projectile[proj].tileCollide = false;
					proj = Projectile.NewProjectile(NPC.GetSource_ReleaseEntity(), bottomEye.Center, bottomEyeRot.ToRotationVector2() * 15f, ProjectileID.CursedFlameHostile, 60 / 4, 0f, player.whoAmI);
					Main.projectile[proj].tileCollide = false;
				}
			}
			else
			{
				eyeRotOffest *= 0.94f;

				WoFAttackCounter++;
				if (WoFAttackCounter > (int)MathHelper.Lerp(120f, 500f, (float)NPC.life / (float)NPC.lifeMax))
				{
					WoFAttackCounter = 0;
					WoFDoingSpread = !WoFDoingSpread;
				}

				int timeBetweenProjectiles = (int)MathHelper.Lerp(45f, 120f, (float)NPC.life / (float)NPC.lifeMax);
				if (timeAlive % timeBetweenProjectiles == timeBetweenProjectiles - 1)
				{
					int proj = Projectile.NewProjectile(NPC.GetSource_ReleaseEntity(), topEye.Center, topEyeRot.ToRotationVector2() * 10f, ProjectileID.EyeLaser, 60 / 4, 0f, player.whoAmI);
					Main.projectile[proj].tileCollide = false;
					proj = Projectile.NewProjectile(NPC.GetSource_ReleaseEntity(), bottomEye.Center, bottomEyeRot.ToRotationVector2() * 10f, ProjectileID.EyeLaser, 60 / 4, 0f, player.whoAmI);
					Main.projectile[proj].tileCollide = false;
				}
			}
		}
	}

    class WallOfFleshEye : TwilightNPCChange
    {
        public override bool HasNewAI(NPC npc)
        {
            return true;
        }

        public override void NewAI(NPC npc)
        {
			WoFEyeAI(npc);
		}

		public void WoFEyeAI(NPC NPC)
		{
			if (Main.wofNPCIndex < 0)
			{
				NPC.active = false;
				return;
			}
			NPC.realLife = Main.wofNPCIndex;
			if (Main.npc[Main.wofNPCIndex].life > 0)
			{
				NPC.life = Main.npc[Main.wofNPCIndex].life;
			}
			NPC.TargetClosest();
			NPC.position.X = Main.npc[Main.wofNPCIndex].position.X;
			NPC.direction = Main.npc[Main.wofNPCIndex].direction;
			NPC.spriteDirection = NPC.direction;
			float num360 = (Main.wofDrawAreaBottom + Main.wofDrawAreaTop) / 2;
			num360 = ((!(NPC.ai[0] > 0f)) ? ((num360 + (float)Main.wofDrawAreaBottom) / 2f) : ((num360 + (float)Main.wofDrawAreaTop) / 2f));
			num360 -= (float)(NPC.height / 2);
			if (NPC.position.Y > num360 + 1f)
			{
				NPC.velocity.Y = -1f;
			}
			else if (NPC.position.Y < num360 - 1f)
			{
				NPC.velocity.Y = 1f;
			}
			else
			{
				NPC.velocity.Y = 0f;
				NPC.position.Y = num360;
			}
			if (NPC.velocity.Y > 5f)
			{
				NPC.velocity.Y = 5f;
			}
			if (NPC.velocity.Y < -5f)
			{
				NPC.velocity.Y = -5f;
			}
			Vector2 vector38 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
			float num361 = Main.player[NPC.target].position.X + (float)(Main.player[NPC.target].width / 2) - vector38.X;
			float num362 = Main.player[NPC.target].position.Y + (float)(Main.player[NPC.target].height / 2) - vector38.Y;
			float num363 = (float)Math.Sqrt(num361 * num361 + num362 * num362);
			float num364 = num363;
			num361 *= num363;
			num362 *= num363;
			bool flag28 = true;
			if (NPC.direction > 0)
			{
				if (Main.player[NPC.target].position.X + (float)(Main.player[NPC.target].width / 2) > NPC.position.X + (float)(NPC.width / 2))
				{

				}
				else
				{

					flag28 = false;
				}
			}
			else if (Main.player[NPC.target].position.X + (float)(Main.player[NPC.target].width / 2) < NPC.position.X + (float)(NPC.width / 2))
			{

			}
			else
			{

				flag28 = false;
			}
			if (Main.netMode == NetmodeID.MultiplayerClient)
			{
				return;
			}
			int num365 = 4;
			NPC.localAI[1] += 1f;
			if ((double)Main.npc[Main.wofNPCIndex].life < (double)Main.npc[Main.wofNPCIndex].lifeMax * 0.75)
			{
				NPC.localAI[1] += 1f;
				num365++;
			}
			if ((double)Main.npc[Main.wofNPCIndex].life < (double)Main.npc[Main.wofNPCIndex].lifeMax * 0.5)
			{
				NPC.localAI[1] += 1f;
				num365++;
			}
			if ((double)Main.npc[Main.wofNPCIndex].life < (double)Main.npc[Main.wofNPCIndex].lifeMax * 0.25)
			{
				NPC.localAI[1] += 1f;
				num365 += 2;
			}
			if ((double)Main.npc[Main.wofNPCIndex].life < (double)Main.npc[Main.wofNPCIndex].lifeMax * 0.1)
			{
				NPC.localAI[1] += 2f;
				num365 += 3;
			}
			if (Main.expertMode)
			{
				NPC.localAI[1] += 0.5f;
				num365++;
				if ((double)Main.npc[Main.wofNPCIndex].life < (double)Main.npc[Main.wofNPCIndex].lifeMax * 0.1)
				{
					NPC.localAI[1] += 2f;
					num365 += 3;
				}
			}
			if (NPC.localAI[2] == 0f)
			{
				if (NPC.localAI[1] > 600f)
				{
					NPC.localAI[2] = 1f;
					NPC.localAI[1] = 0f;
				}
			}
			else
			{
				if (!(NPC.localAI[1] > 45f) || !Collision.CanHit(NPC.position, NPC.width, NPC.height, Main.player[NPC.target].position, Main.player[NPC.target].width, Main.player[NPC.target].height))
				{
					return;
				}
				NPC.localAI[1] = 0f;
				NPC.localAI[2] += 1f;
				if (NPC.localAI[2] >= (float)num365)
				{
					NPC.localAI[2] = 0f;
				}
				if (flag28)
				{
					float num366 = 9f;
					int num367 = 11;
					int num368 = 83;
					if ((double)Main.npc[Main.wofNPCIndex].life < (double)Main.npc[Main.wofNPCIndex].lifeMax * 0.5)
					{
						num367++;
						num366 += 1f;
					}
					if ((double)Main.npc[Main.wofNPCIndex].life < (double)Main.npc[Main.wofNPCIndex].lifeMax * 0.25)
					{
						num367++;
						num366 += 1f;
					}
					if ((double)Main.npc[Main.wofNPCIndex].life < (double)Main.npc[Main.wofNPCIndex].lifeMax * 0.1)
					{
						num367 += 2;
						num366 += 2f;
					}
					vector38 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
					num361 = Main.player[NPC.target].position.X + (float)Main.player[NPC.target].width * 0.5f - vector38.X;
					num362 = Main.player[NPC.target].position.Y + (float)Main.player[NPC.target].height * 0.5f - vector38.Y;
					num363 = (float)Math.Sqrt(num361 * num361 + num362 * num362);
					num363 = num366 / num363;
					num361 *= num363;
					num362 *= num363;
					vector38.X += num361;
					vector38.Y += num362;
				}
			}
		}

		public override bool ShouldChangeNPC(NPC npc)
        {
            return npc.type == NPCID.WallofFleshEye;
        }
    }
}
