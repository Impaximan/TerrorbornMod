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

namespace TerrorbornMod.TwilightMode.NPCs
{
    class FighterAIs : TwilightNPCChange
	{
        public override void NewSetDefaults(NPC npc)
		{
			if (npc.type == NPCID.SlimedZombie)
			{
				if (NPC.downedMoonlord)
				{
					npc.lifeMax = 3000;
					npc.damage = 80;
				}
				else
				{
					npc.lifeMax *= 2;
				}
			}

			if (npc.type == NPCID.PossessedArmor)
			{
				if (NPC.downedMoonlord)
				{
					npc.lifeMax = 5000;
					npc.damage = 100;
				}
				else
				{
					npc.lifeMax *= 2;
				}
			}
		}

        public int fighter_TargetPlayerCounter = 0;
		public int fighter_StillTime = 0;
		public int fighter_JumpCooldown = 0;

		public void ImprovedFighterAI(NPC NPC, float maxSpeed, float accelleration, float decelleration, float jumpSpeed, bool faceDirection = true, int jumpCooldown = 0, int stillTimeUntilTurnaround = 120, int wanderTime = 90)
		{
			Player player = Main.player[NPC.target];

			if (fighter_TargetPlayerCounter > 0)
			{
				fighter_TargetPlayerCounter--;
			}
			else
			{
				NPC.TargetClosest(true);
			}

			if (Math.Abs(NPC.velocity.X) < maxSpeed - accelleration)
			{
				fighter_StillTime++;
				if (fighter_StillTime > stillTimeUntilTurnaround && stillTimeUntilTurnaround != -1)
				{
					fighter_TargetPlayerCounter = wanderTime;
					NPC.direction *= -1;
					fighter_StillTime = 0;
				}
			}
			else
			{
				fighter_StillTime = 0;
			}

			if (NPC.direction == 1 && NPC.velocity.X < maxSpeed)
			{
				NPC.velocity.X += accelleration;
			}

			if (NPC.direction == -1 && NPC.velocity.X > -maxSpeed)
			{
				NPC.velocity.X -= accelleration;
			}

			if (NPC.velocity.Y == 0)
			{
				if (fighter_JumpCooldown > 0)
				{
					fighter_JumpCooldown--;
				}
				else if (!Collision.SolidCollision(NPC.position + new Vector2(NPC.width * NPC.direction, NPC.height), NPC.width, 17) || Collision.SolidCollision(NPC.position + new Vector2(NPC.width * NPC.direction, 0), NPC.width, NPC.height) || (MathHelper.Distance(player.Center.X, NPC.Center.X) < NPC.width && player.position.Y + player.width < NPC.position.Y))
				{
					if (player.Center.Y < NPC.position.Y + NPC.height || Math.Abs(player.Center.X - NPC.Center.X) > 150)
					{
						NPC.velocity.Y -= jumpSpeed;
						fighter_JumpCooldown = jumpCooldown;
					}
				}
			}

			if (faceDirection)
			{
				NPC.spriteDirection = NPC.direction;
			}
		}

        public override bool ShouldChangeNPC(NPC npc)
        {
            return npc.aiStyle == 3;
        }

        public override bool HasNewAI(NPC npc)
        {
            return npc.type == NPCID.SlimedZombie || npc.type == NPCID.PossessedArmor;
        }

        public override void NewAI(NPC npc)
		{
			if (npc.type == NPCID.SlimedZombie)
			{
				if (NPC.downedMoonlord)
				{
					ImprovedFighterAI(npc, 10f, 0.5f, 0.93f, 15f, true, 15, -1, 90);
				}
				else if (Main.hardMode)
				{
					ImprovedFighterAI(npc, 6f, 0.3f, 0.93f, 10f, true, 30, -1, 90);
				}
				else
				{
					ImprovedFighterAI(npc, 4f, 0.3f, 0.93f, 6.5f, true, 60, -1, 90);
				}
			}
			if (npc.type == NPCID.PossessedArmor)
			{
				if (NPC.downedMoonlord)
				{
					ImprovedFighterAI(npc, 12f, 0.5f, 0.93f, 18f, true, 0, -1, 90);
				}
				else
				{
					ImprovedFighterAI(npc, 7.5f, 0.3f, 0.93f, 12f, true, 0, -1, 90);
				}
			}
		}
	}
}
