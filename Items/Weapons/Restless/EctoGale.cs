using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;

namespace TerrorbornMod.Items.Weapons.Restless
{
    class EctoGale : RestlessWeapon
    {
        public override string Texture => "TerrorbornMod/Items/Weapons/Restless/EctoGale";
        int UntilBlast;
        public override void restlessSetStaticDefaults()
        {
            DisplayName.SetDefault("EctoBurst");
        }

        public override string defaultTooltip()
        {
            return "Converts arrows into ecto orbs";
        }

        public override string altTooltip()
        {
            return "Ecto orbs will bounce off walls and eventually explode into arrows";
        }

        public override void restlessSetDefaults(TerrorbornItem modItem)
        {
            item.damage = 120;
            item.ranged = true;
            item.noMelee = true;
            item.width = 38;
            item.height = 18;
            item.useTime = 25;
            item.useAnimation = 25;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.knockBack = 1;
            item.value = Item.sellPrice(0, 1, 0, 0);
            item.rare = ItemRarityID.Cyan;
            item.UseSound = SoundID.DD2_BallistaTowerShot;
            item.autoReuse = true;
            item.shootSpeed = 16f;
            item.shoot = ProjectileID.PurificationPowder;
            item.useAmmo = AmmoID.Arrow;
            modItem.restlessChargeUpUses = 4;
            modItem.restlessTerrorDrain = 6;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, 0);
        }
        public override bool RestlessShoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            TerrorbornItem modItem = TerrorbornItem.modItem(item);
            if (modItem.RestlessChargedUp())
            {
                int proj = Projectile.NewProjectile(position, new Vector2(speedX, speedY), ModContent.ProjectileType<EctoOrb>(), damage, knockBack, player.whoAmI);
                Main.projectile[proj].ai[0] = 1;
                Main.projectile[proj].ai[1] = type;
                Main.projectile[proj].penetrate = 1;
            }
            else
            {
                int proj = Projectile.NewProjectile(position, new Vector2(speedX, speedY), ModContent.ProjectileType<EctoOrb>(), damage, knockBack, player.whoAmI);
            }
            return false;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Shadowcrawler.Nightbrood>());
            recipe.AddIngredient(ItemID.Ectoplasm, 10);
            recipe.AddIngredient(ItemID.Obsidian, 25);
            recipe.AddIngredient(ModContent.ItemType<Items.Materials.TerrorSample>(), 5);
            recipe.AddTile(ModContent.TileType<Tiles.MeldingStation>());
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }

    class EctoOrb : ModProjectile
    {
        int timeUntilReturn = 30;
        int penetrateUntilReturn = 3;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[this.projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[this.projectile.type] = 1;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);

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
                List<Vector2> positions = bezier.GetPoints(50);
                for (int i = 0; i < positions.Count; i++)
                {
                    float mult = (float)(positions.Count - i) / (float)positions.Count;
                    Vector2 drawPos = positions[i] - Main.screenPosition + projectile.Size / 2;
                    Color color = projectile.GetAlpha(Color.Lerp(Color.DarkCyan, Color.LightCyan, mult)) * mult;
                    TBUtils.Graphics.DrawGlow_1(spriteBatch, drawPos, (int)(50f * mult), color);
                }
            }

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);
            return false;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = 15;
            height = 15;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (projectile.ai[0] == 1)
            {
                if (projectile.velocity.X != oldVelocity.X)
                {
                    projectile.position.X = projectile.position.X + projectile.velocity.X;
                    projectile.velocity.X = -oldVelocity.X * 0.5f;
                }
                if (projectile.velocity.Y != oldVelocity.Y)
                {
                    projectile.position.Y = projectile.position.Y + projectile.velocity.Y;
                    projectile.velocity.Y = -oldVelocity.Y * 0.5f;
                }
                return false;
            }
            return base.OnTileCollide(oldVelocity);
        }

        public override string Texture => "TerrorbornMod/Items/Weapons/Restless/AtomReaper"; 

        public override void SetDefaults()
        {
            projectile.width = 56;
            projectile.height = 56;
            projectile.ranged = true;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.tileCollide = true;
            projectile.ignoreWater = false;
            projectile.penetrate = -1;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 20;
            projectile.timeLeft = 180;
        }

        float speed;
        int projectileWait = 0;
        public override void AI()
        {
            Player player = Main.player[projectile.owner];
        }

        public override void Kill(int timeLeft)
        {
            if (projectile.ai[0] == 1)
            {
                for (int i = 0; i < Main.rand.Next(15, 22); i++)
                {
                    float speed = Main.rand.Next(15, 25);
                    Vector2 velocity = MathHelper.ToRadians(Main.rand.Next(360)).ToRotationVector2() * speed;
                    int proj = Projectile.NewProjectile(projectile.Center, velocity, (int)projectile.ai[1], projectile.damage / 2, 0f, projectile.owner);
                    Main.projectile[proj].noDropItem = true;
                }
            }
        }
    }
}
