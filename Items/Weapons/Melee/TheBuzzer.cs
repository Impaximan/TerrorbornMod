using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Weapons.Melee
{
    class TheBuzzer : ModItem
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.BeeWax, 10)
                .AddTile(TileID.Anvils)
                .Register();
        }

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Leaves a trail of bees when thrown");
        }

        public override void SetDefaults()
        {
            TerrorbornItem modItem = TerrorbornItem.modItem(Item);
            modItem.critDamageMult = 1.15f;
            Item.damage = 22;
            Item.width = 70;
            Item.height = 74;
            Item.useTime = 22;
            Item.useAnimation = 22;
            Item.rare = ItemRarityID.Orange;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 3f;
            Item.UseSound = SoundID.Item1;
            Item.value = Item.sellPrice(0, 0, 50, 0);
            Item.shootSpeed = 35f;
            Item.shoot = ModContent.ProjectileType<TheBuzzer_Projectile>();
            Item.noUseGraphic = true;
            Item.autoReuse = true;
            Item.maxStack = 1;
            Item.DamageType = DamageClass.Melee;
            Item.noMelee = true;
        }
    }

    class TheBuzzer_Projectile : ModProjectile
    {
        public override string Texture { get { return "TerrorbornMod/Items/Weapons/Melee/TheBuzzer"; } }

        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Melee;
            Projectile.width = 70;
            Projectile.height = 74;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = false;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 8;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 20;
            height = 20;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
            TimeUntilReturn = 0;
            return false;
        }

        int TimeUntilReturn = 25;
        int ProjectileCounter = 5;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Vector2 vector = player.RotatedRelativePoint(player.MountedCenter, true);
            Projectile.spriteDirection = player.direction * -1;
            Projectile.rotation += 0.5f * player.direction;
            if (TimeUntilReturn <= 0)
            {
                Projectile.tileCollide = false;
                Vector2 targetPosition = Main.player[Projectile.owner].Center;
                float speed = 35f;
                Projectile.velocity = Projectile.DirectionTo(player.Center) * speed;
                if (Main.player[Projectile.owner].Distance(Projectile.Center) <= speed)
                {
                    Projectile.active = false;
                }
            }
            else
            {
                TimeUntilReturn--;
                ProjectileCounter--;
                if (ProjectileCounter <= 0)
                {
                    ProjectileCounter = 5;
                    int proj = Projectile.NewProjectile(Projectile.GetProjectileSource_FromThis(), Projectile.Center, Projectile.velocity / 10, ProjectileID.Bee, Projectile.damage / 2, 0f, Projectile.owner);
                    Main.projectile[proj].DamageType = DamageClass.Melee;
                    Main.projectile[proj].usesIDStaticNPCImmunity = true;
                    Main.projectile[proj].idStaticNPCHitCooldown = 7;
                }
            }
        }
    }
}
