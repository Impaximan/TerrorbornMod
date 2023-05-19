using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace TerrorbornMod.Items.Weapons.Magic
{
    class SongOfTime : ModItem
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
            Tooltip.SetDefault("Summons a spectral clock that accellerates to your cursor" +
                "\nThe clock will deal 5x damage if it is moving at a high velocity");
        }

        public override void SetDefaults()
        {
            Item.damage = 50;
            Item.noMelee = true;
            Item.width = 52;
            Item.height = 52;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 2.5f;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Pink;
            Item.UseSound = SoundID.Item28;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<SpectralClock>();
            Item.shootSpeed = 5f;
            Item.mana = 3;
            Item.DamageType = DamageClass.Magic;;
            Item.channel = true;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            position = Main.MouseWorld;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            foreach (Projectile Projectile in Main.projectile)
            {
                if (Projectile.active && Projectile.type == type)
                {
                    return false;
                }
            }
            return true;
        }
    }

    class SpectralClock : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 50;
            Projectile.height = 50;
            Projectile.aiStyle = 0;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.alpha = 255;
            Projectile.penetrate = -1;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Magic;;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 20;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        float requiredSpeed = 15f;
        public override void AI()
        {
            Projectile.rotation += MathHelper.ToRadians(10);
            Projectile.timeLeft = 20;

            if (Projectile.alpha > (int)(255 * 0.25f))
            {
                Projectile.alpha -= 15;
            }

            if (!Main.player[Projectile.owner].channel || Main.player[Projectile.owner].statMana < Main.player[Projectile.owner].HeldItem.mana * Main.player[Projectile.owner].manaCost)
            {
                DustExplosion(Projectile.Center, 10, 25f, 46f);
                Projectile.active = false;
            }

            if (Projectile.Center != Main.MouseWorld)
            {
                Projectile.velocity += Projectile.DirectionTo(Main.MouseWorld) * 0.5f;
            }
            Projectile.velocity *= 0.98f;

            if (Projectile.velocity.Length() > requiredSpeed)
            {
                Dust dust = Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 127)];
                dust.noGravity = true;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Utils.Graphics.DrawGlow_1(Main.spriteBatch, Projectile.Center - Main.screenPosition, 75, Color.OrangeRed * 0.5f);
            return true;
        }

        public override void Kill(int timeLeft)
        {
            DustExplosion(Projectile.Center, 10, 25f, 46f);
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (Projectile.velocity.Length() > requiredSpeed)
            {
                modifiers.SourceDamage *= 5;
            }
        }

        public void DustExplosion(Vector2 position, int dustAmount, float minDistance, float maxDistance)
        {
            Vector2 direction = new Vector2(0, 1);
            for (int i = 0; i < dustAmount; i++)
            {
                Vector2 newPos = position + direction.RotatedBy(MathHelper.ToRadians((360f / dustAmount) * i)) * Main.rand.NextFloat(minDistance, maxDistance);
                Dust dust = Dust.NewDustPerfect(newPos, 127);
                dust.scale = 1f;
                dust.velocity = (newPos - position) / 5;
                dust.noGravity = true;
            }
        }
    }
}
