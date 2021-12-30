using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
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
            player.armorPenetration += 25;
        }

        public override void restlessSetDefaults(TerrorbornItem modItem)
        {
            item.damage = 24;
            item.noMelee = true;
            item.magic = true;
            item.width = 56;
            item.height = 38;
            item.mana = 3;
            item.useTime = 8;
            item.useAnimation = 8;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.knockBack = 6;
            item.value = Item.sellPrice(0, 5, 0, 0);
            item.rare = ItemRarityID.Cyan;
            item.UseSound = SoundID.Item11;
            item.shoot = ProjectileID.Bee;
            item.shootSpeed = 12.5f;
            item.crit = 7;
            item.autoReuse = true;
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

        public override bool RestlessShoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            TerrorbornItem modItem = TerrorbornItem.modItem(item);
            if (modItem.RestlessChargedUp())
            {
                for (int i = 0; i < 5; i++)
                {
                    int proj = Projectile.NewProjectile(position, new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(15)) * Main.rand.NextFloat(0.8f, 1.2f), type, damage, knockBack, player.whoAmI);
                    Main.projectile[proj].usesLocalNPCImmunity = true;
                    Main.projectile[proj].localNPCHitCooldown = 5;
                    Main.projectile[proj].magic = true;
                }
            }
            else
            {
                int proj = Projectile.NewProjectile(position, new Vector2(speedX, speedY), type, damage, knockBack, player.whoAmI);
                Main.projectile[proj].usesIDStaticNPCImmunity = true;
                Main.projectile[proj].idStaticNPCHitCooldown = 5;
                Main.projectile[proj].magic = true;
            }
            return false;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Weapons.Magic.TarSwarm>());
            recipe.AddIngredient(ModContent.ItemType<Materials.HexingEssence>(), 5);
            recipe.AddIngredient(ModContent.ItemType<Materials.HellbornEssence>(), 5);
            recipe.AddIngredient(ModContent.ItemType<Materials.TerrorSample>(), 4);
            recipe.AddIngredient(ItemID.SoulofFright, 15);
            recipe.AddTile(ModContent.TileType<Tiles.MeldingStation>());
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}