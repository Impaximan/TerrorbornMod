using Terraria.ModLoader;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;

namespace TerrorbornMod.Items.Weapons.Melee.Polearms
{
    class AzuriteGlaive : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Azurite Glaive");
            // Tooltip.SetDefault("Fires a piercing azure beam");
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<Materials.AzuriteBar>(10)
                .AddTile(TileID.Anvils)
                .Register();
        }
        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.width = 48;
            Item.height = 50;
            Item.damage = 28;
            Item.DamageType = DamageClass.Melee;
            Item.noMelee = true;
            Item.useTime = 24;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.useAnimation = 18;
            Item.shootSpeed = 4;
            Item.knockBack = 4;
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item1;
            Item.noUseGraphic = true;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<AzuriteGlaiveProjectile>();
        }
        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[Item.shoot] < 1;
        }
    }
    class AzuriteGlaiveProjectile : ModProjectile
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
        }
        public float movementFactor
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }
        bool HasFiredBolt = false;
        public override void AI()
        {

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
                    if (!HasFiredBolt)
                    {
                        float Speed = 27;
                        Vector2 ProjectileVelocity = (Projectile.rotation - MathHelper.ToRadians(135f)).ToRotationVector2() * Speed;
                        if (Projectile.spriteDirection == -1)
                        {
                            ProjectileVelocity = (Projectile.rotation - MathHelper.ToRadians(45f)).ToRotationVector2() * Speed;
                        }
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center + ProjectileVelocity / Speed * 100, ProjectileVelocity, ModContent.ProjectileType<AquaRay>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                        HasFiredBolt = true;
                        SoundExtensions.PlaySoundOld(SoundID.Item60, Projectile.Center);
                    }
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
    class AquaRay : ModProjectile
    {
        public override string Texture => "TerrorbornMod/placeholder";
        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.tileCollide = true;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.extraUpdates = 100;
            Projectile.timeLeft = 1000;
            Projectile.penetrate = 2;
            Projectile.hide = true;
        }
        public override void AI()
        {
            Projectile.velocity.Y += 0.2f;
            for (int i = 0; i < 4; i++)
            {
                int dust = Dust.NewDust(Projectile.position - Projectile.velocity * i / 4, 1, 1, 88, 0, 0, Scale: 2, newColor: Color.SkyBlue);
                Main.dust[dust].noGravity = true;
            }
        }
    }
}
