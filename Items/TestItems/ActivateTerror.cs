using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.TestItems
{
    class ActivateTerror : ModItem
    {
        public override string Texture => "TerrorbornMod/placeholder";
        public override bool IsLoadingEnabled(Mod mod)
        {
            return TerrorbornMod.IsInTestingMode;
        }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Activate/Deactivate terror");
            Tooltip.SetDefault("--UNOBTAINABLE TESTING ITEM--" +
                "\nUnlocks Shriek of Horror in pre-made worlds" +
                "\nUse again to take it away");
        }
        public override void SetDefaults()
        {
            Item.rare = -12;
            Item.autoReuse = false;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useTime = 20;
            Item.useAnimation = 20;
        }
        public override bool CanUseItem(Player player)
        {
            if (!TerrorbornSystem.obtainedShriekOfHorror)
            {
                TerrorbornSystem.obtainedShriekOfHorror = true;

                TerrorbornPlayer target = TerrorbornPlayer.modPlayer(player);
                target.TriggerAbilityAnimation("Shriek of Horror", "Hold the 'Shriek of Horror' mod hotkey to unleash a scream that generates terror while close to enemies.", "Doing so will slowly take away your health.", 0, "Special abilities and items will consume terror.");
            }
            else
            {
                TerrorbornSystem.obtainedShriekOfHorror = false;
            }
            return base.CanUseItem(player);
        }
    }
}
