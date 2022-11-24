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
                "\n10% increased item/weapon use speed" +
                "\n15% increased critical strike chance" +
                "\nBeing under 75 health doubles these bonuses");
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.accessory = true;
            Item.noMelee = true;
            Item.lifeRegen = 8;
            Item.rare = ItemRarityID.Pink;
            Item.expert = true;
            Item.value = Item.sellPrice(0, 1, 50, 0);
            Item.useAnimation = 5;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            player.statDefense -= 15;
            player.statLifeMax2 -= 50;
            if (player.statLife <= 250)
            {
                player.GetCritChance(DamageClass.Generic) += 15;
                player.GetAttackSpeed(DamageClass.Generic) *= 1.1f;
            }
            if (player.statLife <= 75)
            {
                player.GetCritChance(DamageClass.Generic) += 30;
                player.GetAttackSpeed(DamageClass.Generic) *= 1.2f;
            }
        }
    }
}

