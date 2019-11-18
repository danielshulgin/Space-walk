using UnityEngine;

namespace ItemSystem
{
    public class ItemComponent  : MonoBehaviour, IItemComponent
    {
        public BaseItem Item { get; private set; }
        public void Initialize(BaseItem item)
        {
            Item = item;
            GetComponent<SpriteRenderer>().sprite = Item.ScriptableObject.sprite;
        }

        public void Serialize()
        {
            Destroy(this.gameObject);
        }
    }
}