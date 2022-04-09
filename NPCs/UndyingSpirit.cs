using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader.Utilities;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Bestiary;

namespace TerrorbornMod.NPCs
{
    class UndyingSpirit : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 6;
            NPCID.Sets.TrailCacheLength[NPC.type] = 3;
            NPCID.Sets.TrailingMode[NPC.type] = 1;
        }
        public override void SetDefaults()
        {
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.width = 48;
            NPC.height = 42;
            NPC.damage = 45;
            NPC.defense = 6;
            NPC.lifeMax = 1500;
            NPC.HitSound = SoundID.NPCHit36;
            NPC.DeathSound = SoundID.NPCDeath39;
            NPC.value = 250;
            NPC.knockBackResist = 0f;
            NPC.lavaImmune = true;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundCrimson,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundCorruption,
                new FlavorTextBestiaryInfoElement("A strange eratic spirit that roams the world evil.")
            });
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.ByCondition(new ItemDropRules.Conditions.ShriekOfHorrorUnlockedCondition(), ItemID.SoulofNight, 1, 15, 20));
        }

        public override void OnKill()
        {
            TerrorbornSystem.downedUndyingSpirit = true;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            SpriteEffects effects = new SpriteEffects();
            effects = SpriteEffects.FlipHorizontally;
            Vector2 drawOrigin = new Vector2(NPC.width / 2, NPC.height / 2);
            for (int i = 0; i < NPC.oldPos.Length; i++)
            {
                Vector2 drawPos = NPC.oldPos[i] - Main.screenPosition + drawOrigin + new Vector2(0f, NPC.gfxOffY) + new Vector2(0, 4);
                Color color = NPC.GetAlpha(Color.White) * ((float)(NPC.oldPos.Length - i) / (float)NPC.oldPos.Length);
                spriteBatch.Draw((Texture2D)ModContent.Request<Texture2D>("TerrorbornMod/NPCs/UndyingSpirit_Glow"), drawPos, NPC.frame, color, NPC.rotation, drawOrigin, NPC.scale, effects, 0f);
            }
        }
        int frame = 0;
        Vector2 velocity = Vector2.Zero;
        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter--;
            if (NPC.frameCounter <= 0)
            {
                frame++;
                NPC.frameCounter = 3;
            }
            if (frame >= 6)
            {
                frame = 0;
            }
            NPC.frame.Y = frame * frameHeight;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (Main.hardMode && !NPC.AnyNPCs(NPC.type))
            {
                float multiplier = 0.0075f;
                if (WorldGen.crimson)
                {
                    return SpawnCondition.Crimson.Chance * multiplier;
                }
                else
                {
                    return SpawnCondition.Corruption.Chance * multiplier;
                }
            }
            else
            {
                return 0f;
            }
        }

        float offsetDirection = 0f;
        bool angered = false;
        public override void AI()
        {
            NPC.spriteDirection = 1;
            Player player = Main.player[NPC.target];
            NPC.rotation = NPC.DirectionTo(player.Center).ToRotation();

            if (!angered && NPC.life < NPC.lifeMax)
            {
                angered = true;
                offsetDirection = NPC.DirectionFrom(player.Center).ToRotation();
                CombatText.NewText(NPC.getRect(), Color.PaleVioletRed, "!!!", true);
            }

            offsetDirection += MathHelper.ToRadians(5);

            if (angered)
            {
                int offset = 500;
                moveTowardsPosition(0.85f, 0.975f, offsetDirection.ToRotationVector2() * offset + player.Center);
            }
            else
            {
                int offset = 750;
                moveTowardsPosition(0.3f, 0.97f, offsetDirection.ToRotationVector2() * offset + player.Center);
            }

            NPC.velocity = velocity;
        }

        public void moveTowardsPosition(float speed, float velocityMultiplier, Vector2 position)
        {
            Vector2 direction = NPC.DirectionTo(position);
            velocity += speed * direction;
            velocity *= velocityMultiplier;
        }
    }
}

