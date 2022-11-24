using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;

namespace TerrorbornMod.Items.Weapons.Summons.Whips
{
    class PainLasher : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("\n12 summon tag summon damage" +
                "\nRight click to use the whip in the opposite direction" +
                "\nAlternate between left and right click to deal critical hits" +
                "\nCritical hits also have increased tag damage");
        }

        public override void SetDefaults()
        {
            Item.DefaultToWhip(ModContent.ProjectileType<PainLasher_Projectile>(), 300, 1.5f, 10f);
            Item.rare = ModContent.RarityType<Rarities.Golden>();
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Items.Materials.TorturedEssence>(), 3)
                .AddIngredient(ItemID.LunarBar, 10)
                .AddIngredient(ItemID.RainbowWhip)
                .AddIngredient(ItemID.FragmentStardust, 12)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        int lastUse = 0;
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (lastUse != player.altFunctionUse)
            {
                lastUse = player.altFunctionUse;
                velocity *= 1.5f;
                //damage *= 2;
                type = ModContent.ProjectileType<PainLasher_ProjectileCrit>();
            }
        }
    }

    class PainLasher_Projectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.IsAWhip[Type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.DefaultToWhip();
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            TerrorbornNPC modNPC = TerrorbornNPC.modNPC(target);
            modNPC.tagTime = 300;
            modNPC.currentTagDamageAdditive = 12;
        }

        bool start = true;
        bool playedSound = false;
        public override void PostAI()
        {
            Player player = Main.player[Projectile.owner];
            if (start == true)
            {
                start = false;
                if (player.altFunctionUse == 2)
                {
                    Projectile.ai[0] = (1f / player.GetWeaponAttackSpeed(player.HeldItem) * player.HeldItem.useTime * 2f) - 1;
                }
                Projectile.ai[1] = player.altFunctionUse;
            }

            if (Projectile.ai[1] == 2)
            {
                Projectile.ai[0] -= 2f;
                if (Projectile.ai[0] <= 0)
                {
                    Projectile.active = false;
                }

                if (Projectile.ai[0] <= (1f / player.GetWeaponAttackSpeed(player.HeldItem) * player.HeldItem.useTime * 1.25f) && !playedSound && player.GetWeaponAttackSpeed(player.HeldItem) != 1f)
                {
                    playedSound = true;
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.Item153);
                }
            }
        }

        private void DrawLine(List<Vector2> list)
        {
            Texture2D texture = TextureAssets.FishingLine.Value;
            Rectangle frame = texture.Frame();
            Vector2 origin = new Vector2(frame.Width / 2, 2);

            Vector2 pos = list[0];
            for (int i = 0; i < list.Count - 1; i++)
            {
                Vector2 element = list[i];
                Vector2 diff = list[i + 1] - element;

                float rotation = diff.ToRotation() - MathHelper.PiOver2;
                Color color = Lighting.GetColor(element.ToTileCoordinates(), Color.Pink);
                Vector2 scale = new Vector2(1, (diff.Length() + 2) / frame.Height);

                Main.EntitySpriteDraw(texture, pos - Main.screenPosition, frame, color, rotation, origin, scale, SpriteEffects.None, 0);

                pos += diff;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            List<Vector2> list = new List<Vector2>();
            Projectile.FillWhipControlPoints(Projectile, list);
            DrawLine(list);
            Main.DrawWhip_WhipBland(Projectile, list);
            return false;
        }
    }

    class PainLasher_ProjectileCrit : ModProjectile
    {
        public override string Texture => "TerrorbornMod/Items/Weapons/Summons/Whips/PainLasher_Projectile";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.IsAWhip[Type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.DefaultToWhip();
        }

        bool playedHurtSound = false;
        int freezeFrames = 0;
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            crit = true;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (!playedHurtSound)
            {
                playedHurtSound = true;
                if (Projectile.ai[1] == 2)
                {
                    ModContent.Request<SoundEffect>("TerrorbornMod/Sounds/Effects/CoolerMachineGun").Value.Play(Main.soundVolume * 1f, Main.rand.NextFloat(0.55f, 0.7f), 0f);
                }
                else
                {
                    ModContent.Request<SoundEffect>("TerrorbornMod/Sounds/Effects/CoolerMachineGun").Value.Play(Main.soundVolume * 1f, Main.rand.NextFloat(0.3f, 0.45f), 0f);
                }
                TerrorbornSystem.ScreenShake(3f);
                freezeFrames = 8;
            }

            TerrorbornNPC modNPC = TerrorbornNPC.modNPC(target);
            modNPC.tagTime = 600;
            modNPC.currentTagDamageAdditive = 24;
        }

        public override bool PreAI()
        {
            if (freezeFrames > 0)
            {
                freezeFrames--;
                return false;
            }
            return base.PreAI();
        }

        bool start = true;
        bool playedSound = false;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (start == true)
            {
                start = false;
                if (player.altFunctionUse == 2)
                {
                    Projectile.ai[0] = (1f / player.GetWeaponAttackSpeed(player.HeldItem) * player.HeldItem.useTime * 2f) - 1;
                }
                Projectile.ai[1] = player.altFunctionUse;
            }

            List<Vector2> list = new List<Vector2>();
            Projectile.FillWhipControlPoints(Projectile, list);
            foreach (Vector2 position in list)
            {
                if (player.Distance(position) >= 45f)
                {
                    Lighting.AddLight(position, 1, 0, 0);
                }
            }

            if (Projectile.ai[1] == 2)
            {
                Projectile.ai[0] -= 2f/* / player.GetWeaponAttackSpeed(player.HeldItem)*/;
                if (Projectile.ai[0] <= 0)
                {
                    Projectile.active = false;
                }

                if (Projectile.ai[0] <= (1f / player.GetWeaponAttackSpeed(player.HeldItem) * player.HeldItem.useTime * 1.25f) && !playedSound && player.GetWeaponAttackSpeed(player.HeldItem) != 1f)
                {
                    playedSound = true;
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.Item153);
                }
            }
        }

        private void DrawLine(List<Vector2> list)
		{
			Texture2D texture = TextureAssets.FishingLine.Value;
			Rectangle frame = texture.Frame();
			Vector2 origin = new Vector2(frame.Width / 2, 2);

			Vector2 pos = list[0];
			for (int i = 0; i < list.Count - 1; i++)
			{
				Vector2 element = list[i];
				Vector2 diff = list[i + 1] - element;

				float rotation = diff.ToRotation() - MathHelper.PiOver2;
				Color color = Lighting.GetColor(element.ToTileCoordinates(), Color.Red);
				Vector2 scale = new Vector2(1, (diff.Length() + 2) / frame.Height);

				Main.EntitySpriteDraw(texture, pos - Main.screenPosition, frame, color, rotation, origin, scale, SpriteEffects.None, 0);

				pos += diff;
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			List<Vector2> list = new List<Vector2>();
			Projectile.FillWhipControlPoints(Projectile, list);
			DrawLine(list);
			Main.DrawWhip_WhipBland(Projectile, list);
			return false;
		}
	}
}
