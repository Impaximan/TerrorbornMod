using Terraria;
using System;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Equipable.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class SilentHelmet : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Increases shriek of horror's use speed");
        }

        public override void UpdateEquip(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.ShriekSpeed *= 0.4f;
        }

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 20;
            item.value = Item.sellPrice(0, 0, 50, 0);
            item.rare = ItemRarityID.Blue;
            item.defense = 6;
        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<SilentBreastplate>() && legs.type == ModContent.ItemType<SilentGreaves>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Generates terror over time" +
                "\nGetting hit causes you to lose all terror you had";

            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.GainTerror(1.35f, true, true);
            modPlayer.SilentArmor = true;
        }
    }

    [AutoloadEquip(EquipType.Body)]
    public class SilentBreastplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Increases all item use speeds by 10%");
        }

        public override void UpdateEquip(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.allUseSpeed *= 1.06f;
        }

        public override void SetDefaults()
        {
            item.width = 26;
            item.height = 20;
            item.value = Item.sellPrice(0, 0, 50, 0);
            item.rare = ItemRarityID.Blue;
            item.defense = 7;
        }

        //public override bool DrawBody()
        //{
        //    return false;
        //}
    }

    [AutoloadEquip(EquipType.Legs)]
    public class SilentGreaves : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Increases Shriek of Horror's terror drain");
        }

        public override void UpdateEquip(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.ShriekTerrorMultiplier *= 1.3f;
        }

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 18;
            item.value = Item.sellPrice(0, 0, 50, 0);
            item.rare = ItemRarityID.Blue;
            item.defense = 6;
        }
    }
}

