using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace TerrorbornMod.Items.Weapons.Melee.Swords
{
    public class DeimosteelBroadsword : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("Creates a short ranged piercing wave when swung");
        }

        public override void SetDefaults()
        {
            Item.damage = 18;
            Item.DamageType = DamageClass.Melee;
            Item.width = 36;
            Item.height = 36;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 3f;
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = false;
            Item.shootSpeed = 10f;
            Item.shoot = ModContent.ProjectileType<DeimoWave>();
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Materials.DeimosteelBar>(), 7)
                .AddTile(ModContent.TileType<Tiles.MeldingStation>())
                .Register();
        }
    }

    public class DeimoWave : ModProjectile
    {
        public override string Texture => "TerrorbornMod/Items/Weapons/Magic/MagicGuns/ShriekWave";
        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 14;
            Projectile.aiStyle = 0;
            Projectile.tileCollide = true;
            Projectile.friendly = true;
            Projectile.penetrate = 3;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 300;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90);
            Projectile.velocity *= 0.95f;
            Projectile.alpha += 255 / 30;
            if (Projectile.alpha >= 255)
            {
                Projectile.active = false;
            }
        }
    }
}

