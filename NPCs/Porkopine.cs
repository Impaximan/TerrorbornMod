using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.NPCs
{
    class Porkopine : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 4;
            NPCID.Sets.TrailCacheLength[npc.type] = 3;
            NPCID.Sets.TrailingMode[npc.type] = 1;
        }
        public override void SetDefaults()
        {
            npc.noGravity = false;
            npc.noTileCollide = false;
            npc.aiStyle = -1;
            npc.width = 32;
            npc.height = 26;
            npc.damage = 10;
            npc.defense = 2;
            npc.lifeMax = 25;
            npc.HitSound = SoundID.NPCHit7;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.value = 250;
            npc.knockBackResist = 1f;
            npc.lavaImmune = true;
        }

        public override void NPCLoot()
        {
            if (Main.rand.NextFloat() <= 0.05f)
            {
                Item.NewItem(npc.getRect(), ModContent.ItemType<Items.Equipable.Armor.PineHood>());
            }
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            SpriteEffects effects = new SpriteEffects();
            if (npc.spriteDirection == 1)
            {
                effects = SpriteEffects.FlipHorizontally;
            }
            Vector2 drawOrigin = new Vector2(npc.width / 2, npc.height / 2);
            for (int i = 0; i < npc.oldPos.Length; i++)
            {
                Vector2 drawPos = npc.oldPos[i] - Main.screenPosition + drawOrigin + new Vector2(0f, npc.gfxOffY) + new Vector2(0, 4);
                Color color = npc.GetAlpha(Color.White) * ((float)(npc.oldPos.Length - i) / (float)npc.oldPos.Length);
                spriteBatch.Draw(ModContent.GetTexture("TerrorbornMod/NPCs/Porkopine_Glow"), drawPos, npc.frame, color, npc.rotation, drawOrigin, npc.scale, effects, 0f);
            }
        }

        int frame = 0;
        public override void FindFrame(int frameHeight)
        {
            if (npc.velocity.Y == 0)
            {
                npc.frameCounter--;
                if (npc.frameCounter <= 0)
                {
                    frame++;
                    npc.frameCounter = 3;
                }
                if (frame >= 4)
                {
                    frame = 0;
                }
            }

            npc.frame.Y = frame * frameHeight;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (TerrorbornPlayer.modPlayer(spawnInfo.player).ZoneIncendiary || spawnInfo.player.ZoneBeach || spawnInfo.player.ZoneDesert || spawnInfo.player.ZoneJungle)
            {
                return 0f;
            }
            return SpawnCondition.OverworldDay.Chance * 0.65f;
        }

        bool rolling = false;
        public override void AI()
        {
            TerrorbornNPC modNPC = TerrorbornNPC.modNPC(npc);
            modNPC.ImprovedFighterAI(npc, 1.5f, 0.2f, 0.995f, 5, true, 0, 180, 180);
            npc.spriteDirection *= -1;
        }
    }
}