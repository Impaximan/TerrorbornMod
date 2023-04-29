using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Weapons.Ranged.Thrown
{
    class Shrapnade : ModItem
    {
        public override void AddRecipes()
        {
            CreateRecipe(100)
                .AddIngredient(ModContent.ItemType<Materials.ShellFragments>(), 2)
                .AddRecipeGroup(RecipeGroupID.IronBar, 1)
                .AddTile(TileID.Anvils)
                .Register();
        }

        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("Explodes into multiple bits of shrapnel");
        }

        public override void SetDefaults()
        {
            TerrorbornItem modItem = TerrorbornItem.modItem(Item);
            modItem.critDamageMult = 1.25f;
            Item.damage = 13;
            Item.DamageType = DamageClass.Ranged;
            Item.consumable = true;
            Item.maxStack = 999;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.noUseGraphic = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 10;
            Item.value = Item.sellPrice(0, 0, 0, 20);
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = false;
            Item.noMelee = true;
            Item.shootSpeed = 10;
            Item.shoot = ModContent.ProjectileType<ShrapnadeProjectile>();
        }
    }

    class ShrapnadeProjectile : ModProjectile
    {
        public override string Texture => "TerrorbornMod/Items/Weapons/Ranged/Thrown/Shrapnade";
        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 18;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 60 * 3;
        }

        public override void AI()
        {
            Projectile.rotation += MathHelper.ToRadians(Projectile.velocity.X * 4);
            Projectile.velocity.Y += 0.2f;
        }

        public override void Kill(int timeLeft)
        {
            SoundExtensions.PlaySoundOld(SoundID.Item14, Projectile.Center);
            for (int i = 0; i < Main.rand.Next(3, 6); i++)
            {
                float Speed = Main.rand.Next(7, 10);
                Vector2 ProjectileSpeed = MathHelper.ToRadians(Main.rand.Next(361)).ToRotationVector2() * Speed;
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, ProjectileSpeed, ModContent.ProjectileType<Shrapnel>(), (int)(Projectile.damage * 0.75f), 0, Projectile.owner);
            }

            for (int i = 0; i < Main.rand.Next(3, 5); i++)
            {
                float Speed = Main.rand.Next(2, 5);
                Vector2 ProjectileSpeed = MathHelper.ToRadians(Main.rand.Next(361)).ToRotationVector2() * Speed;
                Gore.NewGore(Projectile.GetSource_Misc("ShrapnadeExplosion"), Projectile.Center, ProjectileSpeed, Main.rand.Next(825, 828));
            }
            TerrorbornSystem.ScreenShake(1f);
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.velocity.X != oldVelocity.X)
            {
                Projectile.position.X = Projectile.position.X + Projectile.velocity.X;
                Projectile.velocity.X = -oldVelocity.X * 0.5f;
            }
            if (Projectile.velocity.Y != oldVelocity.Y)
            {
                Projectile.position.Y = Projectile.position.Y + Projectile.velocity.Y;
                Projectile.velocity.Y = -oldVelocity.Y * 0.5f;
            }
            return false;
        }
    }

    class Shrapnel : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 16;
            Projectile.tileCollide = true;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 600;
            Projectile.frame = Main.rand.Next(3);
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 8;
            height = 8;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }

        public override void AI()
        {
            Projectile.velocity.Y += 0.2f;

            if (Projectile.velocity.X > 0)
            {
                Projectile.rotation += MathHelper.ToRadians(Projectile.velocity.Length());
            }
            else
            {
                Projectile.rotation -= MathHelper.ToRadians(Projectile.velocity.Length());
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            SoundExtensions.PlaySoundOld(SoundID.Dig, Projectile.position);
            return true;
        }
    }
}
