using Terraria;
using Terraria.ModLoader;

namespace TerrorbornMod.Prefixes.Accessories
{
    class Shinobi : ModPrefix
    {
        public override bool Autoload(ref string name)
        {
            name = "Shinobi's";
            return base.Autoload(ref name);
        }

        public override PrefixCategory Category => PrefixCategory.Accessory;

        public override void SetDefaults()
        {

        }


        public override void Apply(Item item)
        {
            TerrorbornItem modItem = TerrorbornItem.modItem(item);
            modItem.allUseSpeedBoost = 1.03f;
        }

        public override void ModifyValue(ref float valueMult)
        {
            valueMult = 1.4982f;
            base.ModifyValue(ref valueMult);
        }
    }
}

