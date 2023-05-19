using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace TerrorbornMod.Items.Weapons.Summons.Whips
{
    class AzuretoothWhip : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("\n1 summon tag damage" +
                "\nRepeated hits increase tag damage further" +
                "\nCapped at 10 tag damage");
        }

        public override void SetDefaults()
        {
            Item.DefaultToWhip(ModContent.ProjectileType<AzuretoothWhip_Projectile>(), 20, 1.5f, 5f, 40);
            Item.rare = ItemRarityID.Green;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {

        }
    }

    class AzuretoothWhip_Projectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.IsAWhip[Type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.DefaultToWhip();
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            TerrorbornNPC modNPC = TerrorbornNPC.modNPC(target);
            modNPC.tagTime = 300;
            if (modNPC.currentTagDamageAdditive < 10)
            {
                modNPC.currentTagDamageAdditive++;
            }
        }

        public override void PostAI()
        {

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
                Color color = Lighting.GetColor(element.ToTileCoordinates(), Color.RoyalBlue);
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

