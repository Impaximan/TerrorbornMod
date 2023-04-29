using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using System.Collections.Generic;
using System;
using Terraria.Audio;

namespace TerrorbornMod.TwilightMode.NPCs.Bosses
{
    class LunaticCultist : TwilightNPCChange
    {

        public override void OnHitByItem(NPC npc, Player player, Item item, NPC.HitInfo hit, int damageDone)
        {
            if (TerrorbornSystem.TwilightMode && NPC.type == NPCID.CultistBossClone)
            {
                NPC NPC2 = Main.npc[(int)NPC.ai[3]];
                int healAmount = NPC2.lifeMax / 15 + ((NPC2.lifeMax - NPC2.life) / 10);
                NPC2.HealEffect(healAmount);
                NPC2.life += healAmount;
                if (NPC2.life >= NPC2.lifeMax)
                {
                    NPC2.life = NPC2.lifeMax;
                }
            }
        }


        public override void OnHitByProjectile(NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone)
        {
            if (TerrorbornSystem.TwilightMode && NPC.type == NPCID.CultistBossClone)
            {
                NPC NPC2 = Main.npc[(int)NPC.ai[3]];
                int healAmount = NPC2.lifeMax / 15 + ((NPC2.lifeMax - NPC2.life) / 10);
                NPC2.HealEffect(healAmount);
                NPC2.life += healAmount;
                if (NPC2.life >= NPC2.lifeMax)
                {
                    NPC2.life = NPC2.lifeMax;
                }
            }
        }

        public override bool HasNewAI(NPC npc)
        {
            return true;
        }

        public override void NewAI(NPC npc)
        {
			LunaticCultistAI(npc);
        }

        public override bool ShouldChangeNPC(NPC npc)
        {
            return npc.type == NPCID.CultistBoss;
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
			if (NPC.ai[0] != -1f && Main.rand.NextBool(1000))
			{
				SoundExtensions.PlaySoundOld(new SoundStyle($"Zombie_" + Main.rand.Next(88, 92)), (int)NPC.position.X, (int)NPC.position.Y);
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
					NPC npcthing = Main.npc[item];
					if (NPC.localAI[1] == NPC.localAI[1] && num12 > 0)
					{
						num12--;
						npcthing.life = 0;
						npcthing.HitEffect();
						npcthing.active = false;
						if (Main.netMode != NetmodeID.MultiplayerClient)
						{
							NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, item);
						}
					}
					else if (num12 > 0)
					{
						num12--;
						npcthing.life = 0;
						npcthing.HitEffect();
						npcthing.active = false;
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
					NetMessage.SendData(MessageID.DamageNPC, -1, -1, null, NPC.whoAmI, -1f);
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
							NetMessage.SendData(MessageID.DamageNPC, -1, -1, null, NPC.whoAmI, -1f);
						}
					}
				}
			}
			float num13 = NPC.ai[3];
			if (NPC.localAI[0] == 0f)
			{
				SoundExtensions.PlaySoundOld(SoundID.Zombie89, (int)NPC.position.X, (int)NPC.position.Y, 89);
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
						SoundExtensions.PlaySoundOld(SoundID.Zombie105, (int)NPC.position.X, (int)NPC.position.Y, 105);
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
					if (expertMode && flag && Main.rand.NextBool(maxValue) && num15 != 0 && num15 != 4 && num15 != 5 && NPC.CountNPCS(523) < 10)
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
									for (int i = 0; i < Main.rand.Next(3, 5); i++)
									{
										spinninpoint = spinninpoint.RotatedByRandom(MathHelper.ToRadians(360));
										Projectile.NewProjectile(NPC.GetSource_ReleaseEntity(), vector3.X, vector3.Y, spinninpoint.X / 2, spinninpoint.Y / 2, ProjectileID.FrostWave, 18, 0f, Main.myPlayer);
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
							Projectile proj = Main.projectile[Projectile.NewProjectile(NPC.GetSource_ReleaseEntity(), vector4.X, vector4.Y, vector5.X, vector5.Y, ProjectileID.CultistBossIceMist, num2, 0f, Main.myPlayer, 0f, 1f)];
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
										Projectile.NewProjectile(NPC.GetSource_ReleaseEntity(), vector6.X, vector6.Y, actualVelocity.X, actualVelocity.Y, ProjectileID.Fireball, num7, 0f, Main.myPlayer);
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
							Projectile.NewProjectile(NPC.GetSource_ReleaseEntity(), vector7.X, vector7.Y, spinninpoint3.X, spinninpoint3.Y, ProjectileID.CultistBossFireBall, num5, 0f, Main.myPlayer);
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
								Projectile.NewProjectile(NPC.GetSource_ReleaseEntity(), vector9.X, vector9.Y - 100f, spinninpoint4.X, spinninpoint4.Y, ProjectileID.CultistBossLightningOrb, num7, 0f, Main.myPlayer);
							}
						}
					}
					if ((int)(NPC.ai[1] - 20f) % num6 == 0)
					{
						float speed = 8.5f;
						Vector2 velocity = player.DirectionFrom(new Vector2(NPC.Center.X, NPC.Center.Y - 100f)) * speed;
						Projectile.NewProjectile(NPC.GetSource_ReleaseEntity(), NPC.Center.X, NPC.Center.Y - 100f, velocity.X, velocity.Y, ProjectileID.CultistBossLightningOrb, num7, 0f, Main.myPlayer);
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
									int num37 = NPC.NewNPC(NPC.GetSource_ReleaseEntity(), (int)center6.X, (int)center6.Y + NPC.height / 2, NPCID.CultistBossClone, NPC.whoAmI);
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
						NPC.ai[2] = Projectile.NewProjectile(NPC.GetSource_ReleaseEntity(), NPC.Center.X, NPC.Center.Y, 0f, 0f, ProjectileID.CultistRitual, 0, 0f, Main.myPlayer, 0f, NPC.whoAmI);
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
									Projectile.NewProjectile(NPC.GetSource_ReleaseEntity(), vector14.X, vector14.Y, spinninpoint5.X, spinninpoint5.Y, ProjectileID.CultistBossFireBallClone, 18, 0f, Main.myPlayer);
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
							int num51 = NPC.NewNPC(NPC.GetSource_ReleaseEntity(), (int)vector15.X, (int)vector15.Y + 7, 522, 0, 0f, ai, spinningpoint2.X, spinningpoint2.Y);
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
								if ((num62 < point2.Y - num58 || num62 > point2.Y + num58 || num61 < point2.X - num58 || num61 > point2.X + num58) && (num62 < point.Y - num57 || num62 > point.Y + num57 || num61 < point.X - num57 || num61 > point.X + num57) && Main.tile[num61, num62] != null)
								{
									bool flag7 = true;
									if (flag7 && Collision.SolidTiles(num61 - num59, num61 + num59, num62 - num59, num62 + num59))
									{
										flag7 = false;
									}
									if (flag7)
									{
										int newNPC = NPC.NewNPC(NPC.GetSource_ReleaseEntity(), num61 * 16 + 8, num62 * 16 + 8, 523, 0, NPC.whoAmI);
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
	}
}
