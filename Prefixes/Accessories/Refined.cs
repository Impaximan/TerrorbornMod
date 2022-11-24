using Terraria;
using Terraria.ModLoader;

namespace TerrorbornMod.Prefixes.Accessories
{
    class Refined : ModPrefix
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
            modItem.critDamageBoost = 0.02f;
        }

        public override void ModifyValue(ref float valueMult)
        {
            valueMult = 1.15f;
            base.ModifyValue(ref valueMult);
        }
    }
}


