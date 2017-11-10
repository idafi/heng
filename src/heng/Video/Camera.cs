namespace heng.Video
{
	/// <summary>
	/// Represents a world-space viewport.
	/// <para>The <see cref="VideoState"/> sends the current <see cref="Camera"/> to
	/// <see cref="IDrawable"/> objects, allowing them to easily convert world-space
	/// positions to screen-space coordinates.</para>
	/// A camera's viewport-space originates at the bottom-left of the screen.
	/// </summary>
	public class Camera
	{
		/// <summary>
		/// The camera's current world-space position.
		/// <para>This is the viewport's bottom-left-most position.</para>
		/// </summary>
		public readonly WorldPoint Position;

		/// <summary>
		/// Constructs a new <see cref="Camera"/>.
		/// <para>The camera's origin (bottom-left) will be placed at the given position.</para>
		/// If you want to place the camera's center at the position, you'll need to come up with
		/// that centered position yourself, most likely using a <see cref="Window"/>'s width and height.
		/// </summary>
		/// <param name="position">The new <see cref="Camera"/>'s world-space position.</param>
		public Camera(WorldPoint position)
		{
			Position = position;
		}

		/// <summary>
		/// Converts the given world-space position to a viewport-space position.
		/// </summary>
		/// <param name="position">The position to convert.</param>
		/// <returns>A <see cref="ScreenPoint"/> in viewport-space.</returns>
		public ScreenPoint WorldToViewportPosition(WorldPoint position)
		{
			return (ScreenPoint)(position.PixelDistance(this.Position));
		}

		/// <summary>
		/// Converts the given viewport-space position to a world-space position.
		/// </summary>
		/// <param name="position">The position to convert.</param>
		/// <returns>A <see cref="WorldPoint"/> in world-space.</returns>
		public WorldPoint ViewportToWorldPosition(ScreenPoint position)
		{
			return this.Position.PixelTranslate((Vector2)(position));
		}
	};
}