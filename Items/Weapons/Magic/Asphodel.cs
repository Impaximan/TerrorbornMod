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
            Item.staff[Item.type] = true;
            Tooltip.SetDefault("Creates a flower at your cursor that explodes into numerous seeds");
        }

        public override void SetDefaults()
        {
            TerrorbornItem modItem = TerrorbornItem.modItem(Item);
            modItem.critDamageMult = 1.33f;
            Item.damage = 11;
            Item.noMelee = true;
            Item.width = 52;
            Item.height = 52;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 2.5f;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.LightRed;
            Item.UseSound = SoundID.Item73;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<IncendiaryFlower>();
            Item.shootSpeed = 5f;
            Item.mana = 10;
            Item.DamageType = DamageClass.Magic;;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            position = Main.MouseWorld;
            velocity.X = 0;
            velocity.Y = 0;
        }
    }

    class IncendiaryFlower : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 46;
            Projectile.height = 46;
            Projectile.aiStyle = 0;
            Projectile.tileCollide = false;
            Projectile.friendly = false;
            Projectile.alpha = 255;
            Projectile.penetrate = -1;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Magic;;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 20;
        }

        public override void AI()
        {
            Projectile.rotation += MathHelper.ToRadians(10);
            //Projectile.scale += 0.5f / 45f;
            
            if (Projectile.alpha > (int)(255 * 0.25f))
            {
                Projectile.alpha -= 15;
            }
        }

        public override void Kill(int timeLeft)
        {
            Terraria.Audio.SoundEngine.PlaySound(SoundID.DD2_FlameburstTowerShot, Projectile.Center);
            for (int i = 0; i < Main.rand.Next(4, 7); i++)
            {
                Vector2 direction = MathHelper.ToRadians(Main.rand.Next(360)).ToRotationVector2();
                float speed = Main.rand.Next(10, 15);
                int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, direction * speed, ModContent.ProjectileType<IncendiarySeed>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
            }
            DustExplosion(Projectile.Center, 10, 25f, 46f);
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
            ProjectileID.Sets.TrailCacheLength[this.Projectile.type] = 8;
            ProjectileID.Sets.TrailingMode[this.Projectile.type] = 1;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);

            BezierCurve bezier = new BezierCurve();
            bezier.Controls.Clear();
            foreach (Vector2 pos in Projectile.oldPos)
            {
                if (pos != Vector2.Zero && pos != null)
                {
                    bezier.Controls.Add(pos);
                }
            }

            if (bezier.Controls.Count > 1)
            {
                List<Vector2> positions = bezier.GetPoints(25);
                for (int i = 0; i < positions.Count; i++)
                {
                    Vector2 drawPos = positions[i] - Main.screenPosition + Projectile.Size / 2;
                    Color color = Projectile.GetAlpha(new Color(247, 84, 37)) * ((float)(positions.Count - i) / (float)positions.Count);
                    Graphics.DrawGlow_1(Main.spriteBatch, drawPos, (int)(35f * ((float)(positions.Count - i) / (float)positions.Count)), color * 0.5f);
                }
            }

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);
            return true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 28;
            Projectile.height = 18;
            Projectile.aiStyle = 0;
            Projectile.tileCollide = true;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.hostile = false;
            Projectile.timeLeft = 110;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 15;
            height = 15;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }

        public override void Kill(int timeLeft)
        {
            DustExplosion(Projectile.Center, 10, 5f, 10f);
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
            Projectile.velocity.Y += 0.2f;
            //Dust dust = Dust.NewDustPerfect(Projectile.Center, 6);
            Projectile.rotation = Projectile.velocity.ToRotation();
        }
    }
}
