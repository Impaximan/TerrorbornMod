﻿using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace TerrorbornMod.Abilities
{
    class HorrificAdaptationInfo : AbilityInfo
    {
        public override int typeInt()
        {
            return 1;
        }

        public override Texture2D texture()
        {
            return (Texture2D)ModContent.Request<Texture2D>("TerrorbornMod/Abilities/HorrificAdaptation_Icon");
        }

        public override float Cost()
        {
            return 30f;
        }

        public override bool HeldDown()
        {
            return true;
        }

        public override string Name()
        {
            return "Horrific Adaptation";
        }

        public override string Description()
        {
            return "Heals you over time while being used." +
                 "\nThe assigned hotkey has to be held down.";
        }

        int noiseCooldown = 20;
        public override void OnUse(Player player)
        {
            noiseCooldown--;
            if (noiseCooldown <= 0)
            {
                noiseCooldown = 20;
                SoundExtensions.PlaySoundOld(SoundID.Item72, player.Center);
                player.HealEffect(5);
                player.statLife += 5;
            }
        }
    }

    class ObtainHorrificAdaptation : ModItem
    {
        public override string Texture => "TerrorbornMod/placeholder";

        public override bool IsLoadingEnabled(Mod mod)
        {
            return TerrorbornMod.IsInTestingMode;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Get Horrific Adaptation");
            Tooltip.SetDefault("--UNOBTAINABLE TESTING ITEM--" +
                "\nUnlocks 'Horrific Adaptation'" +
                "\nRight click to get a list of unlocked abilities");
        }

        public override void SetDefaults()
        {
            Item.rare = -12;
            Item.autoReuse = false;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useTime = 20;
            Item.useAnimation = 20;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            TerrorbornPlayer tPlayer = TerrorbornPlayer.modPlayer(player);
            if (player.altFunctionUse != 2)
            {
                if (tPlayer.unlockedAbilities.Contains(1))
                {
                    Main.NewText("You already have that ability, silly!");
                }
                else
                {
                    tPlayer.unlockedAbilities.Add(1);
                    Main.NewText("Unlocked 'Horrific Adaptation'");
                }
            }
            else
            {
                if (tPlayer.unlockedAbilities.Count < 1)
                {
                    Main.NewText("No abilities currently unlocked!");
                }
                else
                {
                    for (int i = 0; i < tPlayer.unlockedAbilities.Count; i++)
                    {
                        Main.NewText(Utils.General.IntToAbility(tPlayer.unlockedAbilities[i]).Name());
                    }
                }
            }
            return base.CanUseItem(player);
        }
    }

    class HorrificAdaptation : TerrorAbility
    {
        public override string TexturePath => "Abilities/HorrificAdaptation_Icon";

        public override Vector2 lockedPosition()
        {
            return TerrorbornSystem.HorrificAdaptation;
        }

        public override Vector2 dimensions()
        {
            return new Vector2(28, 28);
        }

        public override Vector2 baseOffsets()
        {
            return new Vector2(219, 206);
        }

        public override float getScale()
        {
            return 1.25f;
        }

        public override void ActualAI()
        {
            Float(1.5f, 0.1f);
            UpdateObtainablity(32);
            //Vector2 textVector = Projectile.position - Main.player[Main.myPlayer].position;
            //Main.NewText(textVector.X + ", " + textVector.Y);
        }

        public override void ObtainAbility()
        {
            Projectile.active = false;

            TerrorbornPlayer target = TerrorbornPlayer.modPlayer(Main.player[Player.FindClosest(Projectile.position, Projectile.width, Projectile.height)]);
            target.unlockedAbilities.Add(1);
            target.TriggerAbilityAnimation("Horrific Adaptation", "Heals you over time while being used", "Costs 30% terror per second of use", 0);
        }
    }
}
