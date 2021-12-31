using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Equipable.Armor
{
    [AutoloadEquip(EquipType.Head)]
    class PineHood : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("6% increased magic critical strike chance" +
                "\nIncreased magic casting speed" +
                "\nHas a set bonus when paired with a robe");
        }

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 24;
            item.value = Item.sellPrice(0, 2, 0, 0);
            item.defense = 6;
            item.rare = ItemRarityID.Blue;
        }

        public override void UpdateEquip(Player player)
        {
            TerrorbornPlayer.modPlayer(player).magicUseSpeed *= 1.1f;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ItemID.Robe || body.type == ItemID.DiamondRobe || body.type == ItemID.SapphireRobe || body.type == ItemID.TopazRobe || body.type == ItemID.AmethystRobe || body.type == ItemID.EmeraldRobe || body.type == ItemID.RubyRobe;
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Greatly increases mana regen while standing still" +
                "\nGrants you the 'dryad's blessing' buff while on the surface during the day";
            if (player.velocity.X == 0)
            {
                player.manaRegen += 10;
            }
            if (player.ZoneOverworldHeight && Main.dayTime)
            {
                player.AddBuff(BuffID.DryadsWard, 2);
            }
        }
    }
}

