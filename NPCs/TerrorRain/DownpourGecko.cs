using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;

namespace TerrorbornMod.NPCs.TerrorRain
{
    class DownpourGecko : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 9;
            NPCID.Sets.TrailCacheLength[NPC.type] = 1;
            NPCID.Sets.TrailingMode[NPC.type] = 1;
        }
        public override void SetDefaults()
        {
            NPC.noGravity = false;
            NPC.noTileCollide = false;
            NPC.width = 86;
            NPC.height = 30;
            NPC.damage = 45;
            NPC.defense = 15;
            NPC.lifeMax = 450;
            NPC.HitSound = SoundID.NPCHit50;
            NPC.DeathSound = SoundID.NPCDeath53;
            NPC.value = 250;
            NPC.knockBackResist = 0.3f;
            NPC.aiStyle = 26;

            NPC.lavaImmune = true;
        }

        public override void NPCLoot()
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(Main.LocalPlayer);
            if (TerrorbornSystem.obtainedShriekOfHorror)
            {
                if (modPlayer.DeimosteelCharm)
                {
                    if (Main.rand.NextFloat() <= 0.3f)
                    {
                        Item.NewItem(NPC.getRect(), ModContent.ItemType<Items.Materials.TerrorSample>());
                    }
                }
                else
                {
                    if (Main.rand.NextFloat() <= 0.15f)
                    {
                        Item.NewItem(NPC.getRect(), ModContent.ItemType<Items.Materials.TerrorSample>());
                    }
                }
            }

            if (Main.rand.NextFloat() <= 0.65f)
            {
                Item.NewItem(NPC.getRect(), ModContent.ItemType<Items.Materials.ThunderShard>());
            }
        }

        int projWait = 0;
        public override void PostAI()
        {
            NPC.spriteDirection = NPC.direction;

            projWait--;
            if (projWait <= 0)
            {
                projWait = Main.rand.Next(15, 25);
                Projectile.NewProjectile(NPC.Center, Vector2.Zero, ModContent.ProjectileType<GeckoGloop>(), 30 / 4, 0);
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
                spriteBatch.Draw((Texture2D)ModContent.Request<Texture2D>("TerrorbornMod/NPCs/TerrorRain/DownpourGecko_Glow"), drawPos, NPC.frame, color, NPC.rotation, drawOrigin, NPC.scale, effects, 0f);
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
                if (frame >= Main.npcFrameCount[NPC.type])
                {
                    frame = 0;
                }
            }
            else
            {
                frame = 8;
            }
            NPC.frame.Y = frame * frameHeight;
        }
    }

    class GeckoGloop : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 16;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 1000;
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(ModContent.BuffType<Buffs.Debuffs.Glooped>(), 60 * 10);
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            height = 10;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        int trueTimeLeft = 60 * 5;
        public override void AI()
        {
            Projectile.velocity.Y += 0.2f;

            Projectile.frameCounter--;
            if (Projectile.frameCounter <= 0)
            {
                Projectile.frameCounter = 20;
                Projectile.frame++;
                if (Projectile.frame >= 2)
                {
                    Projectile.frame = 0;
                }
            }

            if (trueTimeLeft > 0)
            {
                trueTimeLeft--;
            }
            else
            {
                Projectile.alpha += 255 / 60;
                if (Projectile.alpha >= 255)
                {
                    Projectile.active = false;
                }
            }
        }
    }
}
