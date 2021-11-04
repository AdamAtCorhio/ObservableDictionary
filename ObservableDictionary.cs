using System;
using System.Linq;
using System.ComponentModel;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ComponentModelEx
{
	public class ObservableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, INotifyPropertyChanged
	{
		public virtual event PropertyChangedEventHandler PropertyChanged;
		public virtual event DictionaryChangedEventHandler<TKey, TValue> DictionaryChanged;

		public static KeyValuePair<TKey, TValue> DefaultKeyValuePair = default(KeyValuePair<TKey, TValue>);

		public virtual new TValue this[TKey key]
		{
			get
			{
				if (!this.ContainsKey(key))
				{
					throw new KeyNotFoundException("The given key was not present in the dictionary.");
				}
				return base[key];
			}
			set
			{
				if (!this.ContainsKey(key))
				{
					this.Add(key, value);
				}
				else
				{
					TValue oldValue = base[key];
					KeyValuePair<TKey, TValue> oldKeyValuePair = GetKeyValuePair(key);
					base[key] = value;
					KeyValuePair<TKey, TValue> newKeyValuePair = GetKeyValuePair(key);
					RaiseDictionaryChanged(DictionaryChangedAction.Replace, oldKeyValuePair, newKeyValuePair);
					RaisePropertyChanged(nameof(Values));
				}
			}
		}

		public virtual new void Add(TKey key, TValue value)
		{
			if (this.ContainsKey(key))
			{
				throw new ArgumentException("An item with the same key has already been added.");
			}
			base.Add(key, value);
			KeyValuePair<TKey, TValue> newValue = GetKeyValuePair(key);
			RaiseDictionaryChanged(DictionaryChangedAction.Add, DefaultKeyValuePair, newValue);
			RaisePropertyChanged(nameof(Count));
			RaisePropertyChanged(nameof(Keys));
			RaisePropertyChanged(nameof(Values));
		}

		public virtual new bool Remove(TKey key)
		{
			if (!this.ContainsKey(key))
			{
				return false;
			}

			KeyValuePair<TKey, TValue> oldValue = GetKeyValuePair(key);

			bool result = base.Remove(key);
			RaiseDictionaryChanged(DictionaryChangedAction.Remove, oldValue, DefaultKeyValuePair);
			RaisePropertyChanged(nameof(Count));
			RaisePropertyChanged(nameof(Keys));
			RaisePropertyChanged(nameof(Values));
			return result;
		}

		public virtual new void Clear()
		{
			base.Clear();
			RaiseDictionaryChanged(DictionaryChangedAction.Clear, DefaultKeyValuePair, DefaultKeyValuePair);
			RaisePropertyChanged(nameof(Count));
			RaisePropertyChanged(nameof(Keys));
			RaisePropertyChanged(nameof(Values));
		}

		protected virtual void RaiseDictionaryChanged(DictionaryChangedAction action, KeyValuePair<TKey, TValue> oldValue, KeyValuePair<TKey, TValue> newValue)
		{
			DictionaryChanged?.Invoke(this, new DictionaryChangedEventArgs<TKey, TValue>(action, oldValue, newValue));
		}

		protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		protected virtual KeyValuePair<TKey, TValue> GetKeyValuePair(TKey key) => this.FirstOrDefault(kvp => this.Comparer.Equals(kvp.Key, key));
	}
}