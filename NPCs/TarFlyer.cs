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
    class TarFlyer : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 3;
            NPCID.Sets.TrailCacheLength[NPC.type] = 3;
            NPCID.Sets.TrailingMode[NPC.type] = 1;
        }
        public override void SetDefaults()
        {
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.aiStyle = -1;
            NPC.width = 62;
            NPC.height = 36;
            NPC.damage = 45;
            NPC.defense = 6;
            NPC.lifeMax = 160;
            NPC.HitSound = SoundID.NPCHit31;
            NPC.DeathSound = SoundID.NPCDeath39;
            NPC.value = 250;
            NPC.knockBackResist = 0.1f;
            NPC.lavaImmune = true;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundDesert,
                new FlavorTextBestiaryInfoElement("The corpse of an antlion swarmer, reanimated by the living tar in the desert. Due to a constant need to consume for sustenance, it is highly aggressive.")
            });
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.ByCondition(new ItemDropRules.Conditions.ShriekOfHorrorUnlockedCondition(), ModContent.ItemType<Items.Materials.TarOfHunger>(), minimumDropped: 5, maximumDropped: 10));
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            SpriteEffects effects = new SpriteEffects();
            if (NPC.spriteDirection == 1)
            {
                effects = SpriteEffects.FlipHorizontally;
            }
            Vector2 drawOrigin = new Vector2(NPC.width / 2, NPC.height / 2);
            for (int i = 0; i < NPC.oldPos.Length; i++)
            {
                Vector2 drawPos = NPC.oldPos[i] - Main.screenPosition + drawOrigin + new Vector2(0f, NPC.gfxOffY) + new Vector2(0, 4);
                Color color = NPC.GetAlpha(Color.White) * ((float)(NPC.oldPos.Length - i) / (float)NPC.oldPos.Length);
                spriteBatch.Draw((Texture2D)ModContent.Request<Texture2D>("TerrorbornMod/NPCs/TarFlyer_Glow"), drawPos, NPC.frame, color, NPC.rotation, drawOrigin, NPC.scale, effects, 0f);
            }
        }
        int frame = 0;
        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter--;
            if (NPC.frameCounter <= 0)
            {
                frame++;
                NPC.frameCounter = 2;
            }
            if (frame >= 3)
            {
                frame = 0;
            }
            NPC.frame.Y = frame * frameHeight;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (NPC.downedBoss3)
            {
                return SpawnCondition.DesertCave.Chance * 0.11f;
            }
            else
            {
                return 0f;
            }
        }
        public override void AI()
        {
            NPC.TargetClosest(true);
            if (Main.player[NPC.target].Center.X > NPC.Center.X)
            {
                NPC.spriteDirection = 1;
            }
            else
            {
                NPC.spriteDirection = -1;
            }
            if (Main.player[NPC.target].dead)
            {
                float speed = -0.2f;
                Vector2 velocity = NPC.DirectionTo(Main.player[NPC.target].Center) * speed;
                NPC.velocity += velocity;

                NPC.velocity *= 0.99f;
                if (NPC.Distance(Main.player[NPC.target].Center) > 4500)
                {
                    NPC.active = false;
                }
            }
            else
            {
                float speed = 0.1f;
                if (!Collision.CanHit(NPC.position, NPC.width, NPC.height, Main.player[NPC.target].position, Main.player[NPC.target].width, Main.player[NPC.target].height))
                {
                    speed = 0.04f;
                }
                else
                {
                    int dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.GoldFlame, Scale: 1f);
                    Main.dust[dust].velocity = NPC.velocity;
                    Main.dust[dust].noGravity = true;
                }
                Vector2 velocity = NPC.DirectionTo(Main.player[NPC.target].Center) * speed;
                NPC.velocity += velocity;
                NPC.velocity += velocity;

                NPC.velocity *= 0.99f;
            }
            NPC.rotation = MathHelper.ToRadians(NPC.velocity.X * 1.5f);
        }
    }
}
