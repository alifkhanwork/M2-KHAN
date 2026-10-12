using System.ComponentModel;

namespace PixelPawn.Desktop.Services
{
    // Lets a DataGridView sort when a column header is clicked.
    public class SortableBindingList<T> : BindingList<T>
    {
        private bool _sorted;
        private ListSortDirection _dir;
        private PropertyDescriptor? _prop;

        public SortableBindingList(IList<T> list) : base(list) { }

        protected override bool SupportsSortingCore => true;
        protected override bool IsSortedCore => _sorted;
        protected override ListSortDirection SortDirectionCore => _dir;
        protected override PropertyDescriptor? SortPropertyCore => _prop;

        protected override void ApplySortCore(PropertyDescriptor prop, ListSortDirection dir)
        {
            var items = (List<T>)Items;
            items.Sort((a, b) =>
            {
                var x = prop.GetValue(a) as IComparable;
                var y = prop.GetValue(b);
                int r = x == null ? -1 : x.CompareTo(y);
                return dir == ListSortDirection.Ascending ? r : -r;
            });
            _prop = prop; _dir = dir; _sorted = true;
            OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
        }

        protected override void RemoveSortCore() => _sorted = false;
    }
}
