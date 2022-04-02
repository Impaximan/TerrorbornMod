using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

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
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.EatingUsing;
            Item.maxStack = 30;
            Item.consumable = true;
            Item.rare = ItemRarityID.Blue;
            Item.autoReuse = false;
            Item.UseSound = SoundID.Item2;
            Item.useTurn = true;
            Item.maxStack = 30;
            Item.buffType = ModContent.BuffType<HorrificallyCheesed>();
            Item.buffTime = 3600 * 5;
        }
    }

    class HorrificallyCheesed : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Horrifically Cheesed");
            Description.SetDefault("Lose terror over time, granting you item use speed that scales with the amount of terror you have left");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = false;
            BuffID.Sets.LongerExpertDebuff[Type] = false;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.LoseTerror(2.5f, true, true);
            modPlayer.allUseSpeed *= 1f + (0.275f * modPlayer.TerrorPercent / 100f);
        }
    }
}


