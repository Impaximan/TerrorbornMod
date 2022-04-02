using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.NPCs
{
    class CaveRoller : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 14;
            NPCID.Sets.TrailCacheLength[NPC.type] = 3;
            NPCID.Sets.TrailingMode[NPC.type] = 1;
        }
        public override void SetDefaults()
        {
            NPC.noGravity = false;
            NPC.noTileCollide = false;
            NPC.aiStyle = -1;
            NPC.width = 34;
            NPC.height = 34;
            NPC.damage = 15;
            NPC.defense = 3;
            NPC.lifeMax = 100;
            NPC.HitSound = SoundID.NPCHit7;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = 250;
            NPC.knockBackResist = 0.75f;
            NPC.lavaImmune = true;
        }

        public override void NPCLoot()
        {
            Item.NewItem(NPC.getRect(), ModContent.ItemType<Items.Materials.ShellFragments>(), Main.rand.Next(2, 4));
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
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
            return SpawnCondition.Cavern.Chance * 0.12f;
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

