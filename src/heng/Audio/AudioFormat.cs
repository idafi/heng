namespace heng.Audio
{
	/// <summary>
	/// Format for audio samples.
	/// <para>The engine's audio system will automatically convert loaded audio samples to
	/// the format specified in the <see cref="CoreConfig.AudioConfig"/>.</para>
	/// </summary>
	public enum AudioFormat
	{
		Invalid,
		U8,
		S8,
		U16,
		S16,
		S32,
		F32
	};
}