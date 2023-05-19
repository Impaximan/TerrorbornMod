using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Weapons.Melee.Swords
{
    public class IceBreaker : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("Guarenteed crit on undamaged enemies");
        }

        public override void SetDefaults()
        {
            TerrorbornItem modItem = TerrorbornItem.modItem(Item);
            modItem.critDamageMult = 1.5f;
            Item.crit = 10;
            Item.damage = 13;
            Item.DamageType = DamageClass.Melee;
            Item.width = 70;
            Item.height = 66;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 4f;
            Item.value = Item.sellPrice(0, 0, 15, 0);
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item71;
            Item.autoReuse = true;
        }

        public override void ModifyHitNPC(Player player, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (target.life == target.lifeMax)
            {
                modifiers.SetCrit();
            }
        }
    }
}