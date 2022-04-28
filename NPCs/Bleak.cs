using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader.Utilities;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Bestiary;

namespace TerrorbornMod.NPCs
{
    class Bleak : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 5;
            NPCID.Sets.TrailCacheLength[NPC.type] = 3;
            NPCID.Sets.TrailingMode[NPC.type] = 1;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Underground,
                new FlavorTextBestiaryInfoElement("A mysterious wandering mass of terror that roams deimostone caves. Though its exact origin is unknown, it is suspected by many that they are the remaining souls of the fearmongerers.")
            });
        }

        public override void SetDefaults()
        {
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.width = 42;
            NPC.height = 48;
            NPC.damage = 20;
            NPC.lifeMax = 52;
            NPC.HitSound = SoundID.NPCHit36;
            NPC.DeathSound = SoundID.NPCDeath39;
            NPC.value = 250;
            NPC.knockBackResist = 0.2f;
            NPC.lavaImmune = true;
            NPC.buffImmune[BuffID.Confused] = false;
            NPC.aiStyle = 22;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.ByCondition(new ItemDropRules.Conditions.ShriekOfHorrorUnlockedCondition(), ModContent.ItemType<Items.DarkEnergy>()));
            npcLoot.Add(ItemDropRule.ByCondition(new ItemDropRules.Conditions.ShriekOfHorrorUnlockedCondition(), ModContent.ItemType<Items.Materials.TerrorSample>(), 3, chanceNumerator: 2));
        }

        public override void ModifyHitByItem(Player player, Item item, ref int damage, ref float knockback, ref bool crit)
        {
            if (invulnerable)
            {
                damage = 1;
            }
        }

        public override void ModifyHitByProjectile(Projectile Projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (invulnerable)
            {
                damage = 1;
            }
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
                Main.spriteBatch.Draw((Texture2D)ModContent.Request<Texture2D>("TerrorbornMod/NPCs/Bleak_Glow"), drawPos, NPC.frame, color, NPC.rotation, drawOrigin, NPC.scale, effects, 0f);
            }
        }

        int frame = 0;
        //float TrueVelocityX = 0;
        public override void FindFrame(int frameHeight)
        {
            if (invulnerable)
            {
                frame = 4;
                NPC.frameCounter = 0;
            }
            else
            {
                NPC.frameCounter--;
                if (NPC.frameCounter <= 0)
                {
                    frame++;
                    NPC.frameCounter = 4;
                }
                if (frame >= 4)
                {
                    frame = 0;
                }
            }
            NPC.frame.Y = frame * frameHeight;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(spawnInfo.Player);
            if (modPlayer.ZoneDeimostone)
            {
                if (modPlayer.DeimosteelCharm)
                {
                    return SpawnCondition.Cavern.Chance * 0.22f;
                }
                else
                {
                    return SpawnCondition.Cavern.Chance * 0.08f;
                }
            }
            else
            {
                return 0f;
            }
        }

        bool invulnerable = false;
        int invulCounter = 90;
        public override void AI()
        {
            Player player = Main.player[NPC.target];
            NPC.TargetClosest();
            if (invulnerable)
            {
                NPC.noTileCollide = false;
                NPC.knockBackResist = 0.8f;
                invulCounter--;
                if (invulCounter <= 0)
                {
                    invulnerable = false;
                    invulCounter = (int)(60 * Main.rand.NextFloat(4f, 8f));
                }
            }
            else
            {
                NPC.noTileCollide = true;
                NPC.knockBackResist = 0.01f;
                base.AI();
                if (Collision.CanHit(NPC.position, NPC.width, NPC.height, player.position, player.width, player.height))
                {
                    invulCounter--;
                }
                if (invulCounter <= 0)
                {
                    invulnerable = true;
                    invulCounter = 120;
                }
            }
        }
    }
}

