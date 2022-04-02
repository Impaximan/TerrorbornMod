using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.NPCs.Luminite
{
    class LuminiteDrone : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 3;
            NPCID.Sets.TrailCacheLength[NPC.type] = 3;
            NPCID.Sets.TrailingMode[NPC.type] = 1;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            SpriteEffects effects = SpriteEffects.None;
            if (NPC.spriteDirection == 1)
            {
                effects = SpriteEffects.FlipHorizontally;
            }
            spriteBatch.Draw((Texture2D)ModContent.Request<Texture2D>(Texture + "_Glow"), NPC.Center - Main.screenPosition + new Vector2(0, 4), NPC.frame, Color.White, NPC.rotation, NPC.Size / 2, NPC.scale, effects, 0f);
        }

        public override void SetDefaults()
        {
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.aiStyle = -1;
            NPC.width = 60;
            NPC.height = 50;
            NPC.damage = 90;
            NPC.defense = 35;
            NPC.lifeMax = 7500;
            NPC.HitSound = SoundID.NPCHit42;
            NPC.DeathSound = SoundID.NPCDeath56;
            NPC.value = Item.buyPrice(0, 15, 0, 0);
            NPC.knockBackResist = 0f;
            NPC.lavaImmune = true;
        }

        int frame = 0;
        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter--;
            if (NPC.frameCounter <= 0)
            {
                frame++;
                NPC.frameCounter = 5;
            }
            if (frame >= 3)
            {
                frame = 0;
            }
            NPC.frame.Y = frame * frameHeight;
        }

        public override void NPCLoot()
        {
            Item.NewItem(NPC.getRect(), ItemID.LunarOre, Main.rand.Next(15, 25));
            Item.NewItem(NPC.getRect(), ItemID.FragmentSolar, Main.rand.Next(6));
            Item.NewItem(NPC.getRect(), ItemID.FragmentVortex, Main.rand.Next(6));
            Item.NewItem(NPC.getRect(), ItemID.FragmentStardust, Main.rand.Next(6));
            Item.NewItem(NPC.getRect(), ItemID.FragmentNebula, Main.rand.Next(6));
            Item.NewItem(NPC.getRect(), ModContent.ItemType<Items.Materials.FusionFragment>(), Main.rand.Next(4));
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (NPC.downedMoonlord)
            {
                return SpawnCondition.OverworldNightMonster.Chance * 0.05f;
            }
            return 0f;
        }

        float speed = 0.2f;
        int ProjectileWait = 0;
        public override void AI()
        {
            Player player = Main.LocalPlayer;
            Vector2 groundPosition = NPC.Center.findGroundUnder();

            speed = MathHelper.Lerp(0.4f, 0.2f, (float)NPC.life / (float)NPC.lifeMax);

            if (NPC.life <= NPC.lifeMax * 0.5f)
            {
                ProjectileWait++;
                if (ProjectileWait > MathHelper.Lerp(45f, 90f, (float)NPC.life * 2f / (float)NPC.lifeMax))
                {
                    ProjectileWait = 0;
                    float projSpeed = 10f;
                    Vector2 velocity = NPC.DirectionTo(player.Center + player.velocity * NPC.Distance(player.Center) / projSpeed) * projSpeed;
                    Projectile.NewProjectile(NPC.Center, velocity / 2, ProjectileID.PhantasmalBolt, NPC.damage / 6, 0f);
                }
            }

            if (Collision.CanHit(NPC.position, NPC.width, NPC.height, player.position, player.width, player.height) || NPC.life < NPC.lifeMax)
            {
                int yDirection = Math.Sign(player.Center.Y - NPC.Center.Y);
                if (NPC.Distance(groundPosition) > 500)
                {
                    yDirection = 1;
                }
                NPC.velocity.Y += speed * yDirection;

                int xDirection = Math.Sign(player.Center.X - NPC.Center.X);
                NPC.velocity.X += speed * xDirection;
                NPC.spriteDirection = xDirection;
            }
            else
            {
                int yDirection = -1;
                if (NPC.Distance(groundPosition) > 200)
                {
                    yDirection = 1;
                }
                NPC.velocity.Y += speed * yDirection;
            }

            NPC.rotation = MathHelper.ToRadians(NPC.velocity.X * 2);
            NPC.velocity *= 0.98f;
        }
    }
}
