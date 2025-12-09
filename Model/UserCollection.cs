using System.Collections.ObjectModel;

namespace wpfOs.Model
{
    public class UserCollection
    {
        public ObservableCollection<User> Users { get; set; } = new();
    }
}
