using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using System.Collections.Generic;
using System;
using Terraria.Audio;

namespace TerrorbornMod.TwilightMode.NPCs.Bosses
{
	class Skeletron : TwilightNPCChange
	{
		public override bool ShouldChangeNPC(NPC npc)
		{
			return npc.type == NPCID.SkeletronHead;
		}

		public override bool HasNewAI(NPC npc)
		{
			return true;
		}

		int skeletronAliveCounter = 0;
		int skeletronTurnOverCounter = 0;
		int skeletronTurnOverTime = 0;
		const float skeletronTurnOverTotalTime = 60f;
		int skeletronTurnOverProjectileTimer = 0;
		int skeletronTurnOverDirection = 1;
        public override void NewAI(NPC npc)
        {
			skeletronAliveCounter++;
			npc.defense = npc.defDefense;
			if (npc.ai[0] == 0f && Main.netMode != 1)
			{
				npc.TargetClosest();
				npc.ai[0] = 1f;
				if (npc.type != 68)
				{
					int num154 = NPC.NewNPC(npc.GetSource_FromThis(), (int)(npc.position.X + (float)(npc.width / 2)), (int)npc.position.Y + npc.height / 2, 36, npc.whoAmI);
					Main.npc[num154].ai[0] = -1f;
					Main.npc[num154].ai[1] = npc.whoAmI;
					Main.npc[num154].target = npc.target;
					Main.npc[num154].netUpdate = true;
					num154 = NPC.NewNPC(npc.GetSource_FromThis(), (int)(npc.position.X + (float)(npc.width / 2)), (int)npc.position.Y + npc.height / 2, 36, npc.whoAmI);
					Main.npc[num154].ai[0] = 1f;
					Main.npc[num154].ai[1] = npc.whoAmI;
					Main.npc[num154].ai[3] = 150f;
					Main.npc[num154].target = npc.target;
					Main.npc[num154].netUpdate = true;
				}
			}
			if (npc.type == 68 && npc.ai[1] != 3f && npc.ai[1] != 2f)
			{
				SoundExtensions.PlaySoundOld(SoundID.Roar, (int)npc.position.X, (int)npc.position.Y, 0);
				npc.ai[1] = 2f;
			}
			if (Main.player[npc.target].dead || Math.Abs(npc.position.X - Main.player[npc.target].position.X) > 2000f || Math.Abs(npc.position.Y - Main.player[npc.target].position.Y) > 2000f)
			{
				npc.TargetClosest();
				if (Main.player[npc.target].dead || Math.Abs(npc.position.X - Main.player[npc.target].position.X) > 2000f || Math.Abs(npc.position.Y - Main.player[npc.target].position.Y) > 2000f)
				{
					npc.ai[1] = 3f;
				}
			}
			if (Main.dayTime && npc.ai[1] != 3f && npc.ai[1] != 2f)
			{
				npc.ai[1] = 2f;
				SoundExtensions.PlaySoundOld(SoundID.Roar, (int)npc.position.X, (int)npc.position.Y, 0);
			}
			int num155 = 0;
			if (Main.expertMode)
			{
				for (int num156 = 0; num156 < 200; num156++)
				{
					if (Main.npc[num156].active && Main.npc[num156].type == npc.type + 1)
					{
						num155++;
					}
				}
				npc.defense += num155 * 1000;
				if (num155 > 0 && skeletronAliveCounter % 60 == 59 && npc.life < npc.lifeMax)
                {
					npc.HealEffect(npc.lifeMax - npc.life);
					npc.life = npc.lifeMax;
                }
				if ((num155 < 2 || (double)npc.life < (double)npc.lifeMax * 0.75) && npc.ai[1] == 0f)
				{
					float num157 = 60f;
					if (num155 == 0)
					{
						num157 /= 2f;
					}
					if (Main.netMode != 1 && npc.ai[2] % num157 == 0f)
					{
						Vector2 center3 = npc.Center;
						float num158 = Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2) - center3.X;
						float num159 = Main.player[npc.target].position.Y + (float)(Main.player[npc.target].height / 2) - center3.Y;
						float num160 = (float)Math.Sqrt(num158 * num158 + num159 * num159);
						if (Collision.CanHit(center3, 1, 1, Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height))
						{
							float num161 = 3f;
							if (num155 == 0)
							{
								num161 += 2f;
							}
							float num162 = Main.player[npc.target].position.X + (float)Main.player[npc.target].width * 0.5f - center3.X + (float)Main.rand.Next(-20, 21);
							float num163 = Main.player[npc.target].position.Y + (float)Main.player[npc.target].height * 0.5f - center3.Y + (float)Main.rand.Next(-20, 21);
							float num164 = (float)Math.Sqrt(num162 * num162 + num163 * num163);
							num164 = num161 / num164;
							num162 *= num164;
							num163 *= num164;
							Vector2 vector19 = new Vector2(num162 * 1f + (float)Main.rand.Next(-50, 51) * 0.01f, num163 * 1f + (float)Main.rand.Next(-50, 51) * 0.01f);
							vector19.Normalize();
							vector19 *= num161;
							vector19 += npc.velocity;
							int num165 = 17;
							int num166 = 270;
							center3 += vector19 * 5f;
							int num167 = Projectile.NewProjectile(npc.GetSource_FromThis(),center3, npc.DirectionTo(Main.player[npc.target].Center) * MathHelper.Lerp(7.5f, 5f, (float)npc.life / (float)npc.lifeMax), num166, num165, 0f, Main.myPlayer, -1f);
							Main.projectile[num167].timeLeft = 300;
						}
					}
				}
			}
			if (npc.ai[1] == 0f)
			{
				npc.damage = npc.defDamage;
				npc.ai[2] += 1f;
				if (npc.ai[2] >= 800f)
				{
					npc.ai[2] = 0f;
					npc.ai[1] = 1f;
					npc.TargetClosest();
					npc.netUpdate = true;
				}
				npc.rotation = npc.velocity.X / 15f;
				float num168 = 0.02f;
				float num169 = 2f;
				float num170 = 0.05f;
				float num171 = 8f;
				if (Main.expertMode)
				{
					num168 = 0.03f;
					num169 = 4f;
					num170 = 0.07f;
					num171 = 9.5f;
				}
				if (npc.position.Y > Main.player[npc.target].position.Y - 250f)
				{
					if (npc.velocity.Y > 0f)
					{
						npc.velocity.Y *= 0.98f;
					}
					npc.velocity.Y -= num168;
					if (npc.velocity.Y > num169)
					{
						npc.velocity.Y = num169;
					}
				}
				else if (npc.position.Y < Main.player[npc.target].position.Y - 250f)
				{
					if (npc.velocity.Y < 0f)
					{
						npc.velocity.Y *= 0.98f;
					}
					npc.velocity.Y += num168;
					if (npc.velocity.Y < 0f - num169)
					{
						npc.velocity.Y = 0f - num169;
					}
				}
				if (npc.position.X + (float)(npc.width / 2) > Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2))
				{
					if (npc.velocity.X > 0f)
					{
						npc.velocity.X *= 0.98f;
					}
					npc.velocity.X -= num170;
					if (npc.velocity.X > num171)
					{
						npc.velocity.X = num171;
					}
				}
				if (npc.position.X + (float)(npc.width / 2) < Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2))
				{
					if (npc.velocity.X < 0f)
					{
						npc.velocity.X *= 0.98f;
					}
					npc.velocity.X += num170;
					if (npc.velocity.X < 0f - num171)
					{
						npc.velocity.X = 0f - num171;
					}
				}
			}
			else if (npc.ai[1] == 1f) //Spin attack
			{
				npc.defense -= 10;
				if (skeletronTurnOverTime <= 0)
				{
					npc.ai[2] += 1f;
					skeletronTurnOverCounter++;
					if (skeletronTurnOverCounter >= MathHelper.Lerp(100, 300, (float)npc.life / (float)npc.lifeMax))
                    {
						skeletronTurnOverTime = (int)skeletronTurnOverTotalTime;
						SoundStyle style = SoundID.ForceRoar;
						style.Pitch = 0.75f;
						SoundEngine.PlaySound(style, npc.Center);
						skeletronTurnOverDirection *= -1;
                    }
				}
                else
                {
					npc.position = npc.Center.RotatedBy(Math.PI / skeletronTurnOverTotalTime * skeletronTurnOverDirection, Main.LocalPlayer.Center) - npc.Size / 2;
					skeletronTurnOverTime--;
					skeletronTurnOverCounter = 0;

					skeletronTurnOverProjectileTimer++;
					if (skeletronTurnOverProjectileTimer > skeletronTurnOverTotalTime / 3)
                    {
						skeletronTurnOverProjectileTimer = 0;
						int num165 = 17;
						int num166 = 270;
						int num167 = Projectile.NewProjectile(npc.GetSource_FromThis(), npc.Center, npc.DirectionTo(Main.player[npc.target].Center) * MathHelper.Lerp(4f, 1f, (float)npc.life / (float)npc.lifeMax), num166, num165, 0f, Main.myPlayer, -1f);
						Main.projectile[num167].timeLeft = 300;

					}
                }
				if (npc.ai[2] == 2f)
				{
					SoundEngine.PlaySound(SoundID.Roar, npc.Center);

				}
				if (npc.ai[2] >= 400f)
				{
					npc.ai[2] = 0f;
					npc.ai[1] = 0f;
				}
				npc.rotation += (float)npc.direction * 0.5f;
				Vector2 vector20 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
				float num172 = Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2) - vector20.X;
				float num173 = Main.player[npc.target].position.Y + (float)(Main.player[npc.target].height / 2) - vector20.Y;
				float num174 = (float)Math.Sqrt(num172 * num172 + num173 * num173);
				float num175 = 1.5f;
				if (Main.expertMode)
				{
					npc.damage = (int)((double)npc.defDamage * 1.3);
					num175 = 4f;
					if (num174 > 150f)
					{
						num175 *= 1.05f;
					}
					if (num174 > 200f)
					{
						num175 *= 1.1f;
					}
					if (num174 > 250f)
					{
						num175 *= 1.1f;
					}
					if (num174 > 300f)
					{
						num175 *= 1.1f;
					}
					if (num174 > 350f)
					{
						num175 *= 1.1f;
					}
					if (num174 > 400f)
					{
						num175 *= 1.1f;
					}
					if (num174 > 450f)
					{
						num175 *= 1.1f;
					}
					if (num174 > 500f)
					{
						num175 *= 1.1f;
					}
					if (num174 > 550f)
					{
						num175 *= 1.1f;
					}
					if (num174 > 600f)
					{
						num175 *= 1.1f;
					}
					switch (num155)
					{
						case 0:
							num175 *= 1.2f;
							break;
						case 1:
							num175 *= 1.1f;
							break;
					}
				}
				num174 = num175 / num174;
				npc.velocity.X = num172 * num174;
				npc.velocity.Y = num173 * num174;
			}
			else if (npc.ai[1] == 2f)
			{
				npc.damage = 1000;
				npc.defense = 9999;
				npc.rotation += (float)npc.direction * 0.3f;
				Vector2 vector21 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
				float num176 = Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2) - vector21.X;
				float num177 = Main.player[npc.target].position.Y + (float)(Main.player[npc.target].height / 2) - vector21.Y;
				float num178 = (float)Math.Sqrt(num176 * num176 + num177 * num177);
				num178 = 8f / num178;
				npc.velocity.X = num176 * num178;
				npc.velocity.Y = num177 * num178;
			}
			else if (npc.ai[1] == 3f)
			{
				npc.velocity.Y += 0.1f;
				if (npc.velocity.Y < 0f)
				{
					npc.velocity.Y *= 0.95f;
				}
				npc.velocity.X *= 0.95f;
				if (npc.timeLeft > 50)
				{
					npc.timeLeft = 50;
				}
			}
			if (npc.ai[1] != 2f && npc.ai[1] != 3f && npc.type != 68 && (num155 != 0 || !Main.expertMode))
			{
				int num179 = Dust.NewDust(new Vector2(npc.position.X + (float)(npc.width / 2) - 15f - npc.velocity.X * 5f, npc.position.Y + (float)npc.height - 2f), 30, 10, 5, (0f - npc.velocity.X) * 0.2f, 3f, 0, default(Color), 2f);
				Main.dust[num179].noGravity = true;
				Main.dust[num179].velocity.X *= 1.3f;
				Main.dust[num179].velocity.X += npc.velocity.X * 0.4f;
				Main.dust[num179].velocity.Y += 2f + npc.velocity.Y;
				for (int num180 = 0; num180 < 2; num180++)
				{
					num179 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y + 120f), npc.width, 60, 5, npc.velocity.X, npc.velocity.Y, 0, default(Color), 2f);
					Main.dust[num179].noGravity = true;
					Dust dust26 = Main.dust[num179];
					Dust dust2 = dust26;
					dust2.velocity -= npc.velocity;
					Main.dust[num179].velocity.Y += 5f;
				}
			}
		}
    }

	class SkeletronHand : TwilightNPCChange
	{
        public override bool ShouldChangeNPC(NPC npc)
        {
            return npc.type == NPCID.SkeletronHand;
        }

        public override bool HasNewAI(NPC npc)
        {
            return true;
        }

        public override void NewAI(NPC npc)
		{
			Player player = Main.player[npc.target];
			npc.spriteDirection = -(int)npc.ai[0];
			if (!Main.npc[(int)npc.ai[1]].active || Main.npc[(int)npc.ai[1]].aiStyle != 11)
			{
				npc.ai[2] += 10f;
				if (npc.ai[2] > 50f || Main.netMode != 2)
				{
					npc.life = -1;
					npc.HitEffect();
					npc.active = false;
				}
			}
			if (npc.ai[2] == 0f || npc.ai[2] == 3f)
			{
				if (Main.npc[(int)npc.ai[1]].ai[1] == 3f)
				{
					npc.EncourageDespawn(10);
				}
				if (Main.npc[(int)npc.ai[1]].ai[1] != 0f)
				{
					if (npc.position.Y > Main.npc[(int)npc.ai[1]].position.Y - 100f)
					{
						if (npc.velocity.Y > 0f)
						{
							npc.velocity.Y *= 0.96f;
						}
						npc.velocity.Y -= 0.07f;
						if (npc.velocity.Y > 6f)
						{
							npc.velocity.Y = 6f;
						}
					}
					else if (npc.position.Y < Main.npc[(int)npc.ai[1]].position.Y - 100f)
					{
						if (npc.velocity.Y < 0f)
						{
							npc.velocity.Y *= 0.96f;
						}
						npc.velocity.Y += 0.07f;
						if (npc.velocity.Y < -6f)
						{
							npc.velocity.Y = -6f;
						}
					}
					if (npc.position.X + (float)(npc.width / 2) > Main.npc[(int)npc.ai[1]].position.X + (float)(Main.npc[(int)npc.ai[1]].width / 2) - 120f * npc.ai[0])
					{
						if (npc.velocity.X > 0f)
						{
							npc.velocity.X *= 0.96f;
						}
						npc.velocity.X -= 0.1f;
						if (npc.velocity.X > 8f)
						{
							npc.velocity.X = 8f;
						}
					}
					if (npc.position.X + (float)(npc.width / 2) < Main.npc[(int)npc.ai[1]].position.X + (float)(Main.npc[(int)npc.ai[1]].width / 2) - 120f * npc.ai[0])
					{
						if (npc.velocity.X < 0f)
						{
							npc.velocity.X *= 0.96f;
						}
						npc.velocity.X += 0.1f;
						if (npc.velocity.X < -8f)
						{
							npc.velocity.X = -8f;
						}
					}
				}
				else
				{
					npc.ai[3] += 1f;
					if (Main.expertMode)
					{
						npc.ai[3] += 0.5f;
					}
					if (npc.ai[3] >= 300f)
					{
						npc.ai[2] += 1f;
						npc.ai[3] = 0f;
						npc.netUpdate = true;
					}
					if (Main.expertMode)
					{
						if (npc.position.Y > Main.npc[(int)npc.ai[1]].position.Y + 230f)
						{
							if (npc.velocity.Y > 0f)
							{
								npc.velocity.Y *= 0.96f;
							}
							npc.velocity.Y -= 0.04f;
							if (npc.velocity.Y > 3f)
							{
								npc.velocity.Y = 3f;
							}
						}
						else if (npc.position.Y < Main.npc[(int)npc.ai[1]].position.Y + 230f)
						{
							if (npc.velocity.Y < 0f)
							{
								npc.velocity.Y *= 0.96f;
							}
							npc.velocity.Y += 0.04f;
							if (npc.velocity.Y < -3f)
							{
								npc.velocity.Y = -3f;
							}
						}
						if (npc.position.X + (float)(npc.width / 2) > Main.npc[(int)npc.ai[1]].position.X + (float)(Main.npc[(int)npc.ai[1]].width / 2) - 200f * npc.ai[0])
						{
							if (npc.velocity.X > 0f)
							{
								npc.velocity.X *= 0.96f;
							}
							npc.velocity.X -= 0.07f;
							if (npc.velocity.X > 8f)
							{
								npc.velocity.X = 8f;
							}
						}
						if (npc.position.X + (float)(npc.width / 2) < Main.npc[(int)npc.ai[1]].position.X + (float)(Main.npc[(int)npc.ai[1]].width / 2) - 200f * npc.ai[0])
						{
							if (npc.velocity.X < 0f)
							{
								npc.velocity.X *= 0.96f;
							}
							npc.velocity.X += 0.07f;
							if (npc.velocity.X < -8f)
							{
								npc.velocity.X = -8f;
							}
						}
					}
					if (npc.position.Y > Main.npc[(int)npc.ai[1]].position.Y + 230f)
					{
						if (npc.velocity.Y > 0f)
						{
							npc.velocity.Y *= 0.96f;
						}
						npc.velocity.Y -= 0.04f;
						if (npc.velocity.Y > 3f)
						{
							npc.velocity.Y = 3f;
						}
					}
					else if (npc.position.Y < Main.npc[(int)npc.ai[1]].position.Y + 230f)
					{
						if (npc.velocity.Y < 0f)
						{
							npc.velocity.Y *= 0.96f;
						}
						npc.velocity.Y += 0.04f;
						if (npc.velocity.Y < -3f)
						{
							npc.velocity.Y = -3f;
						}
					}
					if (npc.position.X + (float)(npc.width / 2) > Main.npc[(int)npc.ai[1]].position.X + (float)(Main.npc[(int)npc.ai[1]].width / 2) - 200f * npc.ai[0])
					{
						if (npc.velocity.X > 0f)
						{
							npc.velocity.X *= 0.96f;
						}
						npc.velocity.X -= 0.07f;
						if (npc.velocity.X > 8f)
						{
							npc.velocity.X = 8f;
						}
					}
					if (npc.position.X + (float)(npc.width / 2) < Main.npc[(int)npc.ai[1]].position.X + (float)(Main.npc[(int)npc.ai[1]].width / 2) - 200f * npc.ai[0])
					{
						if (npc.velocity.X < 0f)
						{
							npc.velocity.X *= 0.96f;
						}
						npc.velocity.X += 0.07f;
						if (npc.velocity.X < -8f)
						{
							npc.velocity.X = -8f;
						}
					}
				}
				Vector2 vector101 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
				float num684 = Main.npc[(int)npc.ai[1]].position.X + (float)(Main.npc[(int)npc.ai[1]].width / 2) - 200f * npc.ai[0] - vector101.X;
				float num685 = Main.npc[(int)npc.ai[1]].position.Y + 230f - vector101.Y;
				float num686 = (float)Math.Sqrt(num684 * num684 + num685 * num685);
				npc.rotation = (float)Math.Atan2(num685, num684) + 1.57f;
			}
			else if (npc.ai[2] == 1f)
			{
				Vector2 vector113 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
				float num687 = Main.npc[(int)npc.ai[1]].position.X + (float)(Main.npc[(int)npc.ai[1]].width / 2) - 200f * npc.ai[0] - vector113.X;
				float num688 = Main.npc[(int)npc.ai[1]].position.Y + 230f - vector113.Y;
				float num689 = (float)Math.Sqrt(num687 * num687 + num688 * num688);
				npc.rotation = (float)Math.Atan2(num688, num687) + 1.57f;
				npc.velocity.X *= 0.95f;
				npc.velocity.Y -= 0.1f;
				if (Main.expertMode)
				{
					npc.velocity.Y -= 0.06f;
					if (npc.velocity.Y < -13f)
					{
						npc.velocity.Y = -13f;
					}
				}
				else if (npc.velocity.Y < -8f)
				{
					npc.velocity.Y = -8f;
				}
				if (npc.position.Y < Main.npc[(int)npc.ai[1]].position.Y - 200f)
				{
					npc.TargetClosest();
					npc.ai[2] = 2f;
					float speed = 32f;
					npc.velocity = npc.DirectionTo(player.Center + player.velocity * (npc.Distance(player.Center) / speed)) * speed;
					npc.netUpdate = true;
				}
			}
			else if (npc.ai[2] == 2f)
			{
				if (npc.position.Y > Main.player[npc.target].position.Y || npc.velocity.Y < 0f)
				{
					npc.ai[2] = 3f;
				}
			}
			else if (npc.ai[2] == 4f)
			{
				Vector2 vector124 = default(Vector2);
				vector124 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
				float num690 = Main.npc[(int)npc.ai[1]].position.X + (float)(Main.npc[(int)npc.ai[1]].width / 2) - 200f * npc.ai[0] - vector124.X;
				float num691 = Main.npc[(int)npc.ai[1]].position.Y + 230f - vector124.Y;
				float num693 = (float)Math.Sqrt(num690 * num690 + num691 * num691);
				npc.rotation = (float)Math.Atan2(num691, num690) + 1.57f;
				npc.velocity.Y *= 0.95f;
				npc.velocity.X += 0.1f * (0f - npc.ai[0]);
				if (Main.expertMode)
				{
					npc.velocity.X += 0.07f * (0f - npc.ai[0]);
					if (npc.velocity.X < -12f)
					{
						npc.velocity.X = -12f;
					}
					else if (npc.velocity.X > 12f)
					{
						npc.velocity.X = 12f;
					}
				}
				else if (npc.velocity.X < -8f)
				{
					npc.velocity.X = -8f;
				}
				else if (npc.velocity.X > 8f)
				{
					npc.velocity.X = 8f;
				}
				if (npc.position.X + (float)(npc.width / 2) < Main.npc[(int)npc.ai[1]].position.X + (float)(Main.npc[(int)npc.ai[1]].width / 2) - 500f || npc.position.X + (float)(npc.width / 2) > Main.npc[(int)npc.ai[1]].position.X + (float)(Main.npc[(int)npc.ai[1]].width / 2) + 500f)
				{
					npc.TargetClosest();
					npc.ai[2] = 5f;
					float speed = 32f;
					npc.velocity = npc.DirectionTo(player.Center + player.velocity * (npc.Distance(player.Center) / speed)) * speed;
					npc.netUpdate = true;
				}
			}
			else if (npc.ai[2] == 5f && ((npc.velocity.X > 0f && npc.position.X + (float)(npc.width / 2) > Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2)) || (npc.velocity.X < 0f && npc.position.X + (float)(npc.width / 2) < Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2))))
			{
				npc.ai[2] = 0f;
			}
		}
	}
}
