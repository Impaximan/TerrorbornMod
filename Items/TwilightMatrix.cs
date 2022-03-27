using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace TerrorbornMod.Items
{
    class TwilightMatrix : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("[c/FF1919:PERMANENTLY] enables [c/9438ff:Twilight Mode]" +
                "\nIn Twilight Mode, all bosses are given more difficult AI, and some are given increased stats" +
                //"\nCannot be used if you have already beaten a boss or if Shriek of Horror has already been obtained" +
                "\nAdditionally, enemy spawnrates are increased and numerous enemies are given new attacks" +
                "\nTerror is lost over time- having no terror worsens your stats and disables life regen" +
                "\nConsuming terror heals you, the amount of which can be increased by the life regen stat" +
                "\n[c/FF1919:Not recommended if this is your first time playing Terrorborn]" +
                "\n'Dragged by fate...'");
        }

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 32;
            item.rare = ItemRarityID.Pink;
            item.useAnimation = 30;
            item.useTime = 30;
            item.useStyle = ItemUseStyleID.HoldingUp;
            item.expert = true;
        }

        public override void UpdateInventory(Player player)
        {
        }

        //public override bool CanUseItem(Player player)
        //{
        //    return !(NPC.downedBoss1 || NPC.downedBoss2 || NPC.downedBoss3 || NPC.downedSlimeKing || NPC.downedQueenBee || NPC.downedMechBoss1 || NPC.downedMechBoss2 || NPC.downedMechBoss3 || TerrorbornWorld.downedInfectedIncarnate || TerrorbornWorld.downedTidalTitan || TerrorbornWorld.downedDunestock || TerrorbornWorld.downedShadowcrawler || TerrorbornWorld.downedPrototypeI || TerrorbornWorld.obtainedShriekOfHorror);
        //}

        public override bool UseItem(Player player)
        {
            TerrorbornWorld.TwilightMode = true;
            return base.UseItem(player);
        }
    }
}