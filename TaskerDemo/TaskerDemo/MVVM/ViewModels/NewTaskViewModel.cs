using System.Collections.ObjectModel;
using TaskerDemo.MVVM.Models;

namespace TaskerDemo.MVVM.ViewModels
{
    public class NewTaskViewModel
    {
        public string Task { get; set; }
        public ObservableCollection<MyTask> Tasks { get; set; }
        public ObservableCollection<Category> Categories { get; set; }
    }
}
