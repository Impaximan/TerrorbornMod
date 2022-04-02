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
            Main.npcFrameCount[NPC.type] = 4;
            NPCID.Sets.TrailCacheLength[NPC.type] = 3;
            NPCID.Sets.TrailingMode[NPC.type] = 1;
        }
        public override void SetDefaults()
        {
            NPC.noGravity = false;
            NPC.noTileCollide = false;
            NPC.aiStyle = -1;
            NPC.width = 32;
            NPC.height = 26;
            NPC.damage = 5;
            NPC.defense = 2;
            NPC.lifeMax = 25;
            NPC.HitSound = SoundID.NPCHit7;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = 250;
            NPC.knockBackResist = 1f;
            NPC.lavaImmune = true;
        }

        public override void NPCLoot()
        {
            if (Main.rand.NextFloat() <= 0.05f)
            {
                Item.NewItem(NPC.getRect(), ModContent.ItemType<Items.Equipable.Armor.PineHood>());
            }
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
                spriteBatch.Draw((Texture2D)ModContent.Request<Texture2D>("TerrorbornMod/NPCs/Porkopine_Glow"), drawPos, NPC.frame, color, NPC.rotation, drawOrigin, NPC.scale, effects, 0f);
            }
        }

        int frame = 0;
        public override void FindFrame(int frameHeight)
        {
            if (NPC.velocity.Y == 0)
            {
                NPC.frameCounter--;
                if (NPC.frameCounter <= 0)
                {
                    frame++;
                    NPC.frameCounter = 3;
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
            if (TerrorbornPlayer.modPlayer(spawnInfo.player).ZoneIncendiary || spawnInfo.player.ZoneBeach || spawnInfo.player.ZoneDesert || spawnInfo.player.ZoneJungle)
            {
                return 0f;
            }
            return SpawnCondition.OverworldDay.Chance * 0.65f;
        }

        bool rolling = false;
        public override void AI()
        {
            TerrorbornNPC modNPC = TerrorbornNPC.modNPC(NPC);
            modNPC.ImprovedFighterAI(NPC, 1.5f, 0.2f, 0.995f, 5, true, 0, 180, 180);
            NPC.spriteDirection *= -1;
        }
    }
}