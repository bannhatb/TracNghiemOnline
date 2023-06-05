

namespace TracNghiem.Domain.Entities
{
    public class Entity
    {
        int _Id;
        public virtual int Id
        {
            get
            {
                return _Id;
            }
            protected set
            {
                _Id = value;
            }
        }
        public void SetId(int id)
        {
            this._Id = id;
        }
    }
}
