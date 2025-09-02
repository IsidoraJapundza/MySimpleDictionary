using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MySimpleDictionary_IsidoraJapundza
{
    internal class MySimpleDictionary<TKey, TValue>
    {
        private LinkedList<Entry>[] buckets;
        private int count;
        private readonly IEqualityComparer<TKey> comparer;

        private class Entry
        {
            public TKey Key;    
            public TValue Value;
            public int HashCode; 

            public Entry(TKey key, TValue value, int hash)
            {
                Key = key;
                Value = value;  
                HashCode = hash;
            }
        }

        #region Constructors
        public MySimpleDictionary(int capacity = 16, IEqualityComparer<TKey>? customComparer = null)
        {
            if (capacity < 1) capacity = 16;
            buckets = new LinkedList<Entry>[capacity];
            comparer = customComparer ?? EqualityComparer<TKey>.Default;
            count = 0;
        }
        #endregion

        #region Add elements
        public void Add(TKey key, TValue value)
        {
            int hash = Hash(key);
            int index = IndexFor(hash);

            buckets[index] ??= new LinkedList<Entry>(); 

            foreach (var e in buckets[index])
            {
                if (e.HashCode == hash && comparer.Equals(e.Key, key))
                    throw new ArgumentException("Key already exists.");
            }

            buckets[index].AddLast(new Entry(key, value, hash));
            count++;
        }
        #endregion

        #region Retrieve value by key; access specific element
        public bool TryGetValue(TKey key, out TValue value)
        {
            int hash = Hash(key); 
            int index = IndexFor(hash);

            var list = buckets[index]; 
            
            if (list != null) 
            {
                foreach (var e in list)
                {
                    if (e.HashCode == hash && comparer.Equals(e.Key, key))
                    {
                        value = e.Value; 
                        return true;
                    }
                }

            }
            value = default!; 
            return false; 
        }

        public TValue this[TKey key]
        {
            get
            {
                if (TryGetValue(key, out var v)) return v;
                throw new KeyNotFoundException();
            }
            set
            {
                int hash = Hash(key);
                int index = IndexFor(hash);

                buckets[index] ??= new LinkedList<Entry>();

                var node = buckets[index].First;
                while (node != null)
                {
                    if (node.Value.HashCode == hash && comparer.Equals(node.Value.Key, key))
                    {
                        node.Value.Value = value;
                        return;
                    }
                    node = node.Next;
                }

                buckets[index].AddLast(new Entry(key, value, hash));
                count++;
            }
        }
        #endregion

        #region Check existence of keys and values
        public bool ContainsKey(TKey key)
        {
            TValue value;
            return TryGetValue(key, out value); 
        }

        public bool ContainsValue(TValue value)
        {
            var cmp = EqualityComparer<TValue>.Default;
            for (int i = 0; i < buckets.Length; i++)
            {
                var list = buckets[i];
                if (list == null) continue;

                foreach (var entry in list)
                    if (cmp.Equals(entry.Value, value))
                        return true;
            }
            return false;
        }

        /*
        public bool ContainsKey(TKey key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            int hash = comparer.GetHashCode(key) & 0x7fffffff;
            int index = hash % buckets.Length;

            var list = buckets[index];
            if (list == null)
                return false; 

            foreach (var e in list)
            {
                if (e.HashCode == hash && comparer.Equals(e.Key, key))
                    return true;
            }
            return false;
        }
        */
        #endregion

        #region Remove individual elements & clear all contents
        public bool Remove(TKey key)
        {
            int hash = Hash(key);
            int index = IndexFor(hash);

            var list = buckets[index];
            if (list == null) return false;

            var node = list.First;
            while (node != null)
            {
                if (node.Value.HashCode == hash && comparer.Equals(node.Value.Key, key))
                {
                    var toRemove = node;
                    node = node.Next; 
                    list.Remove(toRemove);
                    count--;
                    if (list.Count == 0) buckets[index] = null; 
                    return true;
                }
                else
                {
                    node = node.Next;
                }
            }

            return false;
        }

        public void Clear()
        {
            Array.Clear(buckets, 0, buckets.Length);
            count = 0;
        }
        #endregion

        #region Count elements
        public int Count => count;
        #endregion

        #region Ability to iterate through dictionary elements
        public IEnumerable<KeyValuePair<TKey, TValue>> AsPairs() { 
        
            for (int i = 0; i < buckets.Length; i++)
            {
                var list = buckets[i];
                if (list == null) continue;

                foreach (var e in list)
                    yield return new KeyValuePair<TKey, TValue>(e.Key, e.Value);
            }
        }
        #endregion 

        #region List of all keys and values
        public IEnumerable<TKey> Keys()
        {
            foreach (var kv in AsPairs()) yield return kv.Key;
        }

        public IEnumerable<TValue> Values()
        {
            foreach (var kv in AsPairs()) yield return kv.Value;
        }
        #endregion

        private int Hash(TKey key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            return comparer.GetHashCode(key) & 0x7fffffff;
        }

        private int IndexFor(int hash) => hash % buckets.Length;
    }
}
