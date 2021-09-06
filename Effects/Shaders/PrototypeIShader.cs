using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;

namespace TerrorbornMod.Effects.Shaders
{
	public class PrototypeIShader : ScreenShaderData
	{
		private int prototypeIIndex;

		public PrototypeIShader(string passName)
			: base(passName)
		{
		}

		private void UpdatePrototypeIIndex()
		{
			int prototypeIType = ModContent.NPCType<NPCs.Bosses.PrototypeI>();
			if (prototypeIIndex >= 0 && Main.npc[prototypeIIndex].active && Main.npc[prototypeIIndex].type == prototypeIType)
			{
				return;
			}
			prototypeIIndex = -1;
			for (int i = 0; i < Main.maxNPCs; i++)
			{
				if (Main.npc[i].active && Main.npc[i].type == prototypeIType)
				{
					prototypeIIndex = i;
					break;
				}
			}
		}

		public override void Apply()
		{
			UpdatePrototypeIIndex();
			if (prototypeIIndex != -1)
			{
				UseTargetPosition(Main.npc[prototypeIIndex].Center);
			}
			base.Apply();
		}
	}
}
