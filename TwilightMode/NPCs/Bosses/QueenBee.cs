using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using System;
using Terraria.Audio;
using Terraria.DataStructures;
using TerrorbornMod.Utils;
using TerrorbornMod.NPCs;
using TerrorbornMod.NPCs.Bosses;
using TerrorbornMod.NPCs.Bosses.TidalTitan;

namespace TerrorbornMod.TwilightMode.NPCs.Bosses
{
    class QueenBee : TwilightNPCChange
    {
        public override bool HasNewAI(NPC npc)
        {
            return true;
        }

        public override void NewAI(NPC npc)
        {
			QueenBeeAI(npc);
        }

        public override bool ShouldChangeNPC(NPC npc)
        {
            return npc.type == NPCID.QueenBee;
		}

		int queenBeeLaserCounter = 0;
		public void QueenBeeAI(NPC NPC)
		{
			int num598 = 0;
			for (int num599 = 0; num599 < 255; num599++)
			{
				if (Main.player[num599].active && !Main.player[num599].dead && (NPC.Center - Main.player[num599].Center).Length() < 1000f)
				{
					num598++;
				}
			}
			if (Main.expertMode)
			{
				int num600 = (int)(20f * (1f - (float)NPC.life / (float)NPC.lifeMax));
				NPC.defense = NPC.defDefense + num600;
			}
			if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
			{
				NPC.TargetClosest();
			}
			if (Main.player[NPC.target].dead && Main.expertMode)
			{
				if ((double)NPC.position.Y < Main.worldSurface * 16.0 + 2000.0)
				{
					NPC.velocity.Y += 0.04f;
				}
				if (NPC.position.X < (float)(Main.maxTilesX * 8))
				{
					NPC.velocity.X -= 0.04f;
				}
				else
				{
					NPC.velocity.X += 0.04f;
				}
				if (NPC.timeLeft > 10)
				{
					NPC.timeLeft = 10;
				}
			}
			else if (NPC.ai[0] == -1f)
			{
				if (Main.netMode == NetmodeID.MultiplayerClient)
				{
					return;
				}
				float num601 = NPC.ai[1];
				int num602;
				do
				{
					num602 = Main.rand.Next(3);
					switch (num602)
					{
						case 1:
							num602 = 2;
							break;
						case 2:
							num602 = 3;
							break;
					}
				}
				while ((float)num602 == num601);
				NPC.ai[0] = num602;
				NPC.ai[1] = 0f;
				NPC.ai[2] = 0f;
			}
			else if (NPC.ai[0] == 0f)
			{
				int num603 = 2;
				if (Main.expertMode)
				{
					if (NPC.life < NPC.lifeMax / 2)
					{
						num603++;
					}
					if (NPC.life < NPC.lifeMax / 3)
					{
						num603++;
					}
					if (NPC.life < NPC.lifeMax / 5)
					{
						num603++;
					}
				}
				if (NPC.ai[1] > (float)(2 * num603) && NPC.ai[1] % 2f == 0f)
				{
					NPC.ai[0] = -1f;
					NPC.ai[1] = 0f;
					NPC.ai[2] = 0f;
					return;
				}
				if (NPC.ai[1] % 2f == 0f)
				{
					NPC.TargetClosest();
					if (Math.Abs(NPC.position.Y + (float)(NPC.height / 2) - (Main.player[NPC.target].position.Y + (float)(Main.player[NPC.target].height / 2))) < 20f)
					{
						NPC.localAI[0] = 1f;
						NPC.ai[1] += 1f;
						NPC.ai[2] = 0f;
						float num604 = 22f;
						Vector2 vector76 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
						float num605 = Main.player[NPC.target].position.X + (float)(Main.player[NPC.target].width / 2) - vector76.X;
						float num606 = Main.player[NPC.target].position.Y + (float)(Main.player[NPC.target].height / 2) - vector76.Y;
						float num607 = (float)Math.Sqrt(num605 * num605 + num606 * num606);
						num607 = num604 / num607;
						NPC.velocity.X = num605 * num607;
						NPC.velocity.Y = num606 * num607;
						NPC.velocity.Y += Main.player[NPC.target].velocity.Y;
						NPC.spriteDirection = NPC.direction;
						SoundExtensions.PlaySoundOld(SoundID.Roar, (int)NPC.position.X, (int)NPC.position.Y, 0);
						SoundExtensions.PlaySoundOld(SoundID.Item33, Main.player[NPC.target].Center);
						Projectile proj = Main.projectile[Projectile.NewProjectile(NPC.GetSource_ReleaseEntity(), Main.player[NPC.target].Center + new Vector2(Main.player[NPC.target].velocity.X * 67f, 0) + new Vector2(0, 1000), new Vector2(0, -15 + Main.player[NPC.target].velocity.Y), ModContent.ProjectileType<QueenBeeLaser>(), 11, 0)];
						proj.tileCollide = false;
						proj.timeLeft = 300;
						proj = Main.projectile[Projectile.NewProjectile(NPC.GetSource_ReleaseEntity(), Main.player[NPC.target].Center + new Vector2(Main.player[NPC.target].velocity.X * 67f, 0) + new Vector2(0, -1000), new Vector2(0, 15 + Main.player[NPC.target].velocity.Y), ModContent.ProjectileType<QueenBeeLaser>(), 11, 0)];
						proj.tileCollide = false;
						proj.timeLeft = 300;
						return;
					}
					NPC.localAI[0] = 0f;
					float num608 = 12f;
					float num609 = 0.15f;
					if (Main.expertMode)
					{
						if ((double)NPC.life < (double)NPC.lifeMax * 0.75)
						{
							num608 += 1f;
							num609 += 0.05f;
						}
						if ((double)NPC.life < (double)NPC.lifeMax * 0.5)
						{
							num608 += 1f;
							num609 += 0.05f;
						}
						if ((double)NPC.life < (double)NPC.lifeMax * 0.25)
						{
							num608 += 2f;
							num609 += 0.05f;
						}
						if ((double)NPC.life < (double)NPC.lifeMax * 0.1)
						{
							num608 += 2f;
							num609 += 0.1f;
						}
					}
					if (NPC.position.Y + (float)(NPC.height / 2) < Main.player[NPC.target].position.Y + (float)(Main.player[NPC.target].height / 2))
					{
						NPC.velocity.Y += num609;
					}
					else
					{
						NPC.velocity.Y -= num609;
					}
					if (NPC.velocity.Y < -12f)
					{
						NPC.velocity.Y = 0f - num608;
					}
					if (NPC.velocity.Y > 12f)
					{
						NPC.velocity.Y = num608;
					}
					if (Math.Abs(NPC.position.X + (float)(NPC.width / 2) - (Main.player[NPC.target].position.X + (float)(Main.player[NPC.target].width / 2))) > 600f)
					{
						NPC.velocity.X += 0.15f * (float)NPC.direction;
					}
					else if (Math.Abs(NPC.position.X + (float)(NPC.width / 2) - (Main.player[NPC.target].position.X + (float)(Main.player[NPC.target].width / 2))) < 300f)
					{
						NPC.velocity.X -= 0.15f * (float)NPC.direction;
					}
					else
					{
						NPC.velocity.X *= 0.8f;
					}
					if (NPC.velocity.X < -20f)
					{
						NPC.velocity.X = -20f;
					}
					if (NPC.velocity.X > 20f)
					{
						NPC.velocity.X = 20f;
					}
					NPC.spriteDirection = NPC.direction;
					return;
				}
				if (NPC.velocity.X < 0f)
				{
					NPC.direction = -1;
				}
				else
				{
					NPC.direction = 1;
				}
				NPC.spriteDirection = NPC.direction;
				int num610 = 600;
				if (Main.expertMode)
				{
					if ((double)NPC.life < (double)NPC.lifeMax * 0.1)
					{
						num610 = 300;
					}
					else if ((double)NPC.life < (double)NPC.lifeMax * 0.25)
					{
						num610 = 450;
					}
					else if ((double)NPC.life < (double)NPC.lifeMax * 0.5)
					{
						num610 = 500;
					}
					else if ((double)NPC.life < (double)NPC.lifeMax * 0.75)
					{
						num610 = 550;
					}
				}
				int num611 = 1;
				if (NPC.position.X + (float)(NPC.width / 2) < Main.player[NPC.target].position.X + (float)(Main.player[NPC.target].width / 2))
				{
					num611 = -1;
				}
				if (NPC.direction == num611 && Math.Abs(NPC.position.X + (float)(NPC.width / 2) - (Main.player[NPC.target].position.X + (float)(Main.player[NPC.target].width / 2))) > (float)num610)
				{
					NPC.ai[2] = 1f;
				}
				if (NPC.ai[2] == 1f)
				{
					NPC.TargetClosest();
					NPC.spriteDirection = NPC.direction;
					NPC.localAI[0] = 0f;
					NPC.velocity *= 0.9f;
					float num612 = 0.1f;
					if (Main.expertMode)
					{
						if (NPC.life < NPC.lifeMax / 2)
						{
							NPC.velocity *= 0.9f;
							num612 += 0.05f;
						}
						if (NPC.life < NPC.lifeMax / 3)
						{
							NPC.velocity *= 0.9f;
							num612 += 0.05f;
						}
						if (NPC.life < NPC.lifeMax / 5)
						{
							NPC.velocity *= 0.9f;
							num612 += 0.05f;
						}
					}
					if (Math.Abs(NPC.velocity.X) + Math.Abs(NPC.velocity.Y) < num612)
					{
						NPC.ai[2] = 0f;
						NPC.ai[1] += 1f;
					}
				}
				else
				{
					NPC.localAI[0] = 1f;
				}
			}
			else if (NPC.ai[0] == 2f)
			{
				NPC.TargetClosest();
				NPC.spriteDirection = NPC.direction;
				float num613 = 12f;
				float num614 = 0.07f;
				if (Main.expertMode)
				{
					num614 = 0.1f;
				}
				Vector2 vector77 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
				float num615 = Main.player[NPC.target].position.X + (float)(Main.player[NPC.target].width / 2) - vector77.X;
				float num616 = Main.player[NPC.target].position.Y + (float)(Main.player[NPC.target].height / 2) - 200f - vector77.Y;
				float num617 = (float)Math.Sqrt(num615 * num615 + num616 * num616);
				if (num617 < 200f)
				{
					NPC.ai[0] = 1f;
					NPC.ai[1] = 0f;
					NPC.netUpdate = true;
					return;
				}
				num617 = num613 / num617;
				if (NPC.velocity.X < num615)
				{
					NPC.velocity.X += num614;
					if (NPC.velocity.X < 0f && num615 > 0f)
					{
						NPC.velocity.X += num614;
					}
				}
				else if (NPC.velocity.X > num615)
				{
					NPC.velocity.X -= num614;
					if (NPC.velocity.X > 0f && num615 < 0f)
					{
						NPC.velocity.X -= num614;
					}
				}
				if (NPC.velocity.Y < num616)
				{
					NPC.velocity.Y += num614;
					if (NPC.velocity.Y < 0f && num616 > 0f)
					{
						NPC.velocity.Y += num614;
					}
				}
				else if (NPC.velocity.Y > num616)
				{
					NPC.velocity.Y -= num614;
					if (NPC.velocity.Y > 0f && num616 < 0f)
					{
						NPC.velocity.Y -= num614;
					}
				}
			}
			else if (NPC.ai[0] == 1f)
			{
				NPC.localAI[0] = 0f;
				NPC.TargetClosest();
				Vector2 vector78 = new Vector2(NPC.position.X + (float)(NPC.width / 2) + (float)(Main.rand.Next(20) * NPC.direction), NPC.position.Y + (float)NPC.height * 0.8f);
				Vector2 vector79 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
				float num618 = Main.player[NPC.target].position.X + (float)(Main.player[NPC.target].width / 2) - vector79.X;
				float num619 = Main.player[NPC.target].position.Y + (float)(Main.player[NPC.target].height / 2) - vector79.Y;
				float num620 = (float)Math.Sqrt(num618 * num618 + num619 * num619);

				NPC.ai[1] += 1f;
				if (Main.expertMode)
				{
					NPC.ai[1] += num598 / 2;
					if ((double)NPC.life < (double)NPC.lifeMax * 0.75)
					{
						NPC.ai[1] += 0.25f;
					}
					if ((double)NPC.life < (double)NPC.lifeMax * 0.5)
					{
						NPC.ai[1] += 0.25f;
					}
					if ((double)NPC.life < (double)NPC.lifeMax * 0.25)
					{
						NPC.ai[1] += 0.25f;
					}
					if ((double)NPC.life < (double)NPC.lifeMax * 0.1)
					{
						NPC.ai[1] += 0.25f;
					}
				}

				queenBeeLaserCounter++;
				if (queenBeeLaserCounter % 60 == 59)
				{
					Player player = Main.player[NPC.target];
					float speed = 10f;
					Vector2 velocity = NPC.DirectionTo(player.Center + (player.velocity * NPC.Distance(player.Center) / speed)) * speed;
					Projectile.NewProjectile(NPC.GetSource_ReleaseEntity(), NPC.Center, velocity, ModContent.ProjectileType<QueenBeeLaser>(), 10, 0f);
				}

				bool flag36 = false;
				if (NPC.ai[1] > 40f)
				{
					NPC.ai[1] = 0f;
					NPC.ai[2]++;
					flag36 = true;
				}
				if (Collision.CanHit(vector78, 1, 1, Main.player[NPC.target].position, Main.player[NPC.target].width, Main.player[NPC.target].height) && flag36)
				{
					SoundExtensions.PlaySoundOld(SoundID.NPCHit1, (int)NPC.position.X, (int)NPC.position.Y);
					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						int num621 = Main.rand.Next(210, 212);
						int num622 = NPC.NewNPC(NPC.GetSource_ReleaseEntity(), (int)vector78.X, (int)vector78.Y, num621);
						Main.npc[num622].velocity.X = (float)Main.rand.Next(-200, 201) * 0.002f;
						Main.npc[num622].velocity.Y = (float)Main.rand.Next(-200, 201) * 0.002f;
						Main.npc[num622].localAI[0] = 60f;
						Main.npc[num622].netUpdate = true;
					}
				}
				if (num620 > 400f || !Collision.CanHit(new Vector2(vector78.X, vector78.Y - 30f), 1, 1, Main.player[NPC.target].position, Main.player[NPC.target].width, Main.player[NPC.target].height))
				{
					float num623 = 14f;
					float num624 = 0.1f;
					vector79 = vector78;
					num618 = Main.player[NPC.target].position.X + (float)(Main.player[NPC.target].width / 2) - vector79.X;
					num619 = Main.player[NPC.target].position.Y + (float)(Main.player[NPC.target].height / 2) - vector79.Y;
					num620 = (float)Math.Sqrt(num618 * num618 + num619 * num619);
					num620 = num623 / num620;
					if (NPC.velocity.X < num618)
					{
						NPC.velocity.X += num624;
						if (NPC.velocity.X < 0f && num618 > 0f)
						{
							NPC.velocity.X += num624;
						}
					}
					else if (NPC.velocity.X > num618)
					{
						NPC.velocity.X -= num624;
						if (NPC.velocity.X > 0f && num618 < 0f)
						{
							NPC.velocity.X -= num624;
						}
					}
					if (NPC.velocity.Y < num619)
					{
						NPC.velocity.Y += num624;
						if (NPC.velocity.Y < 0f && num619 > 0f)
						{
							NPC.velocity.Y += num624;
						}
					}
					else if (NPC.velocity.Y > num619)
					{
						NPC.velocity.Y -= num624;
						if (NPC.velocity.Y > 0f && num619 < 0f)
						{
							NPC.velocity.Y -= num624;
						}
					}
				}
				else
				{
					NPC.velocity *= 0.9f;
				}
				NPC.spriteDirection = NPC.direction;
				if (NPC.ai[2] > 5f)
				{
					NPC.ai[0] = -1f;
					NPC.ai[1] = 1f;
					NPC.netUpdate = true;
				}
			}
			else
			{
				if (NPC.ai[0] != 3f)
				{
					return;
				}
				float num625 = 4f;
				float num626 = 0.05f;
				if (Main.expertMode)
				{
					num626 = 0.075f;
					num625 = 6f;
				}
				Vector2 vector80 = new Vector2(NPC.position.X + (float)(NPC.width / 2) + (float)(Main.rand.Next(20) * NPC.direction), NPC.position.Y + (float)NPC.height * 0.8f);
				Vector2 vector81 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
				float num627 = Main.player[NPC.target].position.X + (float)(Main.player[NPC.target].width / 2) - vector81.X;
				float num628 = Main.player[NPC.target].position.Y + (float)(Main.player[NPC.target].height / 2) - 300f - vector81.Y;
				float num629 = (float)Math.Sqrt(num627 * num627 + num628 * num628);
				NPC.ai[1] += 1f;
				bool flag37 = false;
				if (NPC.ai[1] % 120f == 119f)
				{
					//SoundExtensions.PlaySoundOld(SoundID.Item33, Main.player[NPC.target].Center);
					//Projectile proj = Main.projectile[Projectile.NewProjectile(Main.player[NPC.target].Center + new Vector2(1000, 0), new Vector2(-15, 0), ModContent.ProjectileType<QueenBeeLaser>(), 11, 0)];
					//proj.tileCollide = false;
					//proj.timeLeft = 300;
					//proj = Main.projectile[Projectile.NewProjectile(Main.player[NPC.target].Center + new Vector2(-1000, 0), new Vector2(15, 0), ModContent.ProjectileType<QueenBeeLaser>(), 11, 0)];
					//proj.tileCollide = false;
					//proj.timeLeft = 300;
				}
				if (NPC.life < NPC.lifeMax / 3)
				{
					if (NPC.ai[1] % 12f == 11f)
					{
						flag37 = true;
					}
				}
				else if (NPC.life < NPC.lifeMax / 2)
				{
					if (NPC.ai[1] % 20f == 19f)
					{
						flag37 = true;
					}
				}
				else if (NPC.ai[1] % 25f == 24f)
				{
					flag37 = true;
				}
				if (flag37 && NPC.position.Y + (float)NPC.height < Main.player[NPC.target].position.Y)
				{
					SoundExtensions.PlaySoundOld(SoundID.Item17, NPC.position);
					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						float num630 = 15f;
						if ((double)NPC.life < (double)NPC.lifeMax * 0.1)
						{
							num630 += 3f;
						}
						float num631 = Main.player[NPC.target].position.X + (float)Main.player[NPC.target].width * 0.5f - vector80.X;
						float num632 = Main.player[NPC.target].position.Y + (float)Main.player[NPC.target].height * 0.5f - vector80.Y;
						float num633 = (float)Math.Sqrt(num631 * num631 + num632 * num632);
						num633 = num630 / num633;
						num631 *= num633;
						num632 *= num633;
						int num634 = 11;
						int num635 = ProjectileID.QueenBeeStinger;
						float lightAmount = 0.5f;
						float rotationAmount = 35;//adds 2 extra Projectiles with a set offset
						int num636 = Projectile.NewProjectile(NPC.GetSource_ReleaseEntity(), vector80.X, vector80.Y, num631, num632, num635, num634, 0f, Main.myPlayer);
						Main.projectile[num636].timeLeft = 300;
						Main.projectile[num636].light = lightAmount;
						Main.projectile[num636].tileCollide = false;
						num636 = Projectile.NewProjectile(NPC.GetSource_ReleaseEntity(), vector80.X, vector80.Y, num631, num632, num635, num634, 0f, Main.myPlayer);
						Main.projectile[num636].timeLeft = 300;
						Main.projectile[num636].light = lightAmount;
						Main.projectile[num636].tileCollide = false;
						Main.projectile[num636].velocity = Main.projectile[num636].velocity.RotatedBy(MathHelper.ToRadians(rotationAmount));
						num636 = Projectile.NewProjectile(NPC.GetSource_ReleaseEntity(), vector80.X, vector80.Y, num631, num632, num635, num634, 0f, Main.myPlayer);
						Main.projectile[num636].timeLeft = 300;
						Main.projectile[num636].light = lightAmount;
						Main.projectile[num636].tileCollide = false;
						Main.projectile[num636].velocity = Main.projectile[num636].velocity.RotatedBy(MathHelper.ToRadians(-rotationAmount));
					}
				}
				if (!Collision.CanHit(new Vector2(vector80.X, vector80.Y - 30f), 1, 1, Main.player[NPC.target].position, Main.player[NPC.target].width, Main.player[NPC.target].height))
				{
					num625 = 14f;
					num626 = 0.1f;
					vector81 = vector80;
					num627 = Main.player[NPC.target].position.X + (float)(Main.player[NPC.target].width / 2) - vector81.X;
					num628 = Main.player[NPC.target].position.Y + (float)(Main.player[NPC.target].height / 2) - vector81.Y;
					num629 = (float)Math.Sqrt(num627 * num627 + num628 * num628);
					num629 = num625 / num629;
					if (NPC.velocity.X < num627)
					{
						NPC.velocity.X += num626;
						if (NPC.velocity.X < 0f && num627 > 0f)
						{
							NPC.velocity.X += num626;
						}
					}
					else if (NPC.velocity.X > num627)
					{
						NPC.velocity.X -= num626;
						if (NPC.velocity.X > 0f && num627 < 0f)
						{
							NPC.velocity.X -= num626;
						}
					}
					if (NPC.velocity.Y < num628)
					{
						NPC.velocity.Y += num626;
						if (NPC.velocity.Y < 0f && num628 > 0f)
						{
							NPC.velocity.Y += num626;
						}
					}
					else if (NPC.velocity.Y > num628)
					{
						NPC.velocity.Y -= num626;
						if (NPC.velocity.Y > 0f && num628 < 0f)
						{
							NPC.velocity.Y -= num626;
						}
					}
				}
				else if (num629 > 100f)
				{
					NPC.TargetClosest();
					NPC.spriteDirection = NPC.direction;
					num629 = num625 / num629;
					if (NPC.velocity.X < num627)
					{
						NPC.velocity.X += num626;
						if (NPC.velocity.X < 0f && num627 > 0f)
						{
							NPC.velocity.X += num626 * 2f;
						}
					}
					else if (NPC.velocity.X > num627)
					{
						NPC.velocity.X -= num626;
						if (NPC.velocity.X > 0f && num627 < 0f)
						{
							NPC.velocity.X -= num626 * 2f;
						}
					}
					if (NPC.velocity.Y < num628)
					{
						NPC.velocity.Y += num626;
						if (NPC.velocity.Y < 0f && num628 > 0f)
						{
							NPC.velocity.Y += num626 * 2f;
						}
					}
					else if (NPC.velocity.Y > num628)
					{
						NPC.velocity.Y -= num626;
						if (NPC.velocity.Y > 0f && num628 < 0f)
						{
							NPC.velocity.Y -= num626 * 2f;
						}
					}
				}
				if (NPC.ai[1] > 800f)
				{
					NPC.ai[0] = -1f;
					NPC.ai[1] = 3f;
					NPC.netUpdate = true;
				}
			}
		}
	}

	class QueenBeeLaser : ModProjectile
	{
		public override string Texture => "TerrorbornMod/WhitePixel";

		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[this.Projectile.type] = 10;
			ProjectileID.Sets.TrailingMode[this.Projectile.type] = 1;
		}

		public override void SetDefaults()
		{
			Projectile.width = 12;
			Projectile.height = 12;
			Projectile.friendly = false;
			Projectile.hostile = true;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = false;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 600;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			BezierCurve bezier = new BezierCurve();
			bezier.Controls.Clear();
			foreach (Vector2 pos in Projectile.oldPos)
			{
				if (pos != Vector2.Zero && pos != null)
				{
					bezier.Controls.Add(pos);
				}
			}

			if (bezier.Controls.Count > 1)
			{
				List<Vector2> positions = bezier.GetPoints(15);
				for (int i = 0; i < positions.Count; i++)
				{
					float mult = (float)(positions.Count - i) / (float)positions.Count;
					Vector2 drawPos = positions[i] - Main.screenPosition + Projectile.Size / 2;
					Color color = Projectile.GetAlpha(Color.Lerp(Color.DarkSlateBlue, Color.Yellow, mult)) * mult;
					Utils.Graphics.DrawGlow_1(Main.spriteBatch, drawPos, (int)(25f * mult), color);
				}
			}
			return false;
		}

		public override void ModifyDamageHitbox(ref Rectangle hitbox)
		{
			int newDimensions = 15;
			Rectangle oldHitbox = hitbox;
			hitbox.Width = newDimensions;
			hitbox.Height = newDimensions;
			hitbox.X = oldHitbox.X - newDimensions / 2;
			hitbox.Y = oldHitbox.Y - newDimensions / 2;
		}

		public override void AI()
		{
			Projectile.rotation = Projectile.velocity.ToRotation();
		}
	}
}
