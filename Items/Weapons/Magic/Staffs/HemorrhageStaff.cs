using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace TerrorbornMod.Items.Weapons.Magic.Staffs
{
    class HemorrhageStaff : ModItem
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.CrimtaneBar, 8)
                .AddTile(TileID.Anvils)
                .Register();
        }

        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = true;
            Tooltip.SetDefault("Fires a crimson heart that bounces around" +
                "\nThis heart will explode into blood after 4 seconds or after hitting an enemy");
        }

        public override void SetDefaults()
        {
            Item.damage = 18;
            Item.noMelee = true;
            Item.width = 34;
            Item.height = 42;
            Item.useTime = 40;
            Item.useAnimation = 40;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 5;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.NPCDeath13;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<CrimsonHeartBomb>();
            Item.shootSpeed = 15f;
            Item.mana = 6;
            Item.DamageType = DamageClass.Magic; ;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            position = player.Center + player.DirectionTo(Main.MouseWorld) * 42;
            return true;
        }
    }

    class CrimsonHeartBomb : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 20;
            Projectile.aiStyle = 0;
            Projectile.tileCollide = true;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Magic; ;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 240;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 18;
            height = 18;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            for (int i = 0; i < Projectile.localNPCImmunity.Length; i++)
            {
                if (Projectile.localNPCImmunity[i] < 0 || Projectile.localNPCImmunity[i] > 5)
                {
                    Projectile.localNPCImmunity[i] = 5;
                }
            }
            if (Projectile.velocity.X != oldVelocity.X)
            {
                Projectile.position.X = Projectile.position.X + Projectile.velocity.X;
                Projectile.velocity.X = -oldVelocity.X;
            }
            if (Projectile.velocity.Y != oldVelocity.Y)
            {
                Projectile.position.Y = Projectile.position.Y + Projectile.velocity.Y;
                Projectile.velocity.Y = -oldVelocity.Y;
            }
            return false;
        }

        public override void AI()
        {
            Projectile.rotation += MathHelper.ToRadians(Projectile.velocity.X);
            Projectile.velocity.Y += 0.2f;
            if (Projectile.velocity.Y > 15)
            {
                Projectile.velocity.Y = 15;
            }
        }

        public override void Kill(int timeLeft)
        {
            SoundExtensions.PlaySoundOld(SoundID.NPCDeath21, Projectile.Center);
            for (int i = 0; i < Main.rand.Next(4, 7); i++)
            {
                Vector2 direction = MathHelper.ToRadians(Main.rand.Next(360)).ToRotationVector2();
                float speed = Main.rand.Next(10, 15);
                int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, direction * speed, ModContent.ProjectileType<Projectiles.VeinBurst>(), Projectile.damage / 2, 0f, Projectile.owner);
                Main.projectile[proj].DamageType = DamageClass.Magic; ;
                Main.projectile[proj].ai[0] = 1;
            }
        }
    }
}

