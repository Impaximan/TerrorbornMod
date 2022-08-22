using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TerrorbornMod.Abilities
{
    class TerrorWarpInfo : AbilityInfo
    {
        public override int typeInt()
        {
            return 2;
        }

        public override Texture2D texture()
        {
            return (Texture2D)ModContent.Request<Texture2D>("TerrorbornMod/Abilities/TerrorWarp_Icon");
        }

        public override float Cost()
        {
            return 25f;
        }

        public override bool HeldDown()
        {
            return false;
        }

        public override string Name()
        {
            return "Terror Warp";
        }

        public override string Description()
        {
            return "Teleports you to the mouse cursor's location." +
                 "\nCannot teleport you through tiles.";
        }

        public override bool canUse(Player player)
        {
            return Collision.CanHit(player.position, player.width, player.height, Main.MouseWorld, 0, 0);
        }

        public override void OnUse(Player player)
        {
            player.Teleport(Main.MouseWorld - new Vector2(player.width / 2, player.height / 2));
        }
    }

    class ObtainTerrorWarp : ModItem
    {
        public override string Texture => "TerrorbornMod/placeholder";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Get Terror Warp");
            Tooltip.SetDefault("--UNOBTAINABLE TESTING ITEM--" +
                "\nUnlocks 'Terror Warp'" +
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
                if (tPlayer.unlockedAbilities.Contains(2))
                {
                    Main.NewText("You already have that ability, silly!");
                }
                else
                {
                    tPlayer.unlockedAbilities.Add(2);
                    Main.NewText("Unlocked 'Terror Warp'");
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
                        Main.NewText(Utils.General.intToAbility(tPlayer.unlockedAbilities[i]).Name());
                    }
                }
            }
            return base.CanUseItem(player);
        }
    }
    class TerrorWarp : TerrorAbility
    {
        public override string TexturePath => "Abilities/TerrorWarp_Icon";

        public override Vector2 lockedPosition()
        {
            return TerrorbornSystem.TerrorWarp * 16;
        }

        public override Vector2 dimensions()
        {
            return new Vector2(26, 28);
        }

        public override Vector2 baseOffsets()
        {
            return new Vector2(216, 206);
        }

        public override float getScale()
        {
            return 1.25f;
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
            target.unlockedAbilities.Add(2);
            target.TriggerAbilityAnimation("Terror Warp", "Teleports you to your mouse cursor, but can't teleport you through tiles", "Costs 20% terror to use", 0, visibilityTime: 700);
        }
    }
}

