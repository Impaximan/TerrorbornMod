using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Equipable.Accessories
{
    class MidnightPlasmaGlobe : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Generates terror over time" +
                "\nBeing under 30% terror inflicts you with the 'Midnight Flames' debuff");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.accessory = true;
            item.noMelee = true;
            item.lifeRegen = 5;
            item.rare = ItemRarityID.Pink;
            item.expert = true;
            item.value = Item.sellPrice(0, 1, 50, 0);
            item.useAnimation = 5;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.GainTerror(2f, true, true);
            if (modPlayer.TerrorPercent <= 30f)
            {
                player.AddBuff(ModContent.BuffType<Buffs.Debuffs.MidnightFlamesDebuff>(), 1);
            }
        }
    }
}


