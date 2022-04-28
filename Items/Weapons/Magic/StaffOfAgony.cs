using Microsoft.Xna.Framework;
using Terraria;
using TerrorbornMod.Projectiles;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Weapons.Magic
{
    class StaffOfAgony : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Rains deathrays from the sky");
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 56;
            Item.noMelee = true;
            Item.width = 48;
            Item.height = 48;
            Item.useTime = 5;
            Item.useAnimation = 5;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.crit = 14;
            Item.knockBack = 2;
            Item.value = Item.sellPrice(0, 10, 0, 0);
            Item.rare = ItemRarityID.Yellow;
            Item.UseSound = SoundID.Item12;
            Item.shootSpeed = 1f;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<AgonyLazer>();
            Item.mana = 3;
            Item.DamageType = DamageClass.Magic;;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, 0);
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            position = new Vector2(Main.MouseWorld.X + Main.rand.Next(-200, 200), player.Center.Y - 1000);
            Vector2 shootSpeed = Main.MouseWorld - position;
            shootSpeed.Normalize();
            velocity.X = shootSpeed.X;
            velocity.Y = shootSpeed.Y;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Materials.SkullmoundBar>(), 12)
                .AddIngredient(ModContent.ItemType<Materials.SoulOfPlight>(), 10)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }

    class AgonyLazer : Deathray
    {
        int timeLeft = 15;
        Vector2 offsetFromPlayer;
        public override string Texture => "TerrorbornMod/Projectiles/IncendiaryDeathray";
        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 22;
            Projectile.penetrate = -1;
            Projectile.tileCollide = true;
            Projectile.hide = false;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;;
            Projectile.timeLeft = timeLeft;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            MoveDistance = 20f;
            RealMaxDistance = 2000f;
            offsetFromPlayer = Projectile.position - Main.player[Projectile.owner].Center;
            bodyRect = new Rectangle(0, 24, 18, 22);
            headRect = new Rectangle(0, 0, 18, 22);
            tailRect = new Rectangle(0, 46, 18, 22);
        }

        public override Vector2 Position()
        {
            return Projectile.position;
        }

        public override void PostAI()
        {
            deathrayWidth -= 1f / (float)timeLeft;
        }
    }
}

