using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;

namespace TerrorbornMod.Items.MiscConsumables
{
    public class TerrorCheese : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Consumes terror over time, making your attacks faster the more terror you have left" +
                "\nThis is not a culinary crutch");
        }
        public override void SetDefaults()
        {
            item.useTime = 10;
            item.useAnimation = 10;
            item.useStyle = ItemUseStyleID.EatingUsing;
            item.maxStack = 30;
            item.consumable = true;
            item.rare = ItemRarityID.Blue;
            item.autoReuse = false;
            item.UseSound = SoundID.Item2;
            item.useTurn = true;
            item.maxStack = 30;
            item.buffType = ModContent.BuffType<HorrificallyCheesed>();
            item.buffTime = 3600 * 5;
        }
    }

    class HorrificallyCheesed : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Horrifically Cheesed");
            Description.SetDefault("Lose terror over time, granting you item use speed that scales with the amount of terror you have left");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = false;
            longerExpertDebuff = false;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.LoseTerror(2.5f, true, true);
            modPlayer.allUseSpeed *= 1f + (0.275f * modPlayer.TerrorPercent / 100f);
        }
    }
}


