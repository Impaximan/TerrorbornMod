using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Reflection;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Events;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.Localization;
using Terraria.World.Generation;
using Terraria.UI;

namespace TerrorbornMod.NPCs
{
    class SlateBanshee : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 14;
            NPCID.Sets.TrailCacheLength[npc.type] = 1;
            NPCID.Sets.TrailingMode[npc.type] = 1;
        }
        public override void SetDefaults()
        {
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.width = 34;
            npc.height = 896 / 14;
            npc.damage = 25;
            npc.defense = 6;
            npc.lifeMax = 250;
            npc.HitSound = SoundID.NPCHit37;
            npc.DeathSound = SoundID.NPCDeath39;
            npc.value = 250;
            npc.aiStyle = -1;
            npc.knockBackResist = 0f;
            npc.lavaImmune = true;
        }

        public override void NPCLoot()
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(Main.LocalPlayer);
            if (TerrorbornWorld.obtainedShriekOfHorror)
            {
                Item.NewItem(npc.getRect(), ModContent.ItemType<Items.DarkEnergy>());
                if (modPlayer.DeimosteelCharm)
                {
                    Item.NewItem(npc.getRect(), ModContent.ItemType<Items.Materials.TerrorSample>(), Main.rand.Next(4, 9));
                }
                else
                {
                    Item.NewItem(npc.getRect(), ModContent.ItemType<Items.Materials.TerrorSample>(), Main.rand.Next(2, 5));
                }
            }
            TerrorbornWorld.downedSlateBanshee = true;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            SpriteEffects effects = new SpriteEffects();
            effects = SpriteEffects.None;
            if (npc.spriteDirection == 1)
            {
                effects = SpriteEffects.FlipHorizontally;
            }
            Vector2 drawOrigin = new Vector2(npc.width / 2, npc.height / 2);
            for (int i = 0; i < npc.oldPos.Length; i++)
            {
                Vector2 drawPos = npc.oldPos[i] - Main.screenPosition + drawOrigin + new Vector2(0f, npc.gfxOffY) + new Vector2(0, 4);
                Color color = npc.GetAlpha(Color.White) * ((float)(npc.oldPos.Length - i) / (float)npc.oldPos.Length);
                spriteBatch.Draw(ModContent.GetTexture("TerrorbornMod/NPCs/SlateBanshee_Glow"), drawPos, npc.frame, color, npc.rotation, drawOrigin, npc.scale, effects, 0f);
            }
        }

        bool charging = false;
        bool resting = false;
        int frame = 0;
        Vector2 velocity = Vector2.Zero;
        public override void FindFrame(int frameHeight)
        {
            if (!charging && !resting)
            {
                npc.frameCounter++;
                if (npc.frameCounter >= 4)
                {
                    frame++;
                    npc.frameCounter = 0;
                }
                if (frame >= 4)
                {
                    frame = 0;
                }
            }
            if (charging)
            {
                npc.frameCounter++;
                if (npc.frameCounter >= 3)
                {
                    frame++;
                    npc.frameCounter = 0;
                }
                if (frame < 4)
                {
                    frame = 4;
                    npc.frameCounter = 0;
                }
                if (frame >= 8)
                {
                    frame = 5;
                }
            }
            if (resting)
            {
                npc.frameCounter++;
                if (npc.frameCounter >= 5)
                {
                    frame++;
                    npc.frameCounter = 0;
                }
                if (frame < 8)
                {
                    frame = 8;
                    npc.frameCounter = 0;
                }
                if (frame >= 14)
                {
                    frame = 9;
                }
            }
            npc.frame.Y = frame * frameHeight;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(spawnInfo.player);
            if (modPlayer.ZoneDeimostone && TerrorbornWorld.obtainedShriekOfHorror)
            {
                if (modPlayer.DeimosteelCharm)
                {
                    return SpawnCondition.Cavern.Chance * 0.0325f;
                }
                else
                {
                    return SpawnCondition.Cavern.Chance * 0.015f;
                }
            }
            else
            {
                return 0f;
            }
        }

        public override void OnHitByItem(Player player, Item item, int damage, float knockback, bool crit)
        {
            if (charging)
            {
                npc.velocity = Vector2.Zero;
                charging = false;
                resting = true;
                restingCounter = 120;
            }
        }

        public override void OnHitByProjectile(Projectile projectile, int damage, float knockback, bool crit)
        {
            if (charging && projectile.melee && npc.Distance(Main.player[npc.target].Center) <= 75)
            {
                npc.velocity = Vector2.Zero;
                charging = false;
                resting = true;
                restingCounter = 120;
            }
        }

        int regularCounter = 120;
        int chargeCounter = 0;
        int restingCounter = 0;
        float offsetDirection = 0f;
        Vector2 targetOffset;
        bool start = true;
        public override void AI()
        {
            npc.TargetClosest();
            Player player = Main.player[npc.target];

            if (start)
            {
                start = false;
                targetOffset = npc.DirectionFrom(player.Center) * 300;
            }

            if (charging)
            {
                npc.rotation = 0f;
                chargeCounter--;
                if (chargeCounter <= 0)
                {
                    charging = false;
                    resting = true;
                    restingCounter = 60;
                }
            }
            else if (resting)
            {
                npc.velocity *= 0.95f;
                restingCounter--;
                if (restingCounter <= 0)
                {
                    resting = false;
                    targetOffset = npc.DirectionFrom(player.Center) * 300;
                }
            }
            else
            {
                npc.spriteDirection = 1;
                if (player.Center.X < npc.Center.X)
                {
                    npc.spriteDirection = -1;
                }
                float speed = 0.4f;
                npc.velocity += npc.DirectionTo(player.Center + targetOffset) * speed;
                npc.velocity *= 0.95f;
                npc.rotation = MathHelper.ToRadians(npc.velocity.X * 2);

                if (npc.Distance(player.Center) <= 600)
                {
                    regularCounter--;
                    if (regularCounter <= 0)
                    {
                        Main.PlaySound(15, (int)npc.Center.X, (int)npc.Center.Y, 2, 1f, 0.5f);
                        charging = true;
                        regularCounter = 240;
                        chargeCounter = 45;
                        speed = 10f;
                        npc.velocity = npc.DirectionTo(player.Center) * speed;
                    }
                }
            }
        }

        public void moveTowardsPosition(float speed, float velocityMultiplier, Vector2 position)
        {
            Vector2 direction = npc.DirectionTo(position);
            velocity += speed * direction;
            velocity *= velocityMultiplier;
        }
    }
}