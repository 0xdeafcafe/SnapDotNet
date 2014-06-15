using System.Collections.Generic;
using System.Linq;
using Windows.Foundation.Collections;

namespace SnapDotNet.Core.Miscellaneous.CustomTypes
{
	public class ObservableDictionary<TKey, TValue> 
		: IObservableMap<TKey, TValue>
	{
		private class ObservableDictionaryChangedEventArgs 
			: IMapChangedEventArgs<TKey>
		{
			public ObservableDictionaryChangedEventArgs(CollectionChange change, TKey key)
			{
				CollectionChange = change;
				Key = key;
			}
			public CollectionChange CollectionChange { get; private set; }
			public TKey Key { get; private set; }
		}

		private readonly Dictionary<TKey, TValue> _dictionary = new Dictionary<TKey, TValue>();
		public event MapChangedEventHandler<TKey, TValue> MapChanged;

		private void InvokeMapChanged(CollectionChange change, TKey key)
		{
			var eventHandler = MapChanged;
			if (eventHandler != null)
				eventHandler(this, new ObservableDictionaryChangedEventArgs(change, key));
		}

		public void Add(TKey key, TValue value)
		{
			_dictionary.Add(key, value);
			InvokeMapChanged(CollectionChange.ItemInserted, key);
		}

		public void Add(KeyValuePair<TKey, TValue> item)
		{
			Add(item.Key, item.Value);
		}

		public bool Remove(TKey key)
		{
			if (_dictionary.Remove(key))
			{
				InvokeMapChanged(CollectionChange.ItemRemoved, key);
				return true;
			}
			return false;
		}

		public bool Remove(KeyValuePair<TKey, TValue> item)
		{
			TValue currentValue;
			if (_dictionary.TryGetValue(item.Key, out currentValue) &&
				Equals(item.Value, currentValue) && _dictionary.Remove(item.Key))
			{
				InvokeMapChanged(CollectionChange.ItemRemoved, item.Key);
				return true;
			}
			return false;
		}

		public TValue this[TKey key]
		{
			get
			{
				return _dictionary[key];
			}
			set
			{
				_dictionary[key] = value;
				InvokeMapChanged(CollectionChange.ItemChanged, key);
			}
		}

		public void Clear()
		{
			var priorKeys = _dictionary.Keys.ToArray();
			_dictionary.Clear();
			foreach (var key in priorKeys)
			{
				InvokeMapChanged(CollectionChange.ItemRemoved, key);
			}
		}

		public ICollection<TKey> Keys
		{
			get { return _dictionary.Keys; }
		}

		public bool ContainsKey(TKey key)
		{
			return _dictionary.ContainsKey(key);
		}

		public bool TryGetValue(TKey key, out TValue value)
		{
			return _dictionary.TryGetValue(key, out value);
		}

		public ICollection<TValue> Values
		{
			get { return _dictionary.Values; }
		}

		public bool Contains(KeyValuePair<TKey, TValue> item)
		{
			return _dictionary.Contains(item);
		}

		public int Count
		{
			get { return _dictionary.Count; }
		}

		public bool IsReadOnly
		{
			get { return false; }
		}

		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			return _dictionary.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return _dictionary.GetEnumerator();
		}

		public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
		{
			var arraySize = array.Length;
			foreach (var pair in _dictionary)
			{
				if (arrayIndex >= arraySize) break;
				array[arrayIndex++] = pair;
			}
		}
	}
}
