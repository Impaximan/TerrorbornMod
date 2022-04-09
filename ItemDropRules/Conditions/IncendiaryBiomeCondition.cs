using Terraria.GameContent.ItemDropRules;

namespace TerrorbornMod.ItemDropRules.Conditions
{
    class IncendiaryBiomeCondition : IItemDropRuleCondition
    {
        public bool CanDrop(DropAttemptInfo info)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(info.player);
            if (modPlayer.ZoneIncendiary)
            {
                return true;
            }
            return false;
        }

        public bool CanShowItemDropInUI()
        {
            return false;
        }

        public string GetConditionDescription()
        {
            return "Only Drops while in the Sisyphean Islands";
        }
    }
}
