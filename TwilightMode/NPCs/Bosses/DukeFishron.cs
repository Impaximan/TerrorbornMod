using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using System.Collections.Generic;
using System;

namespace TerrorbornMod.TwilightMode.NPCs.Bosses
{
	class DukeFishron : TwilightNPCChange
	{
		public override bool HasNewAI(NPC npc)
		{
			return true;
		}

		public override void NewAI(NPC npc)
		{
			AI_069_DukeFishron(npc);
		}

		public override bool ShouldChangeNPC(NPC npc)
		{
			return npc.type == NPCID.DukeFishron;
		}

		float dashVelocityMult = 1f;
		private void AI_069_DukeFishron(NPC NPC)
		{
			List<int> dashes = new List<int>()
			{
				0, 1, 5, 6
			};
			if (!dashes.Contains((int)NPC.ai[0]))
			{
				dashVelocityMult = 1f;
            }
			bool expertMode = Main.expertMode;
			float num = (expertMode ? (0.6f * 2f) : 1f);
			bool phase2 = (double)NPC.life <= (double)NPC.lifeMax * 0.5;
			bool phase3 = expertMode && (double)NPC.life <= (double)NPC.lifeMax * 0.15;
			bool flag3 = NPC.ai[0] > 4f;
			bool flag4 = NPC.ai[0] > 9f;
			bool flag5 = NPC.ai[3] < 10f;
			if (flag4)
			{
				NPC.damage = (int)((float)NPC.defDamage * 1.1f * num);
				NPC.defense = 0;
			}
			else if (flag3)
			{
				NPC.damage = (int)((float)NPC.defDamage * 1.2f * num);
				NPC.defense = (int)((float)NPC.defDefense * 0.8f);
			}
			else
			{
				NPC.damage = NPC.defDamage;
				NPC.defense = NPC.defDefense;
			}
			int num2 = (expertMode ? 40 : 60);
			float num3 = (expertMode ? 0.55f : 0.45f);
			float num4 = (expertMode ? 8.5f : 7.5f);
			if (flag4)
			{
				num3 = 0.7f;
				num4 = 12f;
				num2 = 30;
			}
			else if (flag3 && flag5)
			{
				num3 = (expertMode ? 0.6f : 0.5f);
				num4 = (expertMode ? 10f : 8f);
				num2 = (expertMode ? 40 : 20);
			}
			else if (flag5 && !flag3 && !flag4)
			{
				num2 = 30;
			}
			int num5 = (expertMode ? 28 : 30);
			float num6 = (expertMode ? 17f : 16f); //Dash speed maybe? (phase one only)
			if (flag4)
			{
				num5 = 25;
				num6 = 27f;
			}
			else if (flag5 && flag3)
			{
				num5 = (expertMode ? 27 : 30);
				if (expertMode)
				{
					num6 = 21f;
				}
			}
			num6 *= dashVelocityMult;
			int num7 = 80;
			int num8 = 4;
			float num9 = 0.3f;
			float num10 = 5f; //hover speed
			num10 *= 2f;
			//Main.NewText("ai0:" + NPC.ai[0]);
			//Main.NewText("ai1:" + NPC.ai[1]);
			//Main.NewText("ai2:" + NPC.ai[2]);
			//Main.NewText("ai3:" + NPC.ai[3]);
			int num11 = 90;
			int num12 = 180;
			int num13 = 180;
			int num14 = 30;
			int num15 = 120;
			int num16 = 4;
			float num17 = 6f;
			float num18 = 20f;
			float num19 = (float)Math.PI * 2f / (float)(num15 / 2);
			int num20 = 75;
			Vector2 center = NPC.Center;
			Player player = Main.player[NPC.target];
			if (NPC.target < 0 || NPC.target == 255 || player.dead || !player.active)
			{
				NPC.TargetClosest();
				player = Main.player[NPC.target];
				NPC.netUpdate = true;
			}
			if (player.dead || Vector2.Distance(player.Center, center) > 5600f)
			{
				NPC.velocity.Y -= 0.4f;
				if (NPC.timeLeft > 10)
				{
					NPC.timeLeft = 10;
				}
				if (NPC.ai[0] > 4f)
				{
					NPC.ai[0] = 5f;
				}
				else
				{
					NPC.ai[0] = 0f;
				}
				NPC.ai[2] = 0f;
			}
			if (player.position.Y < 800f || (double)player.position.Y > Main.worldSurface * 16.0 || (player.position.X > 6400f && player.position.X < (float)(Main.maxTilesX * 16 - 6400)))
			{
				num2 = 20;
				NPC.damage = NPC.defDamage * 2;
				NPC.defense = NPC.defDefense * 2;
				NPC.ai[3] = 0f;
				num6 += 6f;
			}
			if (NPC.localAI[0] == 0f)
			{
				NPC.localAI[0] = 1f;
				NPC.alpha = 255;
				NPC.rotation = 0f;
				if (Main.netMode != 1)
				{
					NPC.ai[0] = -1f;
					NPC.netUpdate = true;
				}
			}
			float num21 = (float)Math.Atan2(player.Center.Y - center.Y, player.Center.X - center.X);
			if (NPC.spriteDirection == 1)
			{
				num21 += (float)Math.PI;
			}
			if (num21 < 0f)
			{
				num21 += (float)Math.PI * 2f;
			}
			if (num21 > (float)Math.PI * 2f)
			{
				num21 -= (float)Math.PI * 2f;
			}
			if (NPC.ai[0] == -1f)
			{
				num21 = 0f;
			}
			if (NPC.ai[0] == 3f)
			{
				num21 = 0f;
			}
			if (NPC.ai[0] == 4f)
			{
				num21 = 0f;
			}
			if (NPC.ai[0] == 8f)
			{
				num21 = 0f;
			}
			float num22 = 0.04f;
			if (NPC.ai[0] == 1f || NPC.ai[0] == 6f)
			{
				num22 = 0f;
			}
			if (NPC.ai[0] == 7f)
			{
				num22 = 0f;
			}
			if (NPC.ai[0] == 3f)
			{
				num22 = 0.01f;
			}
			if (NPC.ai[0] == 4f)
			{
				num22 = 0.01f;
			}
			if (NPC.ai[0] == 8f)
			{
				num22 = 0.01f;
			}
			if (NPC.rotation < num21)
			{
				if ((double)(num21 - NPC.rotation) > Math.PI)
				{
					NPC.rotation -= num22;
				}
				else
				{
					NPC.rotation += num22;
				}
			}
			if (NPC.rotation > num21)
			{
				if ((double)(NPC.rotation - num21) > Math.PI)
				{
					NPC.rotation += num22;
				}
				else
				{
					NPC.rotation -= num22;
				}
			}
			if (NPC.rotation > num21 - num22 && NPC.rotation < num21 + num22)
			{
				NPC.rotation = num21;
			}
			if (NPC.rotation < 0f)
			{
				NPC.rotation += (float)Math.PI * 2f;
			}
			if (NPC.rotation > (float)Math.PI * 2f)
			{
				NPC.rotation -= (float)Math.PI * 2f;
			}
			if (NPC.rotation > num21 - num22 && NPC.rotation < num21 + num22)
			{
				NPC.rotation = num21;
			}
			if (NPC.ai[0] != -1f && NPC.ai[0] < 9f)
			{
				if (Collision.SolidCollision(NPC.position, NPC.width, NPC.height))
				{
					NPC.alpha += 15;
				}
				else
				{
					NPC.alpha -= 15;
				}
				if (NPC.alpha < 0)
				{
					NPC.alpha = 0;
				}
				if (NPC.alpha > 150)
				{
					NPC.alpha = 150;
				}
			}
			if (NPC.ai[0] == -1f)
			{
				NPC.velocity *= 0.98f;
				int num23 = Math.Sign(player.Center.X - center.X);
				if (num23 != 0)
				{
					NPC.direction = num23;
					NPC.spriteDirection = -NPC.direction;
				}
				if (NPC.ai[2] > 20f)
				{
					NPC.velocity.Y = -2f;
					NPC.alpha -= 5;
					if (Collision.SolidCollision(NPC.position, NPC.width, NPC.height))
					{
						NPC.alpha += 15;
					}
					if (NPC.alpha < 0)
					{
						NPC.alpha = 0;
					}
					if (NPC.alpha > 150)
					{
						NPC.alpha = 150;
					}
				}
				if (NPC.ai[2] == (float)(num11 - 30))
				{
					int num24 = 36;
					for (int i = 0; i < num24; i++)
					{
						Vector2 vector = (Vector2.Normalize(NPC.velocity) * new Vector2((float)NPC.width / 2f, NPC.height) * 0.75f * 0.5f).RotatedBy((float)(i - (num24 / 2 - 1)) * ((float)Math.PI * 2f) / (float)num24) + NPC.Center;
						Vector2 vector2 = vector - NPC.Center;
						int num25 = Dust.NewDust(vector + vector2, 0, 0, 172, vector2.X * 2f, vector2.Y * 2f, 100, default(Color), 1.4f);
						Main.dust[num25].noGravity = true;
						Main.dust[num25].noLight = true;
						Main.dust[num25].velocity = Vector2.Normalize(vector2) * 3f;
					}
					SoundEngine.PlaySound(SoundID.Zombie20, center);
				}
				NPC.ai[2] += 1f;
				if (NPC.ai[2] >= (float)num20)
				{
					NPC.ai[0] = 0f;
					NPC.ai[1] = 0f;
					NPC.ai[2] = 0f;
					NPC.netUpdate = true;
				}
			}
			else if (NPC.ai[0] == 0f && !player.dead)
			{
				if (NPC.ai[1] == 0f)
				{
					NPC.ai[1] = 300 * Math.Sign((center - player.Center).X);
				}
				Vector2 vector3 = Vector2.Normalize(player.Center + new Vector2(NPC.ai[1], -200f) - center - NPC.velocity) * num4;
				if (NPC.velocity.X < vector3.X)
				{
					NPC.velocity.X += num3;
					if (NPC.velocity.X < 0f && vector3.X > 0f)
					{
						NPC.velocity.X += num3;
					}
				}
				else if (NPC.velocity.X > vector3.X)
				{
					NPC.velocity.X -= num3;
					if (NPC.velocity.X > 0f && vector3.X < 0f)
					{
						NPC.velocity.X -= num3;
					}
				}
				if (NPC.velocity.Y < vector3.Y)
				{
					NPC.velocity.Y += num3;
					if (NPC.velocity.Y < 0f && vector3.Y > 0f)
					{
						NPC.velocity.Y += num3;
					}
				}
				else if (NPC.velocity.Y > vector3.Y)
				{
					NPC.velocity.Y -= num3;
					if (NPC.velocity.Y > 0f && vector3.Y < 0f)
					{
						NPC.velocity.Y -= num3;
					}
				}
				int num26 = Math.Sign(player.Center.X - center.X);
				if (num26 != 0)
				{
					if (NPC.ai[2] == 0f && num26 != NPC.direction)
					{
						NPC.rotation += (float)Math.PI;
					}
					NPC.direction = num26;
					if (NPC.spriteDirection != -NPC.direction)
					{
						NPC.rotation += (float)Math.PI;
					}
					NPC.spriteDirection = -NPC.direction;
				}
				NPC.ai[2] += 1f;
				if (!(NPC.ai[2] >= (float)num2))
				{
					return;
				}
				int num27 = 0;
				switch ((int)NPC.ai[3])
				{
					case 0:
					case 1:
					case 2:
					case 3:
					case 4:
					case 5:
					case 6:
					case 7:
					case 8:
					case 9:
						num27 = 1;
						break;
					case 10:
						NPC.ai[3] = 1f;
						num27 = 2;
						break;
					case 11:
						NPC.ai[3] = 0f;
						num27 = 3;
						break;
				}
				if (phase2)
				{
					num27 = 4;
				}
				switch (num27)
				{
					case 1:
						NPC.ai[0] = 1f;
						NPC.ai[1] = 0f;
						NPC.ai[2] = 0f;
						Vector2 targetPosition = player.Center + (player.velocity * (NPC.Distance(player.Center) / num6));
						NPC.velocity = Vector2.Normalize(targetPosition - center) * num6;
						dashVelocityMult *= 1.15f; //Increase dash velocity (phase 1)
						NPC.rotation = (float)Math.Atan2(NPC.velocity.Y, NPC.velocity.X);
						if (num26 != 0)
						{
							NPC.direction = num26;
							if (NPC.spriteDirection == 1)
							{
								NPC.rotation += (float)Math.PI;
							}
							NPC.spriteDirection = -NPC.direction;
						}
						break;
					case 2:
						NPC.ai[0] = 2f;
						NPC.ai[1] = 0f;
						NPC.ai[2] = 0f;
						break;
					case 3:
						NPC.ai[0] = 3f;
						NPC.ai[1] = 0f;
						NPC.ai[2] = 0f;
						break;
					case 4:
						NPC.ai[0] = 4f;
						NPC.ai[1] = 0f;
						NPC.ai[2] = 0f;
						break;
				}
				NPC.netUpdate = true;
			}
			else if (NPC.ai[0] == 1f)
			{
				int num28 = 7;
				for (int j = 0; j < num28; j++)
				{
					Vector2 vector4 = (Vector2.Normalize(NPC.velocity) * new Vector2((float)(NPC.width + 50) / 2f, NPC.height) * 0.75f).RotatedBy((double)(j - (num28 / 2 - 1)) * Math.PI / (double)(float)num28) + center;
					Vector2 vector5 = ((float)(Main.rand.NextDouble() * 3.1415927410125732) - (float)Math.PI / 2f).ToRotationVector2() * Main.rand.Next(3, 8);
					int num29 = Dust.NewDust(vector4 + vector5, 0, 0, 172, vector5.X * 2f, vector5.Y * 2f, 100, default(Color), 1.4f);
					Main.dust[num29].noGravity = true;
					Main.dust[num29].noLight = true;
					Main.dust[num29].velocity /= 4f;
					Main.dust[num29].velocity -= NPC.velocity;
				}
				NPC.ai[2] += 1f;
				if (NPC.ai[2] >= (float)num5)
				{
					NPC.ai[0] = 0f;
					NPC.ai[1] = 0f;
					NPC.ai[2] = 0f;
					NPC.ai[3] += 2f;
					NPC.netUpdate = true;
				}
			}
			else if (NPC.ai[0] == 2f)
			{
				if (NPC.ai[1] == 0f)
				{
					NPC.ai[1] = 300 * Math.Sign((center - player.Center).X);
				}
				Vector2 vector6 = Vector2.Normalize(player.Center + new Vector2(NPC.ai[1], -200f) - center - NPC.velocity) * num10;
				if (NPC.velocity.X < vector6.X)
				{
					NPC.velocity.X += num9;
					if (NPC.velocity.X < 0f && vector6.X > 0f)
					{
						NPC.velocity.X += num9;
					}
				}
				else if (NPC.velocity.X > vector6.X)
				{
					NPC.velocity.X -= num9;
					if (NPC.velocity.X > 0f && vector6.X < 0f)
					{
						NPC.velocity.X -= num9;
					}
				}
				if (NPC.velocity.Y < vector6.Y)
				{
					NPC.velocity.Y += num9;
					if (NPC.velocity.Y < 0f && vector6.Y > 0f)
					{
						NPC.velocity.Y += num9;
					}
				}
				else if (NPC.velocity.Y > vector6.Y)
				{
					NPC.velocity.Y -= num9;
					if (NPC.velocity.Y > 0f && vector6.Y < 0f)
					{
						NPC.velocity.Y -= num9;
					}
				}
				if (NPC.ai[2] == 0f)
				{
					SoundEngine.PlaySound(SoundID.Zombie20, center);
				}
				if (NPC.ai[2] % (float)num8 == 0f)
				{
					SoundEngine.PlaySound(SoundID.NPCDeath19, NPC.Center);
					if (Main.netMode != 1)
					{
						Vector2 vector7 = Vector2.Normalize(player.Center - center) * (NPC.width + 20) / 2f + center;
						NPC.NewNPC(NPC.GetSource_FromThis(), (int)vector7.X, (int)vector7.Y + 45, 371);
					}
				}
				int num30 = Math.Sign(player.Center.X - center.X);
				if (num30 != 0)
				{
					NPC.direction = num30;
					if (NPC.spriteDirection != -NPC.direction)
					{
						NPC.rotation += (float)Math.PI;
					}
					NPC.spriteDirection = -NPC.direction;
				}
				NPC.ai[2] += 1f;
				if (NPC.ai[2] >= (float)num7)
				{
					NPC.ai[0] = 0f;
					NPC.ai[1] = 0f;
					NPC.ai[2] = 0f;
					NPC.netUpdate = true;
				}
			}
			else if (NPC.ai[0] == 3f)
			{
				NPC.velocity *= 0.98f;
				NPC.velocity.Y = MathHelper.Lerp(NPC.velocity.Y, 0f, 0.02f);
				if (NPC.ai[2] == (float)(num11 - 30))
				{
					SoundEngine.PlaySound(SoundID.Zombie9, center);
				}
				if (Main.netMode != 1 && NPC.ai[2] == (float)(num11 - 30))
				{
					Vector2 vector8 = NPC.rotation.ToRotationVector2() * (Vector2.UnitX * NPC.direction) * (NPC.width + 20) / 2f + center;
					Projectile.NewProjectile(NPC.GetSource_FromThis(), vector8.X, vector8.Y, NPC.direction * 2, 8f, 385, 0, 0f, Main.myPlayer);
					Projectile.NewProjectile(NPC.GetSource_FromThis(), vector8.X, vector8.Y, -NPC.direction * 2, 8f, 385, 0, 0f, Main.myPlayer);
				}
				NPC.ai[2] += 1f;
				if (NPC.ai[2] >= (float)num11)
				{
					NPC.ai[0] = 0f;
					NPC.ai[1] = 0f;
					NPC.ai[2] = 0f;
					NPC.netUpdate = true;
				}
			}
			else if (NPC.ai[0] == 4f)
			{
				NPC.velocity *= 0.98f;
				NPC.velocity.Y = MathHelper.Lerp(NPC.velocity.Y, 0f, 0.02f);
				if (NPC.ai[2] == (float)(num12 - 60))
				{
					SoundEngine.PlaySound(SoundID.Zombie20, center);
				}
				NPC.ai[2] += 1f;
				if (NPC.ai[2] >= (float)num12)
				{
					NPC.ai[0] = 5f;
					NPC.ai[1] = 0f;
					NPC.ai[2] = 0f;
					NPC.ai[3] = 0f;
					NPC.netUpdate = true;
				}
			}
			else if (NPC.ai[0] == 5f && !player.dead)
			{
				if (NPC.ai[1] == 0f)
				{
					NPC.ai[1] = 300 * Math.Sign((center - player.Center).X);
				}
				Vector2 vector9 = Vector2.Normalize(player.Center + new Vector2(NPC.ai[1], -200f) - center - NPC.velocity) * num4;
				if (NPC.velocity.X < vector9.X)
				{
					NPC.velocity.X += num3;
					if (NPC.velocity.X < 0f && vector9.X > 0f)
					{
						NPC.velocity.X += num3;
					}
				}
				else if (NPC.velocity.X > vector9.X)
				{
					NPC.velocity.X -= num3;
					if (NPC.velocity.X > 0f && vector9.X < 0f)
					{
						NPC.velocity.X -= num3;
					}
				}
				if (NPC.velocity.Y < vector9.Y)
				{
					NPC.velocity.Y += num3;
					if (NPC.velocity.Y < 0f && vector9.Y > 0f)
					{
						NPC.velocity.Y += num3;
					}
				}
				else if (NPC.velocity.Y > vector9.Y)
				{
					NPC.velocity.Y -= num3;
					if (NPC.velocity.Y > 0f && vector9.Y < 0f)
					{
						NPC.velocity.Y -= num3;
					}
				}
				int num31 = Math.Sign(player.Center.X - center.X);
				if (num31 != 0)
				{
					if (NPC.ai[2] == 0f && num31 != NPC.direction)
					{
						NPC.rotation += (float)Math.PI;
					}
					NPC.direction = num31;
					if (NPC.spriteDirection != -NPC.direction)
					{
						NPC.rotation += (float)Math.PI;
					}
					NPC.spriteDirection = -NPC.direction;
				}
				NPC.ai[2] += 1f;
				if (!(NPC.ai[2] >= (float)num2))
				{
					return;
				}
				int num32 = 0;
				switch ((int)NPC.ai[3])
				{
					case 0:
					case 1:
					case 2:
					case 3:
					case 4:
					case 5:
						num32 = 1;
						break;
					case 6:
						NPC.ai[3] = 1f;
						num32 = 2;
						break;
					case 7:
						NPC.ai[3] = 0f;
						num32 = 3;
						break;
				}
				if (phase3)
				{
					num32 = 4;
				}
				switch (num32)
				{
					case 1:
						NPC.ai[0] = 6f;
						NPC.ai[1] = 0f;
						NPC.ai[2] = 0f;
						Vector2 targetPosition = player.Center + (player.velocity * (NPC.Distance(player.Center) / num6));
						NPC.velocity = Vector2.Normalize(targetPosition - center) * num6;
						dashVelocityMult *= 1.25f; //Increase dash velocity (phase 2)
						NPC.rotation = (float)Math.Atan2(NPC.velocity.Y, NPC.velocity.X);
						if (num31 != 0)
						{
							NPC.direction = num31;
							if (NPC.spriteDirection == 1)
							{
								NPC.rotation += (float)Math.PI;
							}
							NPC.spriteDirection = -NPC.direction;
						}
						break;
					case 2:
						targetPosition = player.Center + (player.velocity * (NPC.Distance(player.Center) / num6));
						NPC.velocity = Vector2.Normalize(targetPosition - center) * num18;
						dashVelocityMult *= 1.25f; //Increase dash velocity (phase 2)
						NPC.rotation = (float)Math.Atan2(NPC.velocity.Y, NPC.velocity.X);
						if (num31 != 0)
						{
							NPC.direction = num31;
							if (NPC.spriteDirection == 1)
							{
								NPC.rotation += (float)Math.PI;
							}
							NPC.spriteDirection = -NPC.direction;
						}
						NPC.ai[0] = 7f;
						NPC.ai[1] = 0f;
						NPC.ai[2] = 0f;
						break;
					case 3:
						NPC.ai[0] = 8f;
						NPC.ai[1] = 0f;
						NPC.ai[2] = 0f;
						break;
					case 4:
						NPC.ai[0] = 9f;
						NPC.ai[1] = 0f;
						NPC.ai[2] = 0f;
						break;
				}
				NPC.netUpdate = true;
			}
			else if (NPC.ai[0] == 6f)
			{
				int num33 = 7;
				for (int k = 0; k < num33; k++)
				{
					Vector2 vector10 = (Vector2.Normalize(NPC.velocity) * new Vector2((float)(NPC.width + 50) / 2f, NPC.height) * 0.75f).RotatedBy((double)(k - (num33 / 2 - 1)) * Math.PI / (double)(float)num33) + center;
					Vector2 vector11 = ((float)(Main.rand.NextDouble() * 3.1415927410125732) - (float)Math.PI / 2f).ToRotationVector2() * Main.rand.Next(3, 8);
					int num34 = Dust.NewDust(vector10 + vector11, 0, 0, 172, vector11.X * 2f, vector11.Y * 2f, 100, default(Color), 1.4f);
					Main.dust[num34].noGravity = true;
					Main.dust[num34].noLight = true;
					Main.dust[num34].velocity /= 4f;
					Main.dust[num34].velocity -= NPC.velocity;
				}
				NPC.ai[2] += 1f;
				if (NPC.ai[2] >= (float)num5)
				{
					NPC.ai[0] = 5f;
					NPC.ai[1] = 0f;
					NPC.ai[2] = 0f;
					NPC.ai[3] += 2f;
					NPC.netUpdate = true;
				}
			}
			else if (NPC.ai[0] == 7f)
			{
				if (NPC.ai[2] == 0f)
				{
					SoundEngine.PlaySound(SoundID.Zombie20, center);
				}
				if (NPC.ai[2] % (float)num16 == 0f)
				{
					SoundEngine.PlaySound(SoundID.NPCDeath19, NPC.Center);
					if (Main.netMode != 1)
					{
						Vector2 vector12 = Vector2.Normalize(NPC.velocity) * (NPC.width + 20) / 2f + center;
						int num35 = NPC.NewNPC(NPC.GetSource_FromThis(), (int)vector12.X, (int)vector12.Y + 45, 371);
						Main.npc[num35].target = NPC.target;
						Main.npc[num35].velocity = Vector2.Normalize(NPC.velocity).RotatedBy((float)Math.PI / 2f * (float)NPC.direction) * num17;
						Main.npc[num35].netUpdate = true;
						Main.npc[num35].ai[3] = (float)Main.rand.Next(80, 121) / 100f;
					}
				}
				NPC.velocity = NPC.velocity.RotatedBy((0f - num19) * (float)NPC.direction);
				NPC.rotation -= num19 * (float)NPC.direction;
				NPC.ai[2] += 1f;
				if (NPC.ai[2] >= (float)num15)
				{
					NPC.ai[0] = 5f;
					NPC.ai[1] = 0f;
					NPC.ai[2] = 0f;
					NPC.netUpdate = true;
				}
			}
			else if (NPC.ai[0] == 8f)
			{
				NPC.velocity *= 0.98f;
				NPC.velocity.Y = MathHelper.Lerp(NPC.velocity.Y, 0f, 0.02f);
				if (NPC.ai[2] == (float)(num11 - 30))
				{
					SoundEngine.PlaySound(SoundID.Zombie20, center);
				}
				if (Main.netMode != 1 && NPC.ai[2] == (float)(num11 - 30))
				{
					Projectile.NewProjectile(NPC.GetSource_FromThis(), center.X, center.Y, 0f, 0f, 385, 0, 0f, Main.myPlayer, 1f, NPC.target + 1);
				}
				NPC.ai[2] += 1f;
				if (NPC.ai[2] >= (float)num11)
				{
					NPC.ai[0] = 5f;
					NPC.ai[1] = 0f;
					NPC.ai[2] = 0f;
					NPC.netUpdate = true;
				}
			}
			else if (NPC.ai[0] == 9f)
			{
				if (NPC.ai[2] < (float)(num13 - 90))
				{
					if (Collision.SolidCollision(NPC.position, NPC.width, NPC.height))
					{
						NPC.alpha += 15;
					}
					else
					{
						NPC.alpha -= 15;
					}
					if (NPC.alpha < 0)
					{
						NPC.alpha = 0;
					}
					if (NPC.alpha > 150)
					{
						NPC.alpha = 150;
					}
				}
				else if (NPC.alpha < 255)
				{
					NPC.alpha += 4;
					if (NPC.alpha > 255)
					{
						NPC.alpha = 255;
					}
				}
				NPC.velocity *= 0.98f;
				NPC.velocity.Y = MathHelper.Lerp(NPC.velocity.Y, 0f, 0.02f);
				if (NPC.ai[2] == (float)(num13 - 60))
				{
					SoundEngine.PlaySound(SoundID.Zombie20, center);
				}
				NPC.ai[2] += 1f;
				if (NPC.ai[2] >= (float)num13)
				{
					NPC.ai[0] = 10f;
					NPC.ai[1] = 0f;
					NPC.ai[2] = 0f;
					NPC.ai[3] = 0f;
					NPC.netUpdate = true;
				}
			}
			else if (NPC.ai[0] == 10f && !player.dead)
			{
				NPC.dontTakeDamage = false;
				NPC.chaseable = false;
				if (NPC.alpha < 255)
				{
					NPC.alpha += 25;
					if (NPC.alpha > 255)
					{
						NPC.alpha = 255;
					}
				}
				if (NPC.ai[1] == 0f)
				{
					NPC.ai[1] = 360 * Math.Sign((center - player.Center).X);
				}
				Vector2 desiredVelocity = Vector2.Normalize(player.Center + new Vector2(NPC.ai[1], -200f) - center - NPC.velocity) * num4;
				NPC.SimpleFlyMovement(desiredVelocity, num3);
				int num36 = Math.Sign(player.Center.X - center.X);
				if (num36 != 0)
				{
					if (NPC.ai[2] == 0f && num36 != NPC.direction)
					{
						NPC.rotation += (float)Math.PI;
						for (int l = 0; l < NPC.oldPos.Length; l++)
						{
							NPC.oldPos[l] = Vector2.Zero;
						}
					}
					NPC.direction = num36;
					if (NPC.spriteDirection != -NPC.direction)
					{
						NPC.rotation += (float)Math.PI;
					}
					NPC.spriteDirection = -NPC.direction;
				}
				NPC.ai[2] += 1f;
				if (!(NPC.ai[2] >= (float)num2))
				{
					return;
				}
				int num37 = 0;
				switch ((int)NPC.ai[3])
				{
					case 0:
					case 2:
					case 3:
					case 5:
					case 6:
					case 7:
						num37 = 1;
						break;
					case 1:
					case 4:
					case 8:
						num37 = 2;
						break;
				}
				switch (num37)
				{
					case 1:
						NPC.ai[0] = 11f;
						NPC.ai[1] = 0f;
						NPC.ai[2] = 0f;
						Vector2 targetPosition = player.Center + (player.velocity * (NPC.Distance(player.Center) / num6));
						NPC.velocity = Vector2.Normalize(targetPosition - center) * num6;
						NPC.rotation = (float)Math.Atan2(NPC.velocity.Y, NPC.velocity.X);
						if (num36 != 0)
						{
							NPC.direction = num36;
							if (NPC.spriteDirection == 1)
							{
								NPC.rotation += (float)Math.PI;
							}
							NPC.spriteDirection = -NPC.direction;
						}
						break;
					case 2:
						NPC.ai[0] = 12f;
						NPC.ai[1] = 0f;
						NPC.ai[2] = 0f;
						break;
					case 3:
						NPC.ai[0] = 13f;
						NPC.ai[1] = 0f;
						NPC.ai[2] = 0f;
						break;
				}
				NPC.netUpdate = true;
			}
			else if (NPC.ai[0] == 11f)
			{
				NPC.dontTakeDamage = false;
				NPC.chaseable = true;
				NPC.alpha -= 25;
				if (NPC.alpha < 0)
				{
					NPC.alpha = 0;
				}
				int num38 = 7;
				for (int m = 0; m < num38; m++)
				{
					Vector2 vector13 = (Vector2.Normalize(NPC.velocity) * new Vector2((float)(NPC.width + 50) / 2f, NPC.height) * 0.75f).RotatedBy((double)(m - (num38 / 2 - 1)) * Math.PI / (double)(float)num38) + center;
					Vector2 vector14 = ((float)(Main.rand.NextDouble() * 3.1415927410125732) - (float)Math.PI / 2f).ToRotationVector2() * Main.rand.Next(3, 8);
					int num39 = Dust.NewDust(vector13 + vector14, 0, 0, 172, vector14.X * 2f, vector14.Y * 2f, 100, default(Color), 1.4f);
					Main.dust[num39].noGravity = true;
					Main.dust[num39].noLight = true;
					Main.dust[num39].velocity /= 4f;
					Main.dust[num39].velocity -= NPC.velocity;
				}
				NPC.ai[2] += 1f;
				if (NPC.ai[2] >= (float)num5)
				{
					NPC.ai[0] = 10f;
					NPC.ai[1] = 0f;
					NPC.ai[2] = 0f;
					NPC.ai[3] += 1f;
					NPC.netUpdate = true;
				}
			}
			else if (NPC.ai[0] == 12f)
			{
				NPC.dontTakeDamage = true;
				NPC.chaseable = false;
				if (NPC.alpha < 255)
				{
					NPC.alpha += 17;
					if (NPC.alpha > 255)
					{
						NPC.alpha = 255;
					}
				}
				NPC.velocity *= 0.98f;
				NPC.velocity.Y = MathHelper.Lerp(NPC.velocity.Y, 0f, 0.02f);
				if (NPC.ai[2] == (float)(num14 / 2))
				{
					SoundEngine.PlaySound(SoundID.Zombie20, center);
				}
				if (Main.netMode != 1 && NPC.ai[2] == (float)(num14 / 2))
				{
					if (NPC.ai[1] == 0f)
					{
						NPC.ai[1] = 300 * Math.Sign((center - player.Center).X);
					}
					Vector2 vector15 = player.Center + new Vector2(0f - NPC.ai[1], -200f);
					Vector2 vector17 = (NPC.Center = vector15);
					center = vector17;
					int num40 = Math.Sign(player.Center.X - center.X);
					if (num40 != 0)
					{
						if (NPC.ai[2] == 0f && num40 != NPC.direction)
						{
							NPC.rotation += (float)Math.PI;
							for (int n = 0; n < NPC.oldPos.Length; n++)
							{
								NPC.oldPos[n] = Vector2.Zero;
							}
						}
						NPC.direction = num40;
						if (NPC.spriteDirection != -NPC.direction)
						{
							NPC.rotation += (float)Math.PI;
						}
						NPC.spriteDirection = -NPC.direction;
					}
				}
				NPC.ai[2] += 1f;
				if (NPC.ai[2] >= (float)num14)
				{
					NPC.ai[0] = 10f;
					NPC.ai[1] = 0f;
					NPC.ai[2] = 0f;
					NPC.ai[3] += 1f;
					if (NPC.ai[3] >= 9f)
					{
						NPC.ai[3] = 0f;
					}
					NPC.netUpdate = true;
				}
			}
			else if (NPC.ai[0] == 13f)
			{
				if (NPC.ai[2] == 0f)
				{
					SoundEngine.PlaySound(SoundID.Zombie20, center);
				}
				NPC.velocity = NPC.velocity.RotatedBy((0f - num19) * (float)NPC.direction);
				NPC.rotation -= num19 * (float)NPC.direction;
				NPC.ai[2] += 1f;
				if (NPC.ai[2] >= (float)num15)
				{
					NPC.ai[0] = 10f;
					NPC.ai[1] = 0f;
					NPC.ai[2] = 0f;
					NPC.ai[3] += 1f;
					NPC.netUpdate = true;
				}
			}
		}
	}
}
