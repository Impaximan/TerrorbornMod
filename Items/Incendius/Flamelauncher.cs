using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace TerrorbornMod.Items.Incendius
{
    class Flamelauncher : ModItem
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Items.Materials.IncendiusAlloy>(), (int)(25 * TerrorbornMod.IncendiaryAlloyMultiplier))
                .AddIngredient(ModContent.ItemType<Items.Placeable.Blocks.IncendiaryPipe>(), 5)
                .AddRecipeGroup("cobalt", 15)
                .AddTile(ModContent.TileType<Tiles.Incendiary.IncendiaryAltar>())
                .Register();
        }

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Uses Gel as ammo, creating flames" +
                "\nHas a chance to create a lingering flame cloud" +
                "\n95% chance to not consume ammo");
        }

        public override void SetDefaults()
        {
            Item.damage = 34;
            Item.DamageType = DamageClass.Ranged;
            Item.noMelee = true;
            Item.scale = 0.8f;
            Item.width = 58;
            Item.height = 32;
            Item.useTime = 4;
            Item.useAnimation = 12;
            Item.reuseDelay = 15;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 5;
            Item.value = Item.sellPrice(0, 3, 0, 0);
            Item.rare = ItemRarityID.LightRed;
            Item.UseSound = SoundID.Item61;
            Item.shoot = ProjectileID.Flames;
            Item.autoReuse = true;
            Item.shootSpeed = 10f;
            Item.useAmmo = AmmoID.Gel;
        }

        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            return Main.rand.NextFloat() >= .95f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (Main.rand.NextFloat() <= .20f)
            {
                float Multiplier = Main.rand.NextFloat(.4f, .6f);
                Projectile.NewProjectile(source, position, (velocity * Multiplier).RotatedByRandom(MathHelper.ToRadians(10)), ModContent.ProjectileType<FlameCloud>(), damage / 2, 0, player.whoAmI);
            }
            if (velocity.X > 0)
            {
                position += Vector2.Normalize(velocity).RotatedBy(60) * 15f;
            }
            else
            {
                position += Vector2.Normalize(new Vector2(velocity.X, velocity.Y)).RotatedBy(-60) * 15f;
            }

            int proj = Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y), type, damage, knockback, player.whoAmI);
            Main.projectile[proj].usesLocalNPCImmunity = true;
            Main.projectile[proj].localNPCHitCooldown = -1;
            Main.projectile[proj].penetrate = -1;
            return false;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-6, 0);
        }
    }
    class FlameCloud : ModProjectile
    {
        public override string Texture => "TerrorbornMod/placeholder";

        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 600;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.hide = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 15;
            Projectile.tileCollide = false;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire, 300);
        }

        public override void AI()
        {
            int dust = Dust.NewDust(Projectile.position, 25, 25, 6, Scale: 1.5f);
            Main.dust[dust].noGravity = true;
            Projectile.velocity *= 0.96f;
        }
    }
}
