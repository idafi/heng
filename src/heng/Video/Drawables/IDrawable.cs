namespace heng.Video
{
	/// <summary>
	/// Represents an object that can be drawn to a <see cref="Window"/>.
	/// <para>Feel free to create your own implementors alongside <see cref="Sprite"/>,
	/// <see cref="LineDrawable"/>, and such.</para>
	/// </summary>
	public interface IDrawable
	{
		/// <summary>
		/// Draws this <see cref="IDrawable"/> to the given <see cref="Window"/>.
		/// </summary>
		/// <param name="window">The <see cref="Window"/> to draw to.</param>
		/// <param name="camera">The <see cref="Camera"/> to draw to.</param>
		void Draw(Window window, Camera camera);
	};
}