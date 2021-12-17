using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TerrorbornMod.TBUtils;
using System.Collections.Generic;

namespace TerrorbornMod.Items.Weapons.Magic
{
    class Asphodel : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.staff[item.type] = true;
            Tooltip.SetDefault("Creates a flower at your cursor that explodes into numerous seeds");
        }

        public override void SetDefaults()
        {
            item.damage = 25;
            item.noMelee = true;
            item.width = 52;
            item.height = 52;
            item.useTime = 15;
            item.useAnimation = 15;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.knockBack = 2.5f;
            item.value = Item.sellPrice(0, 1, 0, 0);
            item.rare = ItemRarityID.LightRed;
            item.UseSound = SoundID.Item73;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("IncendiaryFlower");
            item.shootSpeed = 5f;
            item.mana = 6;
            item.magic = true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            position = Main.MouseWorld;
            speedX = 0;
            speedY = 0;
            return true;
        }
    }

    class IncendiaryFlower : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 46;
            projectile.height = 46;
            projectile.aiStyle = 0;
            projectile.tileCollide = false;
            projectile.friendly = false;
            projectile.alpha = 255;
            projectile.penetrate = -1;
            projectile.hostile = false;
            projectile.magic = true;
            projectile.ignoreWater = true;
            projectile.timeLeft = 20;
        }

        public override void AI()
        {
            projectile.rotation += MathHelper.ToRadians(10);
            //projectile.scale += 0.5f / 45f;
            
            if (projectile.alpha > (int)(255 * 0.25f))
            {
                projectile.alpha -= 15;
            }
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(SoundID.DD2_FlameburstTowerShot, projectile.Center);
            for (int i = 0; i < Main.rand.Next(4, 7); i++)
            {
                Vector2 direction = MathHelper.ToRadians(Main.rand.Next(360)).ToRotationVector2();
                float speed = Main.rand.Next(10, 15);
                int proj = Projectile.NewProjectile(projectile.Center, direction * speed, ModContent.ProjectileType<IncendiarySeed>(), projectile.damage, projectile.knockBack, projectile.owner);
            }
            DustExplosion(projectile.Center, 10, 25f, 46f);
        }

        public void DustExplosion(Vector2 position, int dustAmount, float minDistance, float maxDistance)
        {
            Vector2 direction = new Vector2(0, 1);
            for (int i = 0; i < dustAmount; i++)
            {
                Vector2 newPos = position + direction.RotatedBy(MathHelper.ToRadians((360f / dustAmount) * i)) * Main.rand.NextFloat(minDistance, maxDistance);
                Dust dust = Dust.NewDustPerfect(newPos, 127);
                dust.scale = 1f;
                dust.velocity = (newPos - position) / 5;
                dust.noGravity = true;
            }
        }
    }

    class IncendiarySeed : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[this.projectile.type] = 8;
            ProjectileID.Sets.TrailingMode[this.projectile.type] = 1;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            //Thanks to Seraph for afterimage code.
            //Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
            //for (int i = 0; i < projectile.oldPos.Length; i++)
            //{
            //    Vector2 drawPos = projectile.oldPos[i] - Main.screenPosition + projectile.Size / 2;
            //    Color color = projectile.GetAlpha(new Color(247, 84, 37)) * ((float)(projectile.oldPos.Length - i) / (float)projectile.oldPos.Length);
            //    Graphics.DrawGlow_1(spriteBatch, drawPos, (int)(25f * ((float)(projectile.oldPos.Length - i) / (float)projectile.oldPos.Length)), color * 0.5f);
            //}
            BezierCurve bezier = new BezierCurve();
            bezier.Controls.Clear();
            foreach (Vector2 pos in projectile.oldPos)
            {
                if (pos != Vector2.Zero && pos != null)
                {
                    bezier.Controls.Add(pos);
                }
            }

            if (bezier.Controls.Count > 1)
            {
                List<Vector2> positions = bezier.GetPoints(15);
                for (int i = 0; i < positions.Count; i++)
                {
                    Vector2 drawPos = positions[i] - Main.screenPosition + projectile.Size / 2;
                    Color color = projectile.GetAlpha(new Color(247, 84, 37)) * ((float)(positions.Count - i) / (float)positions.Count);
                    Graphics.DrawGlow_1(spriteBatch, drawPos, (int)(25f * ((float)(positions.Count - i) / (float)positions.Count)), color * 0.5f);
                }
            }
            return true;
        }

        public override void SetDefaults()
        {
            projectile.width = 28;
            projectile.height = 18;
            projectile.aiStyle = 0;
            projectile.tileCollide = true;
            projectile.friendly = true;
            projectile.magic = true;
            projectile.penetrate = -1;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = -1;
            projectile.hostile = false;
            projectile.timeLeft = 110;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = 15;
            height = 15;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough);
        }

        public override void Kill(int timeLeft)
        {
            DustExplosion(projectile.Center, 10, 5f, 10f);
        }

        public void DustExplosion(Vector2 position, int dustAmount, float minDistance, float maxDistance)
        {
            Vector2 direction = new Vector2(0, 1);
            for (int i = 0; i < dustAmount; i++)
            {
                Vector2 newPos = position + direction.RotatedBy(MathHelper.ToRadians((360f / dustAmount) * i)) * Main.rand.NextFloat(minDistance, maxDistance);
                Dust dust = Dust.NewDustPerfect(newPos, 127);
                dust.scale = 1f;
                dust.velocity = (newPos - position) / 10;
                dust.noGravity = true;
            }
        }

        public override void AI()
        {
            projectile.velocity.Y += 0.2f;
            //Dust dust = Dust.NewDustPerfect(projectile.Center, DustID.Fire);
            projectile.rotation = projectile.velocity.ToRotation();
        }
    }
}
