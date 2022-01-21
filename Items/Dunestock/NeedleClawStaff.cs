using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework.Graphics;

namespace TerrorbornMod.Items.Dunestock
{
    class NeedleClawStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Needle Staff");
            Tooltip.SetDefault("Rapidly fires inaccurate needles");
            Item.staff[item.type] = true;
        }

        public override void SetDefaults()
        {
            TerrorbornItem modItem = TerrorbornItem.modItem(item);
            modItem.critDamageMult = 1.5f;
            item.damage = 18;
            item.noMelee = true;
            item.width = 54;
            item.height = 56;
            item.useTime = 5;
            item.useAnimation = 5;
            item.knockBack = 5;
            item.value = Item.sellPrice(0, 2, 0, 0);
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.rare = ItemRarityID.Orange;
            item.UseSound = SoundID.Item42;
            item.autoReuse = true;
            item.shoot = ModContent.ProjectileType<MagicNeedle>();
            item.shootSpeed = 15f;
            item.mana = 2;
            item.magic = true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 mouse = new Vector2(Main.mouseX, Main.mouseY) + Main.screenPosition;
            position = player.Center + (player.DirectionTo(mouse) * 65);
            int proj = Projectile.NewProjectile(position, new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(25)), type, damage, knockBack, item.owner);
            Main.projectile[proj].ai[1] = 5;
            return false;
        }
    }

    class MagicNeedle : ModProjectile
    {
        public override string Texture => "TerrorbornMod/NPCs/Bosses/TumblerNeedle";
        bool Stick = false;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[this.projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[this.projectile.type] = 1;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            //Thanks to Seraph for afterimage code.
            Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
            for (int i = 0; i < projectile.oldPos.Length; i++)
            {
                Vector2 drawPos = projectile.oldPos[i] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
                Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - i) / (float)projectile.oldPos.Length);
                spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, new Rectangle?(), color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
            }
            return false;
        }

        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = -1;
            projectile.magic = true;
            projectile.ignoreWater = false;
            projectile.tileCollide = true;
            projectile.penetrate = 3;
            projectile.timeLeft = 12000;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            fallThrough = projectile.Center.Y < Main.MouseWorld.Y;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Stick = true;
            return false;
        }

        public override void AI()
        {
            if (projectile.ai[1] <= 0)
            {
                projectile.alpha += 15;
                if (projectile.alpha >= 255)
                {
                    projectile.active = false;
                }
            }

            if (Stick)
            {
                projectile.velocity *= 0;
                projectile.ai[1]--;
            }
            else
            {
                projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
            }
        }
    }
}

