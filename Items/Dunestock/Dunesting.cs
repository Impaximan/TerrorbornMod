using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Dunestock
{
    class Dunesting : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Fires a claw that splits into 3 arrows after travelling for a moment");
        }
        public override void SetDefaults()
        {
            item.damage = 23;
            item.ranged = true;
            item.width = 26;
            item.height = 56;
            item.useTime = 20;
            item.useAnimation = 20;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.noMelee = true;
            item.knockBack = 2;
            item.value = Item.sellPrice(0, 2, 0, 0);
            item.rare = ItemRarityID.Orange;
            item.UseSound = SoundID.DD2_BallistaTowerShot;
            item.shoot = ProjectileID.GreenLaser;
            item.autoReuse = true;
            item.shootSpeed = 18f;
            item.useAmmo = AmmoID.Arrow;
        }
        public override bool CanUseItem(Player player)
        {
            return base.CanUseItem(player);
        }
        public override bool ConsumeAmmo(Player player)
        {
            return Main.rand.Next(101) <= 25f;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            int proj = Projectile.NewProjectile(position, new Vector2(speedX, speedY), ModContent.ProjectileType<Claw>(), damage, knockBack, player.whoAmI);
            Main.projectile[proj].ai[0] = type;
            return false;
        }
    }

    class Claw : ModProjectile
    {
        public override string Texture => "TerrorbornMod/NPCs/Bosses/DuneClaw";

        public override void SetDefaults()
        {
            projectile.width = 28;
            projectile.height = 28;
            projectile.aiStyle = 0;
            projectile.tileCollide = true;
            projectile.friendly = true;
            projectile.penetrate = 3;
            projectile.hostile = false;
            projectile.ranged = true;
            projectile.timeLeft = 45;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = -1;
        }

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[this.projectile.type] = 8;
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

        int CollideCounter = 0;
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (projectile.velocity.X != oldVelocity.X)
            {
                projectile.position.X = projectile.position.X + projectile.velocity.X;
                projectile.velocity.X = -oldVelocity.X;
            }
            if (projectile.velocity.Y != oldVelocity.Y)
            {
                projectile.position.Y = projectile.position.Y + projectile.velocity.Y;
                projectile.velocity.Y = -oldVelocity.Y;
            }
            //Main.PlaySound(SoundID.Run, projectile.Center);
            CollideCounter += 1;
            if (CollideCounter >= 5)
            {
                projectile.timeLeft = 0;
            }
            return false;
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(SoundID.Item42, projectile.Center);
            float speed = 15f;
            Vector2 velocity = projectile.DirectionTo(Main.MouseWorld) * speed;
            int proj = Projectile.NewProjectile(projectile.Center, velocity, (int)projectile.ai[0], projectile.damage, projectile.knockBack / 2, projectile.owner);
            Main.projectile[proj].noDropItem = true;
            proj = Projectile.NewProjectile(projectile.Center, velocity.RotatedBy(MathHelper.ToRadians(30)), (int)projectile.ai[0], projectile.damage, projectile.knockBack / 2, projectile.owner);
            Main.projectile[proj].noDropItem = true;
            proj = Projectile.NewProjectile(projectile.Center, velocity.RotatedBy(MathHelper.ToRadians(-30)), (int)projectile.ai[0], projectile.damage, projectile.knockBack / 2, projectile.owner);
            Main.projectile[proj].noDropItem = true;
            projectile.active = false;
        }

        int Direction = 1;
        int DirectionCounter = 5;
        public override void AI()
        {
            projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
        }
    }
}
