using Terraria.ModLoader;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;

namespace TerrorbornMod.Items.Weapons.Melee
{
    class ShellSpear : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Rapidly hits enemies");
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Materials.ShellFragments>(), 8)
                .AddRecipeGroup(RecipeGroupID.IronBar, 4)
                .AddTile(TileID.Anvils)
                .Register();
        }

        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.width = 48;
            Item.height = 50;
            Item.damage = 8;
            Item.DamageType = DamageClass.Melee;
            Item.noMelee = true;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.shootSpeed = 6;
            Item.knockBack = 1;
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item1;
            Item.noUseGraphic = true;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<ShellSpearProjectile>();
            Item.crit = 10;
        }

        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[Item.shoot] < 1;
        }
    }

    class ShellSpearProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.aiStyle = 19;
            Projectile.penetrate = -1;
            Projectile.scale = 1.3f;
            Projectile.alpha = 0;
            Projectile.hide = true;
            Projectile.ownerHitCheck = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 5;
        }

        public float movementFactor
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        public override void AI()
        {
            Player projOwner = Main.player[Projectile.owner];
            Vector2 ownerMountedCenter = projOwner.RotatedRelativePoint(projOwner.MountedCenter, true);
            Projectile.direction = projOwner.direction;
            projOwner.heldProj = Projectile.whoAmI;
            projOwner.itemTime = projOwner.itemAnimation;
            Projectile.position.X = ownerMountedCenter.X - (float)(Projectile.width / 2);
            Projectile.position.Y = ownerMountedCenter.Y - (float)(Projectile.height / 2);
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
        }
    }
}

