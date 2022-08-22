using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TerrorbornMod.Abilities
{
    class TimeFreezeInfo : AbilityInfo
    {
        public override int typeInt()
        {
            return 7;
        }

        public override Texture2D texture()
        {
            return (Texture2D)ModContent.Request<Texture2D>("TerrorbornMod/Abilities/TimeFreeze_Icon");
        }

        public override float Cost()
        {
            return 65f;
        }

        public override bool HeldDown()
        {
            return false;
        }

        public override string Name()
        {
            return "Time Freeze";
        }

        public override string Description()
        {
            return "Freezes time for 4 seconds upon use." +
                "\nYou, however, are unaffected and can move freely." +
                "\nDuring the time freeze your item use speed is" +
                "\ndecreased.";
        }

        public override bool canUse(Player player)
        {
            return true;
        }

        public override void OnUse(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.TimeFreezeTime = 60 * 4;
        }

        public void DustExplosion(Vector2 position, int RectWidth, int Streams, float DustSpeed, int DustType, float DustScale = 1f, bool NoGravity = false) //Thank you once again Seraph
        {
            float currentAngle = Main.rand.Next(360);

            //if(Main.netMode!=1){
            for (int i = 0; i < Streams; ++i)
            {

                Vector2 direction = Vector2.Normalize(new Vector2(1, 1)).RotatedBy(MathHelper.ToRadians(((360 / Streams) * i) + currentAngle));
                direction.X *= DustSpeed;
                direction.Y *= DustSpeed;

                Dust dust = Dust.NewDustPerfect(position + (new Vector2(Main.rand.Next(RectWidth), Main.rand.Next(RectWidth))), DustType, direction, 0, default(Color), DustScale);
                if (NoGravity)
                {
                    dust.noGravity = true;
                }
            }
        }
    }

    class ObtainTimeFreeze : ModItem
    {
        public override string Texture => "TerrorbornMod/placeholder";

        public override bool IsLoadingEnabled(Mod mod)
        {
            return TerrorbornMod.IsInTestingMode;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Get Time Freeze");
            Tooltip.SetDefault("--UNOBTAINABLE TESTING ITEM--" +
                "\nUnlocks 'Time Freeze'" +
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
                if (tPlayer.unlockedAbilities.Contains(7))
                {
                    Main.NewText("You already have that ability, silly!");
                }
                else
                {
                    tPlayer.unlockedAbilities.Add(7);
                    Main.NewText("Unlocked 'Time Freeze'");
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

    class TimeFreeze : TerrorAbility
    {
        public override string TexturePath => "Abilities/TimeFreeze_Icon";

        public override bool HasLockedPosition()
        {
            return false;
        }

        public override Vector2 dimensions()
        {
            return new Vector2(24, 26);
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
            target.unlockedAbilities.Add(7);
            target.TriggerAbilityAnimation("Time Freeze", "Freezes time for 4 seconds at the cost of 65% terror", "You are mostly unaffected by this time freeze", 0, visibilityTime: 900);
        }
    }
}