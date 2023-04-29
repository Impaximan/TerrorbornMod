using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using System;
using Terraria.Audio;

namespace TerrorbornMod.TwilightMode.NPCs
{
    class Worms : TwilightNPCChange
	{
        public override bool ShouldChangeNPC(NPC npc)
        {
            return npc.aiStyle == 6;
        }

        public override bool HasNewAI(NPC npc)
        {
            return true;
        }

        public override void NewAI(NPC npc)
        {
			WormAI(npc);
        }

        public void WormAI(NPC npc)
		{
			if (npc.type == NPCID.LeechHead && npc.localAI[1] == 0f)
			{
				npc.localAI[1] = 1f;
				SoundEngine.PlaySound(SoundID.NPCDeath13, npc.position);
				int num = 1;
				if (npc.velocity.X < 0f)
				{
					num = -1;
				}
				for (int i = 0; i < 20; i++)
				{
					Dust.NewDust(new Vector2(npc.position.X - 20f, npc.position.Y - 20f), npc.width + 40, npc.height + 40, DustID.Blood, num * 8, -1f);
				}
			}
			if (npc.type == NPCID.CultistDragonHead && npc.localAI[3] == 0f)
			{
				SoundEngine.PlaySound(SoundID.Item119, npc.position);
				npc.localAI[3] = 1f;
			}
			if (npc.type >= NPCID.CultistDragonHead && npc.type <= NPCID.CultistDragonTail)
			{
				npc.dontTakeDamage = npc.alpha > 0;
				if (npc.type == NPCID.CultistDragonHead || (npc.type != NPCID.CultistDragonHead && Main.npc[(int)npc.ai[1]].alpha < 85))
				{
					if (npc.dontTakeDamage)
					{
						for (int j = 0; j < 2; j++)
						{
							int num12 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, DustID.GoldFlame, 0f, 0f, 100, default(Color), 2f);
							Main.dust[num12].noGravity = true;
							Main.dust[num12].noLight = true;
						}
					}
					npc.alpha -= 42;
					if (npc.alpha < 0)
					{
						npc.alpha = 0;
					}
				}
			}
			if (npc.type >= NPCID.BloodEelHead && npc.type <= NPCID.BloodEelTail)
			{
				npc.position += npc.netOffset;
				npc.dontTakeDamage = npc.alpha > 0;
				if (npc.type == NPCID.BloodEelHead || (npc.type != NPCID.BloodEelHead && Main.npc[(int)npc.ai[1]].alpha < 85))
				{
					if (npc.dontTakeDamage)
					{
						for (int k = 0; k < 2; k++)
						{
							Dust.NewDust(npc.position, npc.width, npc.height, DustID.Blood, 0f, 0f, 100);
						}
					}
					npc.alpha -= 42;
					if (npc.alpha < 0)
					{
						npc.alpha = 0;
					}
				}
				if (npc.alpha == 0 && Main.rand.NextBool(5))
				{
					Dust.NewDust(npc.position, npc.width, npc.height, DustID.Blood, 0f, 0f, 100);
				}
				npc.position -= npc.netOffset;
			}
			else if (npc.type == NPCID.StardustWormHead && npc.ai[1] == 0f)
			{
				npc.ai[1] = Main.rand.Next(-2, 0);
				npc.netUpdate = true;
			}
			if (Main.netMode != NetmodeID.MultiplayerClient && Main.expertMode)
			{
				if (npc.type == NPCID.EaterofWorldsBody || Main.getGoodWorld)
				{
					int num23 = (int)(npc.Center.X / 16f);
					int num34 = (int)(npc.Center.Y / 16f);
					if (WorldGen.InWorld(num23, num34) && Main.tile[num23, num34].WallType == 0 && Main.rand.NextBool(3500))
					{
						npc.TargetClosest();
						if (Collision.CanHitLine(npc.Center, 1, 1, Main.player[npc.target].Center, 1, 1))
						{
							NPC.NewNPC(npc.GetSource_FromAI(), (int)(npc.position.X + (float)(npc.width / 2) + npc.velocity.X), (int)(npc.position.Y + (float)(npc.height / 2) + npc.velocity.Y), NPCID.VileSpitEaterOfWorlds, 0, 0f, 1f);
						}
					}
				}
				else if (npc.type == NPCID.EaterofWorldsHead)
				{
					int num44 = 90;
					num44 += (int)((float)npc.life / (float)npc.lifeMax * 60f * 10f);
					if (Main.rand.NextBool(num44))
					{
						npc.TargetClosest();
						if (Collision.CanHitLine(npc.Center, 1, 1, Main.player[npc.target].Center, 1, 1))
						{
							NPC.NewNPC(npc.GetSource_FromAI(), (int)(npc.position.X + (float)(npc.width / 2) + npc.velocity.X), (int)(npc.position.Y + (float)(npc.height / 2) + npc.velocity.Y), NPCID.VileSpitEaterOfWorlds, 0, 0f, 1f);
						}
					}
				}
			}
			bool flag = false;
			float num55 = 0.2f;
			switch (npc.type)
			{
				case 513:
					flag = !Main.player[npc.target].ZoneUndergroundDesert;
					num55 = 0.1f;
					break;
				case 10:
				case 39:
				case 95:
				case 117:
				case 510:
					flag = true;
					break;
				case 621:
					flag = false;
					break;
			}
			if (npc.type >= NPCID.EaterofWorldsHead && npc.type <= NPCID.EaterofWorldsTail)
			{
				npc.realLife = -1;
			}
			else if (npc.ai[3] > 0f)
			{
				npc.realLife = (int)npc.ai[3];
			}
			if (npc.target < 0 || npc.target == 255 || Main.player[npc.target].dead || (flag && (double)Main.player[npc.target].position.Y < Main.worldSurface * 16.0))
			{
				npc.TargetClosest();
			}
			if (Main.player[npc.target].dead || (flag && (double)Main.player[npc.target].position.Y < Main.worldSurface * 16.0))
			{
				npc.EncourageDespawn(300);
				if (flag)
				{
					npc.velocity.Y += num55;
				}
			}
			if (npc.type == NPCID.BloodEelHead && Main.dayTime)
			{
				npc.EncourageDespawn(60);
				npc.velocity.Y += 1f;
			}
			if (Main.netMode != NetmodeID.MultiplayerClient)
			{
				if (npc.type == NPCID.WyvernHead && npc.ai[0] == 0f)
				{
					npc.ai[3] = npc.whoAmI;
					npc.realLife = npc.whoAmI;
					int num65 = 0;
					int num70 = npc.whoAmI;
					for (int l = 0; l < 14; l++)
					{
						int num71 = 89;
						switch (l)
						{
							case 1:
							case 8:
								num71 = 88;
								break;
							case 11:
								num71 = 90;
								break;
							case 12:
								num71 = 91;
								break;
							case 13:
								num71 = 92;
								break;
						}
						num65 = NPC.NewNPC(npc.GetSource_FromAI(), (int)(npc.position.X + (float)(npc.width / 2)), (int)(npc.position.Y + (float)npc.height), num71, npc.whoAmI);
						Main.npc[num65].ai[3] = npc.whoAmI;
						Main.npc[num65].realLife = npc.whoAmI;
						Main.npc[num65].ai[1] = num70;
						Main.npc[num65].CopyInteractions(npc);
						Main.npc[num70].ai[0] = num65;
						NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, num65);
						num70 = num65;
					}
				}
				if (npc.type == NPCID.CultistDragonHead && npc.ai[0] == 0f)
				{
					npc.ai[3] = npc.whoAmI;
					npc.realLife = npc.whoAmI;
					int num2 = 0;
					int num3 = npc.whoAmI;
					for (int m = 0; m < 30; m++)
					{
						int num4 = 456;
						if ((m - 2) % 4 == 0 && m < 26)
						{
							num4 = 455;
						}
						else
						{
							switch (m)
							{
								case 27:
									num4 = 457;
									break;
								case 28:
									num4 = 458;
									break;
								case 29:
									num4 = 459;
									break;
							}
						}
						num2 = NPC.NewNPC(npc.GetSource_FromAI(), (int)(npc.position.X + (float)(npc.width / 2)), (int)(npc.position.Y + (float)npc.height), num4, npc.whoAmI);
						Main.npc[num2].ai[3] = npc.whoAmI;
						Main.npc[num2].realLife = npc.whoAmI;
						Main.npc[num2].ai[1] = num3;
						Main.npc[num2].CopyInteractions(npc);
						Main.npc[num3].ai[0] = num2;
						NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, num2);
						num3 = num2;
					}
				}
				if (npc.type == NPCID.TombCrawlerHead && npc.ai[0] == 0f)
				{
					npc.ai[3] = npc.whoAmI;
					npc.realLife = npc.whoAmI;
					int num5 = 0;
					int num6 = npc.whoAmI;
					int num7 = Main.rand.Next(6, 10);
					for (int n = 0; n < num7; n++)
					{
						int num8 = 514;
						if (n == num7 - 1)
						{
							num8 = 515;
						}
						num5 = NPC.NewNPC(npc.GetSource_FromAI(), (int)(npc.position.X + (float)(npc.width / 2)), (int)(npc.position.Y + (float)npc.height), num8, npc.whoAmI);
						Main.npc[num5].ai[3] = npc.whoAmI;
						Main.npc[num5].realLife = npc.whoAmI;
						Main.npc[num5].ai[1] = num6;
						Main.npc[num5].CopyInteractions(npc);
						Main.npc[num6].ai[0] = num5;
						NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, num5);
						num6 = num5;
					}
				}
				if (npc.type == NPCID.DuneSplicerHead && npc.ai[0] == 0f)
				{
					npc.ai[3] = npc.whoAmI;
					npc.realLife = npc.whoAmI;
					int num9 = 0;
					int num10 = npc.whoAmI;
					int num11 = Main.rand.Next(12, 21);
					for (int num13 = 0; num13 < num11; num13++)
					{
						int num14 = 511;
						if (num13 == num11 - 1)
						{
							num14 = 512;
						}
						num9 = NPC.NewNPC(npc.GetSource_FromAI(), (int)(npc.position.X + (float)(npc.width / 2)), (int)(npc.position.Y + (float)npc.height), num14, npc.whoAmI);
						Main.npc[num9].ai[3] = npc.whoAmI;
						Main.npc[num9].realLife = npc.whoAmI;
						Main.npc[num9].ai[1] = num10;
						Main.npc[num9].CopyInteractions(npc);
						Main.npc[num10].ai[0] = num9;
						NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, num9);
						num10 = num9;
					}
				}
				if (npc.type == NPCID.BloodEelHead && npc.ai[0] == 0f)
				{
					npc.ai[3] = npc.whoAmI;
					npc.realLife = npc.whoAmI;
					int num15 = 0;
					int num16 = npc.whoAmI;
					int num17 = 16;
					for (int num18 = 0; num18 < num17; num18++)
					{
						int num19 = 622;
						if (num18 == num17 - 1)
						{
							num19 = 623;
						}
						num15 = NPC.NewNPC(npc.GetSource_FromAI(), (int)(npc.position.X + (float)(npc.width / 2)), (int)(npc.position.Y + (float)npc.height), num19, npc.whoAmI);
						Main.npc[num15].ai[3] = npc.whoAmI;
						Main.npc[num15].realLife = npc.whoAmI;
						Main.npc[num15].ai[1] = num16;
						Main.npc[num15].CopyInteractions(npc);
						Main.npc[num16].ai[0] = num15;
						NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, num15);
						num16 = num15;
					}
				}
				else if ((npc.type == NPCID.DevourerHead || npc.type == NPCID.DevourerBody || npc.type == NPCID.GiantWormHead || npc.type == NPCID.GiantWormBody || npc.type == NPCID.EaterofWorldsHead || npc.type == NPCID.EaterofWorldsBody || npc.type == NPCID.BoneSerpentHead || npc.type == NPCID.BoneSerpentBody || npc.type == NPCID.DiggerHead || npc.type == NPCID.DiggerBody || npc.type == NPCID.SeekerHead || npc.type == NPCID.SeekerBody || npc.type == NPCID.LeechHead || npc.type == NPCID.LeechBody) && npc.ai[0] == 0f)
				{
					if (npc.type == NPCID.DevourerHead || npc.type == NPCID.GiantWormHead || npc.type == NPCID.EaterofWorldsHead || npc.type == NPCID.BoneSerpentHead || npc.type == NPCID.DiggerHead || npc.type == NPCID.SeekerHead || npc.type == NPCID.LeechHead)
					{
						if (npc.type < NPCID.EaterofWorldsHead || npc.type > NPCID.EaterofWorldsTail)
						{
							npc.ai[3] = npc.whoAmI;
							npc.realLife = npc.whoAmI;
						}
						npc.ai[2] = Main.rand.Next(8, 13);
						if (npc.type == NPCID.GiantWormHead)
						{
							npc.ai[2] = Main.rand.Next(4, 7);
						}
						if (npc.type == NPCID.EaterofWorldsHead)
						{
							npc.ai[2] = NPC.GetEaterOfWorldsSegmentsCount();
						}
						if (npc.type == NPCID.BoneSerpentHead)
						{
							npc.ai[2] = Main.rand.Next(12, 19);
						}
						if (npc.type == NPCID.DiggerHead)
						{
							npc.ai[2] = Main.rand.Next(6, 12);
						}
						if (npc.type == NPCID.SeekerHead)
						{
							npc.ai[2] = Main.rand.Next(20, 26);
						}
						if (npc.type == NPCID.LeechHead)
						{
							npc.ai[2] = Main.rand.Next(3, 6);
						}
						npc.ai[0] = NPC.NewNPC(npc.GetSource_FromAI(), (int)(npc.position.X + (float)(npc.width / 2)), (int)(npc.position.Y + (float)npc.height), npc.type + 1, npc.whoAmI);
						Main.npc[(int)npc.ai[0]].CopyInteractions(npc);
					}
					else if ((npc.type == NPCID.DevourerBody || npc.type == NPCID.GiantWormBody || npc.type == NPCID.EaterofWorldsBody || npc.type == NPCID.BoneSerpentBody || npc.type == NPCID.DiggerBody || npc.type == NPCID.SeekerBody || npc.type == NPCID.LeechBody) && npc.ai[2] > 0f)
					{
						npc.ai[0] = NPC.NewNPC(npc.GetSource_FromAI(), (int)(npc.position.X + (float)(npc.width / 2)), (int)(npc.position.Y + (float)npc.height), npc.type, npc.whoAmI);
						Main.npc[(int)npc.ai[0]].CopyInteractions(npc);
					}
					else
					{
						npc.ai[0] = NPC.NewNPC(npc.GetSource_FromAI(), (int)(npc.position.X + (float)(npc.width / 2)), (int)(npc.position.Y + (float)npc.height), npc.type + 1, npc.whoAmI);
						Main.npc[(int)npc.ai[0]].CopyInteractions(npc);
					}
					if (npc.type < NPCID.EaterofWorldsHead || npc.type > NPCID.EaterofWorldsTail)
					{
						Main.npc[(int)npc.ai[0]].ai[3] = npc.ai[3];
						Main.npc[(int)npc.ai[0]].realLife = npc.realLife;
					}
					Main.npc[(int)npc.ai[0]].ai[1] = npc.whoAmI;
					Main.npc[(int)npc.ai[0]].ai[2] = npc.ai[2] - 1f;
					npc.netUpdate = true;
				}
				if (npc.type == NPCID.SolarCrawltipedeHead && npc.ai[0] == 0f)
				{
					npc.ai[3] = npc.whoAmI;
					npc.realLife = npc.whoAmI;
					int num20 = 0;
					int num21 = npc.whoAmI;
					int num22 = 30;
					for (int num24 = 0; num24 < num22; num24++)
					{
						int num25 = 413;
						if (num24 == num22 - 1)
						{
							num25 = 414;
						}
						num20 = NPC.NewNPC(npc.GetSource_FromAI(), (int)(npc.position.X + (float)(npc.width / 2)), (int)(npc.position.Y + (float)npc.height), num25, npc.whoAmI);
						Main.npc[num20].ai[3] = npc.whoAmI;
						Main.npc[num20].realLife = npc.whoAmI;
						Main.npc[num20].ai[1] = num21;
						Main.npc[num20].CopyInteractions(npc);
						Main.npc[num21].ai[0] = num20;
						NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, num20);
						num21 = num20;
					}
				}
				switch (npc.type)
				{
					case 8:
					case 9:
					case 11:
					case 12:
					case 40:
					case 41:
					case 88:
					case 89:
					case 90:
					case 91:
					case 92:
					case 96:
					case 97:
					case 99:
					case 100:
					case 118:
					case 119:
					case 413:
					case 414:
					case 455:
					case 456:
					case 457:
					case 458:
					case 459:
					case 511:
					case 512:
					case 514:
					case 515:
					case 622:
					case 623:
						if (!Main.npc[(int)npc.ai[1]].active || Main.npc[(int)npc.ai[1]].aiStyle != npc.aiStyle)
						{
							npc.life = 0;
							npc.HitEffect();
							npc.active = false;
							NetMessage.SendData(MessageID.DamageNPC, -1, -1, null, npc.whoAmI, -1f);
						}
						break;
				}
				switch (npc.type)
				{
					case 7:
					case 8:
					case 10:
					case 11:
					case 39:
					case 40:
					case 87:
					case 88:
					case 89:
					case 90:
					case 91:
					case 95:
					case 96:
					case 98:
					case 99:
					case 117:
					case 118:
					case 412:
					case 413:
					case 454:
					case 455:
					case 456:
					case 457:
					case 458:
					case 510:
					case 511:
					case 513:
					case 514:
					case 621:
					case 622:
						if (!Main.npc[(int)npc.ai[0]].active || Main.npc[(int)npc.ai[0]].aiStyle != npc.aiStyle)
						{
							npc.life = 0;
							npc.HitEffect();
							npc.active = false;
							NetMessage.SendData(MessageID.DamageNPC, -1, -1, null, npc.whoAmI, -1f);
						}
						break;
				}
				if (npc.type == NPCID.EaterofWorldsHead || npc.type == NPCID.EaterofWorldsBody || npc.type == NPCID.EaterofWorldsTail)
				{
					if (!Main.npc[(int)npc.ai[1]].active && !Main.npc[(int)npc.ai[0]].active)
					{
						npc.life = 0;
						npc.HitEffect();
						npc.checkDead();
						npc.active = false;
						NetMessage.SendData(MessageID.DamageNPC, -1, -1, null, npc.whoAmI, -1f);
					}
					if (npc.type == NPCID.EaterofWorldsHead && !Main.npc[(int)npc.ai[0]].active)
					{
						npc.life = 0;
						npc.HitEffect();
						npc.checkDead();
						npc.active = false;
						NetMessage.SendData(MessageID.DamageNPC, -1, -1, null, npc.whoAmI, -1f);
					}
					if (npc.type == NPCID.EaterofWorldsTail && !Main.npc[(int)npc.ai[1]].active)
					{
						npc.life = 0;
						npc.HitEffect();
						npc.checkDead();
						npc.active = false;
						NetMessage.SendData(MessageID.DamageNPC, -1, -1, null, npc.whoAmI, -1f);
					}
					if (npc.type == NPCID.EaterofWorldsBody && (!Main.npc[(int)npc.ai[1]].active || Main.npc[(int)npc.ai[1]].aiStyle != npc.aiStyle))
					{
						npc.type = NPCID.EaterofWorldsHead;
						int whoAmI = npc.whoAmI;
						float num26 = (float)npc.life / (float)npc.lifeMax;
						float num27 = npc.ai[0];
						npc.SetDefaultsKeepPlayerInteraction(npc.type);
						npc.life = (int)((float)npc.lifeMax * num26);
						npc.ai[0] = num27;
						npc.TargetClosest();
						npc.netUpdate = true;
						npc.whoAmI = whoAmI;
						npc.alpha = 0;
					}
					if (npc.type == NPCID.EaterofWorldsBody && (!Main.npc[(int)npc.ai[0]].active || Main.npc[(int)npc.ai[0]].aiStyle != npc.aiStyle))
					{
						npc.type = NPCID.EaterofWorldsTail;
						int whoAmI2 = npc.whoAmI;
						float num28 = (float)npc.life / (float)npc.lifeMax;
						float num29 = npc.ai[1];
						npc.SetDefaultsKeepPlayerInteraction(npc.type);
						npc.life = (int)((float)npc.lifeMax * num28);
						npc.ai[1] = num29;
						npc.TargetClosest();
						npc.netUpdate = true;
						npc.whoAmI = whoAmI2;
						npc.alpha = 0;
					}
				}
				if (!npc.active && Main.netMode == NetmodeID.Server)
				{
					NetMessage.SendData(MessageID.DamageNPC, -1, -1, null, npc.whoAmI, -1f);
				}
			}
			int num30 = (int)(npc.position.X / 16f) - 1;
			int num31 = (int)((npc.position.X + (float)npc.width) / 16f) + 2;
			int num32 = (int)(npc.position.Y / 16f) - 1;
			int num33 = (int)((npc.position.Y + (float)npc.height) / 16f) + 2;
			if (num30 < 0)
			{
				num30 = 0;
			}
			if (num31 > Main.maxTilesX)
			{
				num31 = Main.maxTilesX;
			}
			if (num32 < 0)
			{
				num32 = 0;
			}
			if (num33 > Main.maxTilesY)
			{
				num33 = Main.maxTilesY;
			}
			bool flag2 = false;
			if (npc.type >= NPCID.WyvernHead && npc.type <= NPCID.WyvernTail)
			{
				flag2 = true;
			}
			if (npc.type >= NPCID.CultistDragonHead && npc.type <= NPCID.CultistDragonTail)
			{
				flag2 = true;
			}

			//make worms fly
			if (npc.type >= NPCID.EaterofWorldsHead && npc.type <= NPCID.EaterofWorldsTail)
			{
				int amount = 0;
				foreach (NPC iNpc in Main.npc)
				{
					if (iNpc.type == NPCID.EaterofWorldsBody && iNpc.active)
					{
						amount++;
					}
				}
				if (amount <= NPC.GetEaterOfWorldsSegmentsCount() / 2)
				{
					flag2 = true;
				}
			}

			if (npc.type >= NPCID.LeechBody && npc.type <= NPCID.LeechTail)
			{
				flag2 = true;
			}

			if (npc.type >= NPCID.BloodEelHead && npc.type <= NPCID.BloodEelTail)
			{
				flag2 = true;
			}
			if (npc.type == NPCID.StardustWormHead && npc.ai[1] == -1f)
			{
				flag2 = true;
			}
			if (npc.type >= NPCID.SolarCrawltipedeHead && npc.type <= NPCID.SolarCrawltipedeTail)
			{
				flag2 = true;
			}
			if (!flag2)
			{
				Vector2 vector = default(Vector2);
				for (int num35 = num30; num35 < num31; num35++)
				{
					for (int num36 = num32; num36 < num33; num36++)
					{
						if (Main.tile[num35, num36] == null || ((!Main.tile[num35, num36].HasUnactuatedTile || (!Main.tileSolid[Main.tile[num35, num36].TileType] && (!Main.tileSolidTop[Main.tile[num35, num36].TileType] || Main.tile[num35, num36].TileFrameY != 0))) && Main.tile[num35, num36].LiquidAmount <= 64))
						{
							continue;
						}
						vector.X = num35 * 16;
						vector.Y = num36 * 16;
						if (npc.position.X + (float)npc.width > vector.X && npc.position.X < vector.X + 16f && npc.position.Y + (float)npc.height > vector.Y && npc.position.Y < vector.Y + 16f)
						{
							flag2 = true;
							if (Main.rand.NextBool(100) && npc.type != NPCID.LeechHead && Main.tile[num35, num36].HasUnactuatedTile && Main.tileSolid[Main.tile[num35, num36].TileType])
							{
								WorldGen.KillTile(num35, num36, fail: true, effectOnly: true);
							}
						}
					}
				}
			}
			if (!flag2 && (npc.type == NPCID.DevourerHead || npc.type == NPCID.GiantWormHead || npc.type == NPCID.EaterofWorldsHead || npc.type == NPCID.BoneSerpentHead || npc.type == NPCID.DiggerHead || npc.type == NPCID.SeekerHead || npc.type == NPCID.LeechHead || npc.type == NPCID.TruffleWormDigger || npc.type == NPCID.CultistDragonHead || npc.type == NPCID.DuneSplicerHead || npc.type == NPCID.TombCrawlerHead || npc.type == NPCID.BloodEelHead))
			{
				Rectangle rectangle = default(Rectangle);
				rectangle = new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height);
				int num37 = 1000;
				bool flag3 = true;
				Rectangle rectangle2 = default(Rectangle);
				for (int num38 = 0; num38 < 255; num38++)
				{
					if (Main.player[num38].active)
					{
						rectangle2 = new Rectangle((int)Main.player[num38].position.X - num37, (int)Main.player[num38].position.Y - num37, num37 * 2, num37 * 2);
						if (rectangle.Intersects(rectangle2))
						{
							flag3 = false;
							break;
						}
					}
				}
				if (flag3)
				{
					flag2 = true;
				}
			}
			if ((npc.type >= NPCID.WyvernHead && npc.type <= NPCID.WyvernTail) || (npc.type >= NPCID.CultistDragonHead && npc.type <= NPCID.CultistDragonTail) || (npc.type >= NPCID.BloodEelHead && npc.type <= NPCID.BloodEelTail))
			{
				if (npc.velocity.X < 0f)
				{
					npc.spriteDirection = 1;
				}
				else if (npc.velocity.X > 0f)
				{
					npc.spriteDirection = -1;
				}
			}
			if (npc.type == NPCID.SolarCrawltipedeTail)
			{
				if (npc.justHit)
				{
					npc.localAI[3] = 3f;
				}
				if (npc.localAI[2] > 0f)
				{
					npc.localAI[2] -= 16f;
					if (npc.localAI[2] == 0f)
					{
						npc.localAI[2] = -128f;
					}
				}
				else if (npc.localAI[2] < 0f)
				{
					npc.localAI[2] += 16f;
				}
				else if (npc.localAI[3] > 0f)
				{
					npc.localAI[2] = 128f;
					npc.localAI[3] -= 1f;
				}
			}
			if (npc.type == NPCID.SolarCrawltipedeHead)
			{
				npc.position += npc.netOffset;
				Vector2 value = npc.Center + (npc.rotation - (float)Math.PI / 2f).ToRotationVector2() * 8f;
				Vector2 value2 = npc.rotation.ToRotationVector2() * 16f;
				Dust obj = Main.dust[Dust.NewDust(value + value2, 0, 0, DustID.Torch, npc.velocity.X, npc.velocity.Y, 100, Color.Transparent, 1f + Main.rand.NextFloat() * 3f)];
				obj.noGravity = true;
				obj.noLight = true;
				obj.position -= new Vector2(4f);
				obj.fadeIn = 1f;
				obj.velocity = Vector2.Zero;
				Dust obj2 = Main.dust[Dust.NewDust(value - value2, 0, 0, DustID.Torch, npc.velocity.X, npc.velocity.Y, 100, Color.Transparent, 1f + Main.rand.NextFloat() * 3f)];
				obj2.noGravity = true;
				obj2.noLight = true;
				obj2.position -= new Vector2(4f);
				obj2.fadeIn = 1f;
				obj2.velocity = Vector2.Zero;
				npc.position -= npc.netOffset;
			}
			float num39 = 8f;
			float num40 = 0.07f;
			if (npc.type == NPCID.DiggerHead)
			{
				num39 = 5.5f;
				num40 = 0.045f;
			}
			if (npc.type == NPCID.GiantWormHead)
			{
				num39 = 6f;
				num40 = 0.05f;
			}
			if (npc.type == NPCID.TombCrawlerHead)
			{
				num39 = 7f;
				num40 = 0.1f;
			}
			if (npc.type == NPCID.EaterofWorldsHead)
			{
				num39 = 10f;
				num40 = 0.07f;
				if (Main.expertMode)
				{
					num39 = 12f;
					num40 = 0.15f;
				}
				if (Main.getGoodWorld)
				{
					num39 += 4f;
					num40 += 0.05f;
				}
			}
			if (npc.type == NPCID.DuneSplicerHead)
			{
				if (!Main.player[npc.target].dead && Main.player[npc.target].ZoneSandstorm)
				{
					num39 = 16f;
					num40 = 0.35f;
				}
				else
				{
					num39 = 10f;
					num40 = 0.25f;
				}
			}
			if (npc.type == NPCID.WyvernHead)
			{
				num39 = 11f;
				num40 = 0.25f;
			}
			if (npc.type == NPCID.BloodEelHead)
			{
				num39 = 15f;
				num40 = 0.45f;
			}
			if (npc.type == NPCID.TruffleWormDigger)
			{
				num39 = 6f;
				num40 = 0.15f;
			}
			if (npc.type == NPCID.CultistDragonHead)
			{
				num39 = 20f;
				num40 = 0.55f;
			}
			if (npc.type == NPCID.StardustWormHead)
			{
				num39 = 6f;
				num40 = 0.2f;
			}
			if (npc.type == NPCID.LeechHead && Main.wofNPCIndex >= 0)
			{
				float num72 = (float)Main.npc[Main.wofNPCIndex].life / (float)Main.npc[Main.wofNPCIndex].lifeMax;
				if ((double)num72 < 0.5)
				{
					num39 += 1f;
					num40 += 0.1f;
				}
				if ((double)num72 < 0.25)
				{
					num39 += 1f;
					num40 += 0.1f;
				}
				if ((double)num72 < 0.1)
				{
					num39 += 2f;
					num40 += 0.1f;
				}
			}
			Vector2 vector2 = default(Vector2);
			vector2 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
			float num41 = Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2);
			float num42 = Main.player[npc.target].position.Y + (float)(Main.player[npc.target].height / 2);
			if (npc.type == NPCID.SolarCrawltipedeHead)
			{
				num39 = 10f;
				num40 = 0.3f;
				int num43 = -1;
				int num45 = (int)(Main.player[npc.target].Center.X / 16f);
				int num46 = (int)(Main.player[npc.target].Center.Y / 16f);
				for (int num47 = num45 - 2; num47 <= num45 + 2; num47++)
				{
					for (int num48 = num46; num48 <= num46 + 15; num48++)
					{
						if (WorldGen.SolidTile2(num47, num48))
						{
							num43 = num48;
							break;
						}
					}
					if (num43 > 0)
					{
						break;
					}
				}
				if (num43 > 0)
				{
					num43 *= 16;
					float num49 = num43 - 800;
					if (Main.player[npc.target].position.Y > num49)
					{
						num42 = num49;
						if (Math.Abs(npc.Center.X - Main.player[npc.target].Center.X) < 500f)
						{
							num41 = ((!(npc.velocity.X > 0f)) ? (Main.player[npc.target].Center.X - 600f) : (Main.player[npc.target].Center.X + 600f));
						}
					}
				}
				else
				{
					num39 = 14f;
					num40 = 0.5f;
				}
				float num50 = num39 * 1.3f;
				float num51 = num39 * 0.7f;
				float num52 = npc.velocity.Length();
				if (num52 > 0f)
				{
					if (num52 > num50)
					{
						npc.velocity.Normalize();
						npc.velocity *= num50;
					}
					else if (num52 < num51)
					{
						npc.velocity.Normalize();
						npc.velocity *= num51;
					}
				}
				if (num43 > 0)
				{
					for (int num53 = 0; num53 < 200; num53++)
					{
						if (Main.npc[num53].active && Main.npc[num53].type == npc.type && num53 != npc.whoAmI)
						{
							Vector2 vector3 = Main.npc[num53].Center - npc.Center;
							if (vector3.Length() < 400f)
							{
								vector3.Normalize();
								vector3 *= 1000f;
								num41 -= vector3.X;
								num42 -= vector3.Y;
							}
						}
					}
				}
				else
				{
					for (int num54 = 0; num54 < 200; num54++)
					{
						if (Main.npc[num54].active && Main.npc[num54].type == npc.type && num54 != npc.whoAmI)
						{
							Vector2 vector4 = Main.npc[num54].Center - npc.Center;
							if (vector4.Length() < 60f)
							{
								vector4.Normalize();
								vector4 *= 200f;
								num41 -= vector4.X;
								num42 -= vector4.Y;
							}
						}
					}
				}
			}
			if (npc.type != NPCID.EaterofWorldsHead)
			{
				num40 *= 1.4f;
			}
			num41 = (int)(num41 / 16f) * 16;
			num42 = (int)(num42 / 16f) * 16;
			vector2.X = (int)(vector2.X / 16f) * 16;
			vector2.Y = (int)(vector2.Y / 16f) * 16;
			num41 -= vector2.X;
			num42 -= vector2.Y;
			if (npc.type == NPCID.TruffleWormDigger)
			{
				num41 *= -1f;
				num42 *= -1f;
			}
			float num56 = (float)Math.Sqrt((double)(num41 * num41 + num42 * num42));
			if (npc.ai[1] > 0f && npc.ai[1] < (float)Main.npc.Length)
			{
				try
				{
					vector2 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
					num41 = Main.npc[(int)npc.ai[1]].position.X + (float)(Main.npc[(int)npc.ai[1]].width / 2) - vector2.X;
					num42 = Main.npc[(int)npc.ai[1]].position.Y + (float)(Main.npc[(int)npc.ai[1]].height / 2) - vector2.Y;
				}
				catch
				{
				}
				npc.rotation = (float)Math.Atan2((double)num42, (double)num41) + 1.57f;
				num56 = (float)Math.Sqrt((double)(num41 * num41 + num42 * num42));
				int num57 = npc.width;
				if (npc.type >= NPCID.WyvernHead && npc.type <= NPCID.WyvernTail)
				{
					num57 = 42;
				}
				if (npc.type >= NPCID.CultistDragonHead && npc.type <= NPCID.CultistDragonTail)
				{
					num57 = 36;
				}
				if (npc.type >= NPCID.EaterofWorldsHead && npc.type <= NPCID.EaterofWorldsTail)
				{
					num57 = (int)((float)num57 * npc.scale);
				}
				if (npc.type >= NPCID.TombCrawlerHead && npc.type <= NPCID.TombCrawlerTail)
				{
					num57 -= 6;
				}
				if (npc.type >= NPCID.SolarCrawltipedeHead && npc.type <= NPCID.SolarCrawltipedeTail)
				{
					num57 += 6;
				}
				if (npc.type >= NPCID.BloodEelHead && npc.type <= NPCID.BloodEelTail)
				{
					num57 = 24;
				}
				if (Main.getGoodWorld && npc.type >= NPCID.EaterofWorldsHead && npc.type <= NPCID.EaterofWorldsTail)
				{
					num57 = 62;
				}
				num56 = (num56 - (float)num57) / num56;
				num41 *= num56;
				num42 *= num56;
				npc.velocity = Vector2.Zero;
				npc.position.X += num41;
				npc.position.Y += num42;
				if (npc.type >= NPCID.WyvernHead && npc.type <= NPCID.WyvernTail)
				{
					if (num41 < 0f)
					{
						npc.spriteDirection = 1;
					}
					else if (num41 > 0f)
					{
						npc.spriteDirection = -1;
					}
				}
				if (npc.type >= NPCID.CultistDragonHead && npc.type <= NPCID.CultistDragonTail)
				{
					if (num41 < 0f)
					{
						npc.spriteDirection = 1;
					}
					else if (num41 > 0f)
					{
						npc.spriteDirection = -1;
					}
				}
				if (npc.type >= NPCID.BloodEelHead && npc.type <= NPCID.BloodEelTail)
				{
					if (num41 < 0f)
					{
						npc.spriteDirection = 1;
					}
					else if (num41 > 0f)
					{
						npc.spriteDirection = -1;
					}
				}
			}
			else
			{
				if (!flag2)
				{
					npc.TargetClosest();
					npc.velocity.Y += 0.11f;
					if (npc.velocity.Y > num39)
					{
						npc.velocity.Y = num39;
					}
					if ((double)(Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y)) < (double)num39 * 0.4)
					{
						if (npc.velocity.X < 0f)
						{
							npc.velocity.X -= num40 * 1.1f;
						}
						else
						{
							npc.velocity.X += num40 * 1.1f;
						}
					}
					else if (npc.velocity.Y == num39)
					{
						if (npc.velocity.X < num41)
						{
							npc.velocity.X += num40;
						}
						else if (npc.velocity.X > num41)
						{
							npc.velocity.X -= num40;
						}
					}
					else if (npc.velocity.Y > 4f)
					{
						if (npc.velocity.X < 0f)
						{
							npc.velocity.X += num40 * 0.9f;
						}
						else
						{
							npc.velocity.X -= num40 * 0.9f;
						}
					}
				}
				else
				{
					if (npc.type != NPCID.BloodEelHead && npc.type != NPCID.WyvernHead && npc.type != NPCID.LeechHead && npc.type != NPCID.CultistDragonHead && npc.type != NPCID.SolarCrawltipedeHead && npc.soundDelay == 0)
					{
						float num58 = num56 / 40f;
						if (num58 < 10f)
						{
							num58 = 10f;
						}
						if (num58 > 20f)
						{
							num58 = 20f;
						}
						npc.soundDelay = (int)num58;
						SoundExtensions.PlaySoundOld(SoundID.WormDig, (int)npc.position.X, (int)npc.position.Y);
					}
					num56 = (float)Math.Sqrt((double)(num41 * num41 + num42 * num42));
					float num59 = Math.Abs(num41);
					float num60 = Math.Abs(num42);
					float num61 = num39 / num56;
					num41 *= num61;
					num42 *= num61;
					bool flag4 = false;
					if ((npc.type == NPCID.DevourerHead || npc.type == NPCID.EaterofWorldsHead) && ((!Main.player[npc.target].ZoneCorrupt && !Main.player[npc.target].ZoneCrimson) || Main.player[npc.target].dead))
					{
						flag4 = true;
					}
					if ((npc.type == NPCID.TombCrawlerHead && (double)Main.player[npc.target].position.Y < Main.worldSurface * 16.0 && !Main.player[npc.target].ZoneSandstorm && !Main.player[npc.target].ZoneUndergroundDesert) || Main.player[npc.target].dead)
					{
						flag4 = true;
					}
					if ((npc.type == NPCID.DuneSplicerHead && (double)Main.player[npc.target].position.Y < Main.worldSurface * 16.0 && !Main.player[npc.target].ZoneSandstorm && !Main.player[npc.target].ZoneUndergroundDesert) || Main.player[npc.target].dead)
					{
						flag4 = true;
					}
					if (flag4)
					{
						bool flag5 = true;
						for (int num62 = 0; num62 < 255; num62++)
						{
							if (Main.player[num62].active && !Main.player[num62].dead && Main.player[num62].ZoneCorrupt)
							{
								flag5 = false;
							}
						}
						if (flag5)
						{
							if (Main.netMode != NetmodeID.MultiplayerClient && (double)(npc.position.Y / 16f) > (Main.rockLayer + (double)Main.maxTilesY) / 2.0)
							{
								npc.active = false;
								int num63 = (int)npc.ai[0];
								while (num63 > 0 && num63 < 200 && Main.npc[num63].active && Main.npc[num63].aiStyle == npc.aiStyle)
								{
									int num73 = (int)Main.npc[num63].ai[0];
									Main.npc[num63].active = false;
									npc.life = 0;
									if (Main.netMode == NetmodeID.Server)
									{
										NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, num63);
									}
									num63 = num73;
								}
								if (Main.netMode == NetmodeID.Server)
								{
									NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, npc.whoAmI);
								}
							}
							num41 = 0f;
							num42 = num39;
						}
					}
					bool flag6 = false;
					if (npc.type == NPCID.WyvernHead)
					{
						if (((npc.velocity.X > 0f && num41 < 0f) || (npc.velocity.X < 0f && num41 > 0f) || (npc.velocity.Y > 0f && num42 < 0f) || (npc.velocity.Y < 0f && num42 > 0f)) && Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y) > num40 / 2f && num56 < 300f)
						{
							flag6 = true;
							if (Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y) < num39)
							{
								npc.velocity *= 1.1f;
							}
						}
						if (npc.position.Y > Main.player[npc.target].position.Y || (double)(Main.player[npc.target].position.Y / 16f) > Main.worldSurface || Main.player[npc.target].dead)
						{
							flag6 = true;
							if (Math.Abs(npc.velocity.X) < num39 / 2f)
							{
								if (npc.velocity.X == 0f)
								{
									npc.velocity.X -= npc.direction;
								}
								npc.velocity.X *= 1.1f;
							}
							else if (npc.velocity.Y > 0f - num39)
							{
								npc.velocity.Y -= num40;
							}
						}
					}
					if (npc.type == NPCID.CultistDragonHead || npc.type == NPCID.BloodEelHead)
					{
						float num64 = 300f;
						if (npc.type == NPCID.BloodEelHead)
						{
							num64 = 120f;
						}
						if (((npc.velocity.X > 0f && num41 < 0f) || (npc.velocity.X < 0f && num41 > 0f) || (npc.velocity.Y > 0f && num42 < 0f) || (npc.velocity.Y < 0f && num42 > 0f)) && Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y) > num40 / 2f && num56 < num64)
						{
							flag6 = true;
							if (Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y) < num39)
							{
								npc.velocity *= 1.1f;
							}
						}
						if (npc.position.Y > Main.player[npc.target].position.Y || Main.player[npc.target].dead)
						{
							flag6 = true;
							if (Math.Abs(npc.velocity.X) < num39 / 2f)
							{
								if (npc.velocity.X == 0f)
								{
									npc.velocity.X -= npc.direction;
								}
								npc.velocity.X *= 1.1f;
							}
							else if (npc.velocity.Y > 0f - num39)
							{
								npc.velocity.Y -= num40;
							}
						}
					}
					if (!flag6)
					{
						if ((npc.velocity.X > 0f && num41 > 0f) || (npc.velocity.X < 0f && num41 < 0f) || (npc.velocity.Y > 0f && num42 > 0f) || (npc.velocity.Y < 0f && num42 < 0f))
						{
							if (npc.velocity.X < num41)
							{
								npc.velocity.X += num40;
							}
							else if (npc.velocity.X > num41)
							{
								npc.velocity.X -= num40;
							}
							if (npc.velocity.Y < num42)
							{
								npc.velocity.Y += num40;
							}
							else if (npc.velocity.Y > num42)
							{
								npc.velocity.Y -= num40;
							}
							if ((double)Math.Abs(num42) < (double)num39 * 0.2 && ((npc.velocity.X > 0f && num41 < 0f) || (npc.velocity.X < 0f && num41 > 0f)))
							{
								if (npc.velocity.Y > 0f)
								{
									npc.velocity.Y += num40 * 2f;
								}
								else
								{
									npc.velocity.Y -= num40 * 2f;
								}
							}
							if ((double)Math.Abs(num41) < (double)num39 * 0.2 && ((npc.velocity.Y > 0f && num42 < 0f) || (npc.velocity.Y < 0f && num42 > 0f)))
							{
								if (npc.velocity.X > 0f)
								{
									npc.velocity.X += num40 * 2f;
								}
								else
								{
									npc.velocity.X -= num40 * 2f;
								}
							}
						}
						else if (num59 > num60)
						{
							if (npc.velocity.X < num41)
							{
								npc.velocity.X += num40 * 1.1f;
							}
							else if (npc.velocity.X > num41)
							{
								npc.velocity.X -= num40 * 1.1f;
							}
							if ((double)(Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y)) < (double)num39 * 0.5)
							{
								if (npc.velocity.Y > 0f)
								{
									npc.velocity.Y += num40;
								}
								else
								{
									npc.velocity.Y -= num40;
								}
							}
						}
						else
						{
							if (npc.velocity.Y < num42)
							{
								npc.velocity.Y += num40 * 1.1f;
							}
							else if (npc.velocity.Y > num42)
							{
								npc.velocity.Y -= num40 * 1.1f;
							}
							if ((double)(Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y)) < (double)num39 * 0.5)
							{
								if (npc.velocity.X > 0f)
								{
									npc.velocity.X += num40;
								}
								else
								{
									npc.velocity.X -= num40;
								}
							}
						}
					}
				}
				npc.rotation = (float)Math.Atan2((double)npc.velocity.Y, (double)npc.velocity.X) + 1.57f;
				if (npc.type == NPCID.DevourerHead || npc.type == NPCID.GiantWormHead || npc.type == NPCID.EaterofWorldsHead || npc.type == NPCID.BoneSerpentHead || npc.type == NPCID.DiggerHead || npc.type == NPCID.SeekerHead || npc.type == NPCID.LeechHead || npc.type == NPCID.DuneSplicerHead || npc.type == NPCID.TombCrawlerHead || npc.type == NPCID.BloodEelHead)
				{
					if (flag2)
					{
						if (npc.localAI[0] != 1f)
						{
							npc.netUpdate = true;
						}
						npc.localAI[0] = 1f;
					}
					else
					{
						if (npc.localAI[0] != 0f)
						{
							npc.netUpdate = true;
						}
						npc.localAI[0] = 0f;
					}
					if (((npc.velocity.X > 0f && npc.oldVelocity.X < 0f) || (npc.velocity.X < 0f && npc.oldVelocity.X > 0f) || (npc.velocity.Y > 0f && npc.oldVelocity.Y < 0f) || (npc.velocity.Y < 0f && npc.oldVelocity.Y > 0f)) && !npc.justHit)
					{
						npc.netUpdate = true;
					}
				}
				if (npc.type == NPCID.CultistDragonHead)
				{
					float num66 = Vector2.Distance(Main.player[npc.target].Center, npc.Center);
					int num67 = 0;
					if (Vector2.Normalize(Main.player[npc.target].Center - npc.Center).ToRotation().AngleTowards(npc.velocity.ToRotation(), (float)Math.PI / 2f) == npc.velocity.ToRotation() && num66 < 350f)
					{
						num67 = 4;
					}
					if ((double)num67 > npc.frameCounter)
					{
						npc.frameCounter += 1.0;
					}
					if ((double)num67 < npc.frameCounter)
					{
						npc.frameCounter -= 1.0;
					}
					if (npc.frameCounter < 0.0)
					{
						npc.frameCounter = 0.0;
					}
					if (npc.frameCounter > 4.0)
					{
						npc.frameCounter = 4.0;
					}
				}
			}
			if (npc.type < NPCID.EaterofWorldsHead || npc.type > NPCID.EaterofWorldsTail || (npc.type != NPCID.EaterofWorldsHead && (npc.type == NPCID.EaterofWorldsHead || Main.npc[(int)npc.ai[1]].alpha >= 85)))
			{
				return;
			}
			if (npc.alpha > 0 && npc.life > 0)
			{
				for (int num68 = 0; num68 < 2; num68++)
				{
					int num69 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, DustID.Demonite, 0f, 0f, 100, default(Color), 2f);
					Main.dust[num69].noGravity = true;
					Main.dust[num69].noLight = true;
				}
			}
			Vector2 val = npc.position - npc.oldPosition;
			if (val.Length() > 2f)
			{
				npc.alpha -= 42;
				if (npc.alpha < 0)
				{
					npc.alpha = 0;
				}
			}
		}
	}
}
