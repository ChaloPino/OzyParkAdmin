using MudBlazor;
using MudBlazor.Interfaces;
using OzyParkAdmin.Components.Admin.Mantenedores.EscenariosCupo.Models;
using System.Collections.ObjectModel;

namespace OzyParkAdmin.Shared;

internal class ObservableGridData<T> : GridData<T>
{
    public ObservableGridData()
    {
    }

    public ObservableGridData(IEnumerable<T> items, int totalItems, IMudStateHasChanged stateHasChanged)
    {
        ObservableCollection<T> observableItems = new ObservableCollection<T>(items);
        Items = observableItems;
        TotalItems = totalItems;
        observableItems.CollectionChanged += (s, e) => stateHasChanged.StateHasChanged();
    }

    public void Add(T item)
    {
        ((ObservableCollection<T>)Items).Add(item);
    }

    public virtual bool Remove(T item)
    {
        return ((ObservableCollection<T>)Items).Remove(item);
    }

    public virtual bool RemoveWhere(Func<T, bool> predicate)
    {
        T? item = Find(predicate);

        if (item is not null)
        {
            return ((ObservableCollection<T>)Items).Remove(item);
        }

        return false;
    }

    public T? Find(T item)
    {
        return ((ObservableCollection<T>)Items).FirstOrDefault(x => EqualityComparer<T>.Default.Equals(x, item));
    }

    public T? Find(Func<T, bool> predicate)
    {
        return ((ObservableCollection<T>)Items).FirstOrDefault(predicate);
    }

    public static implicit operator ObservableGridData<T>(EscenarioCupoModel v)
    {
        throw new NotImplementedException();
    }
}

internal sealed class ObservableGridData<T, TKey>(IEnumerable<T> items, int totalItems, IMudStateHasChanged stateHasChanged, Func<T, TKey> keySelector) : ObservableGridData<T>(items, totalItems, stateHasChanged)
{
    private readonly Func<T, TKey> _keySelector = keySelector;

    public override bool Remove(T item)
    {
        if (!base.Remove(item))
        {
            T? currentItem = ((ObservableCollection<T>)Items).FirstOrDefault(x => EqualityComparer<TKey>.Default.Equals(_keySelector(x), _keySelector(item)));

            if (currentItem is not null)
            {
                return base.Remove(currentItem);
            }
        }

        return false;
    }
}