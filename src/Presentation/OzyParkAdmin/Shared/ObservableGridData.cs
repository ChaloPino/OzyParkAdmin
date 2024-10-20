﻿using MudBlazor;
using MudBlazor.Interfaces;
using System.Collections.ObjectModel;

namespace OzyParkAdmin.Shared;

internal class ObservableGridData<T> : GridData<T>
{
    protected readonly ObservableCollection<T> _items;

    public ObservableGridData(IEnumerable<T> items, int totalItems, IMudStateHasChanged stateHasChanged)
    {
        _items = new ObservableCollection<T>(items);
        Items = _items;
        TotalItems = totalItems;
        _items.CollectionChanged += (s, e) => stateHasChanged.StateHasChanged();
    }

    public void Add(T item)
    {
        _items.Add(item);
    }

    public virtual bool Remove(T item)
    {
        return _items.Remove(item);
    }

    public virtual bool RemoveWhere(Func<T, bool> predicate)
    {
        T? item = Find(predicate);

        if (item is not null)
        {
            return _items.Remove(item);
        }

        return false;
    }

    public T? Find(T item)
    {
        return _items.FirstOrDefault(x => EqualityComparer<T>.Default.Equals(x, item));
    }

    public T? Find(Func<T, bool> predicate)
    {
        return _items.FirstOrDefault(predicate);
    }
}

internal sealed class ObservableGridData<T, TKey>(IEnumerable<T> items, int totalItems, IMudStateHasChanged stateHasChanged, Func<T, TKey> keySelector) : ObservableGridData<T>(items, totalItems, stateHasChanged)
{
    private readonly Func<T, TKey> _keySelector = keySelector;

    public override bool Remove(T item)
    {
        if (!base.Remove(item))
        {
            T? currentItem = _items.FirstOrDefault(x => EqualityComparer<TKey>.Default.Equals(_keySelector(x), _keySelector(item)));

            if (currentItem is not null)
            {
                return base.Remove(currentItem);
            }
        }

        return false;
    }
}