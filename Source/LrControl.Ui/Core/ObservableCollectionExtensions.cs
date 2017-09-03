using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace LrControl.Ui.Core
{
    public static class ObservableCollectionExtensions
    {
        public static void SyncWith<T>(this ObservableCollection<T> observableCollection, IList<T> source)
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

        public static void DisposeAndClear<T>(this ObservableCollection<T> observableCollection)
        {
            foreach (var item in observableCollection)
            {
                if (item is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }

            observableCollection.Clear();
        }
    }
}