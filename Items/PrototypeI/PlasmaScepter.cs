using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;
using System.Collections.Generic;

namespace TerrorbornMod.Items.PrototypeI
{
    class PlasmaScepter : ModItem
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Items.Materials.PlasmaliumBar>(), 12)
                .AddIngredient(ModContent.ItemType<Items.Materials.ThunderShard>(), 5)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }

        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = true;
            DisplayName.SetDefault("Scepter of Contamination");
            Tooltip.SetDefault("Fires a stream of dark plasma that homes into your cursor");
        }
        public override void SetDefaults()
        {
            Item.damage = 35;
            Item.noMelee = true;
            Item.width = 54;
            Item.height = 54;
            Item.useTime = 8;
            Item.useAnimation = 8;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 5;
            Item.value = Item.sellPrice(0, 16, 0, 0);
            Item.rare = ItemRarityID.Cyan;
            Item.UseSound = SoundID.Item13;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<PlasmaSpray>();
            Item.shootSpeed = 55f / 5f;
            Item.mana = 3;
            Item.DamageType = DamageClass.Magic;;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-20, 0);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            position = player.Center + (player.DirectionTo(Main.MouseWorld) * 74);
            Vector2 directionVector = player.DirectionTo(Main.MouseWorld);
            Projectile.NewProjectile(source, position, directionVector * Main.rand.Next(8, 25), type, damage, knockback, player.whoAmI);
            return false;
        }
    }
    class PlasmaSpray : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[this.Projectile.type] = 20;
            ProjectileID.Sets.TrailingMode[this.Projectile.type] = 1;
        }

        public override string Texture => "TerrorbornMod/Items/Weapons/Magic/TarSwarm";
        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.aiStyle = 0;
            Projectile.tileCollide = true;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 15;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Magic;;
            Projectile.ignoreWater = true;
            Projectile.hide = false;
            Projectile.timeLeft = 100 * 4;
            Projectile.extraUpdates = 1;
        }

        int dustWait = 0;
        public override void AI()
        {
            //int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 74, Scale: 1.35f);
            //Main.dust[dust].noGravity = true;
            //Main.dust[dust].velocity = Projectile.velocity / 3;

            Projectile.velocity = Projectile.velocity.ToRotation().AngleTowards(Projectile.DirectionTo(Main.MouseWorld).ToRotation(), MathHelper.ToRadians(3.5f * (Projectile.velocity.Length() / 20))).ToRotationVector2() * Projectile.velocity.Length();
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            damage += target.defense / 4;
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
                    Color color = Projectile.GetAlpha(Color.Lerp(Color.Lime, Color.LimeGreen, mult)) * mult;
                    Utils.Graphics.DrawGlow_1(Main.spriteBatch, drawPos, (int)(25f * mult), color);
                }
            }

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);
            return false;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.velocity.X != oldVelocity.X)
            {
                Projectile.position.X = Projectile.position.X + Projectile.velocity.X;
                Projectile.velocity.X = -oldVelocity.X;
            }
            if (Projectile.velocity.Y != oldVelocity.Y)
            {
                Projectile.position.Y = Projectile.position.Y + Projectile.velocity.Y;
                Projectile.velocity.Y = -oldVelocity.Y * 0.9f;
            }
            return false;
        }
    }
}
