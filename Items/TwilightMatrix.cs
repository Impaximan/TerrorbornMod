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
                "\nIn Twilight Mode, all bosses are given more difficult AI and increased health to compensate for the buffs to the player" +
                //"\nCannot be used if you have already beaten a boss or if Shriek of Horror has already been obtained" +
                "\nAdditionally, enemy spawnrates are increased and numerous enemies are given new attacks" +
                "\nTerror is lost over time; having less than 3% terror worsens your stats and disables life regen" +
                "\nConsuming terror fills up a twilight meter, which causes you to enter a twilight overload state for 10 seconds when filled" +
                "\nIn a twilight overload, you have increased movement speed, jump speed, attack speed, and attack damage, but it is harder to see" +
                "\nYou will also have increased life regen the more twilight you have" +
                "\nEnemies drop twice as much money, and you gain an extra accessory slot" +
                "\n[c/FF1919:Not recommended if this is your first time playing Terrorborn]" +
                "\n'Dragged by fate...'");
        }

        public override void SetDefaults()
        {
            item.width = 44;
            item.height = 56;
            item.rare = -12;
            item.useTime = 60;
            item.useAnimation = 60;
            item.autoReuse = false;
            item.useStyle = ItemUseStyleID.HoldingUp;
        }

        public override bool CanUseItem(Player player)
        {
            if (!TerrorbornWorld.TwilightMode)
            {
                //if (NPC.downedBoss1 || NPC.downedBoss2 || NPC.downedBoss3 || NPC.downedSlimeKing || NPC.downedQueenBee || NPC.downedMechBoss1 || NPC.downedMechBoss2 || NPC.downedMechBoss3 || TerrorbornWorld.downedInfectedIncarnate || TerrorbornWorld.downedTidalTitan || TerrorbornWorld.downedDunestock || TerrorbornWorld.downedShadowcrawler || TerrorbornWorld.downedPrototypeI || TerrorbornWorld.obtainedShriekOfHorror)
                //{
                //    return false;
                //}
                TerrorbornWorld.TwilightMode = true;
                TerrorbornMod.TerrorThunder();
                Main.NewText("Terrific power fills your foes...", new Color(148, 56, 255));
                Main.NewText("The twilight matrix transforms into something new...", new Color(148 / 2, 56 / 2, 255 / 2));
            }
            else
            {
                if (player.statLife <= (int)(player.statLifeMax2 * 0.15f))
                {
                    return false;
                }
                TerrorbornPlayer.modPlayer(player).TwilightPower += TerrorbornPlayer.modPlayer(player).TerrorPercent / 125f;
                TerrorbornPlayer.modPlayer(player).TerrorPercent = 0f;
                Main.PlaySound(SoundID.Item67, player.Center);
                TerrorbornMod.ScreenShake(5f);
                player.statLife -= (int)(player.statLifeMax2 * 0.15f);
                player.HealEffect((int)(player.statLifeMax2 * -0.15f));
            }
            return base.CanUseItem(player);
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            if (TerrorbornWorld.TwilightMode)
            {
                for (int i = 0; i < tooltips.Count; i++)
                {
                    if (i >= tooltips.Count)
                    {
                        break;
                    }
                    if (tooltips[i].Name.Contains("Tooltip") && tooltips[i].mod == "Terraria")
                    {
                        tooltips.RemoveAt(i);
                        i--;
                    }
                }
                tooltips.Add(new TooltipLine(mod, "TwilightMatrixTooltip", "Use to convert all currently stored terror into twilight at the cost of 15% of your max life"));
            }
        }
    }
}