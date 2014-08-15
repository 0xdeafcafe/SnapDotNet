﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SnapDotNet.Utilities
{
	/// <summary>
	/// Provides a base implementation of <see cref="INotifyPropertyChanged"/> allowing properties
	/// of derived classes to be observed for changes.
	/// </summary>
	public abstract class ObservableObject
		: INotifyPropertyChanged
	{
		/// <summary>
		/// Occurs when a property value changes.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged = delegate { };

		/// <summary>
		/// Assigns the specified <paramref name="value"/> to the specified <paramref name="field"/>
		/// reference, raising the <see cref="PropertyChanged"/> event if the value has been changed.
		/// </summary>
		/// <typeparam name="T">The type of the value.</typeparam>
		/// <param name="field">The reference to a field.</param>
		/// <param name="value">The value to assign to the field.</param>
		/// <param name="suppressEqualityCheck">
		/// If <c>true</c>, always raise the <see cref="PropertyChanged"/> event regardless of whether
		/// the value remains the same before and after this method's invocation.
		/// </param>
		/// <param name="propertyName">
		/// The name of the property. Leave as default to use the member name of the caller of this method.
		/// </param>
		/// <returns>
		/// A boolean value indicating whether the <see cref="PropertyChanged"/> event was raised.
		/// </returns>
		protected bool SetValue<T>(ref T field, T value, bool suppressEqualityCheck = false, [CallerMemberName] string propertyName = "")
		{
			if (!suppressEqualityCheck && EqualityComparer<T>.Default.Equals(field, value))
				return false;

			field = value;
			OnPropertyChanged(propertyName);

			return true;
		}

		/// <summary>
		/// Raises the <see cref="PropertyChanged"/> event.
		/// </summary>
		/// <param name="propertyName">
		/// The name of the property. Leave as default to use the member name of the caller of this method.
		/// </param>
		protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
		{
			PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}

		/// <summary>
		/// Raises the <see cref="PropertyChanged"/> event.
		/// </summary>
		protected void OnPropertyChangedExplicit(string propertyName)
		{
			PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}