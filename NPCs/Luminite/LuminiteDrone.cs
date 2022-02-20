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
            Main.npcFrameCount[npc.type] = 3;
            NPCID.Sets.TrailCacheLength[npc.type] = 3;
            NPCID.Sets.TrailingMode[npc.type] = 1;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            SpriteEffects effects = SpriteEffects.None;
            if (npc.spriteDirection == 1)
            {
                effects = SpriteEffects.FlipHorizontally;
            }
            spriteBatch.Draw(ModContent.GetTexture(Texture + "_Glow"), npc.Center - Main.screenPosition + new Vector2(0, 4), npc.frame, Color.White, npc.rotation, npc.Size / 2, npc.scale, effects, 0f);
        }

        public override void SetDefaults()
        {
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.aiStyle = -1;
            npc.width = 60;
            npc.height = 50;
            npc.damage = 90;
            npc.defense = 35;
            npc.lifeMax = 7500;
            npc.HitSound = SoundID.NPCHit42;
            npc.DeathSound = SoundID.NPCDeath56;
            npc.value = Item.buyPrice(0, 15, 0, 0);
            npc.knockBackResist = 0f;
            npc.lavaImmune = true;
        }

        int frame = 0;
        public override void FindFrame(int frameHeight)
        {
            npc.frameCounter--;
            if (npc.frameCounter <= 0)
            {
                frame++;
                npc.frameCounter = 5;
            }
            if (frame >= 3)
            {
                frame = 0;
            }
            npc.frame.Y = frame * frameHeight;
        }

        public override void NPCLoot()
        {
            Item.NewItem(npc.getRect(), ItemID.LunarOre, Main.rand.Next(15, 25));
            Item.NewItem(npc.getRect(), ItemID.FragmentSolar, Main.rand.Next(6));
            Item.NewItem(npc.getRect(), ItemID.FragmentVortex, Main.rand.Next(6));
            Item.NewItem(npc.getRect(), ItemID.FragmentStardust, Main.rand.Next(6));
            Item.NewItem(npc.getRect(), ItemID.FragmentNebula, Main.rand.Next(6));
            Item.NewItem(npc.getRect(), ModContent.ItemType<Items.Materials.FusionFragment>(), Main.rand.Next(4));
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
        int projectileWait = 0;
        public override void AI()
        {
            Player player = Main.LocalPlayer;
            Vector2 groundPosition = npc.Center.findGroundUnder();

            speed = MathHelper.Lerp(0.4f, 0.2f, (float)npc.life / (float)npc.lifeMax);

            if (npc.life <= npc.lifeMax * 0.5f)
            {
                projectileWait++;
                if (projectileWait > MathHelper.Lerp(45f, 90f, (float)npc.life * 2f / (float)npc.lifeMax))
                {
                    projectileWait = 0;
                    float projSpeed = 10f;
                    Vector2 velocity = npc.DirectionTo(player.Center + player.velocity * npc.Distance(player.Center) / projSpeed) * projSpeed;
                    Projectile.NewProjectile(npc.Center, velocity / 2, ProjectileID.PhantasmalBolt, npc.damage / 6, 0f);
                }
            }

            if (Collision.CanHit(npc.position, npc.width, npc.height, player.position, player.width, player.height) || npc.life < npc.lifeMax)
            {
                int yDirection = Math.Sign(player.Center.Y - npc.Center.Y);
                if (npc.Distance(groundPosition) > 500)
                {
                    yDirection = 1;
                }
                npc.velocity.Y += speed * yDirection;

                int xDirection = Math.Sign(player.Center.X - npc.Center.X);
                npc.velocity.X += speed * xDirection;
                npc.spriteDirection = xDirection;
            }
            else
            {
                int yDirection = -1;
                if (npc.Distance(groundPosition) > 200)
                {
                    yDirection = 1;
                }
                npc.velocity.Y += speed * yDirection;
            }

            npc.rotation = MathHelper.ToRadians(npc.velocity.X * 2);
            npc.velocity *= 0.98f;
        }
    }
}
