using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria;

namespace TerrorbornMod.Items.Weapons.Magic
{
    class AzurePearl : ModItem
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<Materials.AzuriteBar>(10)
                .AddTile(TileID.Anvils)
                .Register();
        }
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Fires an azure beam that explodes into smaller bouncing Projectiles");
        }
        public override void SetDefaults()
        {
            Item.damage = 20;
            Item.noMelee = true;
            Item.width = 32;
            Item.height = 32;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 5;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shootSpeed = 30f;
            Item.shoot = ModContent.ProjectileType<AzureBurst>();
            Item.mana = 10;
            Item.DamageType = DamageClass.Magic;;
            Item.noUseGraphic = true;
        }
    }

    class AzureBurst : ModProjectile
    {
        public override string Texture => "TerrorbornMod/Items/Weapons/Magic/MagicGuns/TarSwarm";
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.aiStyle = 0;
            Projectile.tileCollide = true;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Magic;;
            Projectile.ignoreWater = true;
            Projectile.hide = true;
            Projectile.timeLeft = 400;
        }
        public override void Kill(int timeLeft)
        {
            SoundExtensions.PlaySoundOld(SoundID.Item110, Projectile.Center);
            for (int i = 0; i < Main.rand.Next(3, 5); i++)
            {
                float speed = 35f;
                Vector2 velocity = MathHelper.ToRadians(Main.rand.Next(361)).ToRotationVector2() * speed;
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, velocity, ModContent.ProjectileType<AzureSpray>(), Projectile.damage / 3, 1, Projectile.owner);
            }
        }
        public override void AI()
        {
            int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 88, Scale: 2, newColor: Color.SkyBlue);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].velocity = Projectile.velocity;
        }
    }

    class AzureSpray : ModProjectile
    {
        public override string Texture => "TerrorbornMod/Items/Weapons/Magic/MagicGuns/TarSwarm";
        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.aiStyle = 0;
            Projectile.tileCollide = true;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 8;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Magic;;
            Projectile.ignoreWater = true;
            Projectile.hide = true;
            Projectile.timeLeft = 180;
        }
        public override void AI()
        {
            int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 88, Scale: 1.35f, newColor: Color.SkyBlue);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].velocity = Projectile.velocity / 4;

            Projectile.velocity.Y += 1.5f;
            Projectile.velocity.X *= 0.98f;
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.ArmorPenetration += target.defense / 2;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.velocity.X != oldVelocity.X)
            {
                Projectile.position.X = Projectile.position.X + Projectile.velocity.X;
                Projectile.velocity.X = -oldVelocity.X;
            }
            if (Projectile.velocity.Y != oldVelocity.Y)
            {
                Projectile.position.Y = Projectile.position.Y + Projectile.velocity.Y;
                Projectile.velocity.Y = -oldVelocity.Y * 0.9f;
            }
            return false;
        }
    }
}



