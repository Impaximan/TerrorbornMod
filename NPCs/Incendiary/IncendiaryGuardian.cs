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

namespace TerrorbornMod.NPCs.Incendiary
{
    class IncendiaryGuardian : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 5;
        }
        public override void SetDefaults()
        {
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.width = 48;
            npc.height = 42;
            npc.damage = 45;
            npc.defense = 21;
            npc.lifeMax = 750;
            npc.HitSound = SoundID.NPCHit41;
            npc.DeathSound = SoundID.NPCDeath14;
            npc.value = 250;
            npc.knockBackResist = 0f;
            npc.lavaImmune = true;
        }

        int frame = 0;
        public override void FindFrame(int frameHeight)
        {
            npc.frame.Y = frame * frameHeight;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            if (frame == 4 && fireCounter > 0)
            {
                int laserCount = 4;
                for (int i = 0; i < laserCount; i++)
                {
                    Utils.DrawLine(spriteBatch, npc.Center + new Vector2(0, -15), npc.Center + new Vector2(0, -15) + rotation.ToRotationVector2().RotatedBy(MathHelper.ToRadians(360 / laserCount) * i) * 100, Color.Red, Color.Transparent, 5);
                }
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (TerrorbornPlayer.modPlayer(spawnInfo.player).ZoneIncendiary)
            {
                return SpawnCondition.Sky.Chance * 0.05f;
            }
            else
            {
                return 0f;
            }
        }

        float rotation = 0f;
        int fireCounter = 45;
        int soundCounter = 0;
        public override void AI()
        {
            if (npc.life < npc.lifeMax)
            {
                if (frame < 4)
                {
                    npc.frameCounter--;
                    if (npc.frameCounter <= 0)
                    {
                        frame++;
                        npc.frameCounter = 10;
                    }
                }
                else
                {
                    fireCounter--;
                    if (fireCounter <= 0)
                    {
                        soundCounter--;
                        if (soundCounter <= 0)
                        {
                            soundCounter = 10;
                            Main.PlaySound(2, (int)npc.position.X, (int)npc.position.Y, Style: 15, 4, 0);
                        }

                        rotation += MathHelper.ToRadians(1);
                        TerrorbornMod.ScreenShake(2);
                        int laserCount = 4;
                        for (int i = 0; i < laserCount; i++)
                        {
                            Projectile projectile = Main.projectile[Projectile.NewProjectile(npc.Center, rotation.ToRotationVector2().RotatedBy(MathHelper.ToRadians(360 / laserCount) * i), ModContent.ProjectileType<Projectiles.IncendiaryDeathray>(), 80 / 4, 0)];
                            projectile.ai[0] = npc.whoAmI;
                            projectile.ai[1] = -15;
                        }
                    }
                }
            }
        }
    }
}


