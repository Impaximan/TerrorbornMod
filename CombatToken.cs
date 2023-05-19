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
            CurrencyTextColor = CombatTokenCurrencyTextColor;
        }
    }
    class CombatToken : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("'The reward for peacekeeping'");
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
