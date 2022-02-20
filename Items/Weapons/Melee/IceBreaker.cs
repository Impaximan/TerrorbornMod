using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace TerrorbornMod.Items.Weapons.Melee
{
    public class IceBreaker : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Guarenteed crit on undamaged enemies");
        }

        public override void SetDefaults()
        {
            TerrorbornItem modItem = TerrorbornItem.modItem(item);
            modItem.critDamageMult = 1.5f;
            item.crit = 10;
            item.damage = 22;
            item.melee = true;
            item.width = 70;
            item.height = 66;
            item.useTime = 20;
            item.useAnimation = 20;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.knockBack = 4f;
            item.value = Item.sellPrice(0, 0, 15, 0);
            item.rare = ItemRarityID.Blue;
            item.UseSound = SoundID.Item71;
            item.autoReuse = true;
        }

        public override void ModifyHitNPC(Player player, NPC target, ref int damage, ref float knockBack, ref bool crit)
        {
            if (target.life == target.lifeMax)
            {
                crit = true;
            }
        }
    }
}