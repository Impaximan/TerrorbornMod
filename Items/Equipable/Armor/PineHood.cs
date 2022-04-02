using Terraria;
using Terraria.ID;
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
            Item.width = 22;
            Item.height = 24;
            Item.value = Item.sellPrice(0, 2, 0, 0);
            Item.defense = 4;
            Item.rare = ItemRarityID.Blue;
        }

        public override void UpdateEquip(Player player)
        {
            TerrorbornPlayer.modPlayer(player).magicUseSpeed *= 1.1f;
            player.GetCritChance(DamageClass.Magic) += 6;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ItemID.Robe || body.type == ItemID.DiamondRobe || body.type == ItemID.SapphireRobe || body.type == ItemID.TopazRobe || body.type == ItemID.AmethystRobe || body.type == ItemID.EmeraldRobe || body.type == ItemID.RubyRobe;
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Greatly increases mana regen while standing still" +
                "\nGrants you the 'dryad's blessing' buff while on the surface";
            if (player.velocity.X == 0)
            {
                player.manaRegen += 10;
            }
            if (player.ZoneOverworldHeight)
            {
                player.AddBuff(BuffID.DryadsWard, 2);
            }
        }
    }
}

