using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;

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
            item.useTime = 24;
            item.useAnimation = 36;
            item.useStyle = 4;
            item.maxStack = 30;
            item.consumable = true;
            item.rare = 1;
            item.autoReuse = false;
            item.UseSound = SoundID.Item2;
            item.useTurn = true;
            item.maxStack = 30;
            item.buffType = ModContent.BuffType<HorrificallyNourished>();
            item.buffTime = 3600 * 5;
        }
    }

    class HorrificallyNourished : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Horifically Nourished");
            Description.SetDefault("Grants you life regen that scales with how much terror you have, but you will lose terror over time");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = false;
            longerExpertDebuff = false;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            player.lifeRegen += 1 + (int)(modPlayer.TerrorPercent / 20);

            modPlayer.LoseTerror(0.75f, true, true);
        }
    }
}


