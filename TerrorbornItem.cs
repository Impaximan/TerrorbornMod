using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.World.Generation;
using Terraria.ModLoader;
using Terraria.UI;
using TerrorbornMod;
using Terraria.Map;
using Terraria.GameContent.Dyes;
using Terraria.GameContent.UI;

namespace TerrorbornMod
{
    class TerrorbornItem : GlobalItem
    {
        public override bool InstancePerEntity => true;

        public override bool CloneNewInstances => true;

        public float ShriekDrainBoost = 0f;
        public float ShriekSpeedMultiplier = 1f;

        public float TerrorCost = 0f;

        public bool restless = false;
        public bool guarenteedChargedUse = false;
        public int restlessChargeUpUses = 3;
        public float restlessTerrorDrain;
        public int restlessChargeUpCounter = 0;

        public bool burstJump = false;

        public bool parryShield = false;

        int azureCounter = 2;
        public override bool CanUseItem(Item item, Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            bool darkblood = player.HasBuff(ModContent.BuffType<Buffs.Darkblood>());
            if (player.HasBuff(ModContent.BuffType<Buffs.GraniteSpark>()) || (modPlayer.MidShriek && !darkblood) || !modPlayer.canUseItems)
            {
                return false;
            }
            if (modPlayer.AzuriteArmorBonus && item.magic && base.CanUseItem(item, player) && player.statMana >= item.mana)
            {
                if (azureCounter <= 0)
                {
                    azureCounter = 2;
                    float speed = 22.5f;
                    Vector2 velocity = player.DirectionTo(Main.MouseWorld) * speed;

                    Projectile.NewProjectile(player.Center, velocity, ModContent.ProjectileType<Items.Equipable.Armor.azuriteShockwave>(), item.damage / 3, 30, player.whoAmI);
                }
                else
                {
                    azureCounter--;
                }
            }
            if (TerrorCost > 0f)
            {
                if (modPlayer.TerrorPercent <= TerrorCost)
                {
                    return false;
                }
                else if (base.CanUseItem(item, player))
                {
                    modPlayer.LoseTerror(TerrorCost);
                }
            }
            return base.CanUseItem(item, player);
        }

        public override bool ConsumeAmmo(Item item, Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            if (Main.rand.NextFloat() < modPlayer.noAmmoConsumeChance)
            {
                return false;
            }
            return base.ConsumeAmmo(item, player);
        }

        public bool RestlessChargedUp()
        {
            return restlessChargeUpCounter <= 0;
        }

        public override void PostReforge(Item item)
        {
            if (item.prefix != ModContent.PrefixType<Prefixes.Accessories.Frightening>())
            {
                ShriekDrainBoost = 0f;
            }

            if (item.prefix != ModContent.PrefixType<Prefixes.Accessories.Startling>())
            {
                ShriekSpeedMultiplier = 1f;
            }
            if (item.prefix != ModContent.PrefixType<Prefixes.Weapons.Nightmarish>())
            {
                TerrorCost = 0f;
            }
        }

        public override bool UseItem(Item item, Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            return base.UseItem(item, player);
        }

        public override void UpdateEquip(Item item, Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            if (ShriekDrainBoost != 0f)
            {
                modPlayer.ShriekTerrorMultiplier += ShriekDrainBoost;
            }
            if (ShriekSpeedMultiplier != 1f)
            {
                modPlayer.ShriekSpeed *= ShriekSpeedMultiplier;
            }
        }

        public override void RightClick(Item item, Player player)
        {
            int index = 3;
            Item accessory = null;
            if (burstJump)
            {
                int maxAccessoryIndex = 5 + Main.LocalPlayer.extraAccessorySlots;
                for (int i = 3; i < 3 + maxAccessoryIndex; i++)
                {
                    if ((Main.LocalPlayer.armor[i] != null && !Main.LocalPlayer.armor[i].IsAir) || Main.LocalPlayer.armor[i].type == item.type)
                    {
                        index = i;
                        accessory = Main.LocalPlayer.armor[i];
                    }
                }

                if (accessory != null)
                {
                    Main.LocalPlayer.armor[index] = item.Clone();
                    for (int i = 0; i < player.inventory.Length; i++)
                    {
                        if (player.inventory[i] == item)
                        {
                            index = i;
                        }
                    }
                    player.inventory[index] = accessory;
                }
            }
            base.RightClick(item, player);
        }

        public override void SetDefaults(Item item)
        {

        }

        public static TerrorbornItem modItem(Item item)
        {
            return item.GetGlobalItem<TerrorbornItem>();
        }

        bool equippedInVanity = false;

        public static Color restlessColor1 = Color.FromNonPremultiplied(40 * 2, 55 * 2, 70 * 2, 255);
        public static Color restlessColor2 = Color.FromNonPremultiplied(128, 75, 135, 255);
        public static float restlessColorProgress = 0f;
        public static int restlessColorDirection = 1;
        int restlessTransitionTime = 60;

        public static Color burstJumpColor1 = Color.FromNonPremultiplied(61, 255, 83, 255);
        public static Color burstJumpColor2 = Color.FromNonPremultiplied(189, 42, 255, 255);
        public static float burstJumpColorProgress = 0f;
        public static int burstJumpColorDirection = 1;
        int burstJumpTransitionTime = 120;

        public static Color shieldColor1 = Color.FromNonPremultiplied(186, 228, 233, 255);
        public static Color shieldColor2 = Color.FromNonPremultiplied(255, 239, 207, 255);
        public static float shieldColorProgress = 0f;
        public static int shieldColorDirection = 1;
        int shieldTransitionTime = 120;

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            TooltipLine line = tooltips.FirstOrDefault(x => x.Name == "ItemName" && x.mod == "Terraria");
            if (restless)
            {
                tooltips.Insert(1, new TooltipLine(mod, "restlessItem", "--Restless Weapon--"));
                tooltips.FirstOrDefault(x => x.Name == "restlessItem" && x.mod == "TerrorbornMod").overrideColor = Color.Lerp(restlessColor1, restlessColor2, restlessColorProgress);
            }

            restlessColorProgress += (1f / restlessTransitionTime) * restlessColorDirection;

            if (restlessColorDirection == 1 && restlessColorProgress >= 1f)
            {
                restlessColorDirection *= -1;
            }

            if (restlessColorDirection == -1 && restlessColorProgress <= 0f)
            {
                restlessColorDirection *= -1;
            }

            if (burstJump)
            {
                tooltips.Insert(1, new TooltipLine(mod, "burstJumpItem", "--Burst Jump--"));
                tooltips.FirstOrDefault(x => x.Name == "burstJumpItem" && x.mod == "TerrorbornMod").overrideColor = Color.Lerp(burstJumpColor1, burstJumpColor2, burstJumpColorProgress);
            }

            burstJumpColorProgress += (1f / burstJumpTransitionTime) * burstJumpColorDirection;

            if (burstJumpColorDirection == 1 && burstJumpColorProgress >= 1f)
            {
                burstJumpColorDirection *= -1;
            }

            if (burstJumpColorDirection == -1 && burstJumpColorProgress <= 0f)
            {
                burstJumpColorDirection *= -1;
            }

            if (parryShield)
            {
                tooltips.Insert(1, new TooltipLine(mod, "shieldItem", "--Parry Shield--"));
                tooltips.FirstOrDefault(x => x.Name == "shieldItem" && x.mod == "TerrorbornMod").overrideColor = Color.Lerp(shieldColor1, shieldColor2, shieldColorProgress);
            }

            shieldColorProgress += (1f / shieldTransitionTime) * shieldColorDirection;

            if (shieldColorDirection == 1 && shieldColorProgress >= 1f)
            {
                shieldColorDirection *= -1;
            }

            if (shieldColorDirection == -1 && shieldColorProgress <= 0f)
            {
                shieldColorDirection *= -1;
            }

            line = tooltips.FirstOrDefault(x => x.Name == "Social" && x.mod == "Terraria");
            if (ShriekDrainBoost != 0f && line == null)
            {
                tooltips.Add(new TooltipLine(mod, "shriekterrorprefix", "+" + (ShriekDrainBoost * 100f) + "% Shriek of Horror terror drain"));
                tooltips.FirstOrDefault(x => x.Name == "shriekterrorprefix" && x.mod == "TerrorbornMod").isModifier = true;
            }
            if (ShriekSpeedMultiplier != 1f && line == null)
            {
                bool isBad = true;
                string changeText = "+" + (ShriekSpeedMultiplier - 1) * 100f;
                if (ShriekSpeedMultiplier < 1f)
                {
                    changeText = ((ShriekSpeedMultiplier - 1) * 100f).ToString();
                    isBad = false;
                }

                tooltips.Add(new TooltipLine(mod, "shriekspeedprefix", changeText + "% Shriek of Horror charge up time"));
                tooltips.FirstOrDefault(x => x.Name == "shriekspeedprefix" && x.mod == "TerrorbornMod").isModifier = true;
                tooltips.FirstOrDefault(x => x.Name == "shriekspeedprefix" && x.mod == "TerrorbornMod").isModifierBad = isBad;
            }
            if (TerrorCost > 0f)
            {
                string changeText = "+" + TerrorCost;
                tooltips.Add(new TooltipLine(mod, "terrorcostprefix", changeText + "% terror required per use"));
                tooltips.FirstOrDefault(x => x.Name == "terrorcostprefix" && x.mod == "TerrorbornMod").isModifier = true;
                tooltips.FirstOrDefault(x => x.Name == "terrorcostprefix" && x.mod == "TerrorbornMod").isModifierBad = true;
            }
        }

        public override void OnHitNPC(Item item, Player player, NPC target, int damage, float knockBack, bool crit)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            if (crit && modPlayer.SangoonBand && target.type != NPCID.TargetDummy)
            {
                if (modPlayer.SangoonBandCooldown <= 0)
                {
                    player.HealEffect(1);
                    player.statLife += 1;
                    modPlayer.SangoonBandCooldown = 20;
                }
            }
        }
    }
}
