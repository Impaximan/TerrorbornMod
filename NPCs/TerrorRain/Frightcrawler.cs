using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Bestiary;

namespace TerrorbornMod.NPCs.TerrorRain
{
    internal class FrightcrawlerHead : Frightcrawler
	{
		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Rain,
				new FlavorTextBestiaryInfoElement("A massive worm, which has presumably consumed many, many souls, making it incredibly powerful.")
			});
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			NPC.width = 62;
			NPC.height = 86;
			NPC.aiStyle = -1;
			head = true;
		}

		float movementDirection = 0f;
		float movementSpeed = 5f;
		int AIPhase = 0;
		int phaseTimeLeft = 60 * 5;
		int ProjectileCounter = 60;
		public override void actualAI()
		{
			NPC.TargetClosest(false);
			Player target = Main.player[NPC.target];
			if (target.dead || !TerrorbornSystem.terrorRain)
            {
				NPC.velocity.Y += 0.2f;
				if (NPC.Center.Y >= target.Center.Y + 10000)
                {
					NPC.active = false;
                }
				return;
            }
			NPC.velocity = movementSpeed * movementDirection.ToRotationVector2();
			if (AIPhase == 0)
            {
				movementSpeed = MathHelper.Lerp(movementSpeed, 12f, 0.07f);
				movementDirection = movementDirection.AngleLerp(NPC.DirectionTo(target.Center + (NPC.Distance(target.Center) / movementSpeed) * target.velocity).ToRotation(), 0.03f);
				phaseTimeLeft--;
				if (phaseTimeLeft <= 0)
                {
					phaseTimeLeft = 60 * 3;
					SoundExtensions.PlaySoundOld(SoundID.Roar, (int)NPC.Center.X, (int)NPC.Center.Y, 0, 1, 0.5f);
					TerrorbornSystem.ScreenShake(10);
					AIPhase++;
                }

				ProjectileCounter--;
				if (ProjectileCounter <= 0)
                {
					ProjectileCounter = 45;
					SoundExtensions.PlaySoundOld(SoundID.NPCDeath13, NPC.Center);
					float speed = 17f;
					Vector2 velocity = (NPC.rotation - MathHelper.ToRadians(90)).ToRotationVector2() * speed;
					Projectile.NewProjectile(NPC.GetSource_ReleaseEntity(), NPC.Center, velocity, ModContent.ProjectileType<Projectiles.FrightfulSpit>(), 75 / 4, 0);
                }
            }
			if (AIPhase == 1)
			{
				movementDirection = movementDirection.AngleLerp(NPC.DirectionTo(target.Center + (NPC.Distance(target.Center) / movementSpeed) * target.velocity).ToRotation(), 0.01f);
				movementSpeed = MathHelper.Lerp(movementSpeed, 3f, 0.05f);
				phaseTimeLeft--;
				if (phaseTimeLeft <= 0)
                {
					AIPhase++;
					phaseTimeLeft = 60 * 5;
					SoundExtensions.PlaySoundOld(SoundID.NPCDeath10, (int)NPC.Center.X, (int)NPC.Center.Y, 10, 1, -0.3f);
					TerrorbornSystem.ScreenShake(20);
				}
			}
			if (AIPhase == 2)
			{
				movementSpeed = MathHelper.Lerp(movementSpeed, 20f, 0.05f);
				movementDirection = movementDirection.AngleLerp(NPC.DirectionTo(target.Center).ToRotation(), 0.02f);
				phaseTimeLeft--;
				if (phaseTimeLeft <= 0)
                {
					AIPhase = 0;
					phaseTimeLeft = 60 * 7;
				}

				ProjectileCounter--;
				if (ProjectileCounter <= 0)
				{
					ProjectileCounter = 90;
					SoundExtensions.PlaySoundOld(SoundID.NPCDeath13, NPC.Center);
					float speed = 20f;
					Vector2 velocity = NPC.DirectionTo(target.Center + target.velocity * (NPC.Distance(target.Center) / speed)) * speed;
					Projectile.NewProjectile(NPC.GetSource_ReleaseEntity(), NPC.Center, velocity, ModContent.ProjectileType<Projectiles.FrightfulSpit>(), 75 / 4, 0);
					Projectile.NewProjectile(NPC.GetSource_ReleaseEntity(), NPC.Center, velocity.RotatedBy(-MathHelper.ToRadians(30)), ModContent.ProjectileType<Projectiles.FrightfulSpit>(), 75 / 4, 0);
					Projectile.NewProjectile(NPC.GetSource_ReleaseEntity(), NPC.Center, velocity.RotatedBy(MathHelper.ToRadians(30)), ModContent.ProjectileType<Projectiles.FrightfulSpit>(), 75 / 4, 0);
				}
			}
		}
	}

	internal class FrightcrawlerBody : Frightcrawler
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
			NPC.width = 58;
			NPC.height = 32;
			body = true;
			NPC.defense = 9999;
		}

        public override void ModifyIncomingHit(ref NPC.HitModifiers modifiers)
        {
			modifiers.FinalDamage *= 0f;
        }
    }

	internal class FrightcrawlerTail : Frightcrawler
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
			NPC.width = 34;
			NPC.height = 32;
			NPC.defense = 9999;
			tail = true;
		}

		public override void ModifyIncomingHit(ref NPC.HitModifiers modifiers)
		{
			modifiers.FinalDamage *= 0f;
		}
	}

	public abstract class Frightcrawler : BaseWorm
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Frightcrawler");
			NPCID.Sets.TrailCacheLength[NPC.type] = 1;
			NPCID.Sets.TrailingMode[NPC.type] = 1;
		}

        public override void SetDefaults()
		{
			bodySegmentCount = 40;
			tailType = ModContent.NPCType<FrightcrawlerTail>();
			bodyType = ModContent.NPCType<FrightcrawlerBody>();
			headType = ModContent.NPCType<FrightcrawlerHead>();
			NPC.lifeMax = 4500;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.damage = 40;
			NPC.friendly = false;
			NPC.noTileCollide = true;
			NPC.noGravity = true;
			NPC.defense = 25;
			NPC.knockBackResist = 0f;

			NPC.buffImmune[BuffID.OnFire] = true;
			NPC.buffImmune[BuffID.Venom] = true;
			NPC.buffImmune[BuffID.Frostburn] = true;
			NPC.buffImmune[BuffID.CursedInferno] = true;
			NPC.buffImmune[BuffID.Ichor] = true;
			NPC.buffImmune[BuffID.ShadowFlame] = true;
			NPC.buffImmune[BuffID.Poisoned] = true;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			SpriteEffects effects = new SpriteEffects();
			if (NPC.direction == 1)
			{
				effects = SpriteEffects.FlipHorizontally;
			}
			Vector2 drawOrigin = new Vector2(NPC.width / 2, NPC.height / 2);
			for (int i = 0; i < NPC.oldPos.Length; i++)
			{
				Vector2 drawPos = NPC.oldPos[i] - Main.screenPosition + drawOrigin + new Vector2(0f, NPC.gfxOffY) + new Vector2(0, 4);
				Color color = NPC.GetAlpha(Color.White) * ((float)(NPC.oldPos.Length - i) / (float)NPC.oldPos.Length);
				spriteBatch.Draw((Texture2D)ModContent.Request<Texture2D>(Texture + "_Glow"), drawPos, NPC.frame, color, NPC.rotation, drawOrigin, NPC.scale, effects, 0f);
			}
		}

        public override void OnKill()
		{
			TerrorbornSystem.downedFrightcrawler = true;
			base.OnKill();
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.NoxiousScale>(), 1, 15, 20));
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Equipable.Accessories.SoulEater>(), 3));
		}
    }
}
