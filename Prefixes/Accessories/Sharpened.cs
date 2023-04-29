using Terraria;
using Terraria.ModLoader;

namespace TerrorbornMod.Prefixes.Accessories
{
    class Sharpened : ModPrefix
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Sharpened");
            base.SetStaticDefaults();
        }

        public override PrefixCategory Category => PrefixCategory.Accessory;


        public override void Apply(Item item)
        {
            TerrorbornItem modItem = TerrorbornItem.modItem(item);
            modItem.critDamageBoost = 0.04f;
        }

        public override void ModifyValue(ref float valueMult)
        {
            valueMult = 1.4982f;
            base.ModifyValue(ref valueMult);
        }
    }
}

