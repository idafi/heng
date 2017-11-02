using System.Collections.Generic;

namespace heng.Input
{
	// TODO: InputDevice should be immutable, but, a dictionary reinstantiation for every reamp?
	// consider trying System.Collections.Immutable again, i guess

	/// <summary>
	/// A "virtual device" that can be used with the <see cref="InputState"/>to interpret <see cref="InputData"/>.
	/// <para>An <see cref="InputDevice"/> consists of string-keyed sets of virtual buttons and axes.</para>
	/// The <see cref="IButton"/> and <see cref="IAxis"/> interfaces can be used to construct new button/axis
	/// representations, if existing implementations like <see cref="Key"/> and <see cref="ButtonAxis"/> aren't enough.
	/// </summary>
	public class InputDevice
	{
		readonly Dictionary<string, IButton> buttons;
		readonly Dictionary<string, IAxis> axes;

		InputData lastData;
		InputData newData;
		
		/// <summary>
		/// Constructs a new <see cref="InputDevice"/>, which will read the provided state objects.
		/// <para>Methods such as <see cref="GetButtonPressed(string)"/> interpolated between old and new state.</para>
		/// </summary>
		public InputDevice()
		{
			buttons = new Dictionary<string, IButton>();
			axes = new Dictionary<string, IAxis>();
		}

		/// <summary>
		/// Updates the <see cref="InputData"/> read by the <see cref="InputDevice"/>, allowing methods like
		/// <see cref="GetButtonPressed(string)"/> to interpolate between the previous and current <see cref="InputData"/>.
		/// </summary>
		/// <param name="newData"></param>
		public void UpdateData(InputData newData)
		{
			Assert.Ref(newData);

			this.lastData = this.newData ?? newData;
			this.newData = newData;
		}

		/// <summary>
		/// Remaps the given string to the given virtual button.
		/// <para>If the string is not yet mapped, a new mapping will be created.</para>
		/// </summary>
		/// <param name="name">The string key that will be used to read this button.</param>
		/// <param name="button">The virtual button to map.</param>
		public void RemapButton(string name, IButton button)
		{
			if(name != null)
			{
				if(button != null)
				{ buttons[name] = button; }
				else
				{ Log.Error("couldn't remap button: new button is null"); }
			}
			else
			{ Log.Error("couldn't remap button: new button name is null"); }
		}
		
		/// <summary>
		/// Remaps the given string to the given virtual axis.
		/// <para>If the string is not yet mapped, a new mapping will be created.</para>
		/// </summary>
		/// <param name="name">The string key that will be used to read this axis.</param>
		/// <param name="axis">The virtual axis to map.</param>
		public void RemapAxis(string name, IAxis axis)
		{
			if(name != null)
			{
				if(axis != null)
				{ axes[name] = axis; }
				else
				{ Log.Error("couldn't remap axis: new axis is null"); }
			}
			else
			{ Log.Error("couldn't remap axis: new axis name is null"); }
		}

		/// <summary>
		/// Checks if the button mapped to the given string is currently down.
		/// </summary>
		/// <param name="name">The string to which the desired button was mapped.</param>
		/// <returns>Whether or not the button is currently down.</returns>
		public bool GetButtonDown(string name)
		{
			if(TryGetButton(name, out IButton button))
			{ return button.GetValue(newData); }
		
			return false;
		}
		
		/// <summary>
		/// Checks if the button mapped to the given string entered down state on this frame.
		/// </summary>
		/// <param name="name">The string to which the desired button was mapped.</param>
		/// <returns>Whether or not the button entered down state this frame.</returns>
		public bool GetButtonPressed(string name)
		{
			Assert.Ref(name);

			if(TryGetButton(name, out IButton button))
			{ return (!button.GetValue(lastData) && button.GetValue(newData)); }
		
			return false;
		}
		
		/// <summary>
		/// Checks if the button mapped to the given string left down state on this frame.
		/// </summary>
		/// <param name="name">The string to which the desired button was mapped.</param>
		/// <returns>Whether or not the button left down state this frame.</returns>
		public bool GetButtonReleased(string name)
		{
			if(TryGetButton(name, out IButton button))
			{ return (button.GetValue(lastData) && !button.GetValue(newData)); }
		
			return false;
		}
		
		/// <summary>
		/// Checks the value of the axis mapped to the given string, as a 16-bit signed integer.
		/// </summary>
		/// <param name="name">The string to which the desired axis was mapped.</param>
		/// <returns>The value of the axis.</returns>
		public short GetAxisValue(string name)
		{
			if(TryGetAxis(name, out IAxis axis))
			{ return axis.GetValue(newData); }
		
			return 0;
		}
		
		/// <summary>
		/// Checks the difference in value of the given axis between this frame and the last,
		/// as a 16-bit signed integer.
		/// </summary>
		/// <param name="name">The string to which the desired axis was mapped.</param>
		/// <returns>The difference in value of the axis between this frame and the last.</returns>
		public short GetAxisDelta(string name)
		{
			if(TryGetAxis(name, out IAxis axis))
			{ return (short)(axis.GetValue(newData) - axis.GetValue(lastData)); }
		
			return 0;
		}
		
		/// <summary>
		/// Checks the value of the axis mapped to the given string, as a 0-1 floating point fraction.
		/// </summary>
		/// <param name="name">The string to which the desired axis was mapped.</param>
		/// <returns>The value of the axis.</returns>
		public float GetAxisFrac(string name)
		{
			short val = GetAxisValue(name);
			return GetFrac(val);
		}
		
		/// <summary>
		/// Checks the difference in value of the given axis between this frame and the last,
		/// as a 0-1 floating point fraction.
		/// </summary>
		/// <param name="name">The string to which the desired axis was mapped.</param>
		/// <returns>The difference in value of the axis between this frame and the last.</returns>
		public float GetAxisDeltaFrac(string name)
		{
			short val = GetAxisDelta(name);
			return GetFrac(val);
		}
	
		bool TryGetButton(string name, out IButton button)
		{
			if(name != null)
			{
				if(buttons.TryGetValue(name, out button))
				{ return true; }
				else
				{ Log.Error($"couldn't read button \"{name}\" - no button with that name is mapped"); }
			}
			else
			{ Log.Error("couldn't read button: name is null"); }

			button = null;
			return false;
		}
	
		bool TryGetAxis(string name, out IAxis axis)
		{
			if(name != null)
			{
				if(axes.TryGetValue(name, out axis))
				{ return true; }
				else
				{ Log.Error($"couldn't read axis \"{name}\" - no axis with that name is mapped"); }
			}
			else
			{ Log.Error("couldn't read axis: name is null"); }

			axis = null;
			return false;
		}
	
		float GetFrac(short val)
		{
			float div = (val < 0) ? -short.MinValue : short.MaxValue;
			return val / div;
		}
	};
}