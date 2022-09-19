using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Weapons.Restless
{
    class HighHornetPower : RestlessWeapon
    {
        public override string Texture => "TerrorbornMod/Items/Weapons/Restless/HighHornetPower";
        int UntilBlast;
        public override void restlessSetStaticDefaults()
        {
            DisplayName.SetDefault("High-Hornet Power");
        }

        public override string defaultTooltip()
        {
            return "Fires bees that rapidly hit enemies" +
                "\nHolding this weapon gives you armor penetration";
        }

        public override string altTooltip()
        {
            return "Fires multiple bees at once that can individually hit enemies, causing them to deal massive amounts of damage" +
                "\nHolding this weapon gives you armor penetration";
        }

        public override void HoldItem(Player player)
        {
            player.GetArmorPenetration(DamageClass.Magic) += 25;
        }

        public override void restlessSetDefaults(TerrorbornItem modItem)
        {
            Item.damage = 24;
            Item.noMelee = true;
            Item.DamageType = DamageClass.Magic;;
            Item.width = 56;
            Item.height = 38;
            Item.mana = 3;
            Item.useTime = 8;
            Item.useAnimation = 8;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 6;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.Cyan;
            Item.UseSound = SoundID.Item11;
            Item.shoot = ProjectileID.Bee;
            Item.shootSpeed = 12.5f;
            Item.crit = 7;
            Item.autoReuse = true;
            modItem.restlessTerrorDrain = 3f;
            modItem.restlessChargeUpUses = 10;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, 0);
        }

        public override bool RestlessCanUseItem(Player player)
        {
            return base.RestlessCanUseItem(player);
        }

        public override bool RestlessShoot(Player player, EntitySource_ItemUse_WithAmmo source, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            TerrorbornItem modItem = TerrorbornItem.modItem(Item);
            if (modItem.RestlessChargedUp())
            {
                for (int i = 0; i < 5; i++)
                {
                    int proj = Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(15)) * Main.rand.NextFloat(0.8f, 1.2f), type, damage, knockback, player.whoAmI);
                    Main.projectile[proj].usesLocalNPCImmunity = true;
                    Main.projectile[proj].localNPCHitCooldown = 5;
                    Main.projectile[proj].DamageType = DamageClass.Magic;;
                }
            }
            else
            {
                int proj = Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y), type, damage, knockback, player.whoAmI);
                Main.projectile[proj].usesIDStaticNPCImmunity = true;
                Main.projectile[proj].idStaticNPCHitCooldown = 5;
                Main.projectile[proj].DamageType = DamageClass.Magic;;
            }
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Items.Weapons.Magic.MagicGuns.TarSwarm>())
                .AddIngredient(ModContent.ItemType<Materials.HexingEssence>(), 5)
                .AddIngredient(ModContent.ItemType<Materials.HellbornEssence>(), 5)
                .AddIngredient(ModContent.ItemType<Materials.TerrorSample>(), 4)
                .AddIngredient(ItemID.SoulofFright, 15)
                .AddTile(ModContent.TileType<Tiles.MeldingStation>())
                .Register();
        }
    }
}