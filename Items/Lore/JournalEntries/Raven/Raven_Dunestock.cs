using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TerrorbornMod.Items.Lore.JournalEntries.Raven
{
    class Raven_Dunestock : ModItem
    {
        public override string Texture => "TerrorbornMod/TornPage";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Torn Page");
            Tooltip.SetDefault(TerrorbornUtils.AutoSortTooltip("I, king of Anekronyx, write to you, my fellow leaders, to warn you of oncoming attack from creatures of the desert. I fear that they will, unfortunately, halt our trade routes, and thus we will be unable to continue porting our food. Do not take this as a sign of hostility, but of peace, as we do not want your traders to be harmed further by.. whatever it is that has been inhabiting the desert. I will send this to you over the desert, and simply hope that it makes it through; I've sent my greatest warriors to deliver this. If they don't make it... then... well, you wouldn't be reading this."));
        }
        public override void SetDefaults()
        {
            item.value = 0;
            item.rare = -11;
        }
    }
}
