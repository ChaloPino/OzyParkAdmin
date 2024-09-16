using MudBlazor;
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
}

internal sealed class ObservableGridData<T, TKey> : ObservableGridData<T>
{
    private readonly Func<T, TKey> _keySelector;

    public ObservableGridData(IEnumerable<T> items, int totalItems, IMudStateHasChanged stateHasChanged, Func<T, TKey> keySelector) 
        : base(items, totalItems, stateHasChanged)
    {
        _keySelector = keySelector;
    }

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