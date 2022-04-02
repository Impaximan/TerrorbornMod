using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace TerrorbornMod.Items.Equipable.Accessories
{
    class DeathRollers : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Grave Rollers");
            Tooltip.SetDefault("Double tap down while in the air to do a short flip" +
                "\nLanding while in the middle of a flip will launch you forward and heal you slightly" +
                "\nDo this repeatedly to achieve sonic speed" +
                "\n'Rolled in my grave at the speed of sound... got places to go, gotta follow my rainbow!'");
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<RollerSkates>())
                .AddIngredient(ItemID.Bone, 35)
                .AddTile(TileID.Anvils)
                .Register();
        }

        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.sellPrice(0, 0, 50, 0);
        }

        bool heldDown = false;
        int cancleCounterDown = 0;
        int downTaps = 0;

        int dashDelay = 0;

        bool spinning = false;
        bool spinningVisually = false;
        int spinDir = 1;

        int timeSinceFlip = 0;

        public void Dash(Player player)
        {
            if (player.velocity.Y != 0 && !spinning)
            {
                Terraria.Audio.SoundEngine.PlaySound(SoundID.DD2_DarkMageSummonSkeleton, player.Center);
                spinDir = Math.Sign(player.velocity.X);
                if (player.velocity.X == 0)
                {
                    spinDir = player.direction;
                }
                if (player.controlRight)
                {
                    spinDir = 1;
                }
                if (player.controlLeft)
                {
                    spinDir = -1;
                }
                spinning = true;
                spinningVisually = true;
                timeSinceFlip = 0;
            }
        }

        public override void UpdateEquip(Player player)
        {
            if (player.dash > 0 || player.HasBuff(BuffID.SolarShield1) || player.HasBuff(BuffID.SolarShield2) || player.HasBuff(BuffID.SolarShield3))
            {
                return;
            }

            if (spinning)
            {
                if (player.velocity.Y == 0)
                {
                    player.velocity.X += spinDir * 12f;
                    spinning = false;
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.DD2_SkeletonHurt, player.Center);
                    TerrorbornSystem.ScreenShake(3f);
                    List<string> textOptions = new List<string>(){
                        { "Terrific!" },
                        { "Anarchic!" },
                        { "Horrifying!" },
                        { "Killer!" },
                        { "AHHH!" },
                        { "Deathly!" },
                        { "Insane!" }
                    };
                    CombatText.NewText(player.getRect(), Color.Red, Main.rand.Next(textOptions), true, false);

                    int amount = Main.rand.Next(8);
                    player.HealEffect(amount);
                    player.statLife += amount;
                }
            }

            if (spinningVisually)
            {
                player.fullRotation += spinDir * MathHelper.ToRadians(17f);
                player.fullRotationOrigin = new Vector2(player.width / 2, player.height / 2);

                if (Math.Abs(player.fullRotation) >= MathHelper.ToRadians(360))
                {
                    spinning = false;
                    spinningVisually = false;
                    player.fullRotation = 0f;
                }
            }

            if (dashDelay > 0)
            {
                dashDelay--;
            }

            if (!player.controlDown)
            {
                heldDown = false;
            }
            if (player.controlDown & !heldDown & dashDelay <= 0)
            {
                heldDown = true;
                cancleCounterDown = 20;
                downTaps++;
                if (downTaps >= 2)
                {
                    Dash(player);
                    dashDelay = 30;
                }
            }

            timeSinceFlip++;
            if (timeSinceFlip <= 30)
            {
                player.jumpSpeedBoost += 4f;
            }

            if (cancleCounterDown > 0)
            {
                cancleCounterDown--;
            }
            if (cancleCounterDown <= 0)
            {
                downTaps = 0;
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
}