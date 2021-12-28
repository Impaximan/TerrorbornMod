using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Equipable.Accessories
{
    class DarkAbdomen : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Decreases defense by 15" +
                "\nGreatly increases life regen" +
                "\nDecreases your maximum life by 50" +
                "\nBeing under 250 health grants you the following stats:" +
                "\n20% increased item/weapon use speed" +
                "\n15% increased critical strike chance" +
                "\nBeing under 75 health doubles these bonuses");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.accessory = true;
            item.noMelee = true;
            item.lifeRegen = 8;
            item.rare = ItemRarityID.Pink;
            item.expert = true;
            item.value = Item.sellPrice(0, 1, 50, 0);
            item.useAnimation = 5;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            player.statDefense -= 15;
            player.statLifeMax2 -= 50;
            if (player.statLife <= 250)
            {
                player.magicCrit += 15;
                player.meleeCrit += 15;
                player.rangedCrit += 15;
                modPlayer.allUseSpeed *= 1.2f;
            }
            if (player.statLife <= 75)
            {
                player.magicCrit += 30;
                player.meleeCrit += 30;
                player.rangedCrit += 30;
                modPlayer.allUseSpeed *= 1.4f;
            }
        }
    }
}

