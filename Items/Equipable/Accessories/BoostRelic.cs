using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TerrorbornMod.Items.Equipable.Accessories
{
    class BoostRelic : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Increases item use speed by 10%" +
                "\nIncreases manueverability while airborne" +
                "\nIncreases jump speed");
        }

        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 34;
            Item.accessory = true;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(0, 1, 0, 0);
        }

        public override void UpdateEquip(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            player.GetAttackSpeed(DamageClass.Generic) *= 1.1f;
            player.jumpSpeedBoost += 1.5f;
            
            if (player.velocity.Y != 0)
            {
                player.runAcceleration += 0.2f;
            }
        }
    }
}

