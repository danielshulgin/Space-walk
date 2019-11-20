using ItemSystem;

public interface IItemComponent
{
    BaseItem Item { get; }

    void Initialize(BaseItem item);

    void Serialize();
}