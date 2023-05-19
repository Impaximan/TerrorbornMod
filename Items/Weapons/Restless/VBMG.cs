using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Weapons.Restless
{
    class VBMG : RestlessWeapon
    {
        public override string Texture => "TerrorbornMod/Items/Weapons/Restless/VBMG";
        int UntilBlast;
        public override void RestlessSetStaticDefaults()
        {
            DisplayName.SetDefault("Vein Buster");
        }

        public override string defaultTooltip()
        {
            return "Does nothing of interest by default";
        }

        public override string altTooltip()
        {
            return "Fires a high velocity bullet that causes blood to erupt from hit enemies";
        }

        public override void RestlessSetDefaults(TerrorbornItem modItem)
        {
            Item.damage = 10;
            Item.DamageType = DamageClass.Ranged;
            Item.noMelee = true;
            Item.width = 38;
            Item.height = 18;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 1;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item11;
            Item.autoReuse = true;
            Item.shootSpeed = 16f;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.useAmmo = AmmoID.Bullet;
            modItem.restlessChargeUpUses = 5;
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
                Main.projectile[proj].GetGlobalProjectile<TerrorbornProjectile>().VeinBurster = true;
                return false;
            }
            return true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.CrimtaneBar, 10)
                .AddIngredient(ItemID.TissueSample, 5)
                .AddIngredient<Materials.SanguineFang>(12)
                .AddIngredient(ModContent.ItemType<Items.Materials.TerrorSample>(), 5)
                .AddTile(ModContent.TileType<Tiles.MeldingStation>())
                .Register();
            CreateRecipe()
                .AddIngredient(ItemID.DemoniteBar, 10)
                .AddIngredient(ItemID.ShadowScale, 5)
                .AddIngredient<Materials.SanguineFang>(12)
                .AddIngredient(ModContent.ItemType<Items.Materials.TerrorSample>(), 5)
                .AddTile(ModContent.TileType<Tiles.MeldingStation>())
                .Register();
        }
    }
}

