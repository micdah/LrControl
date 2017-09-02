using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace LrControl.Ui.Core
{
    public static class ObservableCollectionExtensions
    {
        public static void AddRange<T>(this ObservableCollection<T> observableCollection, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                observableCollection.Add(item);
            }
        }

        public static void SyncWith<T>(this ObservableCollection<T> observableCollection, IEnumerable<T> source)
        {
            // Detect removed
            foreach (var item in observableCollection
                .Where(x => !source.Contains(x))
                .ToList())
            {
                if (item is IDisposable disposable)
                {
                    disposable.Dispose();
                }

                observableCollection.Remove(item);
            }

            // Detect added
            foreach (var item in source
                .Where(x => !observableCollection.Contains(x))
                .ToList())
            {
                observableCollection.Add(item);
            }
        }
    }
}