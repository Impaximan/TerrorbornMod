using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using TerrorbornMod.Utils;
using TerrorbornMod.NPCs;

namespace TerrorbornMod.TwilightMode.NPCs.Bosses
{
    class KingSlime : TwilightNPCChange
    {
        public override bool HasNewAI(NPC npc)
        {
            return true;
        }

        public override bool ShouldChangeNPC(NPC npc)
        {
            return npc.type == NPCID.KingSlime;
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

		public override void NewAI(NPC NPC)
        {
			float num234 = 1f;
			bool flag8 = false;
			bool flag9 = false;
			NPC.aiAction = 0;
			if (NPC.ai[3] == 0f && NPC.life > 0)
			{
				NPC.ai[3] = NPC.lifeMax;
			}
			if (NPC.localAI[3] == 0f && Main.netMode != NetmodeID.MultiplayerClient)
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
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					NPC.TargetClosest(false);
					Point point3 = NPC.Center.ToTileCoordinates();
					Point point4 = Main.player[NPC.target].Center.ToTileCoordinates();
					Vector2 vector30 = Main.player[NPC.target].Center - NPC.Center;
					Player player = Main.player[NPC.target];
					Vector2 bottom = new Vector2(player.Center.X + Math.Sign(player.Center.X - NPC.Center.X) * 400f, player.Center.Y - 500f).FindGroundUnder(); //Set teleport position
					NPC.localAI[1] = bottom.X;
					NPC.localAI[2] = bottom.Y;
					DustExplosion(bottom, 0, 40, 25f, DustID.t_Slime, Color.Azure, 2f, true);
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
					Gore.NewGore(NPC.GetSource_Loot(), NPC.Center + new Vector2(-40f, -NPC.height / 2), NPC.velocity, 734);
				}
				if (NPC.ai[0] >= 60f && Main.netMode != NetmodeID.MultiplayerClient)
				{
					NPC.Bottom = new Vector2(NPC.localAI[1], NPC.localAI[2]);
					NPC.ai[1] = 6f;
					NPC.ai[0] = 0f;
					NPC.netUpdate = true;
				}
				if (Main.netMode == NetmodeID.MultiplayerClient && NPC.ai[0] >= 120f)
				{
					NPC.ai[1] = 6f;
					NPC.ai[0] = 0f;
				}
				if (!flag9)
				{
					for (int num244 = 0; num244 < 10; num244++)
					{
						int num245 = Dust.NewDust(NPC.position + Vector2.UnitX * -20f, NPC.width + 40, NPC.height, DustID.t_Slime, NPC.velocity.X, NPC.velocity.Y, 150, new Color(78, 136, 255, 80), 2f);
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
				if (NPC.ai[0] >= 30f && Main.netMode != NetmodeID.MultiplayerClient)
				{
					NPC.ai[1] = 0f;
					NPC.ai[0] = 0f;
					NPC.netUpdate = true;
					NPC.TargetClosest();
				}
				if (Main.netMode == NetmodeID.MultiplayerClient && NPC.ai[0] >= 60f)
				{
					NPC.ai[1] = 0f;
					NPC.ai[0] = 0f;
					NPC.TargetClosest();
				}
				for (int num246 = 0; num246 < 10; num246++)
				{
					int num247 = Dust.NewDust(NPC.position + Vector2.UnitX * -20f, NPC.width + 40, NPC.height, DustID.t_Slime, NPC.velocity.X, NPC.velocity.Y, 150, new Color(78, 136, 255, 80), 2f);
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
								Projectile.NewProjectile(NPC.GetSource_ReleaseEntity(), position, new Vector2(maxSpeedX * i * 2f, -ySpeed), ProjectileID.SpikedSlimeSpike, 60 / 4, 0f);
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
			int num248 = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.t_Slime, NPC.velocity.X, NPC.velocity.Y, 255, new Color(0, 80, 255, 80), NPC.scale * 1.2f);
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
			if (Main.netMode == NetmodeID.MultiplayerClient)
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
				if (Main.expertMode && Main.rand.NextBool(4))
				{
					num253 = 535;
				}
				if (Main.rand.NextBool(6))
				{
					num253 = ModContent.NPCType<TerrorSlime>();
				}
				int num254 = NPC.NewNPC(NPC.GetSource_ReleaseEntity(), x, y, num253);
				Main.npc[num254].SetDefaults(num253);
				Main.npc[num254].velocity.X = (float)Main.rand.Next(-15, 16) * 0.1f;
				Main.npc[num254].velocity.Y = (float)Main.rand.Next(-30, 1) * 0.1f;
				Main.npc[num254].ai[0] = -1000 * Main.rand.Next(3);
				Main.npc[num254].ai[1] = 0f;
				if (num253 != ModContent.NPCType<TerrorSlime>())
				{
					Main.npc[num254].lifeMax /= 2;
					Main.npc[num254].life = Main.npc[num254].lifeMax;
				}
				if (Main.netMode == NetmodeID.Server && num254 < 200)
				{
					NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, num254);
				}
			}
		}
    }
}
