namespace heng.Input
{
	/// <summary>
	/// Represents a virtual axis whose value is defined by two virtual <see cref="IButton"/>s.
	/// </summary>
	public class ButtonAxis : IAxis
	{
		readonly IButton negative;
		readonly IButton positive;
		
		/// <summary>
		/// Creates a new <see cref="ButtonAxis"/> using the given virtual <see cref="IButton"/>s.
		/// </summary>
		/// <param name="negative">The button which, when pressed, adds a negative value to the axis.</param>
		/// <param name="positive">The button which, when pressed, adds a positive value to the axis.</param>
		public ButtonAxis(IButton negative, IButton positive)
		{
			Assert.Ref(negative);
			Assert.Ref(positive);

			this.negative = negative;
			this.positive = positive;
		}

		/// <inheritdoc />
		public short GetValue(InputData data)
		{
			Assert.Ref(data);

			bool neg = negative.GetValue(data);
			bool pos = positive.GetValue(data);
			short val = 0;
		
			if(neg)
			{ val -= short.MinValue; }
			if(pos)
			{ val += short.MaxValue; }
		
			return val;
		}
	};
}