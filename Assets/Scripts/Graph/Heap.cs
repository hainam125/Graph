﻿using System;

public class Heap<T> where T : GraphNode {
	private int[] itemHeapIdx;
	private T[] items;
	private int currentItemCount;
	private Func<T, T, int> Compare;

	public int Count { get { return currentItemCount; } }

	public Heap(int maxHeapSize, Func<T, T, int> compare) {
		items = new T[maxHeapSize];
		itemHeapIdx = new int[maxHeapSize];
		Compare = compare;
	}

	public void Add(T item) {
		itemHeapIdx[item.Index] = currentItemCount;
		items[currentItemCount] = item;
		SortUp(item);
		currentItemCount++;
	}

	public T RemoveFirst() {
		T firstItem = items[0];
		currentItemCount--;
		items[0] = items[currentItemCount];
		itemHeapIdx[items[0].Index] = 0;
		SortDown(items[0]);
		return firstItem;
	}

	public void UpdateItem(T item) {
		SortUp(item);
	}

	public bool Contains(T item) {
		return Equals(items[itemHeapIdx[item.Index]], item);
	}

	void SortDown(T item) {
		while (true) {
			int childIndexLeft = itemHeapIdx[item.Index] * 2 + 1;
			int childIndexRight = itemHeapIdx[item.Index] * 2 + 2;
			int swapIndex = 0;

			if (childIndexLeft < currentItemCount) {
				swapIndex = childIndexLeft;

				if (childIndexRight < currentItemCount && Compare(items[childIndexLeft], items[childIndexRight]) < 0) {
					swapIndex = childIndexRight;
				}

				if (Compare(item, items[swapIndex]) < 0) Swap(item, items[swapIndex]);
				else return;
			}
			else {
				return;
			}

		}
	}

	void SortUp(T item) {
		int parentIndex = (itemHeapIdx[item.Index] - 1) / 2;

		while (true) {
			T parentItem = items[parentIndex];

			if (Compare(item, parentItem) > 0) Swap(item, parentItem);
			else break;

			parentIndex = (itemHeapIdx[item.Index] - 1) / 2;
		}
	}

	void Swap(T itemA, T itemB) {
		items[itemHeapIdx[itemA.Index]] = itemB;
		items[itemHeapIdx[itemB.Index]] = itemA;
		int itemAIndex = itemHeapIdx[itemA.Index];
		itemHeapIdx[itemA.Index] = itemHeapIdx[itemB.Index];
		itemHeapIdx[itemB.Index] = itemAIndex;
	}
}