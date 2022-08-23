using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;

namespace TerrorbornMod.TwilightMode.NPCs.Bosses
{
    class EyeOfCthulhu : TwilightNPCChange
    {
        public override bool HasNewAI(NPC npc)
        {
            return true;
        }

        public override void NewAI(NPC npc)
        {
            EyeOfCthulhuAI(npc);
        }

        public override bool ShouldChangeNPC(NPC npc)
        {
            return npc.type == NPCID.EyeofCthulhu;
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
			if (Main.rand.NextBool(5))
			{
				int num9 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y + (float)NPC.height * 0.25f), NPC.width, (int)((float)NPC.height * 0.5f), DustID.Blood, NPC.velocity.X, 2f);
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
							SoundExtensions.PlaySoundOld(SoundID.NPCDeath10, NPC.Center);
						}
					}
					else
					{
						NPC.rotation += MathHelper.ToRadians(MathHelper.Lerp(1.1f, 0.8f, (float)NPC.life / (float)NPC.lifeMax / 0.33f)) * EoCFlameDir;
						EoCAttackCounter++;
						if (EoCAttackCounter % 3 == 2)
						{
							SoundExtensions.PlaySoundOld(SoundID.Item34, NPC.Center);
							Projectile.NewProjectile(NPC.GetSource_ReleaseEntity(), NPC.Center, NPC.rotation.ToRotationVector2().RotatedBy(MathHelper.ToRadians(90f)) * 24f, ModContent.ProjectileType<EoCFlameThrower>(), 60 / 4, 0f);
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
						int num22 = NPC.NewNPC(NPC.GetSource_ReleaseEntity(), (int)NPC.Center.X, (int)NPC.Center.Y, NPCID.ServantofCthulhu);
						Main.npc[num22].lifeMax = (int)MathHelper.Lerp(25f, 40f, (float)NPC.life / (float)NPC.lifeMax / 0.33f);
						Main.npc[num22].life = Main.npc[num22].lifeMax;
						SoundExtensions.PlaySoundOld(SoundID.NPCDeath13, NPC.Center);
					}
					if (EoCAttackCounter > 90)
					{
						NPC.velocity = NPC.DirectionTo(player.Center) * 25f;
						NPC.rotation = NPC.DirectionTo(player.Center).ToRotation() - MathHelper.ToRadians(90f);
						EoCAutoRotate = false;
						EoCAttackCounter = 0;
						SoundExtensions.PlaySoundOld(SoundID.ForceRoar, (int)NPC.position.X, (int)NPC.position.Y, -1);
						NPC.ai[1] = 2;
					}
				}
				else if (NPC.ai[1] == 2)
				{
					EoCAttackCounter++;
					if (EoCAttackCounter == 29)
					{
						int num22 = NPC.NewNPC(NPC.GetSource_ReleaseEntity(), (int)NPC.Center.X, (int)NPC.Center.Y, NPCID.ServantofCthulhu);
						Main.npc[num22].lifeMax = (int)MathHelper.Lerp(35f, 55f, (float)NPC.life / (float)NPC.lifeMax / 0.33f);
						Main.npc[num22].life = Main.npc[num22].lifeMax;
						SoundExtensions.PlaySoundOld(SoundID.NPCDeath13, NPC.Center);
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
								int num22 = NPC.NewNPC(NPC.GetSource_ReleaseEntity(), (int)vector2.X, (int)vector2.Y, 5);
								Main.npc[num22].velocity.X = vector3.X;
								Main.npc[num22].velocity.Y = vector3.Y;
								if (Main.netMode == NetmodeID.Server && num22 < 200)
								{
									NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, num22);
								}
							}
							SoundExtensions.PlaySoundOld(SoundID.NPCHit1, (int)vector2.X, (int)vector2.Y);
							for (int m = 0; m < 10; m++)
							{
								Dust.NewDust(vector2, 20, 20, DustID.Blood, vector3.X * 0.4f, vector3.Y * 0.4f);
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
						int num33 = NPC.NewNPC(NPC.GetSource_ReleaseEntity(), (int)vector6.X, (int)vector6.Y, 5);
						Main.npc[num33].velocity.X = vector7.X;
						Main.npc[num33].velocity.Y = vector7.Y;
						if (Main.netMode == NetmodeID.Server && num33 < 200)
						{
							NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, num33);
						}
					}
					for (int n = 0; n < 10; n++)
					{
						Dust.NewDust(vector6, 20, 20, DustID.Blood, vector7.X * 0.4f, vector7.Y * 0.4f);
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
						SoundExtensions.PlaySoundOld(SoundID.NPCHit1, (int)NPC.position.X, (int)NPC.position.Y);
						for (int num34 = 0; num34 < 2; num34++)
						{
							Gore.NewGore(NPC.GetSource_Loot(), NPC.position, new Vector2((float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f), 8);
							Gore.NewGore(NPC.GetSource_Loot(), NPC.position, new Vector2((float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f), 7);
							Gore.NewGore(NPC.GetSource_Loot(), NPC.position, new Vector2((float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f), 6);
						}
						for (int num35 = 0; num35 < 20; num35++)
						{
							Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, (float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f);
						}
						SoundExtensions.PlaySoundOld(SoundID.Roar, (int)NPC.position.X, (int)NPC.position.Y, 0);
					}
				}
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, (float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f);
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
					NPC.damage = (int)(40f);
					NPC.defense = -30;
				}
				else
				{
					NPC.damage = (int)(36f);
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
				SoundExtensions.PlaySoundOld(SoundID.ForceRoar, (int)NPC.position.X, (int)NPC.position.Y, 0);
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
					//	//SoundExtensions.PlaySoundOld(SoundID.NPCDeath13, NPC.Center);

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
					SoundExtensions.PlaySoundOld(SoundID.ForceRoar, (int)NPC.position.X, (int)NPC.position.Y, -1);
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
}
