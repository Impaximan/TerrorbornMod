using Terraria.GameContent.ItemDropRules;

namespace TerrorbornMod.ItemDropRules.Conditions
{
    class TwilightModeCondition : IItemDropRuleCondition
    {
        public bool CanDrop(DropAttemptInfo info)
        {
            return TerrorbornSystem.TwilightMode;
        }

        public bool CanShowItemDropInUI()
        {
            return TerrorbornSystem.TwilightMode;
        }

        public string GetConditionDescription()
        {
            return "This is a Twilight Mode drop rate";
        }
    }
}
