using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using Terraria.DataStructures;

namespace TerrorbornMod.Items.Weapons.Magic.SpellBooks
{
    class SoulSpiral : ModItem
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Materials.ThunderShard>(), 18)
                .AddIngredient(ModContent.ItemType<Materials.NoxiousScale>(), 12)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }

        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("Creates a spiral of souls around you");
        }

        public override void SetDefaults()
        {
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.width = 32;
            Item.height = 38;
            Item.DamageType = DamageClass.Magic; ;
            Item.damage = 70;
            Item.useTime = 32;
            Item.useAnimation = 32;
            Item.mana = 8;
            Item.rare = ItemRarityID.Pink;
            Item.shoot = ModContent.ProjectileType<SpiralSoul>();
            Item.shootSpeed = 35;
            Item.UseSound = SoundID.Item20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 0.1f;
            Item.autoReuse = true;
        }

        int rotationDirection = 1;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int soulAmount = 9;
            rotationDirection *= -1;

            for (int i = 0; i < soulAmount; i++)
            {
                velocity = velocity.RotatedBy(MathHelper.ToRadians(i * (360 / soulAmount)));
                int proj = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
                Main.projectile[proj].ai[0] = rotationDirection;
            }
            return false;
        }
    }

    class SpiralSoul : ModProjectile
    {
        public override string Texture => "TerrorbornMod/placeholder";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
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
                    float mult = (positions.Count - i) / (float)positions.Count;
                    Vector2 drawPos = positions[i] - Main.screenPosition + Projectile.Size / 2;
                    Color color = Projectile.GetAlpha(Color.Lerp(Color.MediumPurple, Color.LightPink, mult)) * mult;
                    Utils.Graphics.DrawGlow_1(Main.spriteBatch, drawPos, (int)(25f * mult), color);
                }
            }

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);
            return false;
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 30;
            Projectile.aiStyle = 0;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.penetrate = 25;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Magic; ;
            Projectile.ignoreWater = true;
            Projectile.hide = false;
            Projectile.timeLeft = (int)(360 / rotationSpeed);
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 10;
        }

        bool start = true;
        float rotationSpeed = 10;
        public override void AI()
        {
            if (start)
            {
                start = false;
            }

            Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(rotationSpeed) * Projectile.ai[0]);

            Dust dust = Dust.NewDustPerfect(Projectile.Center, 21);
            dust.noGravity = true;
            dust.velocity = Vector2.Zero;
        }
    }
}

