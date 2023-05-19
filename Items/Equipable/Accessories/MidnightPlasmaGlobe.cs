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
            Item.width = 32;
            Item.height = 32;
            Item.accessory = true;
            Item.noMelee = true;
            Item.lifeRegen = 5;
            Item.rare = ItemRarityID.Pink;
            Item.expert = true;
            Item.value = Item.sellPrice(0, 1, 50, 0);
            Item.useAnimation = 5;
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


