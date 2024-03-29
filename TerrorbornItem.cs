﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace TerrorbornMod
{
    class TerrorbornItem : GlobalItem
    {
        public override bool InstancePerEntity => true;


        public float ShriekDrainBoost = 0f;
        public float ShriekSpeedMultiplier = 1f;
        public float critDamageBoost = 0f;
        public float allUseSpeedBoost = 1f;

        public float TerrorCost = 0f;

        public bool restless = false;
        public bool guarenteedChargedUse = false;
        public int restlessChargeUpUses = 3;
        public float restlessTerrorDrain;
        public int restlessChargeUpCounter = 0;

        public float critDamageMult = 1f;

        public bool burstJump = false;

        public bool countAsThrown = false;

        public bool parryShield = false;

        public float terrorPotionTerror = 0f;

        public Color meterColor = Color.White;

        public override void ModifyWeaponDamage(Item item, Player player, ref StatModifier damage)
        {
            base.ModifyWeaponDamage(item, player, ref damage);
            if (countAsThrown)
            {
                damage.Scale(player.GetDamage(DamageClass.Throwing).Multiplicative);
            }
        }

        public override GlobalItem Clone(Item item, Item itemClone)
        {
            return base.Clone(item, itemClone);
        }

        public override void ModifyWeaponCrit(Item item, Player player, ref float crit)
        {
            if (item.DamageType == DamageClass.Melee && countAsThrown)
            {
                if (player.GetCritChance(DamageClass.Throwing) > player.GetCritChance(DamageClass.Melee))
                {
                    crit = player.GetCritChance(DamageClass.Throwing) + item.crit;
                    return;
                }
            }
            if (item.DamageType == DamageClass.Ranged && countAsThrown)
            {
                if (player.GetCritChance(DamageClass.Throwing) > player.GetCritChance(DamageClass.Ranged))
                {
                    crit = player.GetCritChance(DamageClass.Throwing) + item.crit;
                    return;
                }
            }
        }

        int azureCounter = 2;
        public override bool CanUseItem(Item item, Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);

            bool darkblood = player.HasBuff(ModContent.BuffType<Buffs.Darkblood>());
            if (player.HasBuff(ModContent.BuffType<Buffs.GraniteSpark>()) || (modPlayer.MidShriek && !darkblood) || !modPlayer.canUseItems)
            {
                return false;
            }

            if (modPlayer.AzuriteArmorBonus && item.DamageType == DamageClass.Magic && base.CanUseItem(item, player) && player.statMana >= item.mana)
            {
                if (azureCounter <= 0)
                {
                    azureCounter = 2;
                    float speed = 22.5f;
                    Vector2 velocity = player.DirectionTo(Main.MouseWorld) * speed;

                    Projectile.NewProjectile(player.GetSource_ItemUse(item), player.Center, velocity, ModContent.ProjectileType<Items.Equipable.Armor.AzuriteShockwave>(), item.damage / 3, 30, player.whoAmI);
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

        public override bool CanConsumeAmmo(Item weapon, Item ammo, Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            if (Main.rand.NextFloat() < modPlayer.noAmmoConsumeChance)
            {
                return false;
            }
            return base.CanConsumeAmmo(weapon, ammo, player);
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

            if (item.prefix != ModContent.PrefixType<Prefixes.Accessories.Sharpened>() && item.prefix != ModContent.PrefixType<Prefixes.Accessories.Refined>())
            {
                critDamageBoost = 0f;
            }

            if (item.prefix != ModContent.PrefixType<Prefixes.Accessories.Shinobi>())
            {
                allUseSpeedBoost = 1f;
            }
        }

        public override bool? UseItem(Item item, Player player)
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
            if (critDamageBoost != 0f)
            {
                modPlayer.critDamage += critDamageBoost;
            }
            if (ShriekSpeedMultiplier != 1f)
            {
                modPlayer.ShriekSpeed *= ShriekSpeedMultiplier;
            }
            if (allUseSpeedBoost != 1f)
            {
                player.GetAttackSpeed(DamageClass.Generic) *= allUseSpeedBoost;
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
            bool canBeThrown = true;
            if (item.ModItem != null)
            {
                if (item.ModItem.Mod.Name != "TerrorbornMod" && item.ModItem.Mod.Name != "Terraria")
                {
                    canBeThrown = TerrorbornMod.thrownAffectsMods;
                }
            }

            if (item.ModItem == null && item.useStyle == ItemUseStyleID.Rapier)
            {
                critDamageMult = 1.5f;
            }

            if (item.type == ItemID.CrystalVileShard || item.type == ItemID.FetidBaghnakhs)
            {
                critDamageMult = 1.15f;
            }

            if (item.type == ItemID.Flamethrower || item.type == ItemID.PoisonStaff || item.type == ItemID.ShadowbeamStaff)
            {
                critDamageMult = 1.3f;
            }

            if (item.type == ItemID.LastPrism)
            {
                item.damage = 65;
            }

            if (item.type == ItemID.Zenith)
            {
                item.damage = (int)(item.damage * 0.48f);
            }

            if (canBeThrown)
            {
                if (item.DamageType == DamageClass.Ranged)
                {
                    if (item.consumable && (item.useStyle == ItemUseStyleID.Swing || item.noUseGraphic))
                    {
                        countAsThrown = true;
                    }
                }

                if (item.DamageType == DamageClass.Melee)
                {
                    if (item.consumable || (item.noUseGraphic && item.noMelee && !item.channel && item.useStyle == ItemUseStyleID.Swing))
                    {
                        countAsThrown = true;
                    }
                }
            }

            List<int> otherThrownItems = new List<int>() {
                ItemID.ScourgeoftheCorruptor,
                ModContent.ItemType<Items.Weapons.Ranged.Thrown.PyroclasticKunai>(),
            };

            if (otherThrownItems.Contains(item.type))
            {
                countAsThrown = true;
            }

            List<int> thrownItemsBlacklist = new List<int>() {
                ItemID.CopperCoin,
                ItemID.SilverCoin,
                ItemID.GoldCoin,
                ItemID.PlatinumCoin
            };

            if (thrownItemsBlacklist.Contains(item.type))
            {
                countAsThrown = false;
            }

            if (item.ModItem != null && item.ModItem.Mod.Name == "TerrorbornMod")
            {
                if (item.consumable && item.maxStack > 1)
                {
                    if (item.maxStack > 20)
                    {
                        if (item.maxStack >= 100)
                        {
                            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[item.type] = 100;
                        }
                        else
                        {
                            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[item.type] = item.maxStack;
                        }
                    }
                    else
                    {
                        CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[item.type] = 20;
                    }
                }
                else
                {
                    CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[item.type] = 1;
                }
            }
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
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(Main.LocalPlayer);

            TooltipLine line = tooltips.FirstOrDefault(x => x.Name == "ItemName" && x.Mod == "Terraria");

            if (restless)
            {
                tooltips.Insert(1, new TooltipLine(Mod, "restlessItem", "--Restless Weapon--"));
                tooltips.FirstOrDefault(x => x.Name == "restlessItem" && x.Mod == "TerrorbornMod").OverrideColor = Color.Lerp(restlessColor1, restlessColor2, restlessColorProgress);
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
                tooltips.Insert(1, new TooltipLine(Mod, "burstJumpItem", "--Burst Jump--"));
                tooltips.FirstOrDefault(x => x.Name == "burstJumpItem" && x.Mod == "TerrorbornMod").OverrideColor = Color.Lerp(burstJumpColor1, burstJumpColor2, burstJumpColorProgress);
            }

            if (item.type == ItemID.IronPickaxe || item.type == ItemID.LeadPickaxe || item.type == ItemID.TungstenPickaxe || item.type == ItemID.SilverPickaxe || item.type == ItemID.PlatinumPickaxe || item.type == ItemID.GoldPickaxe)
            {
                tooltips.Add(new TooltipLine(Mod, "MineNovagold", "Can mine novagold ore"));
                tooltips.FirstOrDefault(x => x.Name == "MineNovagold" && x.Mod == "TerrorbornMod").OverrideColor = new Color(207, 253, 255);
            }

            if (item.type == ItemID.MythrilPickaxe || item.type == ItemID.OrichalcumPickaxe)
            {
                tooltips.Add(new TooltipLine(Mod, "MineAlloy", "Can mine incendiary alloy in the Sisyphean Islands"));
                tooltips.FirstOrDefault(x => x.Name == "MineAlloy" && x.Mod == "TerrorbornMod").OverrideColor = new Color(255, 211, 207);
            }

            if (item.type == ItemID.Picksaw)
            {
                tooltips.Add(new TooltipLine(Mod, "MineSkullmound", "Can mine skullmound ore in the Sisyphean Islands"));
                tooltips.FirstOrDefault(x => x.Name == "MineSkullmound" && x.Mod == "TerrorbornMod").OverrideColor = new Color(255, 211, 207);
            }

            if (item.OriginalRarity == ModContent.RarityType<Rarities.Twilight>())
            {
                tooltips.Add(new TooltipLine(Mod, "TwilightRarity", "'Dragged by fate...'"));
                tooltips.FirstOrDefault(x => x.Name == "TwilightRarity" && x.Mod == "TerrorbornMod").OverrideColor = Color.FromNonPremultiplied(40 * 2, 55 * 2, 70 * 2, 255);
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
                tooltips.Insert(1, new TooltipLine(Mod, "shieldItem", "--Parry Shield--"));
                tooltips.FirstOrDefault(x => x.Name == "shieldItem" && x.Mod == "TerrorbornMod").OverrideColor = Color.Lerp(shieldColor1, shieldColor2, shieldColorProgress);
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

            line = tooltips.FirstOrDefault(x => x.Name == "Social" && x.Mod == "Terraria");

            if (ShriekDrainBoost != 0f && line == null)
            {
                tooltips.Add(new TooltipLine(Mod, "shriekterrorprefix", "+" + (ShriekDrainBoost * 100f) + "% Shriek of Horror terror drain"));
                tooltips.FirstOrDefault(x => x.Name == "shriekterrorprefix" && x.Mod == "TerrorbornMod").IsModifier = true;
            }

            if (critDamageBoost != 0f && line == null)
            {
                tooltips.Add(new TooltipLine(Mod, "critdamageprefix", "+" + (critDamageBoost * 100f) + "% critical damage"));
                tooltips.FirstOrDefault(x => x.Name == "critdamageprefix" && x.Mod == "TerrorbornMod").IsModifier = true;
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

                tooltips.Add(new TooltipLine(Mod, "shriekspeedprefix", changeText + "% Shriek of Horror charge up time"));
                tooltips.FirstOrDefault(x => x.Name == "shriekspeedprefix" && x.Mod == "TerrorbornMod").IsModifier = true;
                tooltips.FirstOrDefault(x => x.Name == "shriekspeedprefix" && x.Mod == "TerrorbornMod").IsModifierBad = isBad;
            }

            if (allUseSpeedBoost != 1f && line == null)
            {
                bool isBad = false;
                string changeText = "+3";
                if (allUseSpeedBoost < 1f)
                {
                    changeText = ((allUseSpeedBoost - 1f) * 100f).ToString();
                    isBad = true;
                }

                tooltips.Add(new TooltipLine(Mod, "usespeedprefix", changeText + "% item use speed"));
                tooltips.FirstOrDefault(x => x.Name == "usespeedprefix" && x.Mod == "TerrorbornMod").IsModifier = true;
                tooltips.FirstOrDefault(x => x.Name == "usespeedprefix" && x.Mod == "TerrorbornMod").IsModifierBad = isBad;
            }

            if (TerrorCost > 0f)
            {
                string changeText = "+" + TerrorCost;
                tooltips.Add(new TooltipLine(Mod, "terrorcostprefix", changeText + "% terror required per use"));
                tooltips.FirstOrDefault(x => x.Name == "terrorcostprefix" && x.Mod == "TerrorbornMod").IsModifier = true;
                tooltips.FirstOrDefault(x => x.Name == "terrorcostprefix" && x.Mod == "TerrorbornMod").IsModifierBad = true;
            }

            if (terrorPotionTerror > 0f && !TerrorbornSystem.obtainedShriekOfHorror)
            {
                int index = tooltips.FindIndex(x => x.Name == "Tooltip0" && x.Mod == "Terraria");
                tooltips.RemoveAt(index);
                tooltips.Insert(index, new TooltipLine(Mod, "noUseTerrorPotion", "A flask containing a strange substance" +
                    "\n'Can only be used by those who can shriek'"));
                for (int i = 0; i < tooltips.Count; i++)
                {
                    TooltipLine line2 = tooltips[i];

                    if (line2.Name.Contains("Tooltip"))
                    {
                        tooltips.RemoveAt(i);
                        i--;
                    }

                    if (i >= tooltips.Count)
                    {
                        break;
                    }
                }
            }

            if (TerrorbornMod.showCritDamage && item.damage != 0 && !item.accessory)
            {
                if (tooltips.FirstOrDefault(x => x.Name == "CritChance" && x.Mod == "Terraria") != null)
                {
                    tooltips.Insert(tooltips.FindIndex(x => x.Name == "CritChance" && x.Mod == "Terraria"), new TooltipLine(Mod, "CritDamage", Math.Round(200f * critDamageMult * modPlayer.critDamage).ToString() + "% critical strike damage"));
                    if (critDamageMult != 1f)
                    {
                        tooltips.FirstOrDefault(x => x.Name == "CritDamage" && x.Mod == "TerrorbornMod").OverrideColor = Color.FromNonPremultiplied(255, 251, 168, 255);
                    }
                }
            }

            if (TerrorbornMod.showWingStats && item.wingSlot != -1)
            {
                float shownFlightTime = (float)Math.Round(ArmorIDs.Wing.Sets.Stats[item.wingSlot].FlyTime / 60f * modPlayer.flightTimeMultiplier, 1);
                tooltips.Add(new TooltipLine(Mod, "WingStats1", shownFlightTime.ToString() + " second flight time"));
                tooltips.Add(new TooltipLine(Mod, "WingStats1", GetWingSpeedMultRating(ArmorIDs.Wing.Sets.Stats[item.wingSlot].AccRunAccelerationMult) + " horizontal speed"));
            }

            if (countAsThrown)
            {
                tooltips.Add(new TooltipLine(Mod, "countsAsThrown", "Counts as a thrown weapon"));
                tooltips.FirstOrDefault(x => x.Name == "countsAsThrown" && x.Mod == "TerrorbornMod").OverrideColor = new Color(193, 243, 245);
                //tooltips.Insert(tooltips.FindIndex(x => x.Name == "CritChance" && x.mod == "Terraria") + 1, new TooltipLine(mod, "thrownCrit", (Item.crit + Main.LocalPlayer.thrownCrit).ToString() + "% thrown critical strike chance" +
                //    "\nThrown crit chance replaces regular crit chance every other hit"));
            }

            if (item.useTime <= 5 && TerrorbornMod.showNoUseSpeed)
            {
                tooltips.Add(new TooltipLine(Mod, "noUseSpeed", "Unaffected by external item use speed modifiers"));
            }

            if (item.type == ItemID.CrystalDart || item.type == ItemID.IchorDart)
            {
                tooltips.Add(new TooltipLine(Mod, "reducedDartDamage", "Does reduced damage when used with Terrorborn weapons"));
            }
        }

        public string GetWingSpeedMultRating(float mult, bool capitalized = true)
        {
            if (mult < 0.9f)
            {
                if (capitalized)
                {
                    return "Awful";
                }
                return "awful";
            }

            if (mult >= 0.9f && mult < 1.3f)
            {
                if (capitalized)
                {
                    return "Average";
                }
                return "average";
            }


            if (mult >= 1.25f && mult < 1.75f)
            {
                if (capitalized)
                {
                    return "Fast";
                }
                return "fast";
            }

            if (mult >= 1.75f && mult < 2.25f)
            {
                if (capitalized)
                {
                    return "Very fast";
                }
                return "very fast";
            }


            if (mult >= 2.25f && mult < 2.75f)
            {
                if (capitalized)
                {
                    return "Extremely fast";
                }
                return "extremely fast";
            }

            if (mult >= 2.75f)
            {
                if (capitalized)
                {
                    return "Insanely fast";
                }
                return "insanely fast";
            }

            return "meh";
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
