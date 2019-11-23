using UnityEngine;

namespace ItemSystem
{
    public class ItemComponent  : MonoBehaviour, IItemComponent
    {
        public BaseItem Item { get; private set; }
        public int number { get; private set; }
        
        public void Initialize(BaseItem item, int number)
        {
            this.number = number;
            Item = item;
            GetComponent<SpriteRenderer>().sprite = Item.ScriptableObject.sprite;
        }

        public void Serialize()
        {
            Destroy(this.gameObject);
        }
    }
}