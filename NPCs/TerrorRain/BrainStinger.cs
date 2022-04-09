using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Bestiary;

namespace TerrorbornMod.NPCs.TerrorRain
{
    class BrainStinger : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 11;
            NPCID.Sets.TrailCacheLength[NPC.type] = 1;
            NPCID.Sets.TrailingMode[NPC.type] = 1;
        }

        public override void SetDefaults()
        {
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.aiStyle = -1;
            NPC.width = 42;
            NPC.height = 48;
            NPC.damage = 65;
            NPC.defense = 15;
            NPC.lifeMax = 300;
            NPC.HitSound = SoundID.NPCHit25;
            NPC.DeathSound = SoundID.NPCDeath28;
            NPC.value = 250;
            NPC.knockBackResist = 0.25f;
            NPC.lavaImmune = true;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (TerrorbornSystem.terrorRain && Main.raining && spawnInfo.player.ZoneRain)
            {
                return 0.55f;
            }
            else
            {
                return 0f;
            }
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Rain,
                new FlavorTextBestiaryInfoElement("Funky little jellyfish, which have gained new powers by consuming souls. As such, one might even say they're 'high'.")
            });
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.ThunderShard>(), 2));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Summons.Minions.AnglerStaff>(), 75));
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (frame == 8 && extraFrame == 0)
            {
                TBUtils.Graphics.DrawGlow_1(Main.spriteBatch, NPC.Center - Main.screenPosition, 75, new Color(255, 120, 209) * 0.25f);
            }
            return base.PreDraw(spriteBatch, screenPos, drawColor);
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
                spriteBatch.Draw((Texture2D)ModContent.Request<Texture2D>("TerrorbornMod/NPCs/TerrorRain/BrainStinger_Glow"), drawPos, NPC.frame, color, NPC.rotation, drawOrigin, NPC.scale, effects, 0f);
            }
        }

        bool pushingUp = false;
        int frame = 0;
        int extraFrame = 0;
        public override void FindFrame(int frameHeight)
        {
            if (pushingUp)
            {
                if (frame < 5)
                {
                    frame = 5;
                }
                if (frame < 8)
                {
                    NPC.frameCounter--;
                    if (NPC.frameCounter <= 0)
                    {
                        NPC.frameCounter = 4;
                        frame++;
                    }
                }
                if (frame == 8)
                {
                    NPC.frameCounter--;
                    if (NPC.frameCounter <= 0)
                    {
                        NPC.frameCounter = 4;
                        extraFrame++;
                        if (extraFrame > 1)
                        {
                            extraFrame = 0;
                        }
                    }
                }
            }
            else
            {
                extraFrame = 0;
                if (frame != 1)
                {
                    NPC.frameCounter--;
                    if (NPC.frameCounter <= 0)
                    {
                        NPC.frameCounter = 10;
                        frame++;
                        if (frame >= 11)
                        {
                            frame = 0;
                        }
                    }
                }
            }
            NPC.frame.Y = (frame + extraFrame) * frameHeight;
        }
        public override void AI()
        {
            NPC.TargetClosest(false);
            Player player = Main.player[NPC.target];
            if (pushingUp)
            {
                float speed = 0.5f;
                NPC.velocity.X += speed * NPC.ai[1];
                NPC.velocity.Y -= speed;
                NPC.velocity *= 0.95f;

                NPC.ai[0]--;
                if (NPC.ai[0] <= 0)
                {
                    pushingUp = false;
                    NPC.ai[0] = 60;
                }
            }
            else
            {
                NPC.velocity.Y += 0.3f;
                NPC.velocity *= 0.93f;

                NPC.ai[0]--;
                if (NPC.ai[0] <= 0 && player.Center.Y < NPC.Center.Y)
                {
                    pushingUp = true;
                    NPC.ai[0] = 45;
                    NPC.ai[1] = -1;
                    if (player.Center.X > NPC.Center.X)
                    {
                        NPC.ai[1] = 1;
                    }
                }
            }
        }
    }
}
