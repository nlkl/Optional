using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Optional.Extensions.Collections;

namespace Optional.Tests.Extensions
{
    [TestClass]
    public class LinqTests
    {
        [TestMethod]
        public void Extensions_OptionToEnumerable()
        {
            var none = "a".None();
            var some = "a".Some();

            var noneAsEnumerable = none.ToEnumerable();
            var someAsEnumerable = some.ToEnumerable();

            foreach (var value in noneAsEnumerable)
            {
                Assert.Fail();
            }

            int count = 0;
            foreach (var value in someAsEnumerable)
            {
                Assert.AreEqual(value, "a");
                count += 1;
            }

            Assert.AreEqual(count, 1);
        }

        [TestMethod]
        public void Extensions_FirstOperatorEnumerable()
        {
            var full = Enumerable.Range(0, 100);
            var empty = Enumerable.Empty<int>();
            var single = Enumerable.Repeat(0, 1);

            FirstOperator(full, single, empty);

            var fullList = Enumerable.Range(0, 100).ToList();
            var emptyList = Enumerable.Empty<int>().ToList();
            var singleList = Enumerable.Repeat(0, 1).ToList();

            FirstOperator(fullList, singleList, emptyList);
        }

        [TestMethod]
        public void Extensions_FirstOperatorQueryable()
        {
            var full = Enumerable.Range(0, 100).AsQueryable();
            var empty = Enumerable.Empty<int>().AsQueryable();
            var single = Enumerable.Repeat(0, 1).AsQueryable();

            FirstOperator(full, single, empty);
        }

        [TestMethod]
        public void Extensions_LastOperatorEnumerable()
        {
            var full = Enumerable.Range(0, 100);
            var empty = Enumerable.Empty<int>();
            var single = Enumerable.Repeat(0, 1);

            LastOperator(full, single, empty);

            var fullList = Enumerable.Range(0, 100).ToList();
            var emptyList = Enumerable.Empty<int>().ToList();
            var singleList = Enumerable.Repeat(0, 1).ToList();

            LastOperator(fullList, singleList, emptyList);
        }

        [TestMethod]
        public void Extensions_LastOperatorQueryable()
        {
            var full = Enumerable.Range(0, 100).AsQueryable();
            var empty = Enumerable.Empty<int>().AsQueryable();
            var single = Enumerable.Repeat(0, 1).AsQueryable();

            LastOperator(full, single, empty);
        }

        [TestMethod]
        public void Extensions_SingleOperatorEnumerable()
        {
            var full = Enumerable.Range(0, 100);
            var empty = Enumerable.Empty<int>();
            var single = Enumerable.Repeat(0, 1);

            SingleOperator(full, single, empty);

            var fullList = Enumerable.Range(0, 100).ToList();
            var emptyList = Enumerable.Empty<int>().ToList();
            var singleList = Enumerable.Repeat(0, 1).ToList();

            SingleOperator(fullList, singleList, emptyList);
        }

        [TestMethod]
        public void Extensions_SingleOperatorQueryable()
        {
            var full = Enumerable.Range(0, 100).AsQueryable();
            var empty = Enumerable.Empty<int>().AsQueryable();
            var single = Enumerable.Repeat(0, 1).AsQueryable();

            SingleOperator(full, single, empty);
        }

        [TestMethod]
        public void Extensions_ElementAtOperatorEnumerable()
        {
            var full = Enumerable.Range(0, 100);
            var empty = Enumerable.Empty<int>();
            var single = Enumerable.Repeat(0, 1);

            ElementAtOperator(full, single, empty);

            var fullList = Enumerable.Range(0, 100).ToList();
            var emptyList = Enumerable.Empty<int>().ToList();
            var singleList = Enumerable.Repeat(0, 1).ToList();

            ElementAtOperator(fullList, singleList, emptyList);
        }

        [TestMethod]
        public void Extensions_ElementAtOperatorQueryable()
        {
            var full = Enumerable.Range(0, 100).AsQueryable();
            var empty = Enumerable.Empty<int>().AsQueryable();
            var single = Enumerable.Repeat(0, 1).AsQueryable();

            ElementAtOperator(full, single, empty);
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

        private void FirstOperator(IQueryable<int> full, IQueryable<int> single, IQueryable<int> empty)
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

        private void LastOperator(IQueryable<int> full, IQueryable<int> single, IQueryable<int> empty)
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

        private void SingleOperator(IQueryable<int> full, IQueryable<int> single, IQueryable<int> empty)
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

        private void ElementAtOperator(IQueryable<int> full, IQueryable<int> single, IQueryable<int> empty)
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
    }
}
