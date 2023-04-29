using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Weapons.Ranged
{
    class Wrathburner : ModItem
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Items.Materials.TorturedEssence>(), 3)
                .AddIngredient(ItemID.LunarBar, 10)
                .AddIngredient(ItemID.EldMelter)
                .AddIngredient(ItemID.FragmentVortex, 12)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }

        public override void SetStaticDefaults()
        {
            /* Tooltip.SetDefault("Uses gel for ammo" +
                "\n"); */
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

        public override void SetDefaults()
        {
            Item.damage = 135;
            Item.DamageType = DamageClass.Ranged;
            Item.noMelee = true;
            Item.width = 68;
            Item.height = 24;
            Item.useTime = 4;
            Item.useAnimation = 16;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 5;
            Item.value = Item.sellPrice(0, 3, 0, 0);
            Item.rare = ModContent.RarityType<Rarities.Golden>();
            Item.UseSound = SoundID.Item61;
            Item.shoot = ModContent.ProjectileType<Wrathflame>();
            Item.autoReuse = true;
            Item.shootSpeed = 25f;
            Item.useAmmo = AmmoID.Gel;
        }

        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            return Main.rand.NextFloat() >= .95f;
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            position += velocity.ToRotation().ToRotationVector2() * 60;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-8, 0);
        }
    }

    class Wrathflame : ModProjectile
    {
        public override string Texture => "TerrorbornMod/placeholder";

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 20;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.hide = true;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 4;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 2;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire, 300);
        }

        float scaleMult = 0f;
        public override void AI()
        {
            if (scaleMult < 1f)
            {
                scaleMult += 1f / 7.5f;
            }

            for (int i = 0; i < 4; i++)
            {
                int dust = Dust.NewDust(Projectile.position + Projectile.Size / 2 * (1f - scaleMult), (int)(Projectile.width * scaleMult), (int)(Projectile.height * scaleMult), 235, Scale: 2f * scaleMult);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity.Y -= Main.rand.NextFloat(0f, 5f) * scaleMult;
                Main.dust[dust].velocity += Main.player[Projectile.owner].velocity / 2;
            }
            Projectile.velocity *= 0.96f;
        }
    }
}