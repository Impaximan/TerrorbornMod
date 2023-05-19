using Terraria.ModLoader;
using System.Collections.Generic;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TerrorbornMod.Items.Weapons.Melee.Polearms
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
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Materials.DreadfulEssence>(), 3)
                .AddIngredient(ItemID.LunarBar, 10)
                .AddIngredient(ItemID.NorthPole)
                .AddIngredient(ItemID.FragmentSolar, 12)
                .AddTile(TileID.LunarCraftingStation)
                .Register();

        }

        public override void SetDefaults()
        {
            TerrorbornItem.modItem(Item).critDamageMult = 1.2f;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.width = 104;
            Item.height = 108;
            Item.damage = 150;
            Item.DamageType = DamageClass.Melee;
            Item.noMelee = true;
            Item.useTime = 48;
            Item.useAnimation = 24;
            Item.value = Item.sellPrice(0, 25, 0, 0);
            Item.shootSpeed = 5f;
            Item.knockBack = 4;
            Item.rare = ModContent.RarityType<Rarities.Golden>();
            Item.UseSound = SoundID.Item71;
            Item.noUseGraphic = true;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<PrideRipperProjectile>();
        }
        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[Item.shoot] < 1;
        }
    }

    class PrideRipperProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 5;
            Projectile.width = 104;
            Projectile.height = 108;
            Projectile.aiStyle = 19;
            Projectile.penetrate = -1;
            Projectile.scale = 1f;
            Projectile.alpha = 0;
            Projectile.hide = true;
            Projectile.ownerHitCheck = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
        }

        public float movementFactor
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        int ProjectileCounter = 0;
        public override void AI()
        {
            Projectile.velocity = Projectile.velocity.ToRotation().AngleTowards(Projectile.DirectionTo(Main.MouseWorld).ToRotation(), MathHelper.ToRadians(3f)).ToRotationVector2() * Projectile.velocity.Length();

            Player projOwner = Main.player[Projectile.owner];
            Vector2 ownerMountedCenter = projOwner.RotatedRelativePoint(projOwner.MountedCenter, true);
            Projectile.direction = projOwner.direction;
            projOwner.heldProj = Projectile.whoAmI;
            projOwner.itemTime = projOwner.itemAnimation;
            Projectile.position.X = ownerMountedCenter.X - Projectile.width / 2;
            Projectile.position.Y = ownerMountedCenter.Y - Projectile.height / 2;
            if (!projOwner.frozen)
            {
                if (movementFactor == 0f)
                {
                    movementFactor = 3f;
                    Projectile.netUpdate = true;
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
            Projectile.position += Projectile.velocity * movementFactor;
            if (projOwner.itemAnimation == 0)
            {
                Projectile.Kill();
            }
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(135f);
            if (projOwner.direction == 1)
            {
                Projectile.spriteDirection = -1;
            }
            if (Projectile.spriteDirection == -1)
            {
                Projectile.rotation -= MathHelper.ToRadians(90f);
            }

            ProjectileCounter++;
            if (ProjectileCounter > 2)
            {
                ProjectileCounter = 0;
                Vector2 velocity = Projectile.velocity.ToRotation().ToRotationVector2() * movementFactor;
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, velocity, ModContent.ProjectileType<DreadLight>(), Projectile.damage / 3, Projectile.knockBack / 2, Projectile.owner);
            }
        }
    }

    class DreadLight : ModProjectile
    {
        public override string Texture => "TerrorbornMod/Items/Weapons/Magic/MagicGuns/TarSwarm";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }

        int trueTimeLeft = 360;
        public override void SetDefaults()
        {
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
            Projectile.width = 35;
            Projectile.height = 35;
            Projectile.tileCollide = true;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 1000;
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
                List<Vector2> positions = bezier.GetPoints(30);
                for (int i = 0; i < positions.Count; i++)
                {
                    float mult = (positions.Count - i) / (float)positions.Count;
                    Vector2 drawPos = positions[i] - Main.screenPosition + Projectile.Size / 2;
                    Color color = Projectile.GetAlpha(Color.Lerp(Color.Goldenrod, Color.LightGoldenrodYellow, mult)) * mult;
                    Utils.Graphics.DrawGlow_1(Main.spriteBatch, drawPos, (int)(35f * mult), color);
                }
            }

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);
            return false;
        }

        public override void AI()
        {
            Projectile.velocity = Projectile.velocity.ToRotation().AngleTowards(Projectile.DirectionTo(Main.MouseWorld).ToRotation(), MathHelper.ToRadians(1f)).ToRotationVector2() * Projectile.velocity.Length();
            trueTimeLeft--;
            if (trueTimeLeft <= 0)
            {
                Projectile.alpha += 15;
                if (Projectile.alpha >= 255)
                {
                    Projectile.active = false;
                }
            }
        }
    }
}