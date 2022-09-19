using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Weapons.Restless
{
    class ContaminatedMarinePistol : RestlessWeapon
    {
        public override string Texture => "TerrorbornMod/Items/Weapons/Restless/ContaminatedMarinePistol";
        int UntilBlast;
        public override void RestlessSetStaticDefaults()
        {
            DisplayName.SetDefault("Contaminated Marine Pistol");
        }

        public override string defaultTooltip()
        {
            return "Does nothing of interest by default";
        }

        public override string altTooltip()
        {
            return "Fires a high velocity bullet that causes nightmare flames to erupt from hit enemies";
        }

        public override void RestlessSetDefaults(TerrorbornItem modItem)
        {
            Item.damage = 27;
            Item.DamageType = DamageClass.Ranged;
            Item.noMelee = true;
            Item.width = 38;
            Item.height = 18;
            Item.useTime = 14;
            Item.useAnimation = 14;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 1;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item11;
            Item.autoReuse = true;
            Item.shootSpeed = 16f;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.useAmmo = AmmoID.Bullet;
            modItem.restlessChargeUpUses = 10;
            modItem.restlessTerrorDrain = 8;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, 0);
        }

        public override bool RestlessShoot(Player player, EntitySource_ItemUse_WithAmmo source, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            TerrorbornItem modItem = TerrorbornItem.modItem(Item);
            if (modItem.RestlessChargedUp())
            {
                int proj = Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y), type, damage, knockback, player.whoAmI);
                Main.projectile[proj].extraUpdates = Main.projectile[proj].extraUpdates * 2 + 1;
                Main.projectile[proj].GetGlobalProjectile<TerrorbornProjectile>().ContaminatedMarine = true;
                return false;
            }
            return base.RestlessShoot(player, source, ref position, ref velocity, ref type, ref damage, ref knockback);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Items.Materials.SoulOfPlight>(), 22)
                .AddIngredient(ItemID.HallowedBar, 10)
                .AddIngredient(ModContent.ItemType<Items.Materials.TerrorSample>(), 6)
                .AddTile(ModContent.TileType<Tiles.MeldingStation>())
                .Register();
        }
    }

    class NightmareBoilRanged : ModProjectile
    {
        public override string Texture => "TerrorbornMod/placeholder";
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 60;
            Projectile.penetrate = 5;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.hide = true;
        }

        int moveCounter = 10;
        public override void AI()
        {
            if (moveCounter > 0)
            {
                moveCounter--;
                Projectile.position -= Projectile.velocity;
            }
            else
            {
                int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 74, 0f, 0f, 100, Scale: 1.5f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity = Projectile.velocity;
                Projectile.velocity.Y += 0.2f;
            }
        }
    }
}




