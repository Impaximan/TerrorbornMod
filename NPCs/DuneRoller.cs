﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Bestiary;

namespace TerrorbornMod.NPCs
{
    class DuneRoller : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 14;
            NPCID.Sets.TrailCacheLength[NPC.type] = 3;
            NPCID.Sets.TrailingMode[NPC.type] = 1;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Desert,
                new FlavorTextBestiaryInfoElement("Some time ago, a group of cave rollers migrated to the desert, for an unknown reason. Rolling around in the terror-ridden sand has made their shells more resistent and their muscles more powerful." +
                "\nDuring the day, they will roam the surface of the desert, but during the night you will only find them in the depths of the desert.")
            });
        }

        public override void SetDefaults()
        {
            NPC.noGravity = false;
            NPC.noTileCollide = false;
            NPC.aiStyle = -1;
            NPC.width = 34;
            NPC.height = 34;
            NPC.damage = 15;
            NPC.defense = 12;
            NPC.lifeMax = 125;
            NPC.HitSound = SoundID.NPCHit7;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = 250;
            NPC.knockBackResist = 0.75f;
            NPC.lavaImmune = true;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.ShellFragments>(), minimumDropped: 2, maximumDropped: 3));
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
                spriteBatch.Draw((Texture2D)ModContent.Request<Texture2D>("TerrorbornMod/NPCs/CaveRoller_Glow"), drawPos, NPC.frame, color, NPC.rotation, drawOrigin, NPC.scale, effects, 0f);
            }
        }

        int frame = 0;
        public override void FindFrame(int frameHeight)
        {
            if (rolling)
            {
                NPC.frame.Y = 13 * frameHeight;
                return;
            }

            if (NPC.velocity.Y == 0)
            {
                NPC.frameCounter--;
                if (NPC.frameCounter <= 0)
                {
                    frame++;
                    NPC.frameCounter = 3;
                }
                if (frame >= 12)
                {
                    frame = 0;
                }
            }
            else
            {
                frame = 12;
            }

            NPC.frame.Y = frame * frameHeight;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (Main.dayTime)
            {
                return SpawnCondition.OverworldDayDesert.Chance * 0.5f;
            }
            else
            {
                return SpawnCondition.DesertCave.Chance * 0.15f;
            }
        }

        bool rolling = false;
        public override void AI()
        {
            TerrorbornNPC modNPC = TerrorbornNPC.modNPC(NPC);
            if (rolling)
            {
                modNPC.ImprovedFighterAI(NPC, 10, 0.1f, 0.995f, 10, true, 0, 600, 120);
                NPC.rotation += MathHelper.ToRadians(NPC.velocity.X);
            }
            else
            {
                modNPC.ImprovedFighterAI(NPC, 2, 0.2f, 0.99f, 8, true, 0);
                if (NPC.life <= NPC.lifeMax / 3)
                {
                    rolling = true;
                    NPC.knockBackResist /= 3;
                    NPC.defense *= 2;
                    NPC.damage *= 2;
                }
            }
        }
    }
}


