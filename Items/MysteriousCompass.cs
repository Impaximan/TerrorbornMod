using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TerrorbornMod.Items
{
    class MysteriousCompass : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("You feel emotionally attached to it somehow..." +
                "\n[c/FF1919:Points to something important]");
        }

        public override void SetDefaults()
        {
            item.rare = -11;
        }

        public override void HoldItem(Player player)
        {
            bool arrowExists = false;
            for (int i = 0; i < 1000; i++)
            {
                Projectile projectile = Main.projectile[i];
                if (projectile.type == ModContent.ProjectileType<CompassPointer>() && projectile.active)
                {
                    arrowExists = true;
                    break;
                }
            }
            if (!arrowExists)
            {
                Projectile.NewProjectile(player.Center, Vector2.Zero, ModContent.ProjectileType<CompassPointer>(), 0, 0, player.whoAmI);
            }
        }
    }

    class CompassPointer : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 26;
            projectile.height = 36;
            projectile.friendly = false;
            projectile.hostile = false;
            projectile.timeLeft = 500;
            projectile.tileCollide = false;
            projectile.light = 0.5f;
        }

        Vector2 targetPosition()
        {
            return TerrorbornWorld.ShriekOfHorror;
        }


        public override void AI()
        {
            projectile.timeLeft = 500;

            Player player = Main.player[projectile.owner];
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);

            if (player.HeldItem.type != ModContent.ItemType<MysteriousCompass>())
            {
                projectile.active = false;
            }

            projectile.position = player.Center + new Vector2(0, -100);
            projectile.position -= new Vector2(projectile.width / 2, projectile.height / 2);

            if (targetPosition() == Vector2.Zero)
            {
                return;
            }
            else
            {
                projectile.rotation = projectile.DirectionTo(targetPosition()).ToRotation() + MathHelper.ToRadians(90);
            }
        }
    }
}
