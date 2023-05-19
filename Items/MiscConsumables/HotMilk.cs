using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TerrorbornMod.Items.MiscConsumables
{
    public class HotMilk : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Grants you increased life regen" +
                "\nThe more terror you have, the more your life regen will be increased" +
                "\nYou will slowly lose terror over time" +
                "\n'It's incendairy!'");
        }
        public override void SetDefaults()
        {
            Item.useTime = 24;
            Item.useAnimation = 36;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.maxStack = 30;
            Item.consumable = true;
            Item.rare = ItemRarityID.Blue;
            Item.autoReuse = false;
            Item.UseSound = SoundID.Item2;
            Item.useTurn = true;
            Item.maxStack = 30;
            Item.buffType = ModContent.BuffType<HorrificallyNourished>();
            Item.buffTime = 3600 * 5;
        }
    }

    class HorrificallyNourished : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Horifically Nourished");
            // Description.SetDefault("Grants you life regen that scales with how much terror you have, but you will lose terror over time");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = false;
            BuffID.Sets.LongerExpertDebuff[Type] = false;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            player.lifeRegen += 1 + (int)(modPlayer.TerrorPercent / 20);

            modPlayer.LoseTerror(0.75f, true, true);
        }
    }
}


