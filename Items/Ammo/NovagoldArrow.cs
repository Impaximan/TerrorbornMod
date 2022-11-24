using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TerrorbornMod.Projectiles;

namespace TerrorbornMod.Items.Ammo
{
    class NovagoldArrow : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 13;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 14;
            Item.height = 36;
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.knockBack = 2;
            Item.value = Item.sellPrice(0, 0, 0, 5);
            Item.shootSpeed = 15;
            Item.rare = 1;
            Item.shoot = ModContent.ProjectileType<NovagoldArrowProjectile>();
            Item.ammo = AmmoID.Arrow;
        }

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Travels at the speed of light");
        }

        public override void AddRecipes()
        {
            CreateRecipe(50)
                .AddIngredient<Materials.NovagoldBar>()
                .AddIngredient(ItemID.WoodenArrow, 50)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }

    class NovagoldArrowProjectile : Deathray
    {
        int timeLeft = 10;
        public override string Texture => "TerrorbornMod/Items/Weapons/Magic/LightBlast";
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.penetrate = -1;
            Projectile.tileCollide = true;
            Projectile.hide = false;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = timeLeft;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.arrow = true;
            MoveDistance = 20f;
            RealMaxDistance = 2000f;
            bodyRect = new Rectangle(0, 0, 10, 10);
            headRect = new Rectangle(0, 0, 10, 10);
            tailRect = new Rectangle(0, 0, 10, 10);
            FollowPosition = false;
            drawColor = Color.LightCyan;
        }

        public override void PostAI()
        {
            deathrayWidth -= 1f / (float)timeLeft;
            Projectile.velocity.Normalize();
        }
    }
}
