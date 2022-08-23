using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TerrorbornMod.NPCs.Bosses.TidalTitan;

namespace TerrorbornMod.TwilightMode
{
    public class TwilightNPC : GlobalNPC
	{
		public override bool InstancePerEntity => true;
		public bool twilight = false;
		bool start = true;



		public static TwilightNPC modNPC(NPC NPC)
		{
			if (NPC != null)
			{
				return NPC.GetGlobalNPC<TwilightNPC>();
			}
			else
			{
				return null;
			}
		}

        public override void SetDefaults(NPC NPC)
        {
			if (!TerrorbornSystem.TwilightMode || NPC.friendly || NPC.lifeMax >= int.MaxValue / 2)
            {
				return;
            }

			if (NPC.boss)
			{
				NPC.lifeMax = (int)(NPC.lifeMax * 1.5f);
				NPC.value *= 1.3f;
			}
            else if (NPC.type != ModContent.NPCType<PearlranhaBubble>())
			{
				NPC.lifeMax = (int)(NPC.lifeMax * 1.35f);
				NPC.knockBackResist *= 0.5f;
				NPC.value *= 2f;
			}

			if (NPC.type == NPCID.Vulture)
            {
				NPC.knockBackResist = 0f;
            }
        }

        public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
        {
			if (TerrorbornSystem.TwilightMode)
            {
				spawnRate = (int)(spawnRate * 0.75f);
				maxSpawns = (int)(maxSpawns * 1.3f);
			}
        }

		bool postAIStart = true;
		bool ballStoppedHoming = false;
        public override void PostAI(NPC NPC)
        {
			if (!TerrorbornSystem.TwilightMode)
            {
				return;
            }

			if (postAIStart)
            {
				postAIStart = false;
				ballStoppedHoming = false;
			}

			if (NPC.aiStyle == 9)
            {
				if (!ballStoppedHoming)
                {
					NPC.velocity = NPC.DirectionTo(Main.LocalPlayer.Center) * (NPC.velocity.Length() + 0.07f);
					if (NPC.Distance(Main.LocalPlayer.Center) <= 200f)
                    {
						ballStoppedHoming = true;
                    }
                }
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

		public void DustExplosion(Vector2 position, int RectWidth, int Streams, float DustSpeed, int DustType, Color color, float DustScale = 1f, bool NoGravity = false) //Thank you once again Seraph
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
				dust.color = color;
			}
		}

    }
}