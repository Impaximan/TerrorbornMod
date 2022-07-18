using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TerrorbornMod.Abilities
{
    class HiddenInstinctInfo : AbilityInfo
    {
        public override int typeInt()
        {
            return 9;
        }

        public override Texture2D texture()
        {
            return (Texture2D)ModContent.Request<Texture2D>("TerrorbornMod/Abilities/HiddenInstinct_Icon");
        }

        public override float Cost()
        {
            return 50f;
        }

        public override bool HeldDown()
        {
            return false;
        }

        public override string Name()
        {
            return "Hidden Instinct";
        }

        public override string Description()
        {
            return "Gives you the following buffs for 10 seconds:" +
                "\nView of tiles, traps, and enemies" +
                "\nDoubled mining speed" +
                "\nIncreased jump height and movement speed";
        }

        public override bool canUse(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            return modPlayer.HiddenInstinctTime <= 0;
        }

        public override void OnUse(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.HiddenInstinctTime = 60 * 10;
            SoundExtensions.PlaySoundOld(SoundID.Item117);
        }
    }

    class ObtainHiddenInstinct : ModItem
    {
        public override string Texture => "TerrorbornMod/placeholder";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Get Hidden Instinct");
            Tooltip.SetDefault("--UNOBTAINABLE TESTING ITEM--" +
                "\nUnlocks 'Hidden Instinct'" +
                "\nRight click to get a list of unlocked abilities");
        }
        public override bool IsLoadingEnabled(Mod mod)
        {
            return TerrorbornMod.IsInTestingMode;
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
                if (tPlayer.unlockedAbilities.Contains(9))
                {
                    Main.NewText("You already have that ability, silly!");
                }
                else
                {
                    tPlayer.unlockedAbilities.Add(9);
                    Main.NewText("Unlocked 'Hidden Instinct'");
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
                        Main.NewText(TerrorbornUtils.intToAbility(tPlayer.unlockedAbilities[i]).Name());
                    }
                }
            }
            return base.CanUseItem(player);
        }
    }

    class HiddenInstinct : TerrorAbility
    {
        public override string TexturePath => "Abilities/HiddenInstinct_Icon";

        public override bool HasLockedPosition()
        {
            return false;
        }

        public override Vector2 dimensions()
        {
            return new Vector2(26, 24);
        }

        public override Vector2 baseOffsets()
        {
            return new Vector2(0, 0);
        }

        public override float getScale()
        {
            return 1.5f;
        }

        public override void ActualAI()
        {
            Float(1.5f, 0.1f);
            UpdateObtainablity(32);
        }

        public override void ObtainAbility()
        {
            Projectile.active = false;

            TerrorbornPlayer target = TerrorbornPlayer.modPlayer(Main.player[Player.FindClosest(Projectile.position, Projectile.width, Projectile.height)]);
            target.unlockedAbilities.Add(9);
            target.TriggerAbilityAnimation("Hidden Instinct", "Grants you vision of ore and danger and increases your mining efficiency for 10 seconds", "Consumes 50% terror", 0, visibilityTime: 800);
        }
    }
}


