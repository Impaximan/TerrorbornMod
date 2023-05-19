using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace TerrorbornMod.Items.Weapons.Magic.Staffs
{
    class GrimmScepter : ModItem
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.CrimtaneBar, 8)
                .AddIngredient(ItemID.TissueSample, 5)
                .AddIngredient(ModContent.ItemType<Materials.SanguineFang>(), 12)
                .AddTile(TileID.Anvils)
                .Register();
            CreateRecipe()
                .AddIngredient(ItemID.DemoniteBar, 8)
                .AddIngredient(ItemID.ShadowScale, 5)
                .AddIngredient(ModContent.ItemType<Materials.SanguineFang>(), 12)
                .AddTile(TileID.Anvils)
                .Register();
        }

        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = true;
            // Tooltip.SetDefault("On crits enemies will release collectable Grimm Orbs that heal you for 2 health");
        }

        public override void SetDefaults()
        {
            TerrorbornItem modItem = TerrorbornItem.modItem(Item);
            modItem.critDamageMult = 1.3f;
            Item.damage = 11;
            Item.noMelee = true;
            Item.width = 52;
            Item.height = 52;
            Item.useTime = 16;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.useAnimation = 16;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 5;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item72;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<GrimmRay>();
            Item.shootSpeed = 15f;
            Item.mana = 4;
            Item.DamageType = DamageClass.Magic; ;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            position = player.Center + player.DirectionTo(Main.MouseWorld) * 40;
            Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y), type, damage, knockback, player.whoAmI, 1);
            Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y), type, damage, knockback, player.whoAmI, -1);
            return false;
        }
    }

    class GrimmRay : ModProjectile
    {
        public override string Texture => "TerrorbornMod/placeholder";
        //private bool HasGravity = true;
        //private bool Spawn = true;
        //private bool GravDown = true;

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.aiStyle = 0;
            Projectile.tileCollide = true;
            Projectile.friendly = true;
            Projectile.penetrate = 5;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 15;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Magic; ;
            Projectile.timeLeft = 350;
        }

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
                    Color color = Projectile.GetAlpha(Color.Lerp(Color.Crimson, Color.Red, mult)) * mult;
                    Utils.Graphics.DrawGlow_1(Main.spriteBatch, drawPos, (int)(25f * mult), color);
                }
            }

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);
            return false;
        }

        int Direction = 1;
        int DirectionCounter = 5;
        public override void AI()
        {
            int RotatationSpeed = 4;
            Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(RotatationSpeed * Direction * Projectile.ai[0]));
            DirectionCounter--;
            if (DirectionCounter <= 0)
            {
                DirectionCounter = 10;
                Direction *= -1;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (hit.Crit)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), target.Center, Vector2.Zero, ModContent.ProjectileType<GrimmOrb>(), 0, 0, Projectile.owner);
            }
        }
    }
    class GrimmOrb : ModProjectile
    {
        public override string Texture => "TerrorbornMod/placeholder";
        //private bool HasGravity = true;
        //private bool Spawn = true;
        //private bool GravDown = true;
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.aiStyle = 0;
            Projectile.tileCollide = true;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Magic; ;
            Projectile.timeLeft = 300;
            Projectile.damage = 0;
        }

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
                    Color color = Projectile.GetAlpha(Color.Lerp(Color.Crimson, Color.Red, mult)) * mult;
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
            int speed = 8;
            Projectile.velocity = Projectile.DirectionTo(player.Center) * speed;
            if (Projectile.Distance(player.Center) <= speed)
            {
                int healAmount = 3;
                player.HealEffect(healAmount);
                player.statLife += healAmount;
                Projectile.active = false;
            }
        }
    }
}
