﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HBitcoin.FullBlockSpv;
using HBitcoin.KeyManagement;
using NBitcoin;
using Xunit;

namespace HBitcoin.Tests
{
    public class MiscTests
    {
		[Fact]
		public void ConcurrentObservableDictionaryTest()
		{
			ConcurrentObservableDictionary<int, string> dict = new ConcurrentObservableDictionary<int, string>();
			var times = 0;
			dict.CollectionChanged += delegate
			{
				times++;
			};

			dict.Add(1, "foo");
			dict.Add(2, "moo");
			dict.Add(3, "boo");

			dict.AddOrReplace(1, "boo");
			dict.Remove(dict.First(x => x.Value == "moo"));

			Assert.True(dict.Values.All(x => x == "boo"));
			Assert.Equal(5, times);
		}

		[Fact]
		public void ConcurrentObservableHashSetTest()
		{
			ConcurrentObservableHashSet<string> hashSet = new ConcurrentObservableHashSet<string>();
			var times = 0;
			hashSet.CollectionChanged += delegate
			{
				times++;
			};

			hashSet.Clear(); // no fire
			hashSet.TryAdd("foo"); // fire
			hashSet.TryAdd("foo"); // no fire
			hashSet.TryAdd("moo"); // fire
			hashSet.TryRemove("foo"); // fire
			hashSet.Clear(); // fire
			
			Assert.Equal(4, times);
		}
	}
}
