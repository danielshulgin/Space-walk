using ItemSystem;

public interface IItemComponent
{
    BaseItem Item { get; }
    int number { get; }

    void Initialize(BaseItem item, int number);

    void Serialize();
}