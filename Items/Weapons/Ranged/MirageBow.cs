using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace TerrorbornMod.Items.Weapons.Ranged
{
    class MirageBow : ModItem
    {
        public override void AddRecipes()
        {
            int evilBars = 10;
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Materials.HexingEssence>(), 8)
                .AddIngredient(ItemID.CrimtaneBar, evilBars)
                .AddTile(TileID.MythrilAnvil)
                .Register();
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Materials.HexingEssence>(), 8)
                .AddIngredient(ItemID.DemoniteBar, evilBars)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Creates numerous spectral versions of itself to fire at your cursor");
        }
        public override void SetDefaults()
        {
            Item.damage = 16;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 22;
            Item.height = 52;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 2;
            Item.value = Item.sellPrice(0, 4, 0, 0);
            Item.rare = ItemRarityID.LightRed;
            Item.UseSound = SoundID.Item117;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 15f;
            Item.useAmmo = AmmoID.Arrow;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(2f, 0);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int i = 0; i < 2; i++)
            {
                Projectile Projectile = Main.projectile[Projectile.NewProjectile(source, position.X - 50 + Main.rand.Next(100), position.Y - 50 + Main.rand.Next(100), 0, 0, ModContent.ProjectileType<SpectralBow>(), damage, knockback, player.whoAmI)];
                Projectile.ai[0] = type;
                Projectile.ai[1] = new Vector2(velocity.X, velocity.Y).Length();
            }
            return false;
        }
    }

    class SpectralBow : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 52;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 60;
            Projectile.tileCollide = false;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.alpha = 255;
            Projectile.ignoreWater = true;
        }

        int ProjectileWait = 20;
        public override void AI()
        {
            Projectile.rotation = Projectile.DirectionTo(Main.MouseWorld).ToRotation();

            ProjectileWait--;
            if (ProjectileWait <= 0)
            {
                ProjectileWait = Main.rand.Next(15, 25);
                SoundExtensions.PlaySoundOld(SoundID.Item5, Projectile.Center);
                Vector2 velocity = Projectile.ai[1] * Projectile.DirectionTo(Main.MouseWorld);
                Projectile proj = Main.projectile[Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, velocity, (int)Projectile.ai[0], Projectile.damage, Projectile.knockBack, Projectile.owner)];
                proj.noDropItem = true;
            }

            if (Projectile.alpha > 255 / 2)
            {
                Projectile.alpha -= 15;
            }
        }

        public override void Kill(int timeLeft)
        {
            DustExplosion(Projectile.Center, 0, 10, 15, 6, DustScale: 1.5f, NoGravity: true);
        }

        public void DustExplosion(Vector2 position, int RectWidth, int Streams, float DustSpeed, int DustType, float DustScale = 1f, bool NoGravity = false) //Thank you once again Seraph
        {
            float currentAngle = Main.rand.Next(360);

            //if(Main.netMode!=1){
            for (int i = 0; i < Streams; ++i)
            {

                Vector2 direction = Vector2.Normalize(new Vector2(1, 1)).RotatedBy(MathHelper.ToRadians(((360 / Streams) * i) + currentAngle));
                direction.X *= DustSpeed;
                direction.Y *= DustSpeed;

                Dust dust = Dust.NewDustPerfect(position + (new Vector2(Main.rand.Next(RectWidth), Main.rand.Next(RectWidth))), DustType, direction, 0, default(Color), DustScale);
                if (NoGravity)
                {
                    dust.noGravity = true;
                }
            }
        }
    }
}
