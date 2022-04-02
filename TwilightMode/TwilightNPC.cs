using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using System;

namespace TerrorbornMod.TwilightMode
{
    public class TwilightNPC : GlobalNPC
	{
		public override bool InstancePerEntity => true;
		public bool twilight = false;
		bool start = true;

		public static TwilightNPC modNPC(NPC NPC)
		{
			if (NPC != null)
			{
				return NPC.GetGlobalNPC<TwilightNPC>();
			}
			else
			{
				return null;
			}
		}

        public override void OnHitByProjectile(NPC NPC, Projectile Projectile, int damage, float knockback, bool crit)
		{
			if (NPC.type == NPCID.CultistBossClone)
			{
				NPC NPC2 = Main.npc[(int)NPC.ai[3]];
				if (modNPC(NPC2).twilight)
				{
					int healAmount = NPC2.lifeMax / 15 + ((NPC2.lifeMax - NPC2.life) / 10);
					NPC2.HealEffect(healAmount);
					NPC2.life += healAmount;
					if (NPC2.life >= NPC2.lifeMax)
					{
						NPC2.life = NPC2.lifeMax;
					}
				}
			}
		}

        public override void SetDefaults(NPC NPC)
        {
			if (!TerrorbornSystem.TwilightMode || NPC.friendly || NPC.lifeMax >= int.MaxValue / 2)
            {
				return;
            }

			if (NPC.boss)
			{
				NPC.lifeMax = (int)(NPC.lifeMax * 1.5f);
				NPC.value *= 1.3f;
			}
            else
			{
				NPC.lifeMax = (int)(NPC.lifeMax * 1.35f);
				NPC.knockBackResist *= 0.5f;
				NPC.value *= 2f;
			}
        }

        public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
        {
			if (TerrorbornSystem.TwilightMode)
            {
				spawnRate = (int)(spawnRate * 0.75f);
				maxSpawns = (int)(maxSpawns * 1.3f);
			}
        }

        public override void OnHitByItem(NPC NPC, Player player, Item item, int damage, float knockback, bool crit)
		{
			if (NPC.type == NPCID.CultistBossClone)
			{
				NPC NPC2 = Main.npc[(int)NPC.ai[3]];
				if (modNPC(NPC2).twilight)
				{
					int healAmount = NPC2.lifeMax / 15 + ((NPC2.lifeMax - NPC2.life) / 10);
					NPC2.HealEffect(healAmount);
					NPC2.life += healAmount;
					if (NPC2.life >= NPC2.lifeMax)
					{
						NPC2.life = NPC2.lifeMax;
					}
				}
			}
		}

		bool postAIStart = true;
		bool ballStoppedHoming = false;
        public override void PostAI(NPC NPC)
        {
			if (!TerrorbornSystem.TwilightMode)
            {
				return;
            }

			if (postAIStart)
            {
				postAIStart = false;
				ballStoppedHoming = false;
			}

			if (NPC.aiStyle == 9)
            {
				if (!ballStoppedHoming)
                {
					NPC.velocity = NPC.DirectionTo(Main.LocalPlayer.Center) * (NPC.velocity.Length() + 0.07f);
					if (NPC.Distance(Main.LocalPlayer.Center) <= 200f)
                    {
						ballStoppedHoming = true;
                    }
                }
            }
        }

        public override bool PreAI(NPC NPC)
		{
			if (start)
			{
				twilight = TerrorbornSystem.TwilightMode;
			}
			if (twilight)
			{
				if (NPC.aiStyle == 4)
				{
					EyeOfCthulhuAI(NPC);
					return false;
				}
				if (NPC.aiStyle == 15)
				{
					KingSlimeAI(NPC);
					return false;
				}
				if (NPC.aiStyle == 43)
				{
					QueenBeeAI(NPC);
					return false;
				}
				if (NPC.aiStyle == 84)
				{
					LunaticCultistAI(NPC);
					return false;
				}
				if (NPC.aiStyle == 54)
				{
					BrainOfCthulhuAI(NPC);
					return false;
				}
				if (NPC.aiStyle == 27)
				{
					WoFMouthAI(NPC);
					return false;
				}
				if (NPC.aiStyle == 28)
				{
					WoFEyeAI(NPC);
					return false;
				}
			}
			start = false;
			return base.PreAI(NPC);
		}

		int EoCFlameDir = 0;
		public void SetEoCFlamethrowerDirection(NPC NPC)
        {
			if (Main.player[NPC.target].Center.X > NPC.Center.X) EoCFlameDir = 1;
			else EoCFlameDir = -1;

			if (Main.player[NPC.target].Center.Y < NPC.Center.Y + 100) EoCFlameDir *= -1;

		}

		bool EoCAutoRotate = true;
		int EoCFlamethrowerWait = 0;
		int EoCAttackCounter = 0;
		bool EoCThirdPhaseJustStarted = true;
		public void EyeOfCthulhuAI(NPC NPC)
		{
			bool flag2 = false;
			if (Main.expertMode && (double)NPC.life < (double)NPC.lifeMax * 0.12)
			{
				flag2 = true;
			}
			bool flag3 = false;
			if (Main.expertMode && (double)NPC.life < (double)NPC.lifeMax * 0.04)
			{
				flag3 = true;
			}
			float num4 = 20f;
			if (flag3)
			{
				num4 = 10f;
			}
			if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
			{
				NPC.TargetClosest();
			}
			bool dead = Main.player[NPC.target].dead;
			float num5 = NPC.position.X + (float)(NPC.width / 2) - Main.player[NPC.target].position.X - (float)(Main.player[NPC.target].width / 2);
			float num6 = NPC.position.Y + (float)NPC.height - 59f - Main.player[NPC.target].position.Y - (float)(Main.player[NPC.target].height / 2);
			float num7 = (float)Math.Atan2(num6, num5) + 1.57f;
			if (num7 < 0f)
			{
				num7 += 6.283f;
			}
			else if ((double)num7 > 6.283)
			{
				num7 -= 6.283f;
			}
			float num8 = 0f;
			if (NPC.ai[0] == 0f && NPC.ai[1] == 0f)
			{
				num8 = 0.02f;
			}
			if (NPC.ai[0] == 0f && NPC.ai[1] == 2f && NPC.ai[2] > 40f)
			{
				num8 = 0.05f;
			}
			if (NPC.ai[0] == 3f && NPC.ai[1] == 0f)
			{
				num8 = 0.05f;
			}
			if (NPC.ai[0] == 3f && NPC.ai[1] == 2f && NPC.ai[2] > 40f)
			{
				num8 = 0.08f;
			}
			if (NPC.ai[0] == 3f && NPC.ai[1] == 4f && NPC.ai[2] > num4)
			{
				num8 = 0.15f;
			}
			if (NPC.ai[0] == 3f && NPC.ai[1] == 5f)
			{
				num8 = 0.05f;
			}
			if (Main.expertMode)
			{
				num8 *= 1.5f;
			}
			if (flag3 && Main.expertMode)
			{
				num8 = 0f;
			}
			if (EoCAutoRotate)
			{
				if (NPC.rotation < num7)
				{
					if ((double)(num7 - NPC.rotation) > 3.1415)
					{
						NPC.rotation -= num8;
					}
					else
					{
						NPC.rotation += num8;
					}
				}
				else if (NPC.rotation > num7)
				{
					if ((double)(NPC.rotation - num7) > 3.1415)
					{
						NPC.rotation += num8;
					}
					else
					{
						NPC.rotation -= num8;
					}
				}
				if (NPC.rotation > num7 - num8 && NPC.rotation < num7 + num8)
				{
					NPC.rotation = num7;
				}
				if (NPC.rotation < 0f)
				{
					NPC.rotation += 6.283f;
				}
				else if ((double)NPC.rotation > 6.283)
				{
					NPC.rotation -= 6.283f;
				}
				if (NPC.rotation > num7 - num8 && NPC.rotation < num7 + num8)
				{
					NPC.rotation = num7;
				}
			}
			if (Main.rand.Next(5) == 0)
			{
				int num9 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y + (float)NPC.height * 0.25f), NPC.width, (int)((float)NPC.height * 0.5f), 5, NPC.velocity.X, 2f);
				Main.dust[num9].velocity.X *= 0.5f;
				Main.dust[num9].velocity.Y *= 0.1f;
			}
			if (Main.dayTime || dead)
			{
				NPC.velocity.Y -= 0.04f;
				if (NPC.timeLeft > 10)
				{
					NPC.timeLeft = 10;
				}
				return;
			}
			if (NPC.life <= NPC.lifeMax * 0.33f && NPC.ai[0] > 1f)
            {
				Player player = Main.player[NPC.target];
				if (EoCThirdPhaseJustStarted)
                {
					EoCThirdPhaseJustStarted = false;
					EoCAttackCounter = -30;
					EoCAutoRotate = false;
					NPC.ai[1] = 3;
					NPC.damage = (int)(NPC.damage * 1.5f);
				}
				else if (NPC.ai[1] == 0)
				{
					NPC.velocity *= 0.9f;
					if (EoCFlamethrowerWait > 0)
					{
						NPC.rotation += MathHelper.ToRadians(7f) * EoCFlameDir;
						EoCFlamethrowerWait--;
						if (EoCFlamethrowerWait <= 0)
                        {
							Terraria.Audio.SoundEngine.PlaySound(SoundID.NPCDeath10, NPC.Center);
						}
					}
					else
					{
						NPC.rotation += MathHelper.ToRadians(MathHelper.Lerp(1.1f, 0.8f, (float)NPC.life / (float)NPC.lifeMax / 0.33f)) * EoCFlameDir;
						EoCAttackCounter++;
						if (EoCAttackCounter % 3 == 2)
						{
							Terraria.Audio.SoundEngine.PlaySound(SoundID.Item34, NPC.Center);
							Projectile.NewProjectile(NPC.Center, NPC.rotation.ToRotationVector2().RotatedBy(MathHelper.ToRadians(90f)) * 24f, ModContent.ProjectileType<EoCFlameThrower>(), 60 / 4, 0f);
                        }
						TerrorbornSystem.ScreenShake(1f);
						if (EoCAttackCounter > 120)
                        {
							NPC.ai[1] = 1;
							EoCAttackCounter = 0;
							EoCFlameDir = -Math.Sign(NPC.Center.X - player.Center.X);
						}
					}
				}
				else if (NPC.ai[1] == 1)
				{
					NPC.rotation = NPC.rotation.AngleTowards(num7, MathHelper.ToRadians(10f));
					NPC.velocity += NPC.DirectionTo(player.Center + new Vector2(500 * EoCFlameDir, -100f)) * 2f;
					NPC.velocity *= 0.93f;
					EoCAttackCounter++;
					if (EoCAttackCounter % 30 == 29)
					{
						int num22 = NPC.NewNPC((int)NPC.Center.X, (int)NPC.Center.Y, NPCID.ServantofCthulhu);
						Main.npc[num22].lifeMax = (int)MathHelper.Lerp(25f, 40f, (float)NPC.life / (float)NPC.lifeMax / 0.33f);
						Main.npc[num22].life = Main.npc[num22].lifeMax;
						Terraria.Audio.SoundEngine.PlaySound(SoundID.NPCDeath13, NPC.Center);
					}
					if (EoCAttackCounter > 90)
                    {
						NPC.velocity = NPC.DirectionTo(player.Center) * 25f;
						NPC.rotation = NPC.DirectionTo(player.Center).ToRotation() - MathHelper.ToRadians(90f);
						EoCAutoRotate = false;
						EoCAttackCounter = 0;
						Terraria.Audio.SoundEngine.PlaySound(SoundID.ForceRoar, (int)NPC.position.X, (int)NPC.position.Y, -1);
						NPC.ai[1] = 2;
					}
                }
				else if (NPC.ai[1] == 2)
                {
					EoCAttackCounter++;
					if (EoCAttackCounter == 29)
					{
						int num22 = NPC.NewNPC((int)NPC.Center.X, (int)NPC.Center.Y, NPCID.ServantofCthulhu);
						Main.npc[num22].lifeMax = (int)MathHelper.Lerp(35f, 55f, (float)NPC.life / (float)NPC.lifeMax / 0.33f);
						Main.npc[num22].life = Main.npc[num22].lifeMax;
						Terraria.Audio.SoundEngine.PlaySound(SoundID.NPCDeath13, NPC.Center);
					}
					if (EoCAttackCounter > 65)
                    {
						EoCAttackCounter = 0;
						EoCAutoRotate = true;
						NPC.ai[1] = 3;
                    }

				}
				else if (NPC.ai[1] == 3)
				{
					NPC.rotation = NPC.rotation.AngleTowards(num7, MathHelper.ToRadians(5f));
					NPC.velocity += NPC.DirectionTo(player.Center + new Vector2(600 * Math.Sign(NPC.Center.X - player.Center.X), -300)) * 2f;
					NPC.velocity *= 0.93f;
					EoCAttackCounter++;
					if (EoCAttackCounter > 90)
					{
						EoCFlamethrowerWait = 45;
						EoCAttackCounter = 0;
						SetEoCFlamethrowerDirection(NPC);
						EoCAutoRotate = false;
						NPC.rotation = NPC.DirectionTo(player.Center).ToRotation() - MathHelper.ToRadians(90f);
						NPC.ai[1] = 0;
					}
				}
				return;
            }
			if (NPC.ai[0] == 0f)
			{
				if (NPC.ai[1] == 0f) //Hovering
				{
					float num10 = 22.5f; //hover speed (phase one)
					float num11 = 0.1f;
					Vector2 vector = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
					float num12 = Main.player[NPC.target].position.X + (float)(Main.player[NPC.target].width / 2) - vector.X;
					float num13 = Main.player[NPC.target].position.Y + (float)(Main.player[NPC.target].height / 2) - 200f - vector.Y;
					float num14 = (float)Math.Sqrt(num12 * num12 + num13 * num13);
					float num15 = num14;
					num14 = num10 / num14;
					num12 *= num14;
					num13 *= num14;
					if (NPC.velocity.X < num12)
					{
						NPC.velocity.X += num11;
						if (NPC.velocity.X < 0f && num12 > 0f)
						{
							NPC.velocity.X += num11;
						}
					}
					else if (NPC.velocity.X > num12)
					{
						NPC.velocity.X -= num11;
						if (NPC.velocity.X > 0f && num12 < 0f)
						{
							NPC.velocity.X -= num11;
						}
					}
					if (NPC.velocity.Y < num13)
					{
						NPC.velocity.Y += num11;
						if (NPC.velocity.Y < 0f && num13 > 0f)
						{
							NPC.velocity.Y += num11;
						}
					}
					else if (NPC.velocity.Y > num13)
					{
						NPC.velocity.Y -= num11;
						if (NPC.velocity.Y > 0f && num13 < 0f)
						{
							NPC.velocity.Y -= num11;
						}
					}
					NPC.ai[2] += 1f;
					float num16 = 600f;
					if (Main.expertMode)
					{
						num16 *= 0.35f;
					}
					if (NPC.ai[2] >= num16)
					{
						NPC.ai[1] = 1f;
						NPC.ai[2] = 0f;
						NPC.ai[3] = 0f;
						NPC.target = 255;
					}
					else if ((NPC.position.Y + (float)NPC.height < Main.player[NPC.target].position.Y && num15 < 500f) || (Main.expertMode && num15 < 500f))
					{
						if (!Main.player[NPC.target].dead)
						{
							NPC.ai[3] += 1f;
						}
						float num17 = 110f;
						if (Main.expertMode)
						{
							num17 *= 0.4f;
						}
						if (NPC.ai[3] >= num17)
						{
							NPC.ai[3] = 0f;
							NPC.rotation = num7;
							float num18 = 5f;
							if (Main.expertMode)
							{
								num18 = 6f;
							}
							float num19 = Main.player[NPC.target].position.X + (float)(Main.player[NPC.target].width / 2) - vector.X;
							float num20 = Main.player[NPC.target].position.Y + (float)(Main.player[NPC.target].height / 2) - vector.Y;
							float num21 = (float)Math.Sqrt(num19 * num19 + num20 * num20);
							num21 = num18 / num21;
							Vector2 vector2 = vector;
							Vector2 vector3 = default(Vector2);
							vector3.X = num19 * num21;
							vector3.Y = num20 * num21;
							vector2.X += vector3.X * 10f;
							vector2.Y += vector3.Y * 10f;
							if (Main.netMode != NetmodeID.MultiplayerClient)
							{
								int num22 = NPC.NewNPC((int)vector2.X, (int)vector2.Y, 5);
								Main.npc[num22].velocity.X = vector3.X;
								Main.npc[num22].velocity.Y = vector3.Y;
								if (Main.netMode == NetmodeID.Server && num22 < 200)
								{
									NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, num22);
								}
							}
							Terraria.Audio.SoundEngine.PlaySound(SoundID.NPCHit, (int)vector2.X, (int)vector2.Y);
							for (int m = 0; m < 10; m++)
							{
								Dust.NewDust(vector2, 20, 20, 5, vector3.X * 0.4f, vector3.Y * 0.4f);
							}
						}
					}
				}
				else if (NPC.ai[1] == 1f) //Start dash
				{
					NPC.rotation = num7;
					float num23 = 8f; //charge speed (phase one)
					Vector2 vector4 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
					float num24 = Main.player[NPC.target].Center.X - vector4.X + Main.player[NPC.target].velocity.X * (NPC.Distance(Main.player[NPC.target].Center) / num23);
					float num25 = Main.player[NPC.target].Center.Y - vector4.Y + Main.player[NPC.target].velocity.Y * (NPC.Distance(Main.player[NPC.target].Center) / num23);
					float num26 = (float)Math.Sqrt(num24 * num24 + num25 * num25);
					num26 = num23 / num26;
					NPC.velocity.X = num24 * num26;
					NPC.velocity.Y = num25 * num26;
					NPC.ai[1] = 2f;
				}
				else if (NPC.ai[1] == 2f) //Mid dash
				{
					NPC.ai[2] += 1f;
					if (NPC.ai[2] >= 40f)
					{
						NPC.velocity *= 0.98f;
						if (Main.expertMode)
						{
							NPC.velocity *= 0.985f;
						}
						if ((double)NPC.velocity.X > -0.1 && (double)NPC.velocity.X < 0.1)
						{
							NPC.velocity.X = 0f;
						}
						if ((double)NPC.velocity.Y > -0.1 && (double)NPC.velocity.Y < 0.1)
						{
							NPC.velocity.Y = 0f;
						}
					}
					else
					{
						NPC.rotation = (float)Math.Atan2(NPC.velocity.Y, NPC.velocity.X) - 1.57f;
					}
					int num27 = 150;
					if (Main.expertMode)
					{
						num27 = 100;
					}
					if (NPC.ai[2] >= (float)num27)
					{
						NPC.ai[3] += 1f;
						NPC.ai[2] = 0f;
						NPC.target = 255;
						NPC.rotation = num7;
						if (NPC.ai[3] >= 3f)
						{
							NPC.ai[1] = 0f;
							NPC.ai[3] = 0f;
						}
						else
						{
							NPC.ai[1] = 1f;
						}
					}
				}
				float num28 = 0.66f;
				if ((float)NPC.life < (float)NPC.lifeMax * num28)
				{
					NPC.ai[0] = 1f;
					NPC.ai[1] = 0f;
					NPC.ai[2] = 0f;
					NPC.ai[3] = 0f;
				}
				return;
			}
			if (NPC.ai[0] == 1f || NPC.ai[0] == 2f)
			{
				if (NPC.ai[0] == 1f)
				{
					NPC.ai[2] += 0.005f;
					if ((double)NPC.ai[2] > 0.5)
					{
						NPC.ai[2] = 0.5f;
					}
				}
				else
				{
					NPC.ai[2] -= 0.005f;
					if (NPC.ai[2] < 0f)
					{
						NPC.ai[2] = 0f;
					}
				}
				NPC.rotation += NPC.ai[2];
				NPC.ai[1] += 1f;
				if (Main.expertMode && NPC.ai[1] % 20f == 0f)
				{
					float num29 = 5f;
					Vector2 vector5 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
					float num30 = Main.rand.Next(-200, 200);
					float num31 = Main.rand.Next(-200, 200);
					float num32 = (float)Math.Sqrt(num30 * num30 + num31 * num31);
					num32 = num29 / num32;
					Vector2 vector6 = vector5;
					Vector2 vector7 = default(Vector2);
					vector7.X = num30 * num32;
					vector7.Y = num31 * num32;
					vector6.X += vector7.X * 10f;
					vector6.Y += vector7.Y * 10f;
					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						int num33 = NPC.NewNPC((int)vector6.X, (int)vector6.Y, 5);
						Main.npc[num33].velocity.X = vector7.X;
						Main.npc[num33].velocity.Y = vector7.Y;
						if (Main.netMode == NetmodeID.Server && num33 < 200)
						{
							NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, num33);
						}
					}
					for (int n = 0; n < 10; n++)
					{
						Dust.NewDust(vector6, 20, 20, 5, vector7.X * 0.4f, vector7.Y * 0.4f);
					}
				}
				if (NPC.ai[1] == 100f)
				{
					NPC.ai[0] += 1f;
					NPC.ai[1] = 0f;
					if (NPC.ai[0] == 3f)
					{
						NPC.ai[2] = 0f;
					}
					else
					{
						Terraria.Audio.SoundEngine.PlaySound(SoundID.NPCHit, (int)NPC.position.X, (int)NPC.position.Y);
						for (int num34 = 0; num34 < 2; num34++)
						{
							Gore.NewGore(NPC.position, new Vector2((float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f), 8);
							Gore.NewGore(NPC.position, new Vector2((float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f), 7);
							Gore.NewGore(NPC.position, new Vector2((float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f), 6);
						}
						for (int num35 = 0; num35 < 20; num35++)
						{
							Dust.NewDust(NPC.position, NPC.width, NPC.height, 5, (float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f);
						}
						Terraria.Audio.SoundEngine.PlaySound(SoundID.Roar, (int)NPC.position.X, (int)NPC.position.Y, 0);
					}
				}
				Dust.NewDust(NPC.position, NPC.width, NPC.height, 5, (float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f);
				NPC.velocity.X *= 0.98f;
				NPC.velocity.Y *= 0.98f;
				if ((double)NPC.velocity.X > -0.1 && (double)NPC.velocity.X < 0.1)
				{
					NPC.velocity.X = 0f;
				}
				if ((double)NPC.velocity.Y > -0.1 && (double)NPC.velocity.Y < 0.1)
				{
					NPC.velocity.Y = 0f;
				}
				return;
			}
			NPC.defense = 0;
			NPC.damage = 23;
			if (Main.expertMode)
			{
				if (flag2)
				{
					NPC.defense = -15;
				}
				if (flag3)
				{
					NPC.damage = (int)(20f * Main.expertDamage);
					NPC.defense = -30;
				}
				else
				{
					NPC.damage = (int)(18f * Main.expertDamage);
				}
			}
			if (NPC.ai[1] == 0f && flag2)
			{
				NPC.ai[1] = 5f;
			}
			if (NPC.ai[1] == 0f) //Hovering
			{
				float num36 = 12f; //hover speed (phase two)
				float num37 = 0.07f;
				Vector2 vector8 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
				int direction = 1;
				if (Main.player[NPC.target].Center.X >= NPC.Center.X)
				{
					direction = -1;
				}
				float num38 = Main.player[NPC.target].position.X + (float)(Main.player[NPC.target].width / 2) + (120f * direction) - vector8.X;
				float num39 = Main.player[NPC.target].position.Y + (float)(Main.player[NPC.target].height / 2) - vector8.Y;
				float num40 = (float)Math.Sqrt(num38 * num38 + num39 * num39);
				if (num40 > 400f)
				{
					num36 += 1f;
					num37 += 0.05f;
					if (num40 > 600f)
					{
						num36 += 1f;
						num37 += 0.05f;
						if (num40 > 800f)
						{
							num36 += 1f;
							num37 += 0.05f;
						}
					}
				}
				num40 = num36 / num40;
				num38 *= num40;
				num39 *= num40;
				if (NPC.velocity.X < num38)
				{
					NPC.velocity.X += num37;
					if (NPC.velocity.X < 0f && num38 > 0f)
					{
						NPC.velocity.X += num37;
					}
				}
				else if (NPC.velocity.X > num38)
				{
					NPC.velocity.X -= num37;
					if (NPC.velocity.X > 0f && num38 < 0f)
					{
						NPC.velocity.X -= num37;
					}
				}
				if (NPC.velocity.Y < num39)
				{
					NPC.velocity.Y += num37;
					if (NPC.velocity.Y < 0f && num39 > 0f)
					{
						NPC.velocity.Y += num37;
					}
				}
				else if (NPC.velocity.Y > num39)
				{
					NPC.velocity.Y -= num37;
					if (NPC.velocity.Y > 0f && num39 < 0f)
					{
						NPC.velocity.Y -= num37;
					}
				}
				NPC.ai[2] += 1f;
				if (NPC.ai[2] >= 200f)
				{
					NPC.ai[1] = 1f;
					NPC.ai[2] = 0f;
					NPC.ai[3] = 0f;
					if (Main.expertMode && (double)NPC.life < (double)NPC.lifeMax * 0.35)
					{
						NPC.ai[1] = 3f;
					}
					NPC.target = 255;
				}
				if (Main.expertMode && flag3)
				{
					NPC.TargetClosest();
					NPC.ai[1] = 3f;
					NPC.ai[2] = 0f;
					NPC.ai[3] -= 1000f;
				}
			}
			else if (NPC.ai[1] == 1f) //Frame of dash
			{
				Terraria.Audio.SoundEngine.PlaySound(SoundID.ForceRoar, (int)NPC.position.X, (int)NPC.position.Y, 0);
				NPC.rotation = num7;
				float num41 = 11f; //charge speed (phase two)
				if (NPC.ai[3] == 1f)
				{
					num41 *= 1.15f;
				}
				if (NPC.ai[3] == 2f)
				{
					num41 *= 1.3f;
				}
				Vector2 vector9 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
				float num42 = Main.player[NPC.target].Center.X - vector9.X + Main.player[NPC.target].velocity.X * (NPC.Distance(Main.player[NPC.target].Center) / num41);
				float num43 = Main.player[NPC.target].Center.Y - vector9.Y + Main.player[NPC.target].velocity.Y * (NPC.Distance(Main.player[NPC.target].Center) / num41);
				float num44 = (float)Math.Sqrt(num42 * num42 + num43 * num43);
				num44 = num41 / num44;
				NPC.velocity.X = num42 * num44;
				NPC.velocity.Y = num43 * num44;
				NPC.ai[1] = 2f;
			}
			else if (NPC.ai[1] == 2f) //Mid normal dash
			{
				float num45 = 40f;
				NPC.ai[2] += 1f;
				if (Main.expertMode)
				{
					num45 = 50f;
				}
				if (NPC.ai[2] >= num45)
				{
					NPC.velocity *= 0.97f;
					if (Main.expertMode)
					{
						NPC.velocity *= 0.98f;
					}
					if ((double)NPC.velocity.X > -0.1 && (double)NPC.velocity.X < 0.1)
					{
						NPC.velocity.X = 0f;
					}
					if ((double)NPC.velocity.Y > -0.1 && (double)NPC.velocity.Y < 0.1)
					{
						NPC.velocity.Y = 0f;
					}
				}
				else
				{
					NPC.rotation = (float)Math.Atan2(NPC.velocity.Y, NPC.velocity.X) - 1.57f;
				}
				int num46 = 130;
				if (Main.expertMode)
				{
					num46 = 90;
				}
				if (NPC.ai[2] >= (float)num46)
				{
					NPC.ai[3] += 1f;
					NPC.ai[2] = 0f;
					NPC.target = 255;
					NPC.rotation = num7;
					if (NPC.ai[3] >= 3f)
					{
						NPC.ai[1] = 0f;
						NPC.ai[3] = 0f;
						if (Main.expertMode && Main.netMode != NetmodeID.MultiplayerClient && (double)NPC.life < (double)NPC.lifeMax * 0.5)
						{
							NPC.ai[1] = 3f;
							NPC.ai[3] += Main.rand.Next(1, 4);
						}
					}
					else
					{
						NPC.ai[1] = 1f;
					}
				}
			}
			else if (NPC.ai[1] == 3f) //Frame of fast dash
			{
				if (NPC.ai[3] == 4f && flag2 && NPC.Center.Y > Main.player[NPC.target].Center.Y)
				{
					NPC.TargetClosest();
					NPC.ai[1] = 0f;
					NPC.ai[2] = 0f;
					NPC.ai[3] = 0f;
				}
				else if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					//	NPC.TargetClosest();
					//	float num47 = 30f;

					//	//int num22 = NPC.NewNPC((int)NPC.Center.X, (int)NPC.Center.Y, NPCID.ServantofCthulhu);
					//	//Terraria.Audio.SoundEngine.PlaySound(SoundID.NPCDeath13, NPC.Center);

					//	Vector2 vector10 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
					//	float num48 = Main.player[NPC.target].position.X + (float)(Main.player[NPC.target].width / 2) - vector10.X;
					//	float num49 = Main.player[NPC.target].position.Y + (float)(Main.player[NPC.target].height / 2) - vector10.Y;
					//	float num50 = Math.Abs(Main.player[NPC.target].velocity.X) + Math.Abs(Main.player[NPC.target].velocity.Y) / 4f;
					//	num50 += 10f - num50;
					//	if (num50 < 5f)
					//	{
					//		num50 = 5f;
					//	}
					//	if (num50 > 15f)
					//	{
					//		num50 = 15f;
					//	}
					//	if (NPC.ai[2] == -1f && !flag3)
					//	{
					//		num50 *= 4f;
					//		num47 *= 1.3f;
					//	}
					//	if (flag3)
					//	{
					//		num50 *= 2f;
					//	}
					//	num48 -= Main.player[NPC.target].velocity.X * num50;
					//	num49 -= Main.player[NPC.target].velocity.Y * num50 / 4f;
					//	num48 *= 1f + (float)Main.rand.Next(-10, 11) * 0.01f;
					//	num49 *= 1f + (float)Main.rand.Next(-10, 11) * 0.01f;
					//	if (flag3)
					//	{
					//		num48 *= 1f + (float)Main.rand.Next(-10, 11) * 0.01f;
					//		num49 *= 1f + (float)Main.rand.Next(-10, 11) * 0.01f;
					//	}
					//	float num51 = (float)Math.Sqrt(num48 * num48 + num49 * num49);
					//	float num52 = num51;
					//	num51 = num47 / num51;
					//	NPC.velocity.X = num48 * num51;
					//	NPC.velocity.Y = num49 * num51;
					//	NPC.velocity.X += (float)Main.rand.Next(-20, 21) * 0.1f;
					//	NPC.velocity.Y += (float)Main.rand.Next(-20, 21) * 0.1f;
					//	if (flag3)
					//	{
					//		NPC.velocity.X += (float)Main.rand.Next(-50, 51) * 0.1f;
					//		NPC.velocity.Y += (float)Main.rand.Next(-50, 51) * 0.1f;
					//		float num53 = Math.Abs(NPC.velocity.X);
					//		float num54 = Math.Abs(NPC.velocity.Y);
					//		if (NPC.Center.X > Main.player[NPC.target].Center.X)
					//		{
					//			num54 *= -1f;
					//		}
					//		if (NPC.Center.Y > Main.player[NPC.target].Center.Y)
					//		{
					//			num53 *= -1f;
					//		}
					//		NPC.velocity.X = num54 + NPC.velocity.X;
					//		NPC.velocity.Y = num53 + NPC.velocity.Y;
					//		NPC.velocity.Normalize();
					//		NPC.velocity *= num47;
					//		NPC.velocity.X += (float)Main.rand.Next(-20, 21) * 0.1f;
					//		NPC.velocity.Y += (float)Main.rand.Next(-20, 21) * 0.1f;
					//	}
					//	else if (num52 < 100f)
					//	{
					//		if (Math.Abs(NPC.velocity.X) > Math.Abs(NPC.velocity.Y))
					//		{
					//			float num55 = Math.Abs(NPC.velocity.X);
					//			float num56 = Math.Abs(NPC.velocity.Y);
					//			if (NPC.Center.X > Main.player[NPC.target].Center.X)
					//			{
					//				num56 *= -1f;
					//			}
					//			if (NPC.Center.Y > Main.player[NPC.target].Center.Y)
					//			{
					//				num55 *= -1f;
					//			}
					//			NPC.velocity.X = num56;
					//			NPC.velocity.Y = num55;
					//		}
					//	}
					//	else if (Math.Abs(NPC.velocity.X) > Math.Abs(NPC.velocity.Y))
					//	{
					//		float num57 = (Math.Abs(NPC.velocity.X) + Math.Abs(NPC.velocity.Y)) / 2f;
					//		float num58 = num57;
					//		if (NPC.Center.X > Main.player[NPC.target].Center.X)
					//		{
					//			num58 *= -1f;
					//		}
					//		if (NPC.Center.Y > Main.player[NPC.target].Center.Y)
					//		{
					//			num57 *= -1f;
					//		}
					//		NPC.velocity.X = num58;
					//		NPC.velocity.Y = num57;
					//	}
					NPC.velocity = NPC.DirectionTo(Main.player[NPC.target].Center) * 15f;
					NPC.ai[1] = 4f;
				}
			}
			else if (NPC.ai[1] == 4f) //Mid fast dash
			{
				if (NPC.ai[2] == 0f)
				{
					Terraria.Audio.SoundEngine.PlaySound(SoundID.ForceRoar, (int)NPC.position.X, (int)NPC.position.Y, -1);
				}
				float num59 = num4;
				NPC.ai[2] += 1f;
				if (NPC.ai[2] == num59 && Vector2.Distance(NPC.position, Main.player[NPC.target].position) < 200f)
				{
					NPC.ai[2] -= 1f;
				}
				if (NPC.ai[2] >= num59)
				{
					NPC.velocity *= 0.95f;
					if ((double)NPC.velocity.X > -0.1 && (double)NPC.velocity.X < 0.1)
					{
						NPC.velocity.X = 0f;
					}
					if ((double)NPC.velocity.Y > -0.1 && (double)NPC.velocity.Y < 0.1)
					{
						NPC.velocity.Y = 0f;
					}
				}
				else
				{
					NPC.rotation = (float)Math.Atan2(NPC.velocity.Y, NPC.velocity.X) - 1.57f;
				}
				float num60 = num59 + 13f;
				if (NPC.ai[2] >= num60)
				{
					NPC.ai[3] += 1f;
					NPC.ai[2] = 0f;
					if (NPC.ai[3] >= 5f)
					{
						NPC.ai[1] = 0f;
						NPC.ai[3] = 0f;
					}
					else
					{
						NPC.ai[1] = 3f;
					}
				}
			}
			else if (NPC.ai[1] == 5f)
			{
				float num61 = 600f;
				float num62 = 9f;
				float num63 = 0.3f;
				Vector2 vector11 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
				float num64 = Main.player[NPC.target].position.X + (float)(Main.player[NPC.target].width / 2) - vector11.X;
				float num65 = Main.player[NPC.target].position.Y + (float)(Main.player[NPC.target].height / 2) + num61 - vector11.Y;
				float num66 = (float)Math.Sqrt(num64 * num64 + num65 * num65);
				num66 = num62 / num66;
				num64 *= num66;
				num65 *= num66;
				if (NPC.velocity.X < num64)
				{
					NPC.velocity.X += num63;
					if (NPC.velocity.X < 0f && num64 > 0f)
					{
						NPC.velocity.X += num63;
					}
				}
				else if (NPC.velocity.X > num64)
				{
					NPC.velocity.X -= num63;
					if (NPC.velocity.X > 0f && num64 < 0f)
					{
						NPC.velocity.X -= num63;
					}
				}
				if (NPC.velocity.Y < num65)
				{
					NPC.velocity.Y += num63;
					if (NPC.velocity.Y < 0f && num65 > 0f)
					{
						NPC.velocity.Y += num63;
					}
				}
				else if (NPC.velocity.Y > num65)
				{
					NPC.velocity.Y -= num63;
					if (NPC.velocity.Y > 0f && num65 < 0f)
					{
						NPC.velocity.Y -= num63;
					}
				}
				NPC.ai[2] += 1f;
				if (NPC.ai[2] >= 70f)
				{
					NPC.TargetClosest();
					NPC.ai[1] = 3f;
					NPC.ai[2] = -1f;
					NPC.ai[3] = Main.rand.Next(-3, 1);
				}
			}
			if (flag3 && NPC.ai[1] == 5f)
			{
				NPC.ai[1] = 3f;
			}
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
						Terraria.Audio.SoundEngine.PlaySound(SoundID.Roar, (int)NPC.position.X, (int)NPC.position.Y, 0);
						Terraria.Audio.SoundEngine.PlaySound(SoundID.Item33, Main.player[NPC.target].Center);
						Projectile proj = Main.projectile[Projectile.NewProjectile(Main.player[NPC.target].Center + new Vector2(Main.player[NPC.target].velocity.X * 67f, 0) + new Vector2(0, 1000), new Vector2(0, -15 + Main.player[NPC.target].velocity.Y), ModContent.ProjectileType<QueenBeeLaser>(), 11, 0)];
						proj.tileCollide = false;
						proj.timeLeft = 300;
						proj = Main.projectile[Projectile.NewProjectile(Main.player[NPC.target].Center + new Vector2(Main.player[NPC.target].velocity.X * 67f, 0) + new Vector2(0, -1000), new Vector2(0, 15 + Main.player[NPC.target].velocity.Y), ModContent.ProjectileType<QueenBeeLaser>(), 11, 0)];
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
					Projectile.NewProjectile(NPC.Center, velocity, ModContent.ProjectileType<QueenBeeLaser>(), 10, 0f);
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
					Terraria.Audio.SoundEngine.PlaySound(SoundID.NPCHit, (int)NPC.position.X, (int)NPC.position.Y);
					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						int num621 = Main.rand.Next(210, 212);
						int num622 = NPC.NewNPC((int)vector78.X, (int)vector78.Y, num621);
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
					//Terraria.Audio.SoundEngine.PlaySound(SoundID.Item33, Main.player[NPC.target].Center);
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
					Terraria.Audio.SoundEngine.PlaySound(SoundID.Item17, NPC.position);
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
						int num635 = 55;
						float lightAmount = 0.5f;
						float rotationAmount = 35;//adds 2 extra Projectiles with a set offset
						int num636 = Projectile.NewProjectile(vector80.X, vector80.Y, num631, num632, num635, num634, 0f, Main.myPlayer);
						Main.projectile[num636].timeLeft = 300;
						Main.projectile[num636].light = lightAmount;
						Main.projectile[num636].tileCollide = false;
						num636 = Projectile.NewProjectile(vector80.X, vector80.Y, num631, num632, num635, num634, 0f, Main.myPlayer);
						Main.projectile[num636].timeLeft = 300;
						Main.projectile[num636].light = lightAmount;
						Main.projectile[num636].tileCollide = false;
						Main.projectile[num636].velocity = Main.projectile[num636].velocity.RotatedBy(MathHelper.ToRadians(rotationAmount));
						num636 = Projectile.NewProjectile(vector80.X, vector80.Y, num631, num632, num635, num634, 0f, Main.myPlayer);
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

		public void LunaticCultistAI(NPC NPC)
        {
			if (NPC.AnyNPCs(NPCID.CultistDragonHead))
            {
				NPC.life += NPC.lifeMax / (60 * 30);
				if (NPC.life >= NPC.lifeMax)
				{
					NPC.life = NPC.lifeMax;
				}
			}
			if (NPC.ai[0] != -1f && Main.rand.Next(1000) == 0)
			{
				Terraria.Audio.SoundEngine.PlaySound(SoundID.Zombie, (int)NPC.position.X, (int)NPC.position.Y, Main.rand.Next(88, 92));
			}
			Lighting.AddLight(NPC.Center, new Vector3(0.5f, 0.5f, 0.5f));
			bool expertMode = Main.expertMode;
			bool flag = NPC.life <= NPC.lifeMax / 2;
			int num = 120;
			int num2 = 35;
			if (expertMode)
			{
				num = 90;
				num2 = 25;
			}
			int num3 = 18;
			int num4 = 3;
			int num5 = 30;
			if (expertMode)
			{
				num3 = 12;
				num4 = 4;
				num5 = 20;
			}
			int num6 = 80;
			int num7 = 45;
			if (expertMode)
			{
				num6 = 40;
				num7 = 30;
			}
			int num8 = 20;
			int num9 = 2;
			if (expertMode)
			{
				num8 = 30;
				num9 = 2;
			}
			int num10 = 20;
			int num11 = 3;
			bool flag2 = NPC.type == NPCID.CultistBoss;
			bool flag3 = false;
			bool flag4 = false;
			if (flag)
			{
				NPC.defense = (int)((float)NPC.defDefense * 0.65f);
			}
			if (!flag2)
			{
				if (NPC.ai[3] < 0f || !Main.npc[(int)NPC.ai[3]].active || Main.npc[(int)NPC.ai[3]].type != NPCID.CultistBoss)
				{
					NPC.life = 0;
					NPC.HitEffect();
					NPC.active = false;
					return;
				}
				NPC.ai[0] = Main.npc[(int)NPC.ai[3]].ai[0];
				NPC.ai[1] = Main.npc[(int)NPC.ai[3]].ai[1];
				if (NPC.ai[0] == 5f)
				{
					if (NPC.justHit)
					{
						NPC.life = 0;
						NPC.HitEffect();
						NPC.active = false;
						if (Main.netMode != NetmodeID.MultiplayerClient)
						{
							NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, NPC.whoAmI);
						}
						NPC obj = Main.npc[(int)NPC.ai[3]];
						obj.ai[0] = 6f;
						obj.ai[1] = 0f;
						obj.netUpdate = true;
					}
				}
				else
				{
					flag3 = true;
					flag4 = true;
				}
			}
			else if (NPC.ai[0] == 5f && NPC.ai[1] >= 120f && NPC.ai[1] < 420f && NPC.justHit)
			{
				NPC.ai[0] = 0f;
				NPC.ai[1] = 0f;
				NPC.ai[3] += 1f;
				NPC.velocity = Vector2.Zero;
				NPC.netUpdate = true;
				List<int> list = new List<int>();
				for (int i = 0; i < 200; i++)
				{
					if (Main.npc[i].active && Main.npc[i].type == NPCID.CultistBossClone && Main.npc[i].ai[3] == (float)NPC.whoAmI)
					{
						list.Add(i);
					}
				}
				int num12 = 10;
				if (Main.expertMode)
				{
					num12 = 3;
				}
				foreach (int item in list)
				{
					NPC NPC = Main.npc[item];
					if (NPC.localAI[1] == NPC.localAI[1] && num12 > 0)
					{
						num12--;
						NPC.life = 0;
						NPC.HitEffect();
						NPC.active = false;
						if (Main.netMode != NetmodeID.MultiplayerClient)
						{
							NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, item);
						}
					}
					else if (num12 > 0)
					{
						num12--;
						NPC.life = 0;
						NPC.HitEffect();
						NPC.active = false;
					}
				}
				Main.projectile[(int)NPC.ai[2]].ai[1] = -1f;
				Main.projectile[(int)NPC.ai[2]].netUpdate = true;
			}
			Vector2 center = NPC.Center;
			Player player = Main.player[NPC.target];
			if (NPC.target < 0 || NPC.target == 255 || player.dead || !player.active)
			{
				NPC.TargetClosest(faceTarget: false);
				player = Main.player[NPC.target];
				NPC.netUpdate = true;
			}
			if (player.dead || Vector2.Distance(player.Center, center) > 5600f)
			{
				NPC.life = 0;
				NPC.HitEffect();
				NPC.active = false;
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					NetMessage.SendData(MessageID.StrikeNPC, -1, -1, null, NPC.whoAmI, -1f);
				}
				new List<int>().Add(NPC.whoAmI);
				for (int j = 0; j < 200; j++)
				{
					if (Main.npc[j].active && Main.npc[j].type == NPCID.CultistBossClone && Main.npc[j].ai[3] == (float)NPC.whoAmI)
					{
						Main.npc[j].life = 0;
						Main.npc[j].HitEffect();
						Main.npc[j].active = false;
						if (Main.netMode != NetmodeID.MultiplayerClient)
						{
							NetMessage.SendData(MessageID.StrikeNPC, -1, -1, null, NPC.whoAmI, -1f);
						}
					}
				}
			}
			float num13 = NPC.ai[3];
			if (NPC.localAI[0] == 0f)
			{
				Terraria.Audio.SoundEngine.PlaySound(SoundID.Zombie, (int)NPC.position.X, (int)NPC.position.Y, 89);
				NPC.localAI[0] = 1f;
				NPC.alpha = 255;
				NPC.rotation = 0f;
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					NPC.ai[0] = -1f;
					NPC.netUpdate = true;
				}
			}
			if (NPC.ai[0] == -1f)
			{
				NPC.alpha -= 5;
				if (NPC.alpha < 0)
				{
					NPC.alpha = 0;
				}
				NPC.ai[1] += 1f;
				if (NPC.ai[1] >= 420f)
				{
					NPC.ai[0] = 0f;
					NPC.ai[1] = 0f;
					NPC.netUpdate = true;
				}
				else if (NPC.ai[1] > 360f)
				{
					NPC.velocity *= 0.95f;
					if (NPC.localAI[2] != 13f)
					{
						Terraria.Audio.SoundEngine.PlaySound(SoundID.Zombie, (int)NPC.position.X, (int)NPC.position.Y, 105);
					}
					NPC.localAI[2] = 13f;
				}
				else if (NPC.ai[1] > 300f)
				{
					NPC.velocity = -Vector2.UnitY;
					NPC.localAI[2] = 10f;
				}
				else if (NPC.ai[1] > 120f)
				{
					NPC.localAI[2] = 1f;
				}
				else
				{
					NPC.localAI[2] = 0f;
				}
				flag3 = true;
				flag4 = true;
			}
			if (NPC.ai[0] == 0f)
			{
				if (NPC.ai[1] == 0f)
				{
					NPC.TargetClosest(faceTarget: false);
				}
				NPC.localAI[2] = 10f;
				int num14 = Math.Sign(player.Center.X - center.X);
				if (num14 != 0)
				{
					NPC.direction = (NPC.spriteDirection = num14);
				}
				NPC.ai[1] += 1f;
				if (NPC.ai[1] >= 40f && flag2)
				{
					int num15 = 0;
					if (flag)
					{
						switch ((int)NPC.ai[3])
						{
							case 0:
								num15 = 0;
								break;
							case 1:
								num15 = 1;
								break;
							case 2:
								num15 = 0;
								break;
							case 3:
								num15 = 5;
								break;
							case 4:
								num15 = 0;
								break;
							case 5:
								num15 = 3;
								break;
							case 6:
								num15 = 0;
								break;
							case 7:
								num15 = 5;
								break;
							case 8:
								num15 = 0;
								break;
							case 9:
								num15 = 2;
								break;
							case 10:
								num15 = 0;
								break;
							case 11:
								num15 = 3;
								break;
							case 12:
								num15 = 0;
								break;
							case 13:
								num15 = 4;
								NPC.ai[3] = -1f;
								break;
							default:
								NPC.ai[3] = -1f;
								break;
						}
					}
					else
					{
						switch ((int)NPC.ai[3])
						{
							case 0:
								num15 = 0;
								break;
							case 1:
								num15 = 1;
								break;
							case 2:
								num15 = 0;
								break;
							case 3:
								num15 = 2;
								break;
							case 4:
								num15 = 0;
								break;
							case 5:
								num15 = 3;
								break;
							case 6:
								num15 = 0;
								break;
							case 7:
								num15 = 1;
								break;
							case 8:
								num15 = 0;
								break;
							case 9:
								num15 = 2;
								break;
							case 10:
								num15 = 0;
								break;
							case 11:
								num15 = 4;
								NPC.ai[3] = -1f;
								break;
							default:
								NPC.ai[3] = -1f;
								break;
						}
					}
					int maxValue = 6;
					if (NPC.life < NPC.lifeMax / 3)
					{
						maxValue = 4;
					}
					if (NPC.life < NPC.lifeMax / 4)
					{
						maxValue = 3;
					}
					if (expertMode && flag && Main.rand.Next(maxValue) == 0 && num15 != 0 && num15 != 4 && num15 != 5 && NPC.CountNPCS(523) < 10)
					{
						num15 = 6;
					}
					if (num15 == 0)
					{
						float num16 = (float)Math.Ceiling((player.Center + new Vector2(0f, -100f) - center).Length() / 50f);
						if (num16 == 0f)
						{
							num16 = 1f;
						}
						List<int> list2 = new List<int>();
						int num17 = 0;
						list2.Add(NPC.whoAmI);
						for (int k = 0; k < 200; k++)
						{
							if (Main.npc[k].active && Main.npc[k].type == NPCID.CultistBossClone && Main.npc[k].ai[3] == (float)NPC.whoAmI)
							{
								list2.Add(k);
							}
						}
						bool flag5 = list2.Count % 2 == 0;
						foreach (int item2 in list2)
						{
							NPC NPC2 = Main.npc[item2];
							Vector2 center2 = NPC2.Center;
							float num18 = (float)((num17 + flag5.ToInt() + 1) / 2) * ((float)Math.PI * 2f) * 0.4f / (float)list2.Count;
							if (num17 % 2 == 1)
							{
								num18 *= -1f;
							}
							if (list2.Count == 1)
							{
								num18 = 0f;
							}
							Vector2 vector = new Vector2(0f, -1f).RotatedBy(num18) * new Vector2(300f, 200f);
							Vector2 vector2 = player.Center + vector - center2;
							NPC2.ai[0] = 1f;
							NPC2.ai[1] = num16 * 2f;
							NPC2.velocity = vector2 / num16;
							if (NPC.whoAmI >= NPC2.whoAmI)
							{
								NPC2.position -= NPC2.velocity;
							}
							NPC2.netUpdate = true;
							num17++;
						}
					}
					switch (num15)
					{
						case 1:
							NPC.ai[0] = 3f;
							NPC.ai[1] = 0f;
							break;
						case 2:
							NPC.ai[0] = 2f;
							NPC.ai[1] = 0f;
							break;
						case 3:
							NPC.ai[0] = 4f;
							NPC.ai[1] = 0f;
							break;
						case 4:
							NPC.ai[0] = 5f;
							NPC.ai[1] = 0f;
							break;
					}
					if (num15 == 5)
					{
						NPC.ai[0] = 7f;
						NPC.ai[1] = 0f;
					}
					if (num15 == 6)
					{
						NPC.ai[0] = 8f;
						NPC.ai[1] = 0f;
					}
					NPC.netUpdate = true;
				}
			}
			else if (NPC.ai[0] == 1f)
			{
				flag3 = true;
				NPC.localAI[2] = 10f;
				if ((float)(int)NPC.ai[1] % 2f != 0f && NPC.ai[1] != 1f)
				{
					NPC.position -= NPC.velocity;
				}
				NPC.ai[1] -= 1f;
				if (NPC.ai[1] <= 0f)
				{
					NPC.ai[0] = 0f;
					NPC.ai[1] = 0f;
					NPC.ai[3] += 1f;
					NPC.velocity = Vector2.Zero;
					NPC.netUpdate = true;
				}
			}
			else if (NPC.ai[0] == 2f)
			{
				NPC.localAI[2] = 11f;
				Vector2 vec = Vector2.Normalize(player.Center - center);
				if (vec.HasNaNs())
				{
					vec = new Vector2(NPC.direction, 0f);
				}
				if (NPC.ai[1] >= 4f && flag2 && (int)(NPC.ai[1] - 4f) % num == 0)
				{
					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						List<int> list3 = new List<int>();
						for (int l = 0; l < 200; l++)
						{
							if (Main.npc[l].active && Main.npc[l].type == NPCID.CultistBossClone && Main.npc[l].ai[3] == (float)NPC.whoAmI)
							{
								list3.Add(l);
							}
						}
						foreach (int item3 in list3)
						{
							NPC NPC3 = Main.npc[item3];
							Vector2 center3 = NPC3.Center;
							int num19 = Math.Sign(player.Center.X - center3.X);
							if (num19 != 0)
							{
								NPC3.direction = (NPC3.spriteDirection = num19);
							}
							if (Main.netMode != NetmodeID.MultiplayerClient)
							{
								vec = Vector2.Normalize(player.Center - center3 + player.velocity * 20f);
								if (vec.HasNaNs())
								{
									vec = new Vector2(NPC.direction, 0f);
								}
								Vector2 vector3 = center3 + new Vector2(NPC.direction * 30, 12f);
								for (int m = 0; m < 1; m++)
								{
									Vector2 spinninpoint = vec * (6f + (float)Main.rand.NextDouble() * 4f);
									for (int i = 0; i <  Main.rand.Next(3, 5); i++)
									{
										spinninpoint = spinninpoint.RotatedByRandom(MathHelper.ToRadians(360));
										Projectile.NewProjectile(vector3.X, vector3.Y, spinninpoint.X / 2, spinninpoint.Y / 2, ProjectileID.FrostWave, 18, 0f, Main.myPlayer);
									}
								}
							}
						}
					}
					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						vec = Vector2.Normalize(player.Center - center + player.velocity * 20f);
						if (vec.HasNaNs())
						{
							vec = new Vector2(NPC.direction, 0f);
						}
						Vector2 vector4 = NPC.Center + new Vector2(NPC.direction * 30, 12f);
						for (int n = 0; n < 1; n++)
						{
							Vector2 vector5 = vec * 4f;
							Projectile proj = Main.projectile[Projectile.NewProjectile(vector4.X, vector4.Y, vector5.X, vector5.Y, ProjectileID.CultistBossIceMist, num2, 0f, Main.myPlayer, 0f, 1f)];
							proj.extraUpdates += 1;
						}
					}
				}
				NPC.ai[1] += 1f;
				if (NPC.ai[1] >= (float)(4 + num))
				{
					NPC.ai[0] = 0f;
					NPC.ai[1] = 0f;
					NPC.ai[3] += 1f;
					NPC.velocity = Vector2.Zero;
					NPC.netUpdate = true;
				}
			}
			else if (NPC.ai[0] == 3f)
			{
				NPC.localAI[2] = 11f;
				Vector2 vec2 = Vector2.Normalize(player.Center - center);
				if (vec2.HasNaNs())
				{
					vec2 = new Vector2(NPC.direction, 0f);
				}
				if (NPC.ai[1] >= 4f && flag2 && (int)(NPC.ai[1] - 4f) % num3 == 0)
				{
					if ((int)(NPC.ai[1] - 4f) / num3 == 2)
					{
						List<int> list4 = new List<int>();
						for (int num20 = 0; num20 < 200; num20++)
						{
							if (Main.npc[num20].active && Main.npc[num20].type == NPCID.CultistBossClone && Main.npc[num20].ai[3] == (float)NPC.whoAmI)
							{
								list4.Add(num20);
							}
						}
						if (Main.netMode != NetmodeID.MultiplayerClient)
						{
							foreach (int item4 in list4)
							{
								NPC NPC4 = Main.npc[item4];
								Vector2 center4 = NPC4.Center;
								int num21 = Math.Sign(player.Center.X - center4.X);
								if (num21 != 0)
								{
									NPC4.direction = (NPC4.spriteDirection = num21);
								}
								if (Main.netMode != NetmodeID.MultiplayerClient)
								{
									vec2 = Vector2.Normalize(player.Center - center4 + player.velocity * 20f);
									if (vec2.HasNaNs())
									{
										vec2 = new Vector2(NPC.direction, 0f);
									}
									Vector2 vector6 = center4 + new Vector2(NPC.direction * 30, 12f);
									for (int num22 = 0; num22 < 1; num22++)
									{
										Vector2 spinninpoint2 = vec2 * (6f + (float)Main.rand.NextDouble() * 4f);
										spinninpoint2 = spinninpoint2.RotatedByRandom(0.52359879016876221);
										float speed = 15f;
										Vector2 actualVelocity = NPC4.DirectionTo(player.Center + player.velocity * (NPC4.Distance(player.Center) / speed)) * speed;
										Projectile.NewProjectile(vector6.X, vector6.Y, actualVelocity.X, actualVelocity.Y, ProjectileID.Fireball, num7, 0f, Main.myPlayer);
									}
								}
							}
						}
					}
					int num23 = Math.Sign(player.Center.X - center.X);
					if (num23 != 0)
					{
						NPC.direction = (NPC.spriteDirection = num23);
					}
					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						vec2 = Vector2.Normalize(player.Center - center + player.velocity * 20f);
						if (vec2.HasNaNs())
						{
							vec2 = new Vector2(NPC.direction, 0f);
						}
						Vector2 vector7 = NPC.Center + new Vector2(NPC.direction * 30, 12f);
						for (int num24 = 0; num24 < 1; num24++)
						{
							Vector2 spinninpoint3 = vec2 * (6f + (float)Main.rand.NextDouble() * 4f);
							spinninpoint3 = spinninpoint3.RotatedByRandom(0.52359879016876221);
							Projectile.NewProjectile(vector7.X, vector7.Y, spinninpoint3.X, spinninpoint3.Y, ProjectileID.CultistBossFireBall, num5, 0f, Main.myPlayer);
						}
					}
				}
				NPC.ai[1] += 1f;
				if (NPC.ai[1] >= (float)(4 + num3 * num4))
				{
					NPC.ai[0] = 0f;
					NPC.ai[1] = 0f;
					NPC.ai[3] += 1f;
					NPC.velocity = Vector2.Zero;
					NPC.netUpdate = true;
				}
			}
			else if (NPC.ai[0] == 4f)
			{
				if (flag2)
				{
					NPC.localAI[2] = 12f;
				}
				else
				{
					NPC.localAI[2] = 11f;
				}
				if (NPC.ai[1] == 20f && flag2 && Main.netMode != NetmodeID.MultiplayerClient)
				{
					List<int> list5 = new List<int>();
					for (int num25 = 0; num25 < 200; num25++)
					{
						if (Main.npc[num25].active && Main.npc[num25].type == NPCID.CultistBossClone && Main.npc[num25].ai[3] == (float)NPC.whoAmI)
						{
							list5.Add(num25);
						}
					}
					foreach (int item5 in list5)
					{
						NPC NPC5 = Main.npc[item5];
						Vector2 center5 = NPC5.Center;
						int num26 = Math.Sign(player.Center.X - center5.X);
						if (num26 != 0)
						{
							NPC5.direction = (NPC5.spriteDirection = num26);
						}
						if (Main.netMode != NetmodeID.MultiplayerClient)
						{
							Vector2 vector8 = Vector2.Normalize(player.Center - center5 + player.velocity * 20f);
							if (vector8.HasNaNs())
							{
								vector8 = new Vector2(NPC.direction, 0f);
							}
							Vector2 vector9 = center5 + new Vector2(NPC.direction * 30, 12f);
							for (int num27 = 0; num27 < 1; num27++)
							{
								Vector2 spinninpoint4 = vector8 * (6f + (float)Main.rand.NextDouble() * 4f);
								spinninpoint4 = spinninpoint4.RotatedByRandom(0.52359879016876221);
								Projectile.NewProjectile(vector9.X, vector9.Y - 100f, spinninpoint4.X, spinninpoint4.Y, ProjectileID.CultistBossLightningOrb, num7, 0f, Main.myPlayer);
							}
						}
					}
					if ((int)(NPC.ai[1] - 20f) % num6 == 0)
					{
						float speed = 8.5f;
						Vector2 velocity = player.DirectionFrom(new Vector2(NPC.Center.X, NPC.Center.Y - 100f)) * speed;
						Projectile.NewProjectile(NPC.Center.X, NPC.Center.Y - 100f, velocity.X, velocity.Y, ProjectileID.CultistBossLightningOrb, num7, 0f, Main.myPlayer);
					}
				}
				NPC.ai[1] += 1f;
				if (NPC.ai[1] >= (float)(20 + num6))
				{
					NPC.ai[0] = 0f;
					NPC.ai[1] = 0f;
					NPC.ai[3] += 1f;
					NPC.velocity = Vector2.Zero;
					NPC.netUpdate = true;
				}
			}
			else if (NPC.ai[0] == 5f)
			{
				NPC.localAI[2] = 10f;
				if (Vector2.Normalize(player.Center - center).HasNaNs())
				{
					new Vector2(NPC.direction, 0f);
				}
				if (NPC.ai[1] >= 0f && NPC.ai[1] < 30f)
				{
					flag3 = true;
					flag4 = true;
					float num28 = (NPC.ai[1] - 0f) / 30f;
					NPC.alpha = (int)(num28 * 255f);
				}
				else if (NPC.ai[1] >= 30f && NPC.ai[1] < 90f)
				{
					if (NPC.ai[1] == 30f && Main.netMode != NetmodeID.MultiplayerClient && flag2)
					{
						NPC.localAI[1] += 1f;
						Vector2 spinningpoint = new Vector2(180f, 0f);
						List<int> list6 = new List<int>();
						for (int num29 = 0; num29 < 200; num29++)
						{
							if (Main.npc[num29].active && Main.npc[num29].type == NPCID.CultistBossClone && Main.npc[num29].ai[3] == (float)NPC.whoAmI)
							{
								list6.Add(num29);
							}
						}
						int num30 = 6 - list6.Count;
						if (num30 > 4)
						{
							num30 = 4;
						}
						int num31 = list6.Count + num30 + 1; //Number of clones
						float[] array = new float[num31];
						for (int num32 = 0; num32 < array.Length; num32++)
						{
							array[num32] = Vector2.Distance(NPC.Center + spinningpoint.RotatedBy((float)num32 * ((float)Math.PI * 2f) / (float)num31 - (float)Math.PI / 2f), player.Center);
						}
						int num33 = 0;
						for (int num34 = 1; num34 < array.Length; num34++)
						{
							if (array[num33] > array[num34])
							{
								num33 = num34;
							}
						}
						num33 = ((num33 >= num31 / 2) ? (num33 - num31 / 2) : (num33 + num31 / 2));
						int num35 = num30;
						for (int num36 = 0; num36 < array.Length; num36++)
						{
							if (num33 != num36)
							{
								Vector2 center6 = NPC.Center + spinningpoint.RotatedBy((float)num36 * ((float)Math.PI * 2f) / (float)num31 - (float)Math.PI / 2f);
								if (num35-- > 0)
								{
									int num37 = NPC.NewNPC((int)center6.X, (int)center6.Y + NPC.height / 2, NPCID.CultistBossClone, NPC.whoAmI);
									Main.npc[num37].ai[3] = NPC.whoAmI;
									Main.npc[num37].netUpdate = true;
									Main.npc[num37].localAI[1] = NPC.localAI[1];
								}
								else
								{
									int num38 = list6[-num35 - 1];
									Main.npc[num38].Center = center6;
									NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, num38);
								}
							}
						}
						NPC.ai[2] = Projectile.NewProjectile(NPC.Center.X, NPC.Center.Y, 0f, 0f, ProjectileID.CultistRitual, 0, 0f, Main.myPlayer, 0f, NPC.whoAmI);
						NPC.Center += spinningpoint.RotatedBy((float)num33 * ((float)Math.PI * 2f) / (float)num31 - (float)Math.PI / 2f);
						NPC.netUpdate = true;
						list6.Clear();
					}
					flag3 = true;
					flag4 = true;
					NPC.alpha = 255;
					if (flag2)
					{
						Vector2 vector10 = Main.projectile[(int)NPC.ai[2]].Center;
						vector10 -= NPC.Center;
						if (vector10 == Vector2.Zero)
						{
							vector10 = -Vector2.UnitY;
						}
						vector10.Normalize();
						if (Math.Abs(vector10.Y) < 0.77f)
						{
							NPC.localAI[2] = 11f;
						}
						else if (vector10.Y < 0f)
						{
							NPC.localAI[2] = 12f;
						}
						else
						{
							NPC.localAI[2] = 10f;
						}
						int num39 = Math.Sign(vector10.X);
						if (num39 != 0)
						{
							NPC.direction = (NPC.spriteDirection = num39);
						}
					}
					else
					{
						Vector2 vector11 = Main.projectile[(int)Main.npc[(int)NPC.ai[3]].ai[2]].Center;
						vector11 -= NPC.Center;
						if (vector11 == Vector2.Zero)
						{
							vector11 = -Vector2.UnitY;
						}
						vector11.Normalize();
						if (Math.Abs(vector11.Y) < 0.77f)
						{
							NPC.localAI[2] = 11f;
						}
						else if (vector11.Y < 0f)
						{
							NPC.localAI[2] = 12f;
						}
						else
						{
							NPC.localAI[2] = 10f;
						}
						int num40 = Math.Sign(vector11.X);
						if (num40 != 0)
						{
							NPC.direction = (NPC.spriteDirection = num40);
						}
					}
				}
				else if (NPC.ai[1] >= 90f && NPC.ai[1] < 120f)
				{
					flag3 = true;
					flag4 = true;
					float num41 = (NPC.ai[1] - 90f) / 30f;
					NPC.alpha = 255 - (int)(num41 * 255f);
				}
				else if (NPC.ai[1] >= 120f && NPC.ai[1] < 420f)
				{
					flag4 = true;
					NPC.alpha = 0;
					if (flag2)
					{
						Vector2 vector12 = Main.projectile[(int)NPC.ai[2]].Center;
						vector12 -= NPC.Center;
						if (vector12 == Vector2.Zero)
						{
							vector12 = -Vector2.UnitY;
						}
						vector12.Normalize();
						if (Math.Abs(vector12.Y) < 0.77f)
						{
							NPC.localAI[2] = 11f;
						}
						else if (vector12.Y < 0f)
						{
							NPC.localAI[2] = 12f;
						}
						else
						{
							NPC.localAI[2] = 10f;
						}
						int num42 = Math.Sign(vector12.X);
						if (num42 != 0)
						{
							NPC.direction = (NPC.spriteDirection = num42);
						}
					}
					else
					{
						Vector2 vector13 = Main.projectile[(int)Main.npc[(int)NPC.ai[3]].ai[2]].Center;
						vector13 -= NPC.Center;
						if (vector13 == Vector2.Zero)
						{
							vector13 = -Vector2.UnitY;
						}
						vector13.Normalize();
						if (Math.Abs(vector13.Y) < 0.77f)
						{
							NPC.localAI[2] = 11f;
						}
						else if (vector13.Y < 0f)
						{
							NPC.localAI[2] = 12f;
						}
						else
						{
							NPC.localAI[2] = 10f;
						}
						int num43 = Math.Sign(vector13.X);
						if (num43 != 0)
						{
							NPC.direction = (NPC.spriteDirection = num43);
						}
					}
				}
				NPC.ai[1] += 1f;
				if (NPC.ai[1] >= 420f)
				{
					flag4 = true;
					NPC.ai[0] = 0f;
					NPC.ai[1] = 0f;
					NPC.ai[3] += 1f;
					NPC.velocity = Vector2.Zero;
					NPC.netUpdate = true;
				}
			}
			else if (NPC.ai[0] == 6f)
			{
				NPC.localAI[2] = 13f;
				NPC.ai[1] += 1f;
				if (NPC.ai[1] >= 120f)
				{
					NPC.ai[0] = 0f;
					NPC.ai[1] = 0f;
					NPC.ai[3] += 1f;
					NPC.velocity = Vector2.Zero;
					NPC.netUpdate = true;
				}
			}
			else if (NPC.ai[0] == 7f)
			{
				NPC.localAI[2] = 11f;
				Vector2 vec3 = Vector2.Normalize(player.Center - center);
				if (vec3.HasNaNs())
				{
					vec3 = new Vector2(NPC.direction, 0f);
				}
				if (NPC.ai[1] >= 4f && flag2 && (int)(NPC.ai[1] - 4f) % num8 == 0)
				{
					if ((int)(NPC.ai[1] - 4f) / num8 == 2)
					{
						List<int> list7 = new List<int>();
						for (int num44 = 0; num44 < 200; num44++)
						{
							if (Main.npc[num44].active && Main.npc[num44].type == NPCID.CultistBossClone && Main.npc[num44].ai[3] == (float)NPC.whoAmI)
							{
								list7.Add(num44);
							}
						}
						foreach (int item6 in list7)
						{
							NPC NPC6 = Main.npc[item6];
							Vector2 center7 = NPC6.Center;
							int num45 = Math.Sign(player.Center.X - center7.X);
							if (num45 != 0)
							{
								NPC6.direction = (NPC6.spriteDirection = num45);
							}
							if (Main.netMode != NetmodeID.MultiplayerClient)
							{
								vec3 = Vector2.Normalize(player.Center - center7 + player.velocity * 20f);
								if (vec3.HasNaNs())
								{
									vec3 = new Vector2(NPC.direction, 0f);
								}
								Vector2 vector14 = center7 + new Vector2(NPC.direction * 30, 12f);
								for (int num46 = 0; (float)num46 < 5f; num46++)
								{
									Vector2 spinninpoint5 = vec3 * (6f + (float)Main.rand.NextDouble() * 4f);
									spinninpoint5 = spinninpoint5.RotatedByRandom(1.2566370964050293);
									Projectile.NewProjectile(vector14.X, vector14.Y, spinninpoint5.X, spinninpoint5.Y, ProjectileID.CultistBossFireBallClone, 18, 0f, Main.myPlayer);
								}
							}
						}
					}
					int num47 = Math.Sign(player.Center.X - center.X);
					if (num47 != 0)
					{
						NPC.direction = (NPC.spriteDirection = num47);
					}
					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						vec3 = Vector2.Normalize(player.Center - center + player.velocity * 20f);
						if (vec3.HasNaNs())
						{
							vec3 = new Vector2(NPC.direction, 0f);
						}
						Vector2 vector15 = NPC.Center + new Vector2(NPC.direction * 30, 12f);
						float num48 = 8f;
						float num49 = (float)Math.PI * 2f / 25f;
						for (int num50 = 0; (float)num50 < 5f; num50++)
						{
							Vector2 spinningpoint2 = vec3 * num48;
							spinningpoint2 = spinningpoint2.RotatedBy(num49 * (float)num50 - ((float)Math.PI * 2f / 5f - num49) / 2f);
							float ai = (Main.rand.NextFloat() - 0.5f) * 0.3f * ((float)Math.PI * 2f) / 60f;
							int num51 = NPC.NewNPC((int)vector15.X, (int)vector15.Y + 7, 522, 0, 0f, ai, spinningpoint2.X, spinningpoint2.Y);
							Main.npc[num51].velocity = spinningpoint2;
							Main.npc[num51].dontTakeDamage = true;
						}
					}
				}
				NPC.ai[1] += 1f;
				if (NPC.ai[1] >= (float)(4 + num8 * num9))
				{
					NPC.ai[0] = 0f;
					NPC.ai[1] = 0f;
					NPC.ai[3] += 1f;
					NPC.velocity = Vector2.Zero;
					NPC.netUpdate = true;
				}
			}
			else if (NPC.ai[0] == 8f)
			{
				NPC.localAI[2] = 13f;
				if (NPC.ai[1] >= 4f && flag2 && (int)(NPC.ai[1] - 4f) % num10 == 0)
				{
					List<int> list8 = new List<int>();
					for (int num52 = 0; num52 < 200; num52++)
					{
						if (Main.npc[num52].active && Main.npc[num52].type == NPCID.CultistBossClone && Main.npc[num52].ai[3] == (float)NPC.whoAmI)
						{
							list8.Add(num52);
						}
					}
					int num53 = list8.Count + 1;
					if (num53 > 3)
					{
						num53 = 3;
					}
					int num54 = Math.Sign(player.Center.X - center.X);
					if (num54 != 0)
					{
						NPC.direction = (NPC.spriteDirection = num54);
					}
					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						for (int num55 = 0; num55 < num53; num55++)
						{
							Point point = NPC.Center.ToTileCoordinates();
							Point point2 = Main.player[NPC.target].Center.ToTileCoordinates();
							Vector2 vector16 = Main.player[NPC.target].Center - NPC.Center;
							int num56 = 20;
							int num57 = 3;
							int num58 = 7;
							int num59 = 2;
							int num60 = 0;
							bool flag6 = false;
							if (vector16.Length() > 2000f)
							{
								flag6 = true;
							}
							while (!flag6 && num60 < 100)
							{
								num60++;
								int num61 = Main.rand.Next(point2.X - num56, point2.X + num56 + 1);
								int num62 = Main.rand.Next(point2.Y - num56, point2.Y + num56 + 1);
								if ((num62 < point2.Y - num58 || num62 > point2.Y + num58 || num61 < point2.X - num58 || num61 > point2.X + num58) && (num62 < point.Y - num57 || num62 > point.Y + num57 || num61 < point.X - num57 || num61 > point.X + num57) && !Main.tile[num61, num62].active())
								{
									bool flag7 = true;
									if (flag7 && Collision.SolidTiles(num61 - num59, num61 + num59, num62 - num59, num62 + num59))
									{
										flag7 = false;
									}
									if (flag7)
									{
										int newNPC = NPC.NewNPC(num61 * 16 + 8, num62 * 16 + 8, 523, 0, NPC.whoAmI);
										Main.npc[newNPC].dontTakeDamage = true;
										flag6 = true;
										break;
									}
								}
							}
						}
					}
				}
				NPC.ai[1] += 1f;
				if (NPC.ai[1] >= (float)(4 + num10 * num11))
				{
					NPC.ai[0] = 0f;
					NPC.ai[1] = 0f;
					NPC.ai[3] += 1f;
					NPC.velocity = Vector2.Zero;
					NPC.netUpdate = true;
				}
			}
			if (!flag2)
			{
				NPC.ai[3] = num13;
			}
			NPC.dontTakeDamage = flag3;
			NPC.chaseable = !flag4;
		}

		int invincible = 1;
		bool BoCPartTwo = false;
		bool BoCSpawnedNewCreepers = false;
		int BoCIchorCounter = 0;
		int BoCIchorAmount = 0;
		int BoCDustTelegraphCounter = 0;
		bool BoCHealed = false;
		public void BrainOfCthulhuAI(NPC NPC)
        {
			float teleportDistance = 350f;
			NPC.crimsonBoss = NPC.whoAmI;
			if (Main.netMode != 1 && NPC.localAI[0] == 0f)
			{
				NPC.localAI[0] = 1f;
				for (int num796 = 0; num796 < 20; num796++)
				{
					float x2 = NPC.Center.X;
					float y3 = NPC.Center.Y;
					x2 += (float)Main.rand.Next(-NPC.width, NPC.width);
					y3 += (float)Main.rand.Next(-NPC.height, NPC.height);
					int num797 = NPC.NewNPC((int)x2, (int)y3, NPCID.Creeper);
					Main.npc[num797].velocity = new Vector2((float)Main.rand.Next(-30, 31) * 0.1f, (float)Main.rand.Next(-30, 31) * 0.1f);
					Main.npc[num797].netUpdate = true;
					invincible *= -1;
					if (invincible == 1)
                    {
						Main.npc[num797].dontTakeDamage = true;
						Main.npc[num797].alpha = 120;
						Main.npc[num797].knockBackResist = 0f;
					}
				}
			}

			bool allInvincible = true;
			List<NPC> creepers = new List<NPC>();

			foreach (NPC creeper in Main.npc)
            {
				if (creeper.type == NPCID.Creeper && creeper.active)
                {
					creepers.Add(creeper);
					if (!creeper.dontTakeDamage)
                    {
						allInvincible = false;
                    }
                }
            }

			if (allInvincible)
            {
				BoCPartTwo = true;
				foreach (NPC creeper in creepers)
				{
					invincible *= -1;
					if (invincible == 1 || creepers.Count == 1)
					{
						creeper.dontTakeDamage = false;
						creeper.alpha = 0;
					}
					creeper.defense += 2;
                }
            }

			if (BoCSpawnedNewCreepers && NPC.AnyNPCs(NPCID.Creeper))
            {
				BoCIchorCounter++;
				if (BoCIchorAmount > 0)
                {
					if (BoCIchorCounter > 20)
					{
						BoCIchorAmount--;
						BoCIchorCounter = 0;

						NPC creeper = Main.rand.Next(creepers);
						Vector2 position = creeper.Center;
						float speed = 9f;
						Vector2 velocity = creeper.DirectionTo(Main.player[NPC.target].Center) * speed;
						int proj = Projectile.NewProjectile(position, velocity, ProjectileID.GoldenShowerHostile, 25 / 4, 0f);
						Main.projectile[proj].tileCollide = false;
					}
                }
                else
				{
					if (BoCIchorCounter > 250)
					{
						Terraria.Audio.SoundEngine.PlaySound(SoundID.Roar, (int)NPC.Center.X, (int)NPC.Center.Y, 0, 1, 1);
						TerrorbornSystem.ScreenShake(5f);
						BoCIchorCounter = 0;
						BoCIchorAmount = 13;
					}
				}
            }

			if (Main.netMode != 1)
			{
				NPC.TargetClosest();
				int num798 = 6000;
				if (Math.Abs(NPC.Center.X - Main.player[NPC.target].Center.X) + Math.Abs(NPC.Center.Y - Main.player[NPC.target].Center.Y) > (float)num798)
				{
					NPC.active = false;
					NPC.life = 0;
					if (Main.netMode == 2)
					{
						NetMessage.SendData(23, -1, -1, null, NPC.whoAmI);
					}
				}
			}
			if (NPC.ai[0] < 0f)
			{
				if (NPC.localAI[2] == 0f)
				{
					Terraria.Audio.SoundEngine.PlaySound(3, (int)NPC.position.X, (int)NPC.position.Y);
					NPC.localAI[2] = 1f;
					Gore.NewGore(NPC.position, new Vector2((float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f), 392);
					Gore.NewGore(NPC.position, new Vector2((float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f), 393);
					Gore.NewGore(NPC.position, new Vector2((float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f), 394);
					Gore.NewGore(NPC.position, new Vector2((float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f), 395);
					for (int num799 = 0; num799 < 20; num799++)
					{
						Dust.NewDust(NPC.position, NPC.width, NPC.height, 5, (float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f);
					}
					Terraria.Audio.SoundEngine.PlaySound(15, (int)NPC.position.X, (int)NPC.position.Y, 0);
				}

				NPC.TargetClosest();
				float speed = 5f;
				if (BoCSpawnedNewCreepers && !NPC.AnyNPCs(NPCID.Creeper))
				{
					speed = 7.5f;
					if (!BoCHealed)
                    {
						BoCHealed = true;
						NPC.life = NPC.lifeMax;
					}
				}
				NPC.velocity = NPC.DirectionTo(Main.player[NPC.target].Center) * speed;

				if (NPC.life <= NPC.lifeMax / 4 && !BoCSpawnedNewCreepers)
				{
					BoCSpawnedNewCreepers = true;
					for (int num796 = 0; num796 < 10; num796++)
					{
						float x2 = NPC.Center.X;
						float y3 = NPC.Center.Y;
						x2 += (float)Main.rand.Next(-NPC.width, NPC.width);
						y3 += (float)Main.rand.Next(-NPC.height, NPC.height);
						int num797 = NPC.NewNPC((int)x2, (int)y3, NPCID.Creeper);
						Main.npc[num797].velocity = new Vector2((float)Main.rand.Next(-30, 31) * 0.1f, (float)Main.rand.Next(-30, 31) * 0.1f);
						Main.npc[num797].netUpdate = true;
						invincible *= -1;
						if (invincible == 1)
						{
							Main.npc[num797].dontTakeDamage = true;
							Main.npc[num797].alpha = 120;
							Main.npc[num797].knockBackResist = 0f;
						}
						Main.npc[num797].defense += 4;
					}
				}

				NPC.dontTakeDamage = NPC.AnyNPCs(NPCID.Creeper);

				if (NPC.ai[0] == -1f)
				{
					if (Main.netMode != 1)
					{
						NPC.localAI[1] += 1f;
						if (NPC.justHit)
						{
							NPC.localAI[1] -= Main.rand.Next(5);
						}
						int num804 = 60 + Main.rand.Next(120);
						if (Main.netMode != 0)
						{
							num804 += Main.rand.Next(30, 90);
						}
						if (NPC.localAI[1] >= (float)num804)
						{
							NPC.localAI[1] = 0f;
							NPC.TargetClosest();
							int num814 = 0;
							Vector2 vector = NPC.DirectionTo(Main.player[NPC.target].Center) * teleportDistance + Main.player[NPC.target].Center;
							float num815 = vector.X / 16f;
							float num816 = vector.Y / 16f;
							NPC.ai[0] = 1f;
							NPC.ai[1] = num815;
							NPC.ai[0] = 1f;
							NPC.ai[2] = num816;
							NPC.netUpdate = true;
						}
					}
				}
				else if (NPC.ai[0] == -2f)
				{
					NPC.velocity *= 0.9f;
					if (Main.netMode != 0)
					{
						NPC.ai[3] += 15f;
					}
					else
					{
						NPC.ai[3] += 25f;
					}
					BoCDustTelegraphCounter++;
					if (BoCDustTelegraphCounter >= 5)
                    {
						BoCDustTelegraphCounter = 0;
						DustExplosion(new Vector2(NPC.ai[1] * 16f, NPC.ai[2] * 16f), 0, 20, 20f, DustID.GoldFlame, 1.5f, true);
                    }
					if (NPC.ai[3] >= 255f)
					{
						NPC.ai[3] = 255f;
						NPC.position.X = NPC.ai[1] * 16f - (float)(NPC.width / 2);
						NPC.position.Y = NPC.ai[2] * 16f - (float)(NPC.height / 2);
						Terraria.Audio.SoundEngine.PlaySound(SoundID.Item8, NPC.Center);
						NPC.ai[0] = -3f;
						NPC.netUpdate = true;
						NPC.netSpam = 0;
					}
					NPC.alpha = (int)NPC.ai[3];
				}
				else if (NPC.ai[0] == -3f)
				{
					if (Main.netMode != 0)
					{
						NPC.ai[3] -= 15f;
					}
					else
					{
						NPC.ai[3] -= 25f;
					}
					if (NPC.ai[3] <= 0f)
					{
						NPC.ai[3] = 0f;
						NPC.ai[0] = -1f;
						NPC.netUpdate = true;
						NPC.netSpam = 0;
					}
					NPC.alpha = (int)NPC.ai[3];
				}
			}
			else
			{
				NPC.TargetClosest();
				float speed = 1f;
				if (BoCPartTwo)
                {
					speed = 3f;
                }
				if (BoCSpawnedNewCreepers)
				{
					speed = 5f;
				}
				NPC.velocity = NPC.DirectionTo(Main.player[NPC.target].Center) * speed;
				if (NPC.ai[0] == 0f)
				{
					if (Main.netMode != 1)
					{
						int num812 = 0;
						for (int num813 = 0; num813 < 200; num813++)
						{
							if (Main.npc[num813].active && Main.npc[num813].type == 267)
							{
								num812++;
							}
						}
						if (num812 == 0)
						{
							NPC.ai[0] = -1f;
							NPC.localAI[1] = 0f;
							NPC.alpha = 0;
							NPC.netUpdate = true;
						}
						NPC.localAI[1] += 1f;
						if (NPC.localAI[1] >= (float)(120 + Main.rand.Next(300)))
						{
							NPC.localAI[1] = 0f;
							NPC.TargetClosest();
							int num814 = 0;
							Vector2 vector = NPC.DirectionTo(Main.player[NPC.target].Center) * teleportDistance + Main.player[NPC.target].Center;
							float num815 = vector.X / 16f;
							float num816 = vector.Y / 16f;
							NPC.ai[0] = 1f;
							NPC.ai[1] = num815;
							NPC.ai[0] = 1f;
							NPC.ai[2] = num816;
							NPC.netUpdate = true;
						}
					}
				}
				else if (NPC.ai[0] == 1f)
				{
					BoCDustTelegraphCounter++;
					if (BoCDustTelegraphCounter >= 5)
					{
						BoCDustTelegraphCounter = 0;
						DustExplosion(new Vector2(NPC.ai[1] * 16f, NPC.ai[2] * 16f), 0, 20, 20f, DustID.GoldFlame, 1.5f, true);
					}
					NPC.alpha += 5;
					if (NPC.alpha >= 255)
					{
						Terraria.Audio.SoundEngine.PlaySound(SoundID.Item8, NPC.Center);
						NPC.alpha = 255;
						NPC.position.X = NPC.ai[1] * 16f - (float)(NPC.width / 2);
						NPC.position.Y = NPC.ai[2] * 16f - (float)(NPC.height / 2);
						NPC.ai[0] = 2f;
					}
				}
				else if (NPC.ai[0] == 2f)
				{
					NPC.alpha -= 5;
					if (NPC.alpha <= 0)
					{
						NPC.alpha = 0;
						NPC.ai[0] = 0f;
					}
				}
			}
			if (Main.player[NPC.target].dead || !Main.player[NPC.target].ZoneCrimson)
			{
				if (NPC.localAI[3] < 120f)
				{
					NPC.localAI[3]++;
				}
				if (NPC.localAI[3] > 60f)
				{
					NPC.velocity.Y += (NPC.localAI[3] - 60f) * 0.25f;
				}
				NPC.ai[0] = 2f;
				NPC.alpha = 10;
			}
			else if (NPC.localAI[3] > 0f)
			{
				NPC.localAI[3]--;
			}
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
				Main.wofB = -1;
				Main.wofT = -1;
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
				if (Main.netMode != 1)
				{
					int num334 = NPC.NewNPC((int)(NPC.position.X + (float)(NPC.width / 2)), (int)(NPC.position.Y + (float)(NPC.height / 2) + 20f), 117, 1);
					Main.npc[num334].velocity.X = NPC.direction * 8;
				}
			}
			NPC.localAI[3] += 1f;
			if (NPC.localAI[3] >= (float)(600 + Main.rand.Next(1000)))
			{
				NPC.localAI[3] = -Main.rand.Next(200);
				Terraria.Audio.SoundEngine.PlaySound(4, (int)NPC.position.X, (int)NPC.position.Y, 10);
			}
			Main.wof = NPC.whoAmI;
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
						if (WorldGen.SolidTile(num340, num339) || Main.tile[num340, num339].liquid > 0)
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
			if (Main.wofB == -1)
			{
				Main.wofB = num339 * 16;
			}
			else if (Main.wofB > num339 * 16)
			{
				Main.wofB--;
				if (Main.wofB < num339 * 16)
				{
					Main.wofB = num339 * 16;
				}
			}
			else if (Main.wofB < num339 * 16)
			{
				Main.wofB++;
				if (Main.wofB > num339 * 16)
				{
					Main.wofB = num339 * 16;
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
						if (WorldGen.SolidTile(num341, num339) || Main.tile[num341, num339].liquid > 0)
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
			if (Main.wofT == -1)
			{
				Main.wofT = num339 * 16;
			}
			else if (Main.wofT > num339 * 16)
			{
				Main.wofT--;
				if (Main.wofT < num339 * 16)
				{
					Main.wofT = num339 * 16;
				}
			}
			else if (Main.wofT < num339 * 16)
			{
				Main.wofT++;
				if (Main.wofT > num339 * 16)
				{
					Main.wofT = num339 * 16;
				}
			}
			float num342 = (Main.wofB + Main.wofT) / 2 - NPC.height / 2;
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
			if (Main.expertMode && Main.netMode != 1)
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
				if (Main.rand.Next(num349) == 0)
				{
					int num350 = 0;
					float[] array = new float[10];
					for (int num351 = 0; num351 < 200; num351++)
					{
						if (num350 < 10 && Main.npc[num351].active && Main.npc[num351].type == 115)
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
							int num357 = NPC.NewNPC((int)NPC.position.X, (int)num342, 115, NPC.whoAmI);
							Main.npc[num357].ai[0] = (float)num352 * 0.1f - 0.05f;
						}
					}
				}
			}
			if (NPC.localAI[0] == 1f && Main.netMode != 1)
			{
				NPC.localAI[0] = 2f;
				num342 = (Main.wofB + Main.wofT) / 2;
				num342 = (num342 + (float)Main.wofT) / 2f;

				int num358 = NPC.NewNPC((int)NPC.position.X, (int)num342, 114, NPC.whoAmI);
				Main.npc[num358].ai[0] = 1f;
				num342 = (Main.wofB + Main.wofT) / 2;
				num342 = (num342 + (float)Main.wofB) / 2f;

				topEye = Main.npc[num358];
				topEye.realLife = NPC.whoAmI;

				num358 = NPC.NewNPC((int)NPC.position.X, (int)num342, 114, NPC.whoAmI);
				Main.npc[num358].ai[0] = -1f;
				num342 = (Main.wofB + Main.wofT) / 2;
				num342 = (num342 + (float)Main.wofB) / 2f;

				bottomEye = Main.npc[num358];
				bottomEye.realLife = NPC.whoAmI;

				for (int num359 = 0; num359 < 11; num359++)
				{
					num358 = NPC.NewNPC((int)NPC.position.X, (int)num342, 115, NPC.whoAmI);
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
					int proj = Projectile.NewProjectile(topEye.Center, topEyeRot.ToRotationVector2() * 7.5f, ProjectileID.GoldenShowerHostile, 60 / 4, 0f, player.whoAmI);
					Main.projectile[proj].tileCollide = false;
					proj = Projectile.NewProjectile(bottomEye.Center, bottomEyeRot.ToRotationVector2() * 15f, ProjectileID.CursedFlameHostile, 60 / 4, 0f, player.whoAmI);
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
					int proj = Projectile.NewProjectile(topEye.Center, topEyeRot.ToRotationVector2() * 10f, ProjectileID.EyeLaser, 60 / 4, 0f, player.whoAmI);
					Main.projectile[proj].tileCollide = false;
					proj = Projectile.NewProjectile(bottomEye.Center, bottomEyeRot.ToRotationVector2() * 10f, ProjectileID.EyeLaser, 60 / 4, 0f, player.whoAmI);
					Main.projectile[proj].tileCollide = false;
				}
			}
		}

		public void WoFEyeAI(NPC NPC)
        {
			if (Main.wof < 0)
			{
				NPC.active = false;
				return;
			}
			NPC.realLife = Main.wof;
			if (Main.npc[Main.wof].life > 0)
			{
				NPC.life = Main.npc[Main.wof].life;
			}
			NPC.TargetClosest();
			NPC.position.X = Main.npc[Main.wof].position.X;
			NPC.direction = Main.npc[Main.wof].direction;
			NPC.spriteDirection = NPC.direction;
			float num360 = (Main.wofB + Main.wofT) / 2;
			num360 = ((!(NPC.ai[0] > 0f)) ? ((num360 + (float)Main.wofB) / 2f) : ((num360 + (float)Main.wofT) / 2f));
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
			if (Main.netMode == 1)
			{
				return;
			}
			int num365 = 4;
			NPC.localAI[1] += 1f;
			if ((double)Main.npc[Main.wof].life < (double)Main.npc[Main.wof].lifeMax * 0.75)
			{
				NPC.localAI[1] += 1f;
				num365++;
			}
			if ((double)Main.npc[Main.wof].life < (double)Main.npc[Main.wof].lifeMax * 0.5)
			{
				NPC.localAI[1] += 1f;
				num365++;
			}
			if ((double)Main.npc[Main.wof].life < (double)Main.npc[Main.wof].lifeMax * 0.25)
			{
				NPC.localAI[1] += 1f;
				num365 += 2;
			}
			if ((double)Main.npc[Main.wof].life < (double)Main.npc[Main.wof].lifeMax * 0.1)
			{
				NPC.localAI[1] += 2f;
				num365 += 3;
			}
			if (Main.expertMode)
			{
				NPC.localAI[1] += 0.5f;
				num365++;
				if ((double)Main.npc[Main.wof].life < (double)Main.npc[Main.wof].lifeMax * 0.1)
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
					if ((double)Main.npc[Main.wof].life < (double)Main.npc[Main.wof].lifeMax * 0.5)
					{
						num367++;
						num366 += 1f;
					}
					if ((double)Main.npc[Main.wof].life < (double)Main.npc[Main.wof].lifeMax * 0.25)
					{
						num367++;
						num366 += 1f;
					}
					if ((double)Main.npc[Main.wof].life < (double)Main.npc[Main.wof].lifeMax * 0.1)
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

		public void KingSlimeAI(NPC NPC)
        {
			float num234 = 1f;
			bool flag8 = false;
			bool flag9 = false;
			NPC.aiAction = 0;
			if (NPC.ai[3] == 0f && NPC.life > 0)
			{
				NPC.ai[3] = NPC.lifeMax;
			}
			if (NPC.localAI[3] == 0f && Main.netMode != 1)
			{
				NPC.ai[0] = -100f;
				NPC.localAI[3] = 1f;
				NPC.TargetClosest();
				NPC.netUpdate = true;
			}
			if (Main.player[NPC.target].dead)
			{
				NPC.TargetClosest();
				if (Main.player[NPC.target].dead)
				{
					NPC.timeLeft = 0;
					if (Main.player[NPC.target].Center.X < NPC.Center.X)
					{
						NPC.direction = 1;
					}
					else
					{
						NPC.direction = -1;
					}
				}
			}
			if (!Main.player[NPC.target].dead && NPC.ai[2] >= 400f && NPC.ai[1] < 5f && NPC.velocity.Y == 0f)
			{
				NPC.ai[2] = 0f;
				NPC.ai[0] = 0f;
				NPC.ai[1] = 5f;
				if (Main.netMode != 1)
				{
					NPC.TargetClosest(false);
					Point point3 = NPC.Center.ToTileCoordinates();
					Point point4 = Main.player[NPC.target].Center.ToTileCoordinates();
					Vector2 vector30 = Main.player[NPC.target].Center - NPC.Center;
					int num235 = 10;
					int num236 = 0;
					int num237 = 7;
					int num238 = 0;
					bool flag10 = false;
					if (vector30.Length() > 500f)
					{
						flag10 = true;
						num238 = 100;
					}
					while (!flag10 && num238 < 100)
					{
						num238++;
						int num239 = Main.rand.Next(point4.X - num235, point4.X + num235 + 1);
						int num240 = Main.rand.Next(point4.Y - num235, point4.Y + 1);
						if ((num240 >= point4.Y - num237 && num240 <= point4.Y + num237 && num239 >= point4.X - num237 && num239 <= point4.X + num237) || (num240 >= point3.Y - num236 && num240 <= point3.Y + num236 && num239 >= point3.X - num236 && num239 <= point3.X + num236) || Main.tile[num239, num240].nactive())
						{
							continue;
						}
						int num241 = num240;
						int num242 = 0;
						if (Main.tile[num239, num241].nactive() && Main.tileSolid[Main.tile[num239, num241].type] && !Main.tileSolidTop[Main.tile[num239, num241].type])
						{
							num242 = 1;
						}
						else
						{
							for (; num242 < 150 && num241 + num242 < Main.maxTilesY; num242++)
							{
								int num243 = num241 + num242;
								if (Main.tile[num239, num243].nactive() && Main.tileSolid[Main.tile[num239, num243].type] && !Main.tileSolidTop[Main.tile[num239, num243].type])
								{
									num242--;
									break;
								}
							}
						}
						num240 += num242;
						bool flag11 = true;
						if (flag11 && Main.tile[num239, num240].lava())
						{
							flag11 = false;
						}
						if (flag11 && !Collision.CanHitLine(NPC.Center, 0, 0, Main.player[NPC.target].Center, 0, 0))
						{
							flag11 = false;
						}
						if (flag11)
						{
							NPC.localAI[1] = num239 * 16 + 8;
							NPC.localAI[2] = num240 * 16 + 16;
							flag10 = true;
							break;
						}
					}
					Player player = Main.player[NPC.target];
					Vector2 bottom = new Vector2(player.Center.X + Math.Sign(player.Center.X - NPC.Center.X) * 400f, player.Center.Y - 500f).findGroundUnder(); //Set teleport position
					NPC.localAI[1] = bottom.X;
					NPC.localAI[2] = bottom.Y;
					DustExplosion(bottom, 0, 40, 25f, DustID.t_Slime, Color.Azure, 2f, true);
					if (num238 >= 100)
					{
					}
				}
			}
			NPC.ai[2]++;
			if (Math.Abs(NPC.Top.Y - Main.player[NPC.target].Bottom.Y) > 320f)
			{
				NPC.ai[2]++;
			}
			Dust dust28;
			Dust dust2;
			if (NPC.ai[1] == 5f) //Entering teleport
			{
				flag8 = true;
				NPC.aiAction = 1;
				NPC.ai[0]++;
				num234 = MathHelper.Clamp((60f - NPC.ai[0]) / 60f, 0f, 1f);
				num234 = 0.5f + num234 * 0.5f;
				if (NPC.ai[0] >= 60f)
				{
					flag9 = true;
				}
				if (NPC.ai[0] == 60f)
				{
					Gore.NewGore(NPC.Center + new Vector2(-40f, -NPC.height / 2), NPC.velocity, 734);
				}
				if (NPC.ai[0] >= 60f && Main.netMode != 1)
				{
					NPC.Bottom = new Vector2(NPC.localAI[1], NPC.localAI[2]);
					NPC.ai[1] = 6f;
					NPC.ai[0] = 0f;
					NPC.netUpdate = true;
				}
				if (Main.netMode == 1 && NPC.ai[0] >= 120f)
				{
					NPC.ai[1] = 6f;
					NPC.ai[0] = 0f;
				}
				if (!flag9)
				{
					for (int num244 = 0; num244 < 10; num244++)
					{
						int num245 = Dust.NewDust(NPC.position + Vector2.UnitX * -20f, NPC.width + 40, NPC.height, 4, NPC.velocity.X, NPC.velocity.Y, 150, new Color(78, 136, 255, 80), 2f);
						Main.dust[num245].noGravity = true;
						dust28 = Main.dust[num245];
						dust2 = dust28;
						dust2.velocity *= 0.5f;
					}
				}
			}
			else if (NPC.ai[1] == 6f) //Exiting Teleport
			{
				flag8 = true;
				NPC.aiAction = 0;
				NPC.ai[0]++;
				num234 = MathHelper.Clamp(NPC.ai[0] / 30f, 0f, 1f);
				num234 = 0.5f + num234 * 0.5f;
				if (NPC.ai[0] >= 30f && Main.netMode != 1)
				{
					NPC.ai[1] = 0f;
					NPC.ai[0] = 0f;
					NPC.netUpdate = true;
					NPC.TargetClosest();
				}
				if (Main.netMode == 1 && NPC.ai[0] >= 60f)
				{
					NPC.ai[1] = 0f;
					NPC.ai[0] = 0f;
					NPC.TargetClosest();
				}
				for (int num246 = 0; num246 < 10; num246++)
				{
					int num247 = Dust.NewDust(NPC.position + Vector2.UnitX * -20f, NPC.width + 40, NPC.height, 4, NPC.velocity.X, NPC.velocity.Y, 150, new Color(78, 136, 255, 80), 2f);
					Main.dust[num247].noGravity = true;
					dust28 = Main.dust[num247];
					dust2 = dust28;
					dust2.velocity *= 2f;
				}
			}
			NPC.dontTakeDamage = (NPC.hide = flag9);
			if (NPC.velocity.Y == 0f)
			{
				NPC.velocity.X *= 0.8f;
				if ((double)NPC.velocity.X > -0.1 && (double)NPC.velocity.X < 0.1)
				{
					NPC.velocity.X = 0f;
				}
				if (!flag8)
				{
					NPC.ai[0] += 4f;
					if ((double)NPC.life < (double)NPC.lifeMax * 0.8)
					{
						NPC.ai[0] += 1f;
					}
					if ((double)NPC.life < (double)NPC.lifeMax * 0.6)
					{
						NPC.ai[0] += 1f;
					}
					if ((double)NPC.life < (double)NPC.lifeMax * 0.4)
					{
						NPC.ai[0] += 2f;
					}
					if ((double)NPC.life < (double)NPC.lifeMax * 0.2)
					{
						NPC.ai[0] += 3f;
					}
					if ((double)NPC.life < (double)NPC.lifeMax * 0.1)
					{
						NPC.ai[0] += 4f;
					}
					if (NPC.ai[0] >= 0f)
					{
						NPC.netUpdate = true;
						NPC.TargetClosest();
						if (NPC.ai[1] == 3f) //Big jump
						{
							NPC.velocity.Y = -10f;
							NPC.velocity.X += 4.5f * (float)NPC.direction;
							NPC.ai[0] = -200f;
							NPC.ai[1] = 0f;

							float intervals = 0.1f;
							float ySpeed = 7.5f;
							if (NPC.life <= NPC.lifeMax * 0.75f)
                            {
								intervals = 0.075f;
							}
							if (NPC.life <= NPC.lifeMax * 0.5f)
							{
								intervals = 0.05f;
							}
							if (NPC.life <= NPC.lifeMax * 0.25f)
							{
								intervals = 0.035f;
								ySpeed = 11f;
							}
							Player player = Main.player[NPC.target];
							if (player.Center.Y < NPC.Top.Y)
                            {
								ySpeed *= 1.4f;
                            }
							for (float i = -0.5f; i <= 0.5f; i += intervals)
                            {
								float maxSpeedX = 15f;
								Vector2 position = NPC.Center + new Vector2(NPC.width * i, -NPC.height / 2);
								Projectile.NewProjectile(position, new Vector2(maxSpeedX * i * 2f, -ySpeed), ProjectileID.SpikedSlimeSpike, 60 / 4, 0f);
                            }
						}
						else if (NPC.ai[1] == 2f) //Horizontal jump/short jump
						{
							NPC.velocity.Y = -6f;
							NPC.velocity.X += 6f * (float)NPC.direction;
							NPC.ai[0] = -120f;
							NPC.ai[1] += 1f;
						}
						else //Regular jump
						{
							NPC.velocity.Y = -8f;
							NPC.velocity.X += 5f * (float)NPC.direction;
							NPC.ai[0] = -120f;
							NPC.ai[1] += 1f;
						}
					}
					else if (NPC.ai[0] >= -30f)
					{
						NPC.aiAction = 1;
					}
				}
			}
			else if (NPC.target < 255 && ((NPC.direction == 1 && NPC.velocity.X < 3f) || (NPC.direction == -1 && NPC.velocity.X > -3f)))
			{
				if ((NPC.direction == -1 && (double)NPC.velocity.X < 0.1) || (NPC.direction == 1 && (double)NPC.velocity.X > -0.1))
				{
					NPC.velocity.X += 0.2f * (float)NPC.direction;
				}
				else
				{
					NPC.velocity.X *= 0.93f;
				}
			}
			int num248 = Dust.NewDust(NPC.position, NPC.width, NPC.height, 4, NPC.velocity.X, NPC.velocity.Y, 255, new Color(0, 80, 255, 80), NPC.scale * 1.2f);
			Main.dust[num248].noGravity = true;
			dust28 = Main.dust[num248];
			dust2 = dust28;
			dust2.velocity *= 0.5f;
			if (NPC.life <= 0)
			{
				return;
			}
			float num249 = (float)NPC.life / (float)NPC.lifeMax;
			num249 = num249 * 0.5f + 0.75f;
			num249 *= num234;
			if (num249 != NPC.scale)
			{
				NPC.position.X += NPC.width / 2;
				NPC.position.Y += NPC.height;
				NPC.scale = num249;
				NPC.width = (int)(98f * NPC.scale);
				NPC.height = (int)(92f * NPC.scale);
				NPC.position.X -= NPC.width / 2;
				NPC.position.Y -= NPC.height;
			}
			if (Main.netMode == 1)
			{
				return;
			}
			int num250 = (int)((double)NPC.lifeMax * 0.05);
			if (!((float)(NPC.life + num250) < NPC.ai[3]))
			{
				return;
			}
			NPC.ai[3] = NPC.life;
			int num251 = Main.rand.Next(1, 4);
			for (int num252 = 0; num252 < num251; num252++)
			{
				int x = (int)(NPC.position.X + (float)Main.rand.Next(NPC.width - 32));
				int y = (int)(NPC.position.Y + (float)Main.rand.Next(NPC.height - 32));
				int num253 = 1;
				if (Main.expertMode && Main.rand.Next(4) == 0)
				{
					num253 = 535;
				}
				if (Main.rand.Next(6) == 0)
				{
					num253 = ModContent.NPCType<NPCs.TerrorSlime>();
				}
				int num254 = NPC.NewNPC(x, y, num253);
				Main.npc[num254].SetDefaults(num253);
				Main.npc[num254].velocity.X = (float)Main.rand.Next(-15, 16) * 0.1f;
				Main.npc[num254].velocity.Y = (float)Main.rand.Next(-30, 1) * 0.1f;
				Main.npc[num254].ai[0] = -1000 * Main.rand.Next(3);
				Main.npc[num254].ai[1] = 0f;
				if (num253 != ModContent.NPCType<NPCs.TerrorSlime>())
				{
					Main.npc[num254].lifeMax /= 2;
					Main.npc[num254].life = Main.npc[num254].lifeMax;
				}
				if (Main.netMode == 2 && num254 < 200)
				{
					NetMessage.SendData(23, -1, -1, null, num254);
				}
			}
		}

		public void DustExplosion(Vector2 position, int RectWidth, int Streams, float DustSpeed, int DustType, float DustScale = 1f, bool NoGravity = false) //Thank you once again Seraph
		{
			float currentAngle = Main.rand.Next(360);

			//if(Main.netMode!=1){
			for (int i = 0; i < Streams; ++i)
			{

				Vector2 direction = Vector2.Normalize(new Vector2(1, 1)).RotatedBy(MathHelper.ToRadians(((360 / Streams) * i) + currentAngle));
				direction.X *= DustSpeed;
				direction.Y *= DustSpeed;

				Dust dust = Dust.NewDustPerfect(position + (new Vector2(Main.rand.Next(RectWidth), Main.rand.Next(RectWidth))), DustType, direction, 0, default(Color), DustScale);
				if (NoGravity)
				{
					dust.noGravity = true;
				}
			}
		}

		public void DustExplosion(Vector2 position, int RectWidth, int Streams, float DustSpeed, int DustType, Color color, float DustScale = 1f, bool NoGravity = false) //Thank you once again Seraph
		{
			float currentAngle = Main.rand.Next(360);

			//if(Main.netMode!=1){
			for (int i = 0; i < Streams; ++i)
			{

				Vector2 direction = Vector2.Normalize(new Vector2(1, 1)).RotatedBy(MathHelper.ToRadians(((360 / Streams) * i) + currentAngle));
				direction.X *= DustSpeed;
				direction.Y *= DustSpeed;

				Dust dust = Dust.NewDustPerfect(position + (new Vector2(Main.rand.Next(RectWidth), Main.rand.Next(RectWidth))), DustType, direction, 0, default(Color), DustScale);
				if (NoGravity)
				{
					dust.noGravity = true;
				}
				dust.color = color;
			}
		}

		public override void HitEffect(NPC NPC, int hitDirection, double damage)
        {
			if (NPC.type == NPCID.BrainofCthulhu && twilight && !BoCSpawnedNewCreepers)
            {
				if (NPC.life <= 0)
                {
					NPC.life = 1;
                }
            }
        }
    }

	class EoCFlameThrower : ModProjectile
    {
		//259, 270, 271, 6 = potential dust
		public override string Texture => "TerrorbornMod/WhitePixel";

		public override void SetDefaults()
		{
			Projectile.width = 35;
			Projectile.height = 35;
			Projectile.friendly = false;
			Projectile.hostile = true;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = false;
			Projectile.penetrate = 1;
			Projectile.alpha = 255;
			Projectile.timeLeft = 70;
		}

		float dustScale = 0f;
        public override void AI()
        {
			if (dustScale < 1f)
            {
				dustScale += 0.05f;
            }
			for (int i = 0; i < 3; i++)
			{
				int type = 0;
				switch (Main.rand.Next(4))
				{
					case 0:
						type = 6;
						break;
					case 1:
						type = 259;
						break;
					case 2:
						type = 270;
						break;
					case 3:
						type = 271;
						break;
				}
				Dust dust = Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, type)];
				dust.noGravity = true;
				dust.color = Color.Red;
				dust.velocity = Projectile.velocity * Main.rand.NextFloat();
				dust.scale = Main.rand.NextFloat(2f, 2.5f) * dustScale;
				//if (Main.rand.NextFloat() <= 0.05f) dust.noGravity = false;
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
					TBUtils.Graphics.DrawGlow_1(Main.spriteBatch, drawPos, (int)(25f * mult), color);
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