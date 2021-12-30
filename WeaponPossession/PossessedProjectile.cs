using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.World.Generation;
using Terraria.ModLoader;
using Terraria.UI;
using TerrorbornMod;
using Terraria.Map;
using Terraria.GameContent.Dyes;
using Terraria.GameContent.UI;
using Terraria.ModLoader.IO;
using TerrorbornMod.TBUtils;

namespace TerrorbornMod.WeaponPossession
{
    class PossessedProjectile : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        public override void SetDefaults(Projectile projectile)
        {

        }

        public override void OnHitNPC(Projectile projectile, NPC target, int damage, float knockback, bool crit)
        {
            if (originalItem != null)
            {
                if (!originalItem.IsAir)
                {
                    PossessedItem pItem = PossessedItem.modItem(originalItem);
                    if (pItem.possessType == PossessType.Fright)
                    {
                        TerrorbornPlayer.modPlayer(Main.player[projectile.owner]).terrorDrainCounter = 30;
                    }

                    if (pItem.possessType == PossessType.Light && crit)
                    {
                        int proj = Projectile.NewProjectile(target.Center, Vector2.Zero, ModContent.ProjectileType<Lightsplosion>(), damage / 2, 0, projectile.owner);
                        Main.projectile[proj].ai[0] = target.whoAmI;
                        TerrorbornMod.ScreenShake(5f);
                        Main.PlaySound(SoundID.Item68, target.Center);
                    }

                    if (pItem.possessType == PossessType.Night && crit)
                    {
                        Main.player[projectile.owner].HealEffect(1);
                        Main.player[projectile.owner].statLife++;
                    }
                }
            }
        }

        Item originalItem = null;
        bool start = true;
        public override bool PreAI(Projectile projectile)
        {
            if (projectile.owner == 255 || !projectile.friendly || projectile.damage == 0)
            {
                return base.PreAI(projectile);
            }
            if (Main.player[projectile.owner].HeldItem.IsAir)
            {
                return base.PreAI(projectile);
            }
            if (start)
            {
                start = false;
                originalItem = null;
                if (Main.player[projectile.owner] != null && Main.player[projectile.owner].HeldItem != null && projectile.friendly && !projectile.hostile && !Main.gameMenu)
                {
                    Player player = Main.player[projectile.owner];
                    TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
                    if (player.HeldItem == null)
                    {
                        return base.PreAI(projectile);
                    }
                    Item item = player.HeldItem;
                    if (PossessedItem.modItem(item) == null)
                    {
                        return base.PreAI(projectile);
                    }
                    originalItem = item;
                    PossessedItem pItem = PossessedItem.modItem(item);

                    if (pItem.possessType == PossessType.Might)
                    {
                        projectile.extraUpdates = projectile.extraUpdates * 2 + 1;
                    }

                    if (pItem.possessType == PossessType.Flight)
                    {
                        projectile.tileCollide = false;
                        projectile.velocity *= 0.75f;
                    }

                    if (pItem.possessType == PossessType.Sight)
                    {
                        projectile.velocity *= 0.65f;
                    }
                }
            }
            return base.PreAI(projectile);
        }

        public override void AI(Projectile projectile)
        {
            base.AI(projectile);

            if (projectile.owner == 255 || !projectile.friendly || projectile.damage == 0)
            {
                return;
            }

            if (Main.player[projectile.owner].HeldItem.IsAir)
            {
                return;
            }

            if (Main.player[projectile.owner] != null && projectile.friendly && !projectile.hostile && !Main.gameMenu && !projectile.minion && !projectile.sentry && originalItem != null && Main.player[projectile.owner].HeldItem != null)
            {
                Player player = Main.player[projectile.owner];
                TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
                if (originalItem.IsAir)
                {
                    return;
                }
                Item item = originalItem;
                PossessedItem pItem = PossessedItem.modItem(item);

                if (pItem.possessType == PossessType.Flight)
                {
                    projectile.tileCollide = false;
                    if (projectile.Distance(player.Center) >= Main.screenWidth * 1.5f)
                    {
                        projectile.timeLeft = 0;
                    }
                }

                if (pItem.possessType == PossessType.Sight)
                {
                    NPC targetNPC = Main.npc[0];
                    float Distance = 750; //max distance away
                    bool Targeted = false;
                    for (int i = 0; i < 200; i++)
                    {
                        if (Main.npc[i].Distance(projectile.Center) < Distance && !Main.npc[i].friendly && Main.npc[i].CanBeChasedBy())
                        {
                            targetNPC = Main.npc[i];
                            Distance = Main.npc[i].Distance(projectile.Center);
                            Targeted = true;
                        }
                    }
                    if (Targeted)
                    {
                        //HOME IN
                        projectile.velocity = projectile.velocity.ToRotation().AngleTowards(projectile.DirectionTo(targetNPC.Center).ToRotation(), MathHelper.ToRadians(1f * (projectile.velocity.Length() / 20))).ToRotationVector2() * projectile.velocity.Length();
                    }
                }
            }
        }
    }

    class Lightsplosion : ModProjectile
    {
        public override string Texture => "TerrorbornMod/Effects/Textures/Glow_2";

        int timeLeft = 10;
        const int defaultSize = 500;
        int currentSize = defaultSize;
        public override void SetDefaults()
        {
            projectile.width = defaultSize;
            projectile.height = defaultSize;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.tileCollide = false;
            projectile.penetrate = -1;
            projectile.localNPCHitCooldown = -1;
            projectile.usesLocalNPCImmunity = true;
            projectile.timeLeft = timeLeft;
        }

        public override bool? CanHitNPC(NPC target)
        {
            if (target.whoAmI == (int)projectile.ai[0])
            {
                return false;
            }
            return base.CanHitNPC(target);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Graphics.DrawGlow_1(spriteBatch, projectile.Center - Main.screenPosition, currentSize, new Color(255, 212, 255));
            return false;
        }

        public override void AI()
        {
            currentSize -= defaultSize / timeLeft;
        }
    }
}
