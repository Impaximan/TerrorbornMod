using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TerrorbornMod.Abilities
{
    class BlinkDashInfo : AbilityInfo
    {
        public override int typeInt()
        {
            return 8;
        }

        public override Texture2D texture()
        {
            return (Texture2D)ModContent.Request<Texture2D>("TerrorbornMod/Abilities/BlinkDash_Icon");
        }

        public override float Cost()
        {
            return 10f;
        }

        public override bool HeldDown()
        {
            return false;
        }

        public override string Name()
        {
            return "Blink Dash";
        }

        public override string Description()
        {
            return "Performs a blink dash, allowing you to warp past foes" +
                "\nand confuse them." +
                "\nDirection depends on movement keys." +
                "\nPress the ability hotkey again right before you exit the" +
                "\ndash to launch yourself forward.";
        }

        public override bool canUse(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            return (player.controlRight || player.controlLeft || player.controlUp || player.controlDown) && modPlayer.BlinkDashTime <= 0 && modPlayer.BlinkDashCooldown <= 0;
        }

        public override void OnUse(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.BlinkDashTime = 20;
            float speed = 15f;

            SoundExtensions.PlaySoundOld(SoundID.Item72, player.Center);
            DustExplosion(player.Center, 0, 15, 30, 162, 1.5f, true);

            if (player.controlRight)
            {
                modPlayer.BlinkDashVelocity.X = speed;
            }
            else if (player.controlLeft)
            {
                modPlayer.BlinkDashVelocity.X = -speed;
            }
            else
            {
                modPlayer.BlinkDashVelocity.X = 0;
            }

            if (player.controlDown)
            {
                modPlayer.BlinkDashVelocity.Y = speed;
            }
            else if (player.controlUp)
            {
                modPlayer.BlinkDashVelocity.Y = -speed;
            }
            else
            {
                modPlayer.BlinkDashVelocity.Y = 0;
            }
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

    class ObtainBlinkDash : ModItem
    {
        public override string Texture => "TerrorbornMod/placeholder";

        public override bool IsLoadingEnabled(Mod mod)
        {
            return TerrorbornMod.IsInTestingMode;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Get Blink Dash");
            Tooltip.SetDefault("--UNOBTAINABLE TESTING ITEM--" +
                "\nUnlocks 'Blink Dash'" +
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
                if (tPlayer.unlockedAbilities.Contains(8))
                {
                    Main.NewText("You already have that ability, silly!");
                }
                else
                {
                    tPlayer.unlockedAbilities.Add(8);
                    Main.NewText("Unlocked 'Blink Dash'");
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

    class BlinkDash : TerrorAbility
    {
        public override string TexturePath => "Abilities/BlinkDash_Icon";

        public override bool HasLockedPosition()
        {
            return false;
        }

        public override Vector2 dimensions()
        {
            return new Vector2(32, 22);
        }

        public override Vector2 baseOffsets()
        {
            return new Vector2(0, 0);
        }

        public override float getScale()
        {
            return 1f;
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
            target.unlockedAbilities.Add(8);
            target.TriggerAbilityAnimation("Blink Dash", "Performs a blink dash for 10% terror, allowing you to warp past foes and confuse them", "The direction of the dash depends on your movement keys", 0, visibilityTime: 900);
        }
    }
}
