using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Bestiary;

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
			NPC.width = 52;
			NPC.height = 86;
			NPC.aiStyle = -1;
			head = true;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			SpriteEffects effects = SpriteEffects.None;

			if (NPC.rotation <= MathHelper.ToRadians(0f) || NPC.rotation >= MathHelper.ToRadians(180f))
			{
				effects = SpriteEffects.FlipHorizontally;
			}

			if (effects == SpriteEffects.None)
			{
				spriteBatch.Draw(ModContent.Request<Texture2D>(Texture).Value, NPC.Center - Main.screenPosition, null, drawColor, NPC.rotation, new Vector2(33, 43), NPC.scale, effects, 0);
			}
			else
			{
				spriteBatch.Draw(ModContent.Request<Texture2D>(Texture).Value, NPC.Center - Main.screenPosition, null, drawColor, NPC.rotation, new Vector2(52 - 33, 43), NPC.scale, effects, 0);
			}
			return false;
		}

		float movementDirection = 0f;
		float movementSpeed = 5f;
		int AIPhase = 0;
		int phaseTimeLeft = 60 * 5;
		public override void actualAI()
		{
			NPC.TargetClosest(false);
			Player target = Main.player[NPC.target];
			NPC.velocity = movementSpeed * movementDirection.ToRotationVector2();
			if (AIPhase == 0)
			{
				movementSpeed = MathHelper.Lerp(movementSpeed, 20f, 0.05f);
				movementDirection = movementDirection.AngleLerp(NPC.DirectionTo(target.Center).ToRotation(), 0.02f);
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
				movementDirection = movementDirection.AngleLerp(NPC.DirectionTo(target.Center).ToRotation(), 0.04f);
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
		public override void SetStaticDefaults()
		{
			NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
			{
				Hide = true
			};
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			NPC.aiStyle = -1;
			NPC.width = 64;
			NPC.height = 48;
			body = true;
		}

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			SpriteEffects effects = SpriteEffects.None;

			if (NPC.rotation <= MathHelper.ToRadians(0f) || NPC.rotation >= MathHelper.ToRadians(180f))
			{
				effects = SpriteEffects.FlipHorizontally;
			}

			if (effects == SpriteEffects.None)
			{
				spriteBatch.Draw(ModContent.Request<Texture2D>(Texture).Value, NPC.Center - Main.screenPosition, null, drawColor, NPC.rotation, new Vector2(19, 24), NPC.scale, effects, 0);
			}
			else
			{
				spriteBatch.Draw(ModContent.Request<Texture2D>(Texture).Value, NPC.Center - Main.screenPosition, null, drawColor, NPC.rotation, new Vector2(64 - 19, 24), NPC.scale, effects, 0);
			}
			return false;
        }
    }

	internal class IncendiaryWyvernBody_1 : IncendiaryWyvern
	{
		public override void SetStaticDefaults()
		{
			NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
			{
				Hide = true
			};
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			NPC.aiStyle = -1;
			NPC.width = 38;
			NPC.height = 48;
			body = true;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			SpriteEffects effects = SpriteEffects.None;

			if (NPC.rotation <= MathHelper.ToRadians(0f) || NPC.rotation >= MathHelper.ToRadians(180f))
			{
				effects = SpriteEffects.FlipHorizontally;
			}

			spriteBatch.Draw(ModContent.Request<Texture2D>(Texture).Value, NPC.Center - Main.screenPosition, null, drawColor, NPC.rotation, new Vector2(19, 24), NPC.scale, effects, 0);
			return false;
		}
	}

	internal class IncendiaryWyvernBody_2 : IncendiaryWyvern
	{
		public override void SetStaticDefaults()
		{
			NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
			{
				Hide = true
			};
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			NPC.aiStyle = -1;
			NPC.width = 36;
			NPC.height = 48;
			body = true;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			SpriteEffects effects = SpriteEffects.None;

			if (NPC.rotation <= MathHelper.ToRadians(0f) || NPC.rotation >= MathHelper.ToRadians(180f))
			{
				effects = SpriteEffects.FlipHorizontally;
			}

			spriteBatch.Draw(ModContent.Request<Texture2D>(Texture).Value, NPC.Center - Main.screenPosition, null, drawColor, NPC.rotation, new Vector2(19, 24), NPC.scale, effects, 0);
			return false;
		}
	}

	internal class IncendiaryWyvernBody_3 : IncendiaryWyvern
	{
		public override void SetStaticDefaults()
		{
			NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
			{
				Hide = true
			};
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			NPC.aiStyle = -1;
			NPC.width = 32;
			NPC.height = 48;
			body = true;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			SpriteEffects effects = SpriteEffects.None;

			if (NPC.rotation <= MathHelper.ToRadians(0f) || NPC.rotation >= MathHelper.ToRadians(180f))
			{
				effects = SpriteEffects.FlipHorizontally;
			}

			spriteBatch.Draw(ModContent.Request<Texture2D>(Texture).Value, NPC.Center - Main.screenPosition, null, drawColor, NPC.rotation, new Vector2(19, 24), NPC.scale, effects, 0);
			return false;
		}
	}

	internal class IncendiaryWyvernTail : IncendiaryWyvern
	{
		public override void SetStaticDefaults()
		{
			NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
			{
				Hide = true
			};
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			NPC.aiStyle = -1;
			NPC.width = 24;
			NPC.height = 58;
			tail = true;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			SpriteEffects effects = SpriteEffects.None;

			if (NPC.rotation <= MathHelper.ToRadians(0f) || NPC.rotation >= MathHelper.ToRadians(180f))
			{
				effects = SpriteEffects.FlipHorizontally;
			}

			spriteBatch.Draw(ModContent.Request<Texture2D>(Texture).Value, NPC.Center - Main.screenPosition, null, drawColor, NPC.rotation, new Vector2(19, 29), NPC.scale, effects, 0);
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
			NPC.lifeMax = 4500;
			NPC.HitSound = SoundID.NPCHit7;
			NPC.DeathSound = SoundID.NPCDeath8;
			NPC.damage = 40;
			NPC.friendly = false;
			NPC.noTileCollide = true;
			NPC.noGravity = true;
			NPC.defense = 25;
			NPC.knockBackResist = 0f;
		}

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
			npcLoot.Add(ItemDropRule.Common(ItemID.SoulofFlight, 1, 20, 30));
        }
	}
}
