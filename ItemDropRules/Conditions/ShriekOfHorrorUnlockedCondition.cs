using Terraria.GameContent.ItemDropRules;

namespace TerrorbornMod.ItemDropRules.Conditions
{
    class ShriekOfHorrorUnlockedCondition : IItemDropRuleCondition
    {
        public bool CanDrop(DropAttemptInfo info)
        {
            return TerrorbornSystem.obtainedShriekOfHorror;
        }

        public bool CanShowItemDropInUI()
        {
            return false;
        }

        public string GetConditionDescription()
        {
            return "Only drops if you have obtained Shriek of Horror";
        }
    }
}

