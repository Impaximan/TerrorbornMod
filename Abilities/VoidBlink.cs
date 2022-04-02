using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TerrorbornMod.Abilities
{
    class VoidBlinkInfo : AbilityInfo
    {
        public override int typeInt()
        {
            return 3;
        }

        public override Texture2D texture()
        {
            return (Texture2D)ModContent.Request<Texture2D>("TerrorbornMod/Abilities/VoidBlink_Icon");
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
            return "Voidslip";
        }

        public override string Description()
        {
            return "Lets you 'slip away' for 3.5 seconds when used," +
                "\ngranting the following:" +
                "\n • Immunity to damage" +
                "\n • Increased item use speed" +
                "\n • Increased movement speed";
        }

        public override bool canUse(Player player)
        {
            return true;
        }

        public override void OnUse(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.VoidBlinkTime = 60 * 3 + 30;
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item72, player.Center);
            DustExplosion(player.Center, 0, 15, 15, 27, 1.5f, true);
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

    class ObtainVoidBlink : ModItem
    {
        public override string Texture => "TerrorbornMod/placeholder";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Get Voidslip");
            Tooltip.SetDefault("--UNOBTAINABLE TESTING ITEM--" +
                "\nUnlocks 'Void Blink'" +
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
                if (tPlayer.unlockedAbilities.Contains(3))
                {
                    Main.NewText("You already have that ability, silly!");
                }
                else
                {
                    tPlayer.unlockedAbilities.Add(3);
                    Main.NewText("Unlocked 'Voidslip'");
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
    class VoidBlink : TerrorAbility
    {
        public override string TexturePath => "Abilities/VoidBlink_Icon";

        public override Vector2 lockedPosition()
        {
            return TerrorbornSystem.VoidBlink * 16;
        }

        public override Vector2 dimensions()
        {
            return new Vector2(22, 24);
        }

        public override Vector2 baseOffsets()
        {
            return new Vector2(215, 206);
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
            target.unlockedAbilities.Add(3);
            target.TriggerAbilityAnimation("Voidslip", "Grants you immunity and increased speed for 3.5 seconds when used", "Costs 65% terror to use", 0, visibilityTime: 700);
        }
    }
}


