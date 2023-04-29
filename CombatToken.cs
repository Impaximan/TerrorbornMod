using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.UI;
using Terraria.Localization;
using Terraria.ModLoader;

namespace TerrorbornMod
{
    class CombatTokenCurrency : CustomCurrencySingleCoin
    {
        public Color CombatTokenCurrencyTextColor = Color.FromNonPremultiplied(186, 75, 15, 255);

        public CombatTokenCurrency(int coinItemID, long currencyCap) : base(coinItemID, currencyCap)
        {
        }

        public override void GetPriceText(string[] lines, ref int currentLine, int price)
        {
            Color color = CombatTokenCurrencyTextColor * ((float)Main.mouseTextColor / 255f);
            lines[currentLine++] = string.Format("[c/{0:X2}{1:X2}{2:X2}:{3} {4} {5}]", new object[]
                {
                    color.R,
                    color.G,
                    color.B,
                    Language.GetTextValue("LegacyTooltip.50"),
                    price,
                    "Combat Tokens"
                });
        }
    }
    class CombatToken : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("'The reward for peacekeeping'");
        }
        public override void SetDefaults()
        {
            Item.maxStack = 999;
            Item.width = 28;
            Item.height = 30;
            Item.value = 100;
            Item.rare = -11;
        }
    }
}
