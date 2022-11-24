using Terraria;
using Terraria.ModLoader;

namespace TerrorbornMod.Prefixes.Accessories
{
    class Startling : ModPrefix
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Frightening");
            base.SetStaticDefaults();
        }

        public override PrefixCategory Category => PrefixCategory.Accessory;


        public override void Apply(Item item)
        {
            TerrorbornItem modItem = TerrorbornItem.modItem(item);
            modItem.ShriekSpeedMultiplier = 0.85f;
        }

        public override void ModifyValue(ref float valueMult)
        {
            valueMult = 1.4982f;
            base.ModifyValue(ref valueMult);
        }
    }
}
