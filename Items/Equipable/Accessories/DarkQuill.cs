﻿using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TerrorbornMod.Items.Equipable.Accessories
{
    class DarkQuill : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dark Quill");
            Tooltip.SetDefault("The quill of a scholar who studied the properties of terror" +
                "\nAllows you to move at 50% speed while using Shriek of Horror, but doing so will hurt you 50% more");
        }

        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 34;
            Item.accessory = true;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(0, 2, 0, 0);
        }

        public override void UpdateEquip(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.ShriekOfHorrorMovement += 0.5f;
            modPlayer.ShriekPain += 0.5f;
        }
    }
}

