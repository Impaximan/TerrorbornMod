using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Reflection;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Events;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.Localization;
using Terraria.World.Generation;
using Terraria.UI;

namespace TerrorbornMod.NPCs.Incendiary
{
	internal class IncendiaryWyvernHead : IncendiaryWyvern
	{
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (TerrorbornPlayer.modPlayer(spawnInfo.player).ZoneIncendiary)
			{
				return SpawnCondition.Sky.Chance * 0.015f;
			}
			else
			{
				return 0f;
			}
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			npc.width = 52;
			npc.height = 86;
			npc.aiStyle = -1;
			head = true;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			SpriteEffects effects = SpriteEffects.None;

			if (npc.rotation <= MathHelper.ToRadians(0f) || npc.rotation >= MathHelper.ToRadians(180f))
			{
				effects = SpriteEffects.FlipHorizontally;
			}

			if (effects == SpriteEffects.None)
			{
				spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition, null, drawColor, npc.rotation, new Vector2(33, 43), npc.scale, effects, 0);
			}
			else
			{
				spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition, null, drawColor, npc.rotation, new Vector2(52 - 33, 43), npc.scale, effects, 0);
			}
			return false;
		}

		float movementDirection = 0f;
		float movementSpeed = 5f;
		int AIPhase = 0;
		int phaseTimeLeft = 60 * 5;
		public override void actualAI()
		{
			npc.TargetClosest(false);
			Player target = Main.player[npc.target];
			npc.velocity = movementSpeed * movementDirection.ToRotationVector2();
			if (AIPhase == 0)
			{
				movementSpeed = MathHelper.Lerp(movementSpeed, 20f, 0.05f);
				movementDirection = movementDirection.AngleLerp(npc.DirectionTo(target.Center).ToRotation(), 0.02f);
				phaseTimeLeft--;
				if (phaseTimeLeft <= 0)
				{
					phaseTimeLeft = 60 * 2;
					AIPhase++;
				}
			}
			if (AIPhase == 1)
			{
				movementSpeed = MathHelper.Lerp(movementSpeed, 6f, 0.1f);
				movementDirection = movementDirection.AngleLerp(npc.DirectionTo(target.Center).ToRotation(), 0.04f);
				phaseTimeLeft--;
				if (phaseTimeLeft <= 0)
				{
					AIPhase = 0;
					phaseTimeLeft = 60 * 4;
				}
			}
		}
	}

	internal class IncendiaryWyvernBody_0 : IncendiaryWyvern
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			npc.aiStyle = -1;
			npc.width = 64;
			npc.height = 48;
			body = true;
		}

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			SpriteEffects effects = SpriteEffects.None;

			if (npc.rotation <= MathHelper.ToRadians(0f) || npc.rotation >= MathHelper.ToRadians(180f))
			{
				effects = SpriteEffects.FlipHorizontally;
			}

			if (effects == SpriteEffects.None)
			{
				spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition, null, drawColor, npc.rotation, new Vector2(19, 24), npc.scale, effects, 0);
			}
            else
			{
				spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition, null, drawColor, npc.rotation, new Vector2(64 - 19, 24), npc.scale, effects, 0);
			}
            return false;
        }
    }

	internal class IncendiaryWyvernBody_1 : IncendiaryWyvern
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			npc.aiStyle = -1;
			npc.width = 38;
			npc.height = 48;
			body = true;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			SpriteEffects effects = SpriteEffects.None;

			if (npc.rotation <= MathHelper.ToRadians(0f) || npc.rotation >= MathHelper.ToRadians(180f))
			{
				effects = SpriteEffects.FlipHorizontally;
			}

			spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition, null, drawColor, npc.rotation, new Vector2(19, 24), npc.scale, effects, 0);
			return false;
		}
	}

	internal class IncendiaryWyvernBody_2 : IncendiaryWyvern
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			npc.aiStyle = -1;
			npc.width = 36;
			npc.height = 48;
			body = true;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			SpriteEffects effects = SpriteEffects.None;

			if (npc.rotation <= MathHelper.ToRadians(0f) || npc.rotation >= MathHelper.ToRadians(180f))
			{
				effects = SpriteEffects.FlipHorizontally;
			}

			spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition, null, drawColor, npc.rotation, new Vector2(19, 24), npc.scale, effects, 0);
			return false;
		}
	}

	internal class IncendiaryWyvernBody_3 : IncendiaryWyvern
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			npc.aiStyle = -1;
			npc.width = 32;
			npc.height = 48;
			body = true;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			SpriteEffects effects = SpriteEffects.None;

			if (npc.rotation <= MathHelper.ToRadians(0f) || npc.rotation >= MathHelper.ToRadians(180f))
			{
				effects = SpriteEffects.FlipHorizontally;
			}

			spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition, null, drawColor, npc.rotation, new Vector2(19, 24), npc.scale, effects, 0);
			return false;
		}
	}

	internal class IncendiaryWyvernTail : IncendiaryWyvern
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			npc.aiStyle = -1;
			npc.width = 24;
			npc.height = 58;
			tail = true;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			SpriteEffects effects = SpriteEffects.None;

			if (npc.rotation <= MathHelper.ToRadians(0f) || npc.rotation >= MathHelper.ToRadians(180f))
			{
				effects = SpriteEffects.FlipHorizontally;
			}

			spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition, null, drawColor, npc.rotation, new Vector2(19, 29), npc.scale, effects, 0);
			return false;
		}
	}

	public abstract class IncendiaryWyvern : BaseWorm
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Infernal Lindwyrm");
		}

		public override void SetDefaults()
		{
			int segmentCount = 15;
			tailType = ModContent.NPCType<IncendiaryWyvernTail>();
			customBodyList = true;
			customBodyTypeList.Add(ModContent.NPCType<IncendiaryWyvernBody_0>());
			bodySegmentCount++;
			for (int i = 0; i < segmentCount / 2; i++)
			{
				customBodyTypeList.Add(ModContent.NPCType<IncendiaryWyvernBody_1>());
				bodySegmentCount++;
			}
			for (int i = 0; i < segmentCount / 2; i++)
			{
				customBodyTypeList.Add(ModContent.NPCType<IncendiaryWyvernBody_2>());
				bodySegmentCount++;
			}
			customBodyTypeList.Add(ModContent.NPCType<IncendiaryWyvernBody_3>());
			bodySegmentCount++;
			headType = ModContent.NPCType<IncendiaryWyvernHead>();
			npc.lifeMax = 4500;
			npc.HitSound = SoundID.NPCHit7;
			npc.DeathSound = SoundID.NPCDeath8;
			npc.damage = 40;
			npc.friendly = false;
			npc.noTileCollide = true;
			npc.noGravity = true;
			npc.defense = 25;
			npc.knockBackResist = 0f;
		}

		public override void NPCLoot()
		{
			Item.NewItem(npc.getRect(), ItemID.SoulofFlight, Main.rand.Next(20, 42));
		}
	}
}
