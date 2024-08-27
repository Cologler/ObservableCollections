﻿using System;
using System.Collections.Specialized;
using R3;
using System.Linq;
using ObservableCollections;




var list = new ObservableList<int>();
list.ObserveAdd()
    .Subscribe(x =>
    {
        Console.WriteLine(x);
    });

list.Add(10);
list.Add(20);
list.AddRange(new[] { 10, 20, 30 });








var models = new ObservableList<int>(Enumerable.Range(0, 10));

var viewModels = models.CreateView(x => new ViewModel
{
    Id = x,
    Value = "@" + x
});

viewModels.AttachFilter(new HogeFilter());

models.Add(100);

foreach (var x in viewModels)
{
    System.Console.WriteLine(x);
}

class ViewModel
{
    public int Id { get; set; }
    public string Value { get; set; } = default!;
}

class HogeFilter : ISynchronizedViewFilter<int>
{
    public bool IsMatch(int value)
    {
        return value % 2 == 0;
    }

    public void OnCollectionChanged(in SynchronizedViewChangedEventArgs<int, ViewModel> eventArgs)
    {
        switch (eventArgs.Action)
        {
            case NotifyCollectionChangedAction.Add:
                eventArgs.NewView.Value += " Add";
                break;
            case NotifyCollectionChangedAction.Remove:
                eventArgs.OldView.Value += " Remove";
                break;
            case NotifyCollectionChangedAction.Move:
                eventArgs.NewView.Value += $" Move {eventArgs.OldViewIndex} {eventArgs.NewViewIndex}";
                break;
            case NotifyCollectionChangedAction.Replace:
                eventArgs.NewView.Value += $" Replace {eventArgs.NewViewIndex}";
                break;
            case NotifyCollectionChangedAction.Reset:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(eventArgs.Action), eventArgs.Action, null);
        }
    }
}