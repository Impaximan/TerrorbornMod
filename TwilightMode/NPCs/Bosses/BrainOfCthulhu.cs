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
    class BrainOfCthulhu : TwilightNPCChange
    {
        public override bool HasNewAI(NPC npc)
        {
            return true;
        }

        public override void NewAI(NPC npc)
        {
			BrainOfCthulhuAI(npc);
		}

		public override void HitEffect(NPC NPC, int hitDirection, double damage)
		{
			if (NPC.type == NPCID.BrainofCthulhu && TerrorbornSystem.TwilightMode && !BoCSpawnedNewCreepers)
			{
				if (NPC.life <= 0)
				{
					NPC.life = 1;
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

		public override bool ShouldChangeNPC(NPC npc)
        {
            return npc.type == NPCID.BrainofCthulhu;
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
			if (Main.netMode != NetmodeID.MultiplayerClient && NPC.localAI[0] == 0f)
			{
				NPC.localAI[0] = 1f;
				for (int num796 = 0; num796 < 20; num796++)
				{
					float x2 = NPC.Center.X;
					float y3 = NPC.Center.Y;
					x2 += (float)Main.rand.Next(-NPC.width, NPC.width);
					y3 += (float)Main.rand.Next(-NPC.height, NPC.height);
					int num797 = NPC.NewNPC(NPC.GetSource_ReleaseEntity(), (int)x2, (int)y3, NPCID.Creeper);
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
						int proj = Projectile.NewProjectile(NPC.GetSource_ReleaseEntity(), position, velocity, ProjectileID.GoldenShowerHostile, 25 / 4, 0f);
						Main.projectile[proj].tileCollide = false;
					}
				}
				else
				{
					if (BoCIchorCounter > 250)
					{
						SoundExtensions.PlaySoundOld(SoundID.Roar, (int)NPC.Center.X, (int)NPC.Center.Y, 0, 1, 1);
						TerrorbornSystem.ScreenShake(5f);
						BoCIchorCounter = 0;
						BoCIchorAmount = 13;
					}
				}
			}

			if (Main.netMode != NetmodeID.MultiplayerClient)
			{
				NPC.TargetClosest();
				int num798 = 6000;
				if (Math.Abs(NPC.Center.X - Main.player[NPC.target].Center.X) + Math.Abs(NPC.Center.Y - Main.player[NPC.target].Center.Y) > (float)num798)
				{
					NPC.active = false;
					NPC.life = 0;
					if (Main.netMode == NetmodeID.Server)
					{
						NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, NPC.whoAmI);
					}
				}
			}
			if (NPC.ai[0] < 0f)
			{
				if (NPC.localAI[2] == 0f)
				{
					SoundExtensions.PlaySoundOld(SoundID.NPCHit1, (int)NPC.position.X, (int)NPC.position.Y);
					NPC.localAI[2] = 1f;
					Gore.NewGore(NPC.GetSource_Loot(), NPC.position, new Vector2((float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f), 392);
					Gore.NewGore(NPC.GetSource_Loot(), NPC.position, new Vector2((float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f), 393);
					Gore.NewGore(NPC.GetSource_Loot(), NPC.position, new Vector2((float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f), 394);
					Gore.NewGore(NPC.GetSource_Loot(), NPC.position, new Vector2((float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f), 395);
					for (int num799 = 0; num799 < 20; num799++)
					{
						Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, (float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f);
					}
					SoundExtensions.PlaySoundOld(SoundID.Zombie105, (int)NPC.position.X, (int)NPC.position.Y, 0);
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
						int num797 = NPC.NewNPC(NPC.GetSource_ReleaseEntity(), (int)x2, (int)y3, NPCID.Creeper);
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
					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						NPC.localAI[1] += 1f;
						if (NPC.justHit)
						{
							NPC.localAI[1] -= Main.rand.Next(5);
						}
						int num804 = 60 + Main.rand.Next(120);
						if (Main.netMode != NetmodeID.SinglePlayer)
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
					if (Main.netMode != NetmodeID.SinglePlayer)
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
						SoundExtensions.PlaySoundOld(SoundID.Item8, NPC.Center);
						NPC.ai[0] = -3f;
						NPC.netUpdate = true;
						NPC.netSpam = 0;
					}
					NPC.alpha = (int)NPC.ai[3];
				}
				else if (NPC.ai[0] == -3f)
				{
					if (Main.netMode != NetmodeID.SinglePlayer)
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
					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						int num812 = 0;
						for (int num813 = 0; num813 < 200; num813++)
						{
							if (Main.npc[num813].active && Main.npc[num813].type == NPCID.Creeper)
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
						SoundExtensions.PlaySoundOld(SoundID.Item8, NPC.Center);
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
	}
}
