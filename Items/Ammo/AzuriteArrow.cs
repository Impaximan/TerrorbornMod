using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Ammo
{
    class AzuriteArrow : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 11;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 14;
            Item.height = 32;
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.knockBack = 2;
            Item.value = Item.sellPrice(0, 0, 0, 20);
            Item.shootSpeed = 3;
            Item.rare = ItemRarityID.Green;
            Item.shoot = ModContent.ProjectileType<AzuriteArrowProjectile>();
            Item.ammo = AmmoID.Arrow;
        }

        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("Creates an azure orb that returns to you after a moment");
        }

        public override void AddRecipes()
        {
            CreateRecipe(111)
                .AddIngredient<Materials.AzuriteBar>()
                .AddIngredient(ItemID.WoodenArrow, 111)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }

    class AzuriteArrowProjectile : ModProjectile
    {
        public override string Texture => "TerrorbornMod/Items/Ammo/AzuriteArrow";
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 32;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 3600;
            Projectile.penetrate = 3;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.tileCollide = true;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.arrow = true;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 14;
            height = 14;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }

        public override void Kill(int timeLeft)
        {
            if (timeUntilOrb > 0)
            {
                DustExplosion(Projectile.Center, 10, 5f, 10f);
                SoundExtensions.PlaySoundOld(SoundID.Item92, Projectile.Center);
                Projectile.velocity /= 2;
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<AzureOrb>(), Projectile.damage / 4, Projectile.knockBack / 10f, Projectile.owner);
            }
            SoundExtensions.PlaySoundOld(SoundID.Dig, Projectile.position);
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            hitbox.Width = 14;
            hitbox.Height = 14;
            hitbox.Y += (int)(32f * (14f / 32f) / 2f);
            base.ModifyDamageHitbox(ref hitbox);
        }

        public void DustExplosion(Vector2 position, int dustAmount, float minDistance, float maxDistance)
        {
            Vector2 direction = new Vector2(0, 1);
            for (int i = 0; i < dustAmount; i++)
            {
                Vector2 newPos = position + direction.RotatedBy(MathHelper.ToRadians((360f / dustAmount) * i)) * Main.rand.NextFloat(minDistance, maxDistance);
                Dust dust = Dust.NewDustPerfect(newPos, 88);
                dust.scale = 1f;
                dust.velocity = (newPos - position) / 10;
                dust.noGravity = true;
            }
        }

        int timeUntilOrb = 45;
        bool start = true;
        public override void AI()
        {
            if (start)
            {
                start = false;
                Projectile.velocity *= 1.5f;
            }

            if (timeUntilOrb > 0)
            {
                timeUntilOrb--;
                if (timeUntilOrb <= 0)
                {
                    DustExplosion(Projectile.Center, 10, 5f, 10f);
                    SoundExtensions.PlaySoundOld(SoundID.Item92, Projectile.Center);
                    Projectile.velocity /= 2;
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<AzureOrb>(), Projectile.damage / 4, Projectile.knockBack / 10f, Projectile.owner);
                }
            }

            Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.ToRadians(90);
        }
    }

    class AzureOrb : ModProjectile
    {
        public override string Texture => "TerrorbornMod/placeholder";

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.aiStyle = 0;
            Projectile.tileCollide = true;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 600;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[this.Projectile.type] = 7;
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
                List<Vector2> positions = bezier.GetPoints(50);
                for (int i = 0; i < positions.Count; i++)
                {
                    float mult = (float)(positions.Count - i) / (float)positions.Count;
                    Vector2 drawPos = positions[i] - Main.screenPosition + Projectile.Size / 2;
                    Color color = Projectile.GetAlpha(Color.Lerp(Color.Azure, Color.Azure, mult)) * mult;
                    Utils.Graphics.DrawGlow_1(Main.spriteBatch, drawPos, (int)(15f * mult), color);
                }
            }

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);
            return false;
        }


        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            int speed = 12;
            Projectile.velocity = Projectile.DirectionTo(player.Center) * speed;
            if (Projectile.Distance(player.Center) <= speed)
            {
                Projectile.active = false;
            }
        }
    }
}