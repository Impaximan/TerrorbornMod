using Terraria.ModLoader;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TerrorbornMod.Items.Weapons.Melee
{
    class PrideRipper : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Prideripper");
            Tooltip.SetDefault("Rips through reality, creating many fragments of light to attack your foes");
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Items.Materials.DreadfulEssence>(), 3);
            recipe.AddIngredient(ItemID.LunarBar, 10);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void SetDefaults()
        {
            TerrorbornItem.modItem(item).critDamageMult = 1.2f;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.width = 104;
            item.height = 108;
            item.damage = 150;
            item.melee = true;
            item.noMelee = true;
            item.useTime = 48;
            item.useAnimation = 24;
            item.value = Item.sellPrice(0, 25, 0, 0);
            item.shootSpeed = 5f;
            item.knockBack = 4;
            item.rare = 12;
            item.UseSound = SoundID.Item71;
            item.noUseGraphic = true;
            item.autoReuse = true;
            item.shoot = ModContent.ProjectileType<PrideRipperProjectile>();
        }
        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[item.shoot] < 1;
        }
    }

    class PrideRipperProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 5;
            projectile.width = 104;
            projectile.height = 108;
            projectile.aiStyle = 19;
            projectile.penetrate = -1;
            projectile.scale = 1f;
            projectile.alpha = 0;
            projectile.hide = true;
            projectile.ownerHitCheck = true;
            projectile.melee = true;
            projectile.tileCollide = false;
            projectile.friendly = true;
        }

        public float movementFactor
        {
            get => projectile.ai[0];
            set => projectile.ai[0] = value;
        }

        int projectileCounter = 0;
        public override void AI()
        {
            projectile.velocity = projectile.velocity.ToRotation().AngleTowards(projectile.DirectionTo(Main.MouseWorld).ToRotation(), MathHelper.ToRadians(3f)).ToRotationVector2() * projectile.velocity.Length();

            Player projOwner = Main.player[projectile.owner];
            Vector2 ownerMountedCenter = projOwner.RotatedRelativePoint(projOwner.MountedCenter, true);
            projectile.direction = projOwner.direction;
            projOwner.heldProj = projectile.whoAmI;
            projOwner.itemTime = projOwner.itemAnimation;
            projectile.position.X = ownerMountedCenter.X - (float)(projectile.width / 2);
            projectile.position.Y = ownerMountedCenter.Y - (float)(projectile.height / 2);
            if (!projOwner.frozen)
            {
                if (movementFactor == 0f)
                {
                    movementFactor = 3f;
                    projectile.netUpdate = true;
                }
                if (projOwner.itemAnimation < projOwner.itemAnimationMax / 3)
                {
                    movementFactor -= 2.4f;
                }
                else
                {
                    movementFactor += 2.1f;
                }
            }
            projectile.position += projectile.velocity * movementFactor;
            if (projOwner.itemAnimation == 0)
            {
                projectile.Kill();
            }
            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(135f);
            if (projOwner.direction == 1)
            {
                projectile.spriteDirection = -1;
            }
            if (projectile.spriteDirection == -1)
            {
                projectile.rotation -= MathHelper.ToRadians(90f);
            }

            projectileCounter++;
            if (projectileCounter > 2)
            {
                projectileCounter = 0;
                Vector2 velocity = projectile.velocity.ToRotation().ToRotationVector2() * movementFactor;
                Projectile.NewProjectile(projectile.Center, velocity, ModContent.ProjectileType<DreadLight>(), projectile.damage / 2, projectile.knockBack / 2, projectile.owner);
            }
        }
    }

    class DreadLight : ModProjectile
    {
        public override string Texture => "TerrorbornMod/Items/Weapons/Magic/TarSwarm";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[this.projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[this.projectile.type] = 1;
        }

        int trueTimeLeft = 360;
        public override void SetDefaults()
        {
            projectile.penetrate = -1;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 20;
            projectile.width = 35;
            projectile.height = 35;
            projectile.tileCollide = true;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.melee = true;
            projectile.ignoreWater = true;
            projectile.timeLeft = 1000;
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
                List<Vector2> positions = bezier.GetPoints(30);
                for (int i = 0; i < positions.Count; i++)
                {
                    float mult = (float)(positions.Count - i) / (float)positions.Count;
                    Vector2 drawPos = positions[i] - Main.screenPosition + projectile.Size / 2;
                    Color color = projectile.GetAlpha(Color.Lerp(Color.Goldenrod, Color.LightGoldenrodYellow, mult)) * mult;
                    TBUtils.Graphics.DrawGlow_1(spriteBatch, drawPos, (int)(35f * mult), color);
                }
            }

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);
            return false;
        }

        public override void AI()
        {
            projectile.velocity = projectile.velocity.ToRotation().AngleTowards(projectile.DirectionTo(Main.MouseWorld).ToRotation(), MathHelper.ToRadians(1f)).ToRotationVector2() * projectile.velocity.Length();
            trueTimeLeft--;
            if (trueTimeLeft <= 0)
            {
                projectile.alpha += 15;
                if (projectile.alpha >= 255)
                {
                    projectile.active = false;
                }
            }
        }
    }
}