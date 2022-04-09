using Microsoft.Xna.Framework.Audio;
using Terraria;
using Terraria.ModLoader;

namespace TerrorbornMod.Sounds.Effects
{
    class DreadAngelHurt : ModSound
	{
        public override SoundEffectInstance PlaySound(ref SoundEffectInstance soundInstance, float volume, float pan)
		{
			soundInstance.Volume = volume;
			soundInstance.Pan = pan;
			soundInstance.Pitch = Main.rand.NextFloat(-0.1f, 0.1f);
			return soundInstance;
		}
	}
}
