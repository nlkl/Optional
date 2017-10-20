using Microsoft.VisualStudio.TestTools.UnitTesting;
using Optional.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Optional.Tests
{
    [TestClass]
    public class CollectionTests
    {
        [TestMethod]
        public void Collections_Enumerable_ValuesMaybe()
        {
            var list1 = new List<Option<string>>
            {
                Option.Some<string>("a"),
                Option.None<string>(),
                Option.None<string>(),
                Option.Some<string>("b"),
                Option.Some<string>(null),
                Option.None<string>(),
                Option.Some<string>("c"),
            };
            var list1Expected = new List<string> { "a", "b", null, "c" };

            var list2 = new List<Option<string>>();
            var list2Expected = new List<string>();

            var list3 = new List<Option<string>> { "a".Some() };
            var list3Expected = new List<string> { "a" };

            var list4 = new List<Option<string>> { Option.None<string>() };
            var list4Expected = new List<string>();


            CollectionAssert.AreEqual(list1.Values().ToList(), list1Expected);
            CollectionAssert.AreEqual(list2.Values().ToList(), list2Expected);
            CollectionAssert.AreEqual(list3.Values().ToList(), list3Expected);
            CollectionAssert.AreEqual(list4.Values().ToList(), list4Expected);
        }

        [TestMethod]
        public void Collections_Enumerable_ValuesEither()
        {
            var list1 = new List<Option<string, string>>
            {
                Option.Some<string, string>("a"),
                Option.None<string, string>("error"),
                Option.None<string, string>("error"),
                Option.Some<string, string>("b"),
                Option.Some<string, string>(null),
                Option.None<string, string>("error"),
                Option.Some<string, string>("c"),
            };
            var list1Expected = new List<string> { "a", "b", null, "c" };

            var list2 = new List<Option<string, string>>();
            var list2Expected = new List<string>();

            var list3 = new List<Option<string, string>> { "a".Some<string, string>() };
            var list3Expected = new List<string> { "a" };

            var list4 = new List<Option<string, string>> { Option.None<string, string>("error") };
            var list4Expected = new List<string>();


            CollectionAssert.AreEqual(list1.Values().ToList(), list1Expected);
            CollectionAssert.AreEqual(list2.Values().ToList(), list2Expected);
            CollectionAssert.AreEqual(list3.Values().ToList(), list3Expected);
            CollectionAssert.AreEqual(list4.Values().ToList(), list4Expected);
        }

        [TestMethod]
        public void Collections_Enumerable_ExceptionsEither()
        {
            var list1 = new List<Option<string, string>>
            {
                Option.None<string, string>("a"),
                Option.Some<string, string>("value"),
                Option.Some<string, string>("value"),
                Option.None<string, string>("b"),
                Option.None<string, string>(null),
                Option.Some<string, string>("value"),
                Option.None<string, string>("c"),
            };
            var list1Expected = new List<string> { "a", "b", null, "c" };

            var list2 = new List<Option<string, string>>();
            var list2Expected = new List<string>();

            var list3 = new List<Option<string, string>> { Option.None<string, string>("a") };
            var list3Expected = new List<string> { "a" };

            var list4 = new List<Option<string, string>> { Option.Some<string, string>("error") };
            var list4Expected = new List<string>();


            CollectionAssert.AreEqual(list1.Exceptions().ToList(), list1Expected);
            CollectionAssert.AreEqual(list2.Exceptions().ToList(), list2Expected);
            CollectionAssert.AreEqual(list3.Exceptions().ToList(), list3Expected);
            CollectionAssert.AreEqual(list4.Exceptions().ToList(), list4Expected);
        }

        [TestMethod]
        public void Collections_Enumerable_FirstOrNone()
        {
            var full = Enumerable.Range(0, 100);
            var empty = Enumerable.Empty<int>();
            var single = Enumerable.Repeat(0, 1);

            FirstOperator(full, single, empty);

            var fullList = Enumerable.Range(0, 100).ToList();
            var emptyList = Enumerable.Empty<int>().ToList();
            var singleList = Enumerable.Repeat(0, 1).ToList();

            FirstOperator(fullList, singleList, emptyList);

#if !NET35
            var fullReadOnlyList = new TestReadOnlyList<int>(Enumerable.Range(0, 100).ToList());
            var emptyReadOnlyList = new TestReadOnlyList<int>(Enumerable.Empty<int>().ToList());
            var singleReadOnlyList = new TestReadOnlyList<int>(Enumerable.Repeat(0, 1).ToList());

            FirstOperator(fullReadOnlyList, singleReadOnlyList, emptyReadOnlyList);
#endif
        }

        [TestMethod]
        public void Collections_Enumerable_LastOrNone()
        {
            var full = Enumerable.Range(0, 100);
            var empty = Enumerable.Empty<int>();
            var single = Enumerable.Repeat(0, 1);

            LastOperator(full, single, empty);

            var fullList = Enumerable.Range(0, 100).ToList();
            var emptyList = Enumerable.Empty<int>().ToList();
            var singleList = Enumerable.Repeat(0, 1).ToList();

            LastOperator(fullList, singleList, emptyList);

#if !NET35
            var fullReadOnlyList = new TestReadOnlyList<int>(Enumerable.Range(0, 100).ToList());
            var emptyReadOnlyList = new TestReadOnlyList<int>(Enumerable.Empty<int>().ToList());
            var singleReadOnlyList = new TestReadOnlyList<int>(Enumerable.Repeat(0, 1).ToList());

            LastOperator(fullReadOnlyList, singleReadOnlyList, emptyReadOnlyList);
#endif
        }

        [TestMethod]
        public void Collections_Enumerable_SingleOrNone()
        {
            var full = Enumerable.Range(0, 100);
            var empty = Enumerable.Empty<int>();
            var single = Enumerable.Repeat(0, 1);

            SingleOperator(full, single, empty);

            var fullList = Enumerable.Range(0, 100).ToList();
            var emptyList = Enumerable.Empty<int>().ToList();
            var singleList = Enumerable.Repeat(0, 1).ToList();

            SingleOperator(fullList, singleList, emptyList);

#if !NET35
            var fullReadOnlyList = new TestReadOnlyList<int>(Enumerable.Range(0, 100).ToList());
            var emptyReadOnlyList = new TestReadOnlyList<int>(Enumerable.Empty<int>().ToList());
            var singleReadOnlyList = new TestReadOnlyList<int>(Enumerable.Repeat(0, 1).ToList());

            SingleOperator(fullReadOnlyList, singleReadOnlyList, emptyReadOnlyList);
#endif
        }

        [TestMethod]
        public void Collections_Enumerable_ElementAtOrNone()
        {
            var full = Enumerable.Range(0, 100);
            var empty = Enumerable.Empty<int>();
            var single = Enumerable.Repeat(0, 1);

            ElementAtOperator(full, single, empty);

            var fullList = Enumerable.Range(0, 100).ToList();
            var emptyList = Enumerable.Empty<int>().ToList();
            var singleList = Enumerable.Repeat(0, 1).ToList();

            ElementAtOperator(fullList, singleList, emptyList);

#if !NET35
            var fullReadOnlyList = new TestReadOnlyList<int>(Enumerable.Range(0, 100).ToList());
            var emptyReadOnlyList = new TestReadOnlyList<int>(Enumerable.Empty<int>().ToList());
            var singleReadOnlyList = new TestReadOnlyList<int>(Enumerable.Repeat(0, 1).ToList());

            ElementAtOperator(fullReadOnlyList, singleReadOnlyList, emptyReadOnlyList);
#endif
        }

        [TestMethod]
        public void Collections_Dictionary_GetValueOrNone()
        {
            var dictionaryA = Enumerable.Range(50, 50).ToDictionary(i => i, i => i.ToString());
            var excludedKeysA = Enumerable.Range(-50, 50);
#if !NET35
            GetValueOperator(new TestReadOnlyDictionary<int, string>(dictionaryA), excludedKeysA);
#endif
            GetValueOperator(new TestDictionary<int, string>(dictionaryA), excludedKeysA);
            GetValueOperator(dictionaryA.ToList(), excludedKeysA);

            var dictionaryB = new Dictionary<string, Guid>
            {
                { "a", Guid.NewGuid() },
                { "b", Guid.NewGuid() },
                { "c", Guid.NewGuid() },
                { "d", Guid.NewGuid() },
                { "e", Guid.NewGuid() },
            };
            var excludedKeysB = new List<string> { "h", "i", "j", "k" };
#if !NET35
            GetValueOperator(new TestReadOnlyDictionary<string, Guid>(dictionaryB), excludedKeysB);
#endif
            GetValueOperator(new TestDictionary<string, Guid>(dictionaryB), excludedKeysB);
            GetValueOperator(dictionaryB.ToList(), excludedKeysB);
        }

        private void FirstOperator(IEnumerable<int> full, IEnumerable<int> single, IEnumerable<int> empty)
        {
            Assert.IsTrue(full.FirstOrNone().HasValue);
            Assert.IsTrue(full.FirstOrNone(x => x == 50).HasValue);
            Assert.IsFalse(full.FirstOrNone(x => x == -1).HasValue);

            Assert.AreEqual(full.FirstOrNone().ValueOr(-1), full.First());
            Assert.AreEqual(full.FirstOrNone(x => x == 50).ValueOr(-1), 50);
            Assert.AreEqual(full.FirstOrNone(x => x > 50).ValueOr(-1), 51);
            Assert.AreEqual(full.FirstOrNone(x => x < 50).ValueOr(-1), full.First());

            Assert.IsTrue(single.FirstOrNone().HasValue);
            Assert.IsTrue(single.FirstOrNone(x => x == 0).HasValue);
            Assert.IsFalse(single.FirstOrNone(x => x == -1).HasValue);
            Assert.AreEqual(single.FirstOrNone().ValueOr(-1), single.First());

            Assert.IsFalse(empty.FirstOrNone().HasValue);
            Assert.IsFalse(empty.FirstOrNone(x => x == 50).HasValue);
        }

        private void LastOperator(IEnumerable<int> full, IEnumerable<int> single, IEnumerable<int> empty)
        {
            Assert.IsTrue(full.LastOrNone().HasValue);
            Assert.IsTrue(full.LastOrNone(x => x == 50).HasValue);
            Assert.IsFalse(full.LastOrNone(x => x == -1).HasValue);

            Assert.AreEqual(full.LastOrNone().ValueOr(-1), full.Last());
            Assert.AreEqual(full.LastOrNone(x => x == 50).ValueOr(-1), 50);
            Assert.AreEqual(full.LastOrNone(x => x > 50).ValueOr(-1), full.Last());
            Assert.AreEqual(full.LastOrNone(x => x < 50).ValueOr(-1), 49);

            Assert.IsTrue(single.LastOrNone().HasValue);
            Assert.IsTrue(single.LastOrNone(x => x == 0).HasValue);
            Assert.IsFalse(single.LastOrNone(x => x == -1).HasValue);
            Assert.AreEqual(single.LastOrNone().ValueOr(-1), single.Last());

            Assert.IsFalse(empty.LastOrNone().HasValue);
            Assert.IsFalse(empty.LastOrNone(x => x == 50).HasValue);
        }

        private void SingleOperator(IEnumerable<int> full, IEnumerable<int> single, IEnumerable<int> empty)
        {
            Assert.IsFalse(full.SingleOrNone().HasValue);
            Assert.IsTrue(full.SingleOrNone(x => x == 50).HasValue);
            Assert.IsFalse(full.SingleOrNone(x => x == -1).HasValue);
            Assert.IsFalse(full.SingleOrNone(x => x > 50).HasValue);
            Assert.IsFalse(full.SingleOrNone(x => x < 50).HasValue);
            Assert.AreEqual(full.SingleOrNone(x => x == 50).ValueOr(-1), 50);

            Assert.IsTrue(single.SingleOrNone().HasValue);
            Assert.IsTrue(single.SingleOrNone(x => x == 0).HasValue);
            Assert.IsFalse(single.SingleOrNone(x => x == -1).HasValue);
            Assert.AreEqual(single.SingleOrNone().ValueOr(-1), single.Single());

            Assert.IsFalse(empty.SingleOrNone().HasValue);
            Assert.IsFalse(empty.SingleOrNone(x => x == 50).HasValue);
        }

        private void ElementAtOperator(IEnumerable<int> full, IEnumerable<int> single, IEnumerable<int> empty)
        {
            Assert.IsFalse(full.ElementAtOrNone(-1).HasValue);
            Assert.IsFalse(full.ElementAtOrNone(full.Count()).HasValue);

            for (int i = 0; i < full.Count(); i++)
            {
                Assert.IsTrue(full.ElementAtOrNone(i).HasValue);
                Assert.AreEqual(full.ElementAtOrNone(i).ValueOr(-1), full.ElementAt(i));
            }

            Assert.IsTrue(single.ElementAtOrNone(0).HasValue);
            Assert.IsFalse(single.ElementAtOrNone(2).HasValue);
            Assert.IsFalse(single.ElementAtOrNone(-1).HasValue);
            Assert.AreEqual(single.ElementAtOrNone(0).ValueOr(-1), single.Single());

            Assert.IsFalse(empty.ElementAtOrNone(0).HasValue);
        }

        private void GetValueOperator<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>> dictionary, IEnumerable<TKey> excludedKeys)
        {
            foreach (var pair in dictionary)
            {
                Assert.IsTrue(dictionary.GetValueOrNone(pair.Key).HasValue);
                Assert.AreEqual(dictionary.GetValueOrNone(pair.Key).ValueOr(default(TValue)), pair.Value);
            }

            foreach (var key in excludedKeys)
            {
                Assert.IsFalse(dictionary.GetValueOrNone(key).HasValue);
            }
        }

#if !NET35
        private class TestReadOnlyDictionary<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>
        {
            private readonly Dictionary<TKey, TValue> dictionary;

            public TestReadOnlyDictionary(Dictionary<TKey, TValue> dictionary)
            {
                this.dictionary = dictionary;
            }

            public TValue this[TKey key] => dictionary[key];
            public int Count => dictionary.Count;
            public IEnumerable<TKey> Keys => dictionary.Keys;
            public IEnumerable<TValue> Values => dictionary.Values;
            public bool ContainsKey(TKey key) => dictionary.ContainsKey(key);
            public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => dictionary.GetEnumerator();
            public bool TryGetValue(TKey key, out TValue value) => dictionary.TryGetValue(key, out value);
            IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)dictionary).GetEnumerator();
        }

        private class TestReadOnlyList<T> : IReadOnlyList<T>
        {
            private readonly List<T> list;

            public TestReadOnlyList(List<T> list)
            {
                this.list = list;
            }

            public T this[int index] => list[index];
            public int Count => list.Count;
            public IEnumerator<T> GetEnumerator() => list.GetEnumerator();
            IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)list).GetEnumerator();
        }
#endif

        private class TestDictionary<TKey, TValue> : IDictionary<TKey, TValue>
        {
            private readonly Dictionary<TKey, TValue> dictionary;

            public TestDictionary(Dictionary<TKey, TValue> dictionary)
            {
                this.dictionary = dictionary;
            }

            private ICollection<KeyValuePair<TKey, TValue>> Collection => dictionary;

            public TValue this[TKey key]
            {
                get { return dictionary[key]; }
                set { dictionary[key] = value; }
            }

            public int Count => dictionary.Count;
            public bool IsReadOnly => Collection.IsReadOnly;
            public ICollection<TKey> Keys => dictionary.Keys;
            public ICollection<TValue> Values => dictionary.Values;
            public void Add(KeyValuePair<TKey, TValue> item) => Collection.Add(item);
            public void Add(TKey key, TValue value) => dictionary.Add(key, value);
            public void Clear() => Collection.Clear();
            public bool Contains(KeyValuePair<TKey, TValue> item) => dictionary.Contains(item);
            public bool ContainsKey(TKey key) => dictionary.ContainsKey(key);
            public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) => Collection.CopyTo(array, arrayIndex);
            public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => dictionary.GetEnumerator();
            public bool Remove(KeyValuePair<TKey, TValue> item) => Collection.Remove(item);
            public bool Remove(TKey key) => dictionary.Remove(key);
            public bool TryGetValue(TKey key, out TValue value) => dictionary.TryGetValue(key, out value);
            IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)dictionary).GetEnumerator();
        }
    }
}
