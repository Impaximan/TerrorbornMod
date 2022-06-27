using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

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
                "\nTerror is lost over time while in combat; having less than 3% terror worsens your stats and disables life regen" +
                "\nConsuming terror fills up a twilight meter, which causes you to enter a twilight overload state for 10 seconds when filled" +
                "\nIn a twilight overload, you have increased movement speed, jump speed, attack speed, and attack damage" +
                "\nYou will also have increased life regen the more twilight you have" +
                "\nEnemies drop twice as much money, and you are given greatly increased luck" +
                "\nIn master mode, enemies have further buffed AI but your twilight overload is buffed as well" +
                "\n[c/FF1919:Not recommended if this is your first time playing Terrorborn]");
        }

        public override void SetDefaults()
        {
            Item.width = 44;
            Item.height = 56;
            Item.rare = ModContent.RarityType<Rarities.Twilight>();
            Item.useTime = 60;
            Item.useAnimation = 60;
            Item.autoReuse = false;
            Item.useStyle = ItemUseStyleID.HoldUp;
        }

        public override bool CanUseItem(Player player)
        {
            if (!TerrorbornSystem.TwilightMode)
            {
                //if (NPC.downedBoss1 || NPC.downedBoss2 || NPC.downedBoss3 || NPC.downedSlimeKing || NPC.downedQueenBee || NPC.downedMechBoss1 || NPC.downedMechBoss2 || NPC.downedMechBoss3 || TerrorbornWorld.downedInfectedIncarnate || TerrorbornWorld.downedTidalTitan || TerrorbornWorld.downedDunestock || TerrorbornWorld.downedShadowcrawler || TerrorbornWorld.downedPrototypeI || TerrorbornWorld.obtainedShriekOfHorror)
                //{
                //    return false;
                //}
                TerrorbornSystem.TwilightMode = true;
                TerrorbornSystem.TerrorThunder();
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
                SoundExtensions.PlaySoundOld(SoundID.Item67, player.Center);
                TerrorbornSystem.ScreenShake(5f);
                player.statLife -= (int)(player.statLifeMax2 * 0.15f);
                player.HealEffect((int)(player.statLifeMax2 * -0.15f));
            }
            return base.CanUseItem(player);
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            if (TerrorbornSystem.TwilightMode)
            {
                for (int i = 0; i < tooltips.Count; i++)
                {
                    if (i >= tooltips.Count)
                    {
                        break;
                    }
                    if (tooltips[i].Name.Contains("Tooltip") && tooltips[i].Mod == "Terraria")
                    {
                        tooltips.RemoveAt(i);
                        i--;
                    }
                }
                tooltips.Add(new TooltipLine(Mod, "TwilightMatrixTooltip", "Use to convert all currently stored terror into twilight at the cost of 15% of your max life"));
            }
        }
    }
}