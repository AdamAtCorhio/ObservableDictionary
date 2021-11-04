using System;
using System.Collections.Generic;

namespace ComponentModelEx
{
	public class DictionaryChangedEventArgs<TKey, TValue> : EventArgs
	{
		public DictionaryChangedAction Action => _action;
		public KeyValuePair<TKey, TValue> OldValue => _oldValue;
		public KeyValuePair<TKey, TValue> NewValue => _newValue;

		private DictionaryChangedAction _action;
		private KeyValuePair<TKey, TValue> _oldValue;
		private KeyValuePair<TKey, TValue> _newValue;

		public DictionaryChangedEventArgs(DictionaryChangedAction action, KeyValuePair<TKey, TValue> oldValue, KeyValuePair<TKey, TValue> newValue)
		{
			_action = action;
			_oldValue = oldValue;
			_newValue = newValue;
		}
	}
}