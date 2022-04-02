using Terraria;
using Terraria.GameContent.ItemDropRules;

namespace TerrorbornMod.ItemDropRules.Conditions
{
    class GolemCondition : IItemDropRuleCondition
    {
        public bool CanDrop(DropAttemptInfo info)
        {
            return NPC.downedGolemBoss;
        }

        public bool CanShowItemDropInUI()
        {
            return false;
        }

        public string GetConditionDescription()
        {
            return "Only drops after having defeated Golem";
        }
    }
}
