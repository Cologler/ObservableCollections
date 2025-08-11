using System.Collections;

namespace ObservableCollections.Tests;

public class ToNotifyCollectionChangedTest
{
    [Fact]
    public void ToNotifyCollectionChanged()
    {
        var list = new ObservableList<int>();

        list.Add(10);
        list.Add(20);
        list.Add(30);

        var notify = list.CreateView(x => $"${x}").ToNotifyCollectionChanged();

        list.Add(40);
        list.Add(50);

        using var e = notify.GetEnumerator();
        e.MoveNext().Should().BeTrue();
        e.Current.Should().Be("$10");
        e.MoveNext().Should().BeTrue();
        e.Current.Should().Be("$20");
        e.MoveNext().Should().BeTrue();
        e.Current.Should().Be("$30");
        e.MoveNext().Should().BeTrue();
        e.Current.Should().Be("$40");
        e.MoveNext().Should().BeTrue();
        e.Current.Should().Be("$50");
        e.MoveNext().Should().BeFalse();

        var array = new string[7];
        ((ICollection)notify).CopyTo(array, 1);
        array.Should().BeEquivalentTo(new string[] { null, "$10", "$20", "$30", "$40", "$50", null });
    }

    [Fact]
    public void ToNotifyCollectionChanged_Filter()
    {
        var list = new ObservableList<int>();

        list.Add(1);
        list.Add(2);
        list.Add(5);
        list.Add(3);

        var view = list.CreateView(x => $"${x}");
        var notify = view.ToNotifyCollectionChanged();

        view.AttachFilter((value) => value % 2 == 0);

        list.Add(4);

        using var e = notify.GetEnumerator();
        e.MoveNext().Should().BeTrue();
        e.Current.Should().Be("$2");
        e.MoveNext().Should().BeTrue();
        e.Current.Should().Be("$4");
        e.MoveNext().Should().BeFalse();

        var array = new string[7];
        ((ICollection)notify).CopyTo(array, 1);
        array.Should().BeEquivalentTo(new string[] { null, "$2", "$4", null, null, null, null });
    }
}
